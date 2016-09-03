using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

/*
 *  Right now the code goes my sequential order, thinking of somehow making it modular
 * 
 * 
 */

namespace osuppkg
{
    class Program
    {

        //public const string osuBeatmapUrl = "https://osu.ppy.sh/d/";
        public const string osuBeatmapUrl = "http://bloodcat.com/osu/s/";
        public const string osuBeatmapConfigFileName = "osu.conf";
        public static string osuDirectory;
        public static string tmpDirectory;

        static void Main(string[] args)
        {

            WebRequest.DefaultWebProxy = null;
            ServicePointManager.DefaultConnectionLimit = 100;
            
            if(File.Exists(Directory.GetCurrentDirectory() + "\\" + osuBeatmapConfigFileName))
            {
                string[] confs = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\" + osuBeatmapConfigFileName);
                
                foreach(string s in confs)
                {
                    int i;
                    if((i = s.IndexOf("InstallLocation=")) >= 0)
                    {
                        osuDirectory = s.Substring("InstallLocation=".Length);
                    }
                }

                //Defaults to default osu install folder
                if(osuDirectory == null)
                {
                    osuDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "/osu!/Songs";
                }

            } else {

                Console.WriteLine("Please enter your osu! install location, leave it empty if it is at the default install location.");
                Console.Write("Location: ");
                osuDirectory = Console.ReadLine();

                if(osuDirectory == "")
                {
                    osuDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "/osu!/Songs";
                }

                StreamWriter sr = File.CreateText(Directory.GetCurrentDirectory() + "\\" + osuBeatmapConfigFileName);
                sr.WriteLine("InstallLocation=" + osuDirectory);
                sr.Close();

                try
                {
                    Console.Clear();
                } catch (Exception ex)
                {

                }
            }
                
            Console.WriteLine("Welcome to 'osuppkg'\n");


            tmpDirectory = Directory.GetCurrentDirectory() + "\\tmp\\";

            Arguments help = new Arguments("help", "-h", "--help");
            Arguments query = new Arguments("query", "-q", "--query");
            Arguments dl = new Arguments("download", "-d", "--download");

            if (args.Length > 0)
            {
                Arguments a = Arguments.CheckArgsFromIdentifier(args[0].ToLower());

                if (a == Arguments.GetArgsByName("help"))
                {
                    DisplayHelp();
                }
                else if (a == Arguments.GetArgsByName("query"))
                {
                    DisplayBeatmaps();
                }
                else if (a == Arguments.GetArgsByName("download")) //Needs rework
                {

                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\tmp\\");

                    //TODO: Need to rewrite code
                    //Starting from here
                    //if (args.Length > 1)
                    //{
                    //    //Check if string is a .txt file
                    //    if (File.Exists(args[1]))
                    //    {
                    //        String[] beatmapList = File.ReadAllLines(args[1]);
                    //        foreach (string s in beatmapList)
                    //        {
                    //            String[] tmp = s.Split(' ');
                    //            string beatmap = osuBeatmapUrl + tmp[0];
                    //            Process.Start(beatmap);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //Search for osu beatmaps
                    //    }
                    //}
                    //else
                    //{
                    //    Console.WriteLine("No beatmap was found..");
                    //}
                    //TODO: Need to rewrite code
                    //Ends here
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (File.Exists(args[i]))
                        {
                            String[] beatmapList = File.ReadAllLines(args[1]);
                            foreach (string s in beatmapList)
                            {
                                String[] tmp = s.Split(' ');
                                string beatmap = osuBeatmapUrl + tmp[0];
                                //Process.Start(beatmap); //DANGEROUS AF

                                Console.Write("Downloading '{0}'\t", beatmap);

                                for(int j = 1; j < tmp.Length; j++)
                                {
                                    Console.Write(tmp[j]);
                                }

                                Console.WriteLine();

                                //using (var progress = new ProgressBar())
                                //{
                                //    for (int j = 0; j <= 100; j++)
                                //    {
                                //        progress.Report((double)j / 100);
                                //        Thread.Sleep(20);
                                //    }
                                //}

                                using(var client = new WebClient())
                                {
                                    client.DownloadFile(beatmap, tmpDirectory + s + ".osz");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The command \"{0}\" does not exists.", args[0]);
                    Console.WriteLine("Type -h / -help to display all the commands available");
                }
            }
            else
            {
                DisplayAbout();
            }
        }

        public static void DisplayAbout()
        {
            Console.WriteLine("Type -h / -help to display all the commands available");
        }

        public static void DisplayHelp()
        {
            Console.WriteLine("All the commands available: ");
            Console.WriteLine("-h or --help\tDisplays available commands");
            Console.WriteLine("-q or --query\tLists all beatmaps installed");
            Console.WriteLine("-d or --download\tDownloads the beatmap using the list of beatmaps provided/specific beatmap (not working at the moment)");
        }

        public static void DisplayBeatmaps()
        {
            string[] s = Directory.GetDirectories(osuDirectory);
            foreach (string str in s)
            {
                string songdir = "Local/osu!/Songs\\";
                int index = str.IndexOf(songdir);
                Console.WriteLine("{0}", str.Substring(index + songdir.Length));
            }
        }
    }
}
