using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YARAV
{
    class realTime
    {
        public static DriveInfo[] allDrives = DriveInfo.GetDrives();

        public static void loadDriveList()
        {
            foreach (DriveInfo info in allDrives)
                globals.driveList.Add(info.Name);
        }


        public static void startWatcher()
        {
            foreach (var drive in globals.driveList)
            {
                //Console.WriteLine("Drive is {0}", drive.ToString());

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    try
                    {


                        FileSystemWatcher watcher = new FileSystemWatcher();

                        watcher.Path = drive.ToString();

                        watcher.IncludeSubdirectories = true;
                        watcher.EnableRaisingEvents = true;
                        watcher.InternalBufferSize = 64 * 1024;

                        watcher.Filter = "*.*";

                        watcher.Created += new FileSystemEventHandler(onFile);
                        watcher.Changed += new FileSystemEventHandler(onFile);
                        watcher.Deleted += new FileSystemEventHandler(onFile);
                        watcher.Renamed += onFile;
                        watcher.Error += onError;


                    }
                    catch (System.AccessViolationException)
                    {
                        Console.WriteLine("File Not Accessible");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Directory {0} Not Accessible", drive);
                    }
                }).Start();


            }
        }

        private static void onFile(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (globals.printFiles == true)
                {
                    Console.WriteLine("-----------------");
                    Console.WriteLine("New File Created.");
                    Console.WriteLine("Name: {0}", e.Name.ToString());
                    Console.WriteLine("Path: {0}", e.FullPath.ToString());
                    Console.WriteLine("-----------------");
                }
                YARASCANNER.scan(e.FullPath);
            }
            catch (System.AccessViolationException)
            {
                Console.WriteLine("File {0} Not Accessible", e.FullPath.ToString());
            }
            catch (Exception)
            {

            }
        }
        private static void onError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
