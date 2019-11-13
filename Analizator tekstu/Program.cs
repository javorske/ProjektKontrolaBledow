using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Analizator_tekstu
{
    class Program
    {
        /// <summary>
        /// The url path to file.
        /// </summary>
        public static string urlPath { get; set; } = "https://s3.zylowski.net/public/input/1.txt";
        /// <summary>
        /// The name of file.
        /// </summary>
        public static string filePath { get; set; } = "File.txt";
        static void Main(string[] args)
        {
            int option; //it needs to be assigned from the beginnig
            bool checkForExit = true;
            do // checking if user gave correct input
            {
                // menu displayed in console
                Console.Clear();
                Console.WriteLine("1. Pobierz plik z internetu.");
                Console.WriteLine("2. Zlicz liczbę liter w pobranym pliku.");
                Console.WriteLine("3. Zlicz liczbę wyrazów w pliku.");
                Console.WriteLine("4. Zlicz liczbę znaków interpunkcyjnych w pliku.");
                Console.WriteLine("5. Zlicz liczbę zdań w pliku.");
                Console.WriteLine("6. Wygeneruj raport o użyciu liter (A-Z)");
                Console.WriteLine("7. Zapisz statystyki z punktów 2-5 do pliku statystyki.txt.");
                Console.WriteLine("8. Wyjście z programu.");
                try
                {
                    option = int.Parse(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            GetFileFromInternet(urlPath, filePath);
                            break;
                        case 2:
                            getNumberOfLetters();
                            break;
                        case 3:
                            CountNumberOfWords(filePath);
                            break;
                        case 4:
                            CountNumberOfPunctationMarks(filePath);
                            break;
                        case 5:
                            CountNumberOfSentences(filePath);
                            break;
                        case 6:
                            break;
                        case 7:
                            break;
                        case 8: //  exit from program
                            checkForExit = false;
                            break;
                    }
                }
                    catch (FormatException)
                {
                    // checking if exception occured
                    Console.WriteLine("Nie wybrano zadnej opcji, badz podano zly znak.");
                    waitForUser();
                }
            } while (checkForExit);
        }
        /// <summary>
        /// Download and save file form given url to given file name
        /// </summary>
        /// <param name="urlPath">Url path to file.</param>
        /// <param name="fileName">File name.</param>
        public static void GetFileFromInternet(string urlPath, string fileName)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(urlPath, fileName);
            }
            catch (WebException err)
            {
                Console.WriteLine(string.Format("Error code:{0}", err));
            }
        }
        public static void getNumberOfLetters()
        {
            try
            {
                string textFromFile = System.IO.File.ReadAllText("File.txt");
                Console.WriteLine("This file contains: " + textFromFile.Count(char.IsLetter) + " letters");
            }
            catch (FileNotFoundException)
            {   // in case the file has not been downloaded/does not exist
                Console.WriteLine("File does not exist.");
            }
            waitForUser();
        }
        public static void waitForUser()
        {
            Console.ReadKey();
            Console.Clear();
        }
        /// <summary>
        /// Count number of words in given file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public static void CountNumberOfWords(string fileName)
        {
            try
            {
                string[] file = File.ReadAllText(fileName).Split(' ');
                Console.WriteLine(string.Format("Liczba słow wynosi:{0}", file.Where(x => Regex.IsMatch(x, "[a-z]", RegexOptions.IgnoreCase)).Count()));
            }
            catch (FileNotFoundException err)
            {
                Console.WriteLine(string.Format("Error code:/n{0}", err));

            }

            waitForUser();
        }
        /// <summary>
        /// Count number of punctation marks in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static void CountNumberOfPunctationMarks(string fileName)
        {
            try
            {
                Console.WriteLine(string.Format("Liczba znakow interpunkcyjnych wynosi: {0}", (Regex.Matches(File.ReadAllText(fileName), @"[\p{P}]").Count)));
                Console.ReadKey();
            }
            catch (FileNotFoundException)
            {
                // in case the file has not been downloaded/does not exist
                Console.WriteLine("File does not exist.");
            }
        }
        /// <summary>
        /// Count number of sentences in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static void CountNumberOfSentences(string fileName)
        {
            try
            {
                Console.WriteLine(string.Format("Liczba zdan wynosi: {0}", (Regex.Matches(File.ReadAllText(fileName), @"(?<=[.!?])\s?([A-Z]?)").Count)));
                Console.ReadKey();
            }
            catch (FileNotFoundException)
            {
                // in case the file has not been downloaded/does not exist
                Console.WriteLine("File does not exist.");
            }
        }
    }
}