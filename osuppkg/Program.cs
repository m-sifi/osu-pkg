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

        public const string osuBeatmapUrl = "https://osu.ppy.sh/d/";
        //public const string osuBeatmapUrl = "http://bloodcat.com/osu/s/";
        public const string osuBeatmapConfigFileName = "osu.conf";
        public static string osuDirectory;
        public static string osuSongDirectory;

        static void Main(string[] args)
        {

            WebRequest.DefaultWebProxy = null;
            ServicePointManager.DefaultConnectionLimit = 100;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + osuBeatmapConfigFileName))
            {
                string[] confs = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\" + osuBeatmapConfigFileName);

                foreach (string s in confs)
                {
                    int i;
                    if ((i = s.IndexOf("InstallLocation=")) >= 0)
                    {
                        osuDirectory = s.Substring("InstallLocation=".Length);
                    }
                }

                //Defaults to default osu install folder
                if (osuDirectory == null)
                {
                    osuDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "/osu!/";
                }

            }
            else
            {

                Console.WriteLine("Please enter your osu! install location, leave it empty if it is at the default install location.");
                Console.Write("Location: ");
                osuDirectory = Console.ReadLine();

                if (osuDirectory == "")
                {
                    osuDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "/osu!/";
                }

                StreamWriter sr = File.CreateText(Directory.GetCurrentDirectory() + "\\" + osuBeatmapConfigFileName);
                sr.WriteLine("InstallLocation=" + osuDirectory);
                sr.Close();

                Console.Clear();
            }

            Console.WriteLine("Welcome to 'osuppkg'\n");

            osuSongDirectory = osuDirectory + "\\Songs\\";

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
                    if (args.Length > 1)
                    {
                        StreamWriter sw = File.CreateText(args[1]);
                        List<string> beatmaps = GetBeatmaps();
                        foreach (string song in beatmaps)
                        {
                            sw.WriteLine(song);
                        }
                    }
                }
                else if (a == Arguments.GetArgsByName("download")) //Needs rework
                {

                    List<string> beatmapURL = new List<string>();
                    int count = 0;

                    for (int i = 1; i < args.Length; i++)
                    {
                        if (File.Exists(args[i]))
                        {
                            List<string> beatmapList = GetIDFromList(File.ReadAllLines(args[i]).ToList<string>());
                            List<string> currentList = GetIDFromList(GetBeatmaps());

                            int duplicateCount = 0;
                            int originalListCount = beatmapList.Count;
                            foreach (string str in currentList)
                            {
                                if(beatmapList.Contains(str)) {
                                    duplicateCount++;
                                    beatmapList.Remove(str);
                                }
                            }

                            Console.WriteLine("There are {0} duplicates out of the {1} from the given list, removing for optimisations..", duplicateCount, originalListCount);

                            count = beatmapList.Count;

                            foreach (string s in beatmapList)
                            {
                                String[] tmp = s.Split(' ');
                                beatmapURL.Add(osuBeatmapUrl + tmp[0]);
                            }
                        }
                        
                        if(count > 0) {
                            string beatmapURLJS = "var beatmapURL = [ ";

                            for (int j = 0; j < beatmapURL.Count; j++)
                            {
                                beatmapURLJS += "'" + beatmapURL[j] + "'";

                                if (i != beatmapURL.Count - 1)
                                {
                                    beatmapURLJS += ", ";
                                }
                                else
                                {
                                    beatmapURLJS += " ];";
                                }
                            }

                            StreamWriter sw = File.CreateText(Directory.GetCurrentDirectory() + "\\html\\osuppkg.js");
                            sw.WriteLine(beatmapURLJS);

                            sw.WriteLine("");

                            sw.Close();

                            Console.WriteLine("Your browser will now attempt to download {0} beatmaps..", count);
                            Console.WriteLine("** Do not attempt to close any tabs while the script is running **");
                            Process.Start(Directory.GetCurrentDirectory() + "\\html\\index.html");
                        } else
                        {
                            Console.WriteLine("There is nothing to download. Exiting..");
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
            List<string> s = GetBeatmaps();
            foreach (string str in s)
            {
                Console.WriteLine("{0}", str);
            }
        }

        public static List<string> GetBeatmaps()
        {
            string[] s = Directory.GetDirectories(osuSongDirectory);
            List<string> result = new List<string>();

            foreach (string str in s)
            {
                int index = str.IndexOf(osuSongDirectory);
                result.Add(str.Substring(index + osuSongDirectory.Length));
            }

            return result;
        }

        public static List<string> GetIDFromList(List<string> list)
        {
            List<string> result = new List<string>();

            foreach (string str in list)
            {
                string s;
                s = str.Split(' ')[0];
                result.Add(s);
            }

            return result;
        }
    }
}
