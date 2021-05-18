using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace UnityABExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2 || Directory.Exists(args[0]) == false)
                throw new Exception("argError: arg[0] = WebExtract & binary2text root path.");

            string[] filePaths;
            if (Directory.Exists(args[1]))
            {
                if (args.Length > 2)
                {
                    SearchOption searchOption = SearchOption.TopDirectoryOnly;
                    if (args.Length > 4)
                    {
                        searchOption = SearchOption.AllDirectories;
                    }
                    filePaths = Directory.GetFiles(args[1], args[2], searchOption);
                }
                else
                {
                    filePaths = Directory.GetFiles(args[1]);
                }
            }
            else
            {
                if (File.Exists(args[1]))
                {
                    filePaths = new string[1] { args[1] };
                }
                else
                {
                    throw new Exception("argError: arg[1] = file or folder path exists.");
                }
            }


            Init();


            string webExtract_Exe_Path = args[0] + "/WebExtract.exe";
            string binary2text_Exe_Path = args[0] + "/binary2text.exe";

            foreach (var filePath in filePaths)
            {

                CallProcess(webExtract_Exe_Path, filePath);

                string extractDataPath = filePath + "_data";
                var directoryInfo = Directory.CreateDirectory(extractDataPath);
                if (directoryInfo.Exists)
                {
                    CallProcess(binary2text_Exe_Path, directoryInfo.GetFiles()[0].FullName);
                }
                else
                {
                    Console.WriteLine("extract error：" + extractDataPath);
                    continue;
                }
            }
        }


        private static System.Diagnostics.Process process;

        static void Init()
        {
            process = new System.Diagnostics.Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
        }
        static void CallProcess(string fileName, string arguments)
        {
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;

            process.Start();

            process.StandardInput.AutoFlush = true;

            process.WaitForExit();

            Console.WriteLine("-------------Log Start---------------");
            Console.WriteLine();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine("-----------  Log End  ---------------");

            process.Close();
        }
    }
}