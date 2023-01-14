using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using libyaraNET;

namespace YARAV
{
    class globals
    {
        public static List<string> driveList = new List<string>();

        public static bool printFiles = false;

        public static string rulesFile = @"C:\Users\Owner\source\repos\YARAV\YARAV\rules.yara";
        public static string rulesString = "";


        //YARA Functions
        public static YaraContext ctx = new YaraContext();
        public static Rules rules = null;
        public static Compiler compiler = new Compiler();
        public static Scanner scanner = new Scanner();

        
    }
    public class RecurseFileStructure
    {

        
        public void TraverseDirectory(DirectoryInfo directoryInfo)
        {
            try
            {
                var subdirectories = directoryInfo.EnumerateDirectories();

                foreach (var subdirectory in subdirectories)
                {
                    TraverseDirectory(subdirectory);
                }

                var files = directoryInfo.EnumerateFiles();

                foreach (var file in files)
                {
                    HandleFile(file);
                }
            }
            catch (UnauthorizedAccessException ex)
            {

            }
            catch (IOException)
            {

            }
            catch (Exception)
            { 

            }

        }

        public static void ParallelLoopThroughDirectories(string root)
        {
            var directories = new Queue<string>();
            directories.Enqueue(root);

            Parallel.ForEach(directories, directoryPath =>
            {
                try
                {
                    var subDirectories = Directory.GetDirectories(directoryPath);
                    foreach (var subDirectory in subDirectories)
                    {
                        //Console.WriteLine(subDirectory);
                        ParallelLoopThroughDirectories(subDirectory);
                        //directories.Enqueue(subDirectory);
                    }

                    // Perform processing on files in the current directory
                    var files = Directory.GetFiles(directoryPath);
                    foreach (var file in files)
                    {
                        //Console.WriteLine(file);
                        YARASCANNER.scan(file, ScanFlags.Fast);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // handle exception
                }
            });
        }





        void HandleFile(FileInfo file)
        {
            //Console.WriteLine("{0}", file.Name);
            YARASCANNER.scan(file.FullName);
        }
    }


    class scanner
    {
        
        public static void loadRules()
        {
            globals.compiler.AddRuleFile(globals.rulesFile);
            globals.rules = globals.compiler.GetRules();
        }



        public static void systemScan()
        {

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                DriveInfo[] allDrives = DriveInfo.GetDrives();

                Console.WriteLine("Start: " + DateTime.Now);

                foreach (DriveInfo drive in allDrives)
                {

                    /*
                    DirectoryInfo startDir = new DirectoryInfo(info.Name);
                    Console.WriteLine(info.Name); ;
                    RecurseFileStructure recurseFileStructure = new RecurseFileStructure();
                    recurseFileStructure.TraverseDirectory(startDir);
                    */


                    try
                    {
                        RecurseFileStructure.ParallelLoopThroughDirectories(drive.Name);
                    }
                    catch (Exception) { }

                }
                

                /*
                Parallel.ForEach(allDrives, drive =>
                {
                    //DirectoryInfo startDir = new DirectoryInfo(drive.Name);
                    Console.WriteLine(drive.Name); ;
                    //RecurseFileStructure recurseFileStructure = new RecurseFileStructure();
                    try
                    {
                        RecurseFileStructure.ParallelLoopThroughDirectories(drive.Name);
                    }
                    catch (Exception) { }

                });
                */
                Console.WriteLine("End:   " + DateTime.Now);

            }).Start();
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            realTime.loadDriveList();

            globals.rulesString = File.ReadAllText(globals.rulesFile);
            scanner.loadRules();

            scanner.systemScan();

            //realTime.startWatcher();

            while (true)
            {
                Thread.Sleep(1000);
                Console.ReadLine();
            }
        }
    }
}
