using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Reflection;
using System.Diagnostics;

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
        public static string osuDirectory;

        int progressBar = 0;

        static void Main(string[] args)
        {

            osuDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "/osu!/Songs";

            Arguments help = new Arguments("help", "-h", "--help");
            Arguments query = new Arguments("query", "-q", "--query");
            Arguments dl = new Arguments("download", "-d", "--download");

            //foreach (string _args in args) Console.WriteLine("{0}\t", _args);

            if(args.Length > 0)
            {
                Arguments a = Arguments.CheckArgsFromIdentifier(args[0].ToLower());

                if (a == Arguments.GetArgsByName("help")) {
                    DisplayHelp();
                } else if(a == Arguments.GetArgsByName("query"))
                {
                    DisplayBeatmaps();
                } else if (a == Arguments.GetArgsByName("download"))
                {
                    if(args.Length > 1)
                    {
                        //Check if string is a .txt file
                        if(File.Exists(args[1]))
                        {
                            String[] beatmapList = File.ReadAllLines(args[1]);
                            foreach (string s in beatmapList) {
                                String[] tmp = s.Split(' ');

                                using(WebClient client = new WebClient())
                                {

                                    string beatmap = osuBeatmapUrl + tmp[0];
                                    Process.Start(beatmap);
                                }
                            }
                        } else
                        {
                            //Search for osu beatmaps
                        }
                    } else
                    {
                        Console.WriteLine("No beatmap was found..");
                    }
                } else
                {
                    Console.WriteLine("The command \"{0}\" does not exists.", args[0]);
                    Console.WriteLine("Type -h / -help to display all the commands available");
                }
            } else
            {
                DisplayAbout();
            }
        }

        public static void DisplayAbout()
        {
            Console.WriteLine("Welcome to 'osuppkg'");
            Console.WriteLine("Type -h / -help to display all the commands available");
        }

        public static void DisplayHelp()
        {
            Console.WriteLine("Welcome to 'osuppkg'");
            Console.WriteLine("All the commands available: ");
        }

        public static void DisplayBeatmaps()
        {
            string[] s = Directory.GetDirectories(osuDirectory);
                foreach(string str in s) {
                    string songdir = "Local/osu!/Songs\\";
                    int index = str.IndexOf(songdir);
                    Console.WriteLine("{0}", str.Substring(index + songdir.Length));
                }
        }
    }
}
