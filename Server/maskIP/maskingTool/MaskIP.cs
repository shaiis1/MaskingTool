using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maskingTool
{
    public class MaskIP
    {
        private Dictionary<string, string> maskedNetworkDictionary = new Dictionary<string, string>();
        private Dictionary<string, string> maskedHostedAddressDictionary = new Dictionary<string, string>();

        public MaskIP(){}

        public string GetMaskedIp(string origIPAddress)
        {
            return createdMaskedIPAddress(origIPAddress);
        }

        #region private methods
        private string createdMaskedIPAddress(string origIPAddress)
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            string networkAddress = getNetworkAddress(origIPAddress);
            string maskedNetworkAddress = "";
            string maskedHostedAddress = "";
            if (isNetworkAddressAlreadyMasked(networkAddress))
            {
                maskedNetworkAddress = maskedNetworkDictionary[networkAddress];
            }
            else // mask network address
            {
                maskedNetworkAddress = createMaskedIp(networkAddress);
                maskedNetworkDictionary[networkAddress] = maskedNetworkAddress;
            }
            stringBuilder.Append(maskedNetworkAddress); // appeand the network mask
            string hostAddressTypeC = origIPAddress.Split('.')[3];
            var splittedhostAddressTypeC = getStringTuple(hostAddressTypeC);
            if (isHostedAddressAlreadyMasked(splittedhostAddressTypeC.Item1))
            {
                maskedHostedAddress = maskedHostedAddressDictionary[splittedhostAddressTypeC.Item1];
            }
            else //mask hosted address
            {
                maskedHostedAddress = createMaskedIp(splittedhostAddressTypeC.Item1);
                maskedHostedAddressDictionary[splittedhostAddressTypeC.Item1] = maskedHostedAddress;
            }
            stringBuilder.Append(".");
            stringBuilder.Append(maskedHostedAddress + splittedhostAddressTypeC.Item2);//append hosted mask and last digits, e.g: '\n' || '\r'
            return stringBuilder.ToString();
        }

        private string createMaskedIp(string ip)
        {

            return buildStringWithBuffer(ip, '.', true);
        }

        private string buildStringWithBuffer(string ip, char bufferChar, bool toMask = false)
        {
            string[] octets = ip.Split('.');
            StringBuilder stringBuilder = new StringBuilder(String.Empty);

            for (int i = 0; i < octets.Length; i++)
            {
                if (isToAddBuffer(i))
                    stringBuilder.Append(bufferChar.ToString());
                if (toMask)// check if need to mask IP
                {
                    Random random = new Random();
                    //calculate new masked value
                    octets[i] = ((int.Parse(octets[i]) + random.Next(0, 256)) % 256).ToString();
                }
                stringBuilder.Append(int.Parse(octets[i]).ToString());
            }
            return stringBuilder.ToString();

        }

        private string getNetworkAddress(string fullIpAddress, int networkClass = 2)
        
        {
            string[] ipParts = fullIpAddress.Split('.');
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            for (int i = 0; i <= networkClass; i++)
            {
                if (isToAddBuffer(i))
                    stringBuilder.Append(".");
                stringBuilder.Append(ipParts[i]);
            }
            return stringBuilder.ToString();
        }

        private bool isNetworkAddressAlreadyMasked(string networkAddress)
        {
            return (maskedNetworkDictionary.ContainsKey(networkAddress));
        }

        private bool isHostedAddressAlreadyMasked(string hostedAddress)
        {
            return (maskedHostedAddressDictionary.ContainsKey(hostedAddress));
        }

        private bool isToAddBuffer(int partIndex)
        {
            return partIndex > 0;
        }

        private Tuple<string, string> getStringTuple(string input) /* to split digits and chars of the last octet */
        {
            string digitsString = "";
            string notDigits = "";
            foreach(var ch in input)
            {
                if (char.IsDigit(ch))
                {
                    digitsString += ch.ToString();
                }
                else
                {
                    notDigits += ch.ToString();
                }
            }
            return new Tuple<string, string>(digitsString, notDigits);
        }
        #endregion
    }
}
