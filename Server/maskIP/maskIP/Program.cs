using System;

namespace maskIP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("******* Welcome To MaskIP Tool!!! *******\nPlease Enter Your file path or drag file into the console window!");
            string userInput = Console.ReadLine();
            FileManager fileManager = new FileManager(userInput);
            fileManager.CreateNewFileAfterIpMask();
        }
    }
}
