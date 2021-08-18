using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace TiqUtils.Utils
{
    public class HwId
    {
        private static string CpuId()
        {
            var moc = GetMoC("Win32_Processor");
            //Uses first CPU identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            var retVal = Identifier(moc, "UniqueId");
            if (retVal != "") return retVal;
            retVal = Identifier(moc, "ProcessorId");
            if (retVal != "") return retVal;
            retVal = Identifier(moc, "Name");
            if (retVal == "") //If no Name, use Manufacturer
            {
                retVal = Identifier(moc, "Manufacturer");
            }
            //Add clock speed for extra security
            retVal += Identifier(moc, "MaxClockSpeed");
            return retVal;
        }

        private static string Identifier(ManagementObjectCollection moc, string wmiProperty, string wmiMustBeTrue)
        {
            var result = "";
            foreach (var mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() != "True") continue;
                //Only get the first one
                if (result != "") continue;
                try
                {
                    result = mo[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
            return result;
        }

        private static ManagementObjectCollection GetMoC(string wmiClass)
        {
            var mc = new System.Management.ManagementClass(wmiClass);
            var moc = mc.GetInstances();
            return moc;
        }

        private static string _fingerPrint = string.Empty;

        public static string Value()
        {
            //You don't need to generate the HWID again if it has already been generated. This is better for performance
            //Also, your HWID generally doesn't change when your computer is turned on but it can happen.
            //It's up to you if you want to keep generating a HWID or not if the function is called.
            if (string.IsNullOrEmpty(_fingerPrint))
            {
                _fingerPrint = GetHash("CPU >> " + CpuId() + "\nBASE >> " + BaseId());
            }
            return _fingerPrint;
        }

        public static string GetHash(string s)
        {
            //Initialize a new MD5 Crypto Service Provider in order to generate a hash
            MD5 sec = new MD5CryptoServiceProvider();
            //Grab the bytes of the variable 's'
            var bt = Encoding.ASCII.GetBytes(s);
            //Grab the Hexadecimal value of the MD5 hash
            return GetHexString(sec.ComputeHash(bt));
        }

        private static string GetHexString(IList<byte> bt)
        {
            var s = string.Empty;
            for (var i = 0; i < bt.Count; i++)
            {
                var b = bt[i];
                int n = b;
                var n1 = n & 15;
                var n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n2.ToString(CultureInfo.InvariantCulture);
                if (n1 > 9)
                    s += ((char)(n1 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n1.ToString(CultureInfo.InvariantCulture);
                if ((i + 1) != bt.Count && (i + 1) % 2 == 0) s += "-";
            }
            return s;
        }

        private static string Identifier(ManagementObjectCollection moc, string wmiProperty)
        {
            var result = "";
            foreach (var mo in moc)
            {
                //Only get the first one
                if (result != "") continue;
                try
                {
                    var prop = mo[wmiProperty];
                    if (prop != null)
                    {
                        result = prop.ToString();
                        break;
                    }
                }
                catch
                {
                }
            }
            return result;
        }

        //BIOS Identifier
        private static string BiosId()
        {
            var moc = GetMoC("Win32_BIOS");
            return Identifier(moc, "Manufacturer") + Identifier(moc, "SMBIOSBIOSVersion") +
                   Identifier(moc, "IdentificationCode") + Identifier(moc, "SerialNumber") +
                   Identifier(moc, "ReleaseDate") + Identifier(moc, "Version");
        }

        private static string BaseId()
        {
            var moc = GetMoC("Win32_BaseBoard");
            return Identifier(moc, "Model") + Identifier(moc, "Manufacturer") +
                   Identifier(moc, "Name") + Identifier(moc, "SerialNumber");
        }

    }
}