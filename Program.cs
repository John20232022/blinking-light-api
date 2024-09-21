using Microsoft.Owin.Hosting;
using newblinkinglight;
using newblinkinglight.API.Service;
using Owin;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace testserial
{
    public static class Program
    {
        static void Main()
        {
            // tao object Serial port va chay tim cong COM moi cam
            newblinkinglight.Common.Portiniandconvertnumber.portinitialconstantchecking();
            StartService.Start();
        }
    }
}
