using maskingTool.Interfaces;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace maskingTool
{
    public class Mask : IMask
    {
        private MaskIP maskIp = new MaskIP();

        public Mask(){}

        public string MaskAllIPs(string originalText, string regexPattern)
        {
            var newText = "";
            if (originalText == string.Empty)
            {
                Console.WriteLine("File text is empty.");
                return "";
            }
            Regex regex = new Regex(regexPattern);
            StringBuilder stringBuilder = new StringBuilder(originalText);
            if (regex.IsMatch(originalText))
            {
                MatchCollection matchCollection = regex.Matches(originalText);
                foreach (Match match in matchCollection)
                {

                    string matchValue = match.Value;
                    if (isIpValid(matchValue)) //check if match is a valid ip
                    {
                        string maskedIp = maskIp.GetMaskedIp(matchValue);
                        maskedIp = matchValue[0] + maskedIp + matchValue[matchValue.Length - 1]; //string to keep everything as it was
                        stringBuilder.Replace(match.Value, maskedIp);
                        newText = stringBuilder.ToString();
                    }
                }
                Console.WriteLine("IP addresses replaced");
            }
            return newText;
        }

        #region private methods
        private bool isIpValid(string ip)
        {
            string[] parts = ip.Split('.');
            int octetValue; // integer value of string part
            foreach (string part in parts)
            {
                octetValue = int.Parse(part);
                if (octetValue > 255)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
