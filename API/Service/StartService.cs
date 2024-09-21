using Microsoft.Owin.Hosting;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace newblinkinglight.API.Service
{
    public class StartService
    {
        public static void Start()
        {

            string port = ConfigurationManager.AppSettings[AppSettingKeys.PortApp];
            if (!string.IsNullOrWhiteSpace(port))
            {
                string ipAddress = GetLocalIPv4();
                if (!IsPortOpen(port))
                {
                    string ruleName = $"Allow Port {port} for Web API";
                    AddFirewallRule(ruleName, port);
                }
                string baseAddress = $"http://{ipAddress}:{port}/";
                //string baseAddress = $"http://127.0.0.1:9000/";
                // Bắt đầu OWIN host
                using (WebApp.Start<Startup>(url: baseAddress))
                {
                    Console.WriteLine("Web API dang chay tai " + baseAddress);
                    Console.ReadLine();
                }
            }
        }
        /// <summary>
        /// Lấy ip của máy hiện tại
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPv4()
        {
            string localIP = string.Empty;

            // Lấy tên máy
            string hostName = Dns.GetHostName();

            // Lấy tất cả các địa chỉ IP của máy
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            // Lọc chỉ địa chỉ IPv4
            localIP = hostEntry
                        .AddressList
                        .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                        .ToString();

            return localIP ?? "Không tìm thấy địa chỉ IPv4";
        }
        // Phương thức kiểm tra xem cổng có đang được mở trong firewall hay không
        public static bool IsPortOpen(string port)
        {
            // Tạo đối tượng firewall manager
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(type);

            // Duyệt qua tất cả các rule trong firewall
            foreach (INetFwRule rule in firewallPolicy.Rules)
            {
                // Kiểm tra nếu rule cho cổng đó đã tồn tại và đang cho phép kết nối
                if (rule.LocalPorts != null && rule.LocalPorts.Contains(port) &&
                    rule.Enabled && rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW &&
                    rule.Protocol == (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP)
                {
                    return true; // Cổng đã được mở
                }
            }

            return false; // Cổng chưa được mở
        }

        // Phương thức thêm quy tắc firewall nếu cổng chưa được mở
        public static void AddFirewallRule(string name, string port)
        {
            // Tạo đối tượng firewall manager
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(type);

            // Tạo một rule mới
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

            // Đặt thuộc tính cho rule
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW; // Cho phép kết nối
            firewallRule.Description = $"Cho phép kết nối đến cổng {port}";
            firewallRule.Enabled = true; // Bật rule
            firewallRule.InterfaceTypes = "All"; // Áp dụng cho tất cả các loại kết nối (LAN, Wi-Fi, v.v.)
            firewallRule.Name = name; // Đặt tên cho rule
            firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP; // Sử dụng TCP protocol
            firewallRule.LocalPorts = port; // Áp dụng cho cổng cụ thể

            // Thêm rule vào Windows Firewall
            firewallPolicy.Rules.Add(firewallRule);
        }
    }
}
