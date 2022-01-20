using maskingTool;
using maskingTool.Utils;
using System;
using System.IO;

namespace maskIP
{
    class FileManager
    {
        private string filePath;
        private string readedText = "";
        private const int maxFileLength = 5000000;

        public FileManager(string path)
        {
            filePath = path;
            readFromFile();
        }

        public void CreateNewFileAfterIpMask()
        {
            try
            {
                Mask mask = new Mask();
                var maskedIP = mask.MaskAllIPs(readedText, Regex.IPV4_RGX);
                string outputPath = getOutputPathString(filePath);
                File.WriteAllText(outputPath, maskedIP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region private methods
        private void readFromFile()
        {
            try
            {
                filePath = removeApostrophes(filePath);
                if (File.Exists(filePath))
                {
                    if (isFileSizeValid() && isFileExtensionValid()) //check if input is valid
                        readedText = File.ReadAllText(filePath);
                }
                else
                    Console.WriteLine("File Doesn't exist");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string removeApostrophes(string str)
        {
            if (checkIfStringInsideApostrophes(str))
                str = str.Substring(1, str.Length - 2);
            return str;
        }

        private bool checkIfStringInsideApostrophes(string str)
        {
            return str[0] == '"' || str[str.Length - 1] == '"';
        }

        private bool isFileSizeValid()
        {
            long fileSize = new FileInfo(filePath).Length;
            if (fileSize <= maxFileLength)
                return true;
            throw new Exception("File size is too big. The max size is " + maxFileLength + " (Bytes)");
        }

        private bool isFileExtensionValid()
        {
            if (Path.GetExtension(filePath) == ".log")
                return true;
            throw new Exception("Invalid file input: we support only .log files");
        }

        private string getOutputPathString(string fullPath, string outputFileName = "MaskedFile", bool combineToOriginalFileName = false)
        {
            try
            {
                string path;
                if (combineToOriginalFileName)
                {
                    path = Path.GetFileNameWithoutExtension(fullPath);
                }
                else
                {
                    path = Directory.GetParent(fullPath).ToString();
                }

                path = Path.Combine(path, outputFileName);
                return Path.ChangeExtension(path, ".log");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            throw new Exception("ERROR IN GetOutputPathString");
        }

        #endregion
    }
}
