using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace newblinkinglight.Common
{
    public class Command
    {
        public static void transfercommand(string commandscript)
        {
            // Gửi một chuỗi dữ liệu tới thiết bị nối tiếp
            // Chuỗi hexadecimal cần chuyển đổi
            if (commandscript == "redbuzzsleepturnoff")
            {
                string hexString = "11";
                string hexString1 = "18";
                string hexString2 = "21";
                string hexString3 = "28";

                //Chuyển đổi chuỗi hexadecimal thành mảng byte
                byte[] byteArray = Portiniandconvertnumber.HexStringToByteArray(hexString);
                byte[] byteArray1 = Portiniandconvertnumber.HexStringToByteArray(hexString1);
                byte[] byteArray2 = Portiniandconvertnumber.HexStringToByteArray(hexString2);
                byte[] byteArray3 = Portiniandconvertnumber.HexStringToByteArray(hexString3);

                Portiniandconvertnumber.objectserialconstantrefresh.Write(byteArray, 0, byteArray.Length);
                Portiniandconvertnumber.objectserialconstantrefresh.Write(byteArray1, 0, byteArray1.Length);
                Thread.Sleep(5000);
                Portiniandconvertnumber.objectserialconstantrefresh.Write(byteArray2, 0, byteArray2.Length);
                Portiniandconvertnumber.objectserialconstantrefresh.Write(byteArray3, 0, byteArray3.Length);
            }
        }
    }
}
