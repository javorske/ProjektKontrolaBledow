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
        /// <summary>
        /// If downloading of file succeded, change value to true.
        /// </summary>
        public static bool checkFile { get; set; } = false;
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
                            getFileFromInternet(urlPath, filePath);
                            break;
                        case 2:
                            getNumberOfLetters(filePath);
                            break;
                        case 3:
                            countNumberOfWords(filePath);
                            break;
                        case 4:
                            CountNumberOfPunctationMarks(filePath);
                            break;
                        case 5:
                            break;
                        case 6:
                            Console.WriteLine(GenerateLetterCountReport(filePath));
                            Console.ReadKey();
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
                    Console.ReadKey();
                }
            } while (checkForExit);
        }
        /// <summary>
        /// Download and save file form given url to given file name
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="fileName"></param>
        public static void getFileFromInternet(string urlPath, string fileName)
        {
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFile(urlPath, fileName);
                checkFile = true;
            }
            catch (WebException)
            {
                checkFile = false;
                Console.WriteLine("Nie udalo sie pobrac pliku.");
            }
        }
        /// <summary>
        /// Counts number of letters contained in downloaded file.
        /// </summary>
        /// <param name="fileName"></param>
        public static string getNumberOfLetters(string fileName)
        {
            if (CheckIfFileExists(fileName))
            {
                string textFromFile = File.ReadAllText(fileName);
                return ("Ten plik zawiera: " + textFromFile.Count(char.IsLetter) + " liter");
            }

            return string.Empty;
        }

        /// <summary>
        /// Count number of words in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static void countNumberOfWords(string fileName)
        {
            if (CheckIfFileExists(fileName))
            {
                Console.WriteLine(string.Format("Liczba słow wynosi:{0}", File.ReadAllText(fileName).Split(' ').Length));
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Count number of punctation marks in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static void CountNumberOfPunctationMarks(string fileName)
        {
            if (CheckIfFileExists(fileName))
            {
                Console.WriteLine(string.Format("Liczba znakow interpunkcyjnych wynosi: {0}", (Regex.Matches(File.ReadAllText(fileName), @"[\p{P}]").Count)));
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Generates report of number of each letter that given file contains.
        /// </summary>
        /// <param name="fileName"></param>
        public static string GenerateLetterCountReport(string fileName)
        {
            if (CheckIfFileExists(filePath))
            {
                int[] arrayOfLetters = new int[char.MaxValue];
                string textFromFile = File.ReadAllText(fileName);

                foreach (char letter in textFromFile)
                {
                    if (letter >= 'A' && letter <= 'z')
                    {
                        arrayOfLetters[letter]++;
                    }
                }

                textFromFile = "";

                for (char letter = 'A'; letter <= 'z'; letter++)
                {
                    if (Char.IsLetter(letter))
                    {
                        textFromFile += (letter + " : " + arrayOfLetters[letter] + "\n");
                    }
                }

                return textFromFile;
            }

            return string.Empty;
        }

        /// <summary>
        /// Check if file is downloaded or exists.
        /// </summary>
        /// <param name="fileName"></param>
        public static bool CheckIfFileExists(string fileName)
        {
            if (File.Exists(fileName) || checkFile)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Plik nie został pobrany, bądź nie istnieje.");
                Console.ReadKey();
                return false;
            }
        }
    }
}