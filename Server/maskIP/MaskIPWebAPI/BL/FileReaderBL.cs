using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MaskIPWebAPI.BL
{
    public static class FileReaderBL
    {
        public static string GetStringWithoutApostrophes(string text)
        {
            if (checkIfStringInsideApostrophes(text))
                text = text.Substring(1, text.Length - 2);
            return text;
        }

        private static bool checkIfStringInsideApostrophes(string str)
        {
            return str[0] == '"' || str[str.Length - 1] == '"';
        }
    }
}
