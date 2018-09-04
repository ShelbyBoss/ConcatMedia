using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcatMedia
{
    class Program
    {
        private const string ffmpegFileName = "ffmpeg.exe", outputAddition = "_concat";

        static void Main(string[] args)
        {
            if (args.Length < 2) return;

            try
            {
                string[] list = args.Select(a => "file '" + a + "'").ToArray();
                string listPath = GetFullPath(Path.GetRandomFileName());

                string ffmpegPath = GetFullPath(ffmpegFileName);
                string outputPath = AddToPath(args[0], outputAddition);
                string batchCmd = string.Format("\"{0}\" -f concat -safe 0 -i \"{1}\" -c copy \"{2}\"", ffmpegPath, listPath, outputPath);
                string batchPath = GetFullPath(Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".bat");

                File.WriteAllLines(listPath, list);
                File.WriteAllText(batchPath, batchCmd);

                ProcessStartInfo processInfo = new ProcessStartInfo(batchPath);
                processInfo.UseShellExecute = false;

                Process process = Process.Start(processInfo);

                process.WaitForExit();

                Delete(listPath);
                Delete(batchPath);

                Console.WriteLine();
                Console.Write("Drücken Sie eine beliebige Taste . . .");
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Console.WriteLine(e.GetType());
                    Console.WriteLine(e.Message);
                    Console.WriteLine();

                    e = e.InnerException;
                }

                Console.ReadLine();
            }
        }

        private static string GetFullPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        private static string AddToPath(string path, string addition)
        {
            string fileNameWithoutExtenion = Path.GetFileNameWithoutExtension(path);
            string extention = Path.GetExtension(path);
            string directory = path.Remove(path.Length - fileNameWithoutExtenion.Length - extention.Length);

            return directory + fileNameWithoutExtenion + addition + extention;
        }

        private static void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Console.WriteLine(e.GetType());
                    Console.WriteLine(e.Message);
                    Console.WriteLine();

                    e = e.InnerException;
                }

                Console.ReadLine();
                return;
            }
        }
    }
}
