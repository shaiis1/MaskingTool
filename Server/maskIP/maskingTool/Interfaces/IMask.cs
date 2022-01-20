using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maskingTool.Interfaces
{
     public interface IMask
    {
        public string MaskAllIPs(string regexPattern, string newString = "");
    }
}
