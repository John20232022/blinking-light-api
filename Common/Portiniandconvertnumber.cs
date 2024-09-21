using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace newblinkinglight.Common
{
    public class Portiniandconvertnumber
    {
        public static SerialPort objectserialconstantrefresh;
        public static void portinitialconstantchecking()
        {
            Console.WriteLine("hi")
            Thread thread = new Thread(x =>
            {

                string[] initialportslist = SerialPort.GetPortNames();
                foreach (string initialport in initialportslist)
                {
                    Console.WriteLine(initialport);
                }
                while (true)
                {
                    string[] constantrefreshportlist = SerialPort.GetPortNames();
                    // Find elements in initialportslist that are not in constantrefreshportlist
                    var diff1 = initialportslist.Except(constantrefreshportlist);

                    // Find elements in array2 that are not in array1
                    var diff2 = constantrefreshportlist.Except(initialportslist);

                    // Combine the differences
                    var differences = diff1.Union(diff2);

                    if (differences.Any())
                    {
                        // thong bao co su thay doi
                        if (constantrefreshportlist.Length < initialportslist.Length)
                        {
                            Console.WriteLine($"device taken out at port : {string.Join("", diff1)}");
                        }
                        // checking if new device insert , if new device inserted --> update new objectserialconstantrefresh instance
                        if (constantrefreshportlist.Length > initialportslist.Length)
                        {
                            Session.CurrentPort = constantrefreshportlist.Last();
                            Console.WriteLine($"new device insert at port : {string.Join("", diff2)}");
                            //Thoi gian cho toi thieu truoc khi truyen lenh
                            Thread.Sleep(1000);
                            // Tạo đối tượng SerialPort và cấu hình các thuộc tính cơ bản
                            objectserialconstantrefresh = new SerialPort();

                            // Cấu hình cổng COM, thay đổi COM3 thành cổng COM thực tế của bạn
                            objectserialconstantrefresh.PortName = Session.CurrentPort; // Tên cổng COM  // dùng session vì tất cả biến này có thể dùng trên toàn bộ chương trình
                            Console.WriteLine(objectserialconstantrefresh.PortName);
                            objectserialconstantrefresh.BaudRate = 9600;   // Tốc độ Baud
                            objectserialconstantrefresh.Parity = Parity.None; // Không có parity
                            objectserialconstantrefresh.DataBits = 8;      // 8 bit dữ liệu
                            objectserialconstantrefresh.StopBits = StopBits.One; // 1 bit dừng
                            objectserialconstantrefresh.Handshake = Handshake.None; // Không dùng Handshake

                            // Cài đặt thời gian chờ
                            objectserialconstantrefresh.ReadTimeout = 30000; // Thời gian chờ đọc dữ liệu
                            objectserialconstantrefresh.WriteTimeout = 30000; // Thời gian chờ ghi dữ liệu

                            // thu ket noi
                            try
                            {
                                // Mở kết nối với cổng COM
                                objectserialconstantrefresh.Open();
                                Console.WriteLine($"{constantrefreshportlist.Last()} Kết nối tới cổng COM thành công.");
                                // Đọc dữ liệu trả về từ thiết bị (nếu có)
                                objectserialconstantrefresh.DataReceived += SerialPort_DataReceived;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi khi kết nối cổng {objectserialconstantrefresh.PortName}: " + ex.ToString());
                            }
                        }
                        //update initialportlist
                        initialportslist = constantrefreshportlist;
                    }

                }
            });
            thread.Start();
        }
        

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            // Đọc toàn bộ dữ liệu có sẵn từ Serial Port
            string inData = sp.ReadExisting();

            // Xử lý dữ liệu (ví dụ: in ra màn hình)
            Console.WriteLine("Data Received: " + inData);
            try
            {
                //string response = serialPort.ReadLine();// chỗ này chạy bất đồng bộ mà mình đang chạy đồng bộ nên nó bị time out
                Console.WriteLine("Nhận được từ thiết bị: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static byte[] HexStringToByteArray(string hex)
        {
            // Kiểm tra chuỗi hex có độ dài hợp lệ
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Chuỗi hex phải có độ dài chẵn.");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16); // Chuyển đổi mỗi cặp ký tự hex thành 1 byte
            }
            return bytes;
        }
    }
}
