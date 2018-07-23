using BLL;
using System;

namespace C_Sharp_Project_with_DB
{
    class Program
    {
        /// <summary>
        /// Functions that displays an information on console application
        /// </summary> 
        #region Print functions

        /// <summary>
        /// DisplayFileName displays FileNames in console application
        /// </summary>
        /// <param name="fileName"></param>
        public static void DisplayFileName(string fileName)
        {
            WriteInColor("Found:" + fileName, ConsoleColor.Cyan);
        }


        /// <summary>
        /// DisplayError displays an error in console application
        /// </summary>
        /// <param name="error"></param>

        public static void DisplayError(string error)
        {
            WriteInColor($"Ooops... {error}", ConsoleColor.DarkRed);
        }

        /// <summary>
        /// DisplayFinalMessage displays a message when the scan is finished
        /// </summary>
        /// <param name="msg"></param>

        public static void DisplayFinalMessage(string msg)
        {
            Console.WriteLine("---------------------------------------------");
            WriteInColor(msg, ConsoleColor.Green);
            Console.WriteLine("---------------------------------------------");
        }



        /// <summary>
        /// WriteInColor displays meseseags in a different colors
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="color"></param>
        
        public static void WriteInColor(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
            Console.ResetColor();
        }

        #endregion

        static void Main(string[] args)
        {
            FileSearchManager.SearchHandler += DisplayFileName;
            FileSearchManager.ErrorHandler += DisplayError;
            FileSearchManager.ScanFinishedHandler += DisplayFinalMessage;


            bool quit = false;
            string searchStr, searchDrct;

            #region Menu in Console Application

            while (!quit)
            {
                Console.WriteLine("1. Enter filename to search.");
                Console.WriteLine("2. Enter filename to search + parent directory to search in.");
                Console.WriteLine("3. Exit");

                string menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        Console.WriteLine("Enter file name to search: ");
                        searchStr = Console.ReadLine();
                        Console.WriteLine("Searching ...");

                        FileSearchManager.StartNewSearch(searchStr);

                        WriteInColor("Press any key to start search again...", ConsoleColor.DarkYellow);

                        break;

                    case "2":
                        Console.WriteLine("Enter file name to search: ");
                        searchStr = Console.ReadLine();

                        Console.WriteLine("Enter root directory to search in: ");
                        searchDrct = Console.ReadLine();

                        Console.WriteLine("Searching ...");

                        FileSearchManager.StartNewSearch(searchStr, searchDrct);
                        WriteInColor("Press any key to start search again...", ConsoleColor.DarkYellow);

                        break;

                    case "3":
                        quit = true;
                        break;

                    default:
                        WriteInColor("Error. Please enter only 1, 2 or 3", ConsoleColor.Red);
                        WriteInColor("Press any key to start search again...", ConsoleColor.DarkYellow);
                        break;
                }

                Console.ReadKey();
                Console.Clear();

            }

            #endregion

            Console.ReadKey();

        }
    }
}
