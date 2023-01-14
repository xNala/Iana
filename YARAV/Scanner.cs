using libyaraNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YARAV
{
    class YARASCANNER
    {
        public static void scan(string filePath, ScanFlags flags = ScanFlags.None)
        {

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                string tempPath = Path.GetRandomFileName();

                /*
                using (var ctx = new YaraContext())
                {
                    try
                    {
                        List<ScanResult> results = new List<ScanResult>();


                        try
                        {
                            var scanner = new Scanner();
                            results = scanner.ScanFile(filePath, globals.rules);
                        }
                        catch (System.AccessViolationException)
                        {
                            Console.WriteLine("File Not Accessible");
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("File Not Found");
                        }
                        catch (Exception)
                        {

                        }
                        // Scanner and ScanResults do not need to be disposed.

                        foreach (var value in results)
                        {
                            Console.WriteLine("-----------------");
                            Console.WriteLine("Rule: {0}", value.MatchingRule.Identifier);
                            Console.WriteLine("Path: {0}", filePath);
                            Console.WriteLine("-----------------");
                        }
                    }
                    catch (Exception) { }
                }
                */



                try
                {
                    List<ScanResult> results = new List<ScanResult>();

                    try
                    {
                        results = globals.scanner.ScanFile(filePath, globals.rules, flags);
                    }
                    catch (System.AccessViolationException)
                    {
                        Console.WriteLine("File Not Accessible");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("File Not Found");
                    }
                    catch (Exception)
                    {

                    }

                    foreach (var value in results)
                    {
                        Console.WriteLine("-----------------");
                        Console.WriteLine("Rule: {0}", value.MatchingRule.Identifier);
                        Console.WriteLine("Path: {0}", filePath);
                        Console.WriteLine("-----------------");
                    }

                }
                catch (System.AccessViolationException)
                {
                    Console.WriteLine("File Not Accessible");
                }



                //File.Delete(tempPath);

            }).Start();

        }
    }
}
