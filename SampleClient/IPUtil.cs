using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace AreaParty.util
{
    class IPUtil
    {
        //public static string GetInternalIP()
        //{
        //    string ip = util.config.GetIPAddress.GetActivatedAdaptorMacAddress();
        //    if (!ip.Equals(""))
        //    {
        //        return ip;
        //    }
        //    //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

        //    //ManagementObjectCollection moc = mc.GetInstances();
        //    //string MACAddress = String.Empty;
        //    //foreach (ManagementObject mo in moc)
        //    //{
        //    //    if ((bool)mo["IPEnabled"] == true)
        //    //    {
        //    //        return ((String[])mo["IPAddress"])[0];
        //    //    }
        //    //}
        //    return "127.0.0.1";
        //}
        //public static string GetInternalMAC()
        //{
        //    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

        //    ManagementObjectCollection moc = mc.GetInstances();
        //    string MACAddress = String.Empty;
        //    foreach (ManagementObject mo in moc)
        //    {
        //        if ((bool)mo["IPEnabled"] == true)
        //        {
        //            return ((String)mo["MacAddress"]);
        //        }
        //    }
        //    return "";
        //}

        public static IPAddress[] DoGetHostAddresses(string hostname)
        {
            IPAddress[] ips;

            ips = Dns.GetHostAddresses(hostname);

            Console.WriteLine("GetHostAddresses({0}) returns:", hostname);

            Console.WriteLine(ips[0].ToString());

            return ips;
        }
    }
}
