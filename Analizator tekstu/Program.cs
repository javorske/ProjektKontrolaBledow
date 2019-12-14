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
        public static string urlPath { get; set; }
        //https://s3.zylowski.net/public/input/1.txt
        /// <summary>
        /// The name of file.
        /// </summary>
        public static string filePath { get; set; } = "Plik.txt";
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
                Console.WriteLine("1. Wybierz plik wejściowy");
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
                            GetFile(filePath);
                            break;
                        case 2:
                            Console.WriteLine(GetNumberOfLetters(filePath));
                            Console.ReadKey();
                            break;
                        case 3:
                            Console.WriteLine(CountNumberOfWords(filePath));
                            Console.ReadKey();
                            break;
                        case 4:
                            Console.WriteLine(CountNumberOfPunctationMarks(filePath));
                            Console.ReadKey();
                            break;
                        case 5:
                            Console.WriteLine(CountNumberOfSentences(filePath));
                            Console.ReadKey();
                            break;
                        case 6:
                            Console.WriteLine(GenerateLetterCountReport(filePath));
                            Console.ReadKey();
                            break;
                        case 7:
                            StatisticsFile();
                            Console.ReadKey();
                            break;
                        case 8: //  exit from program
                            CloseProgram();
                            break;
                    }
                }
                catch (FormatException)
                {
                    // checking if exception occured
                    Console.WriteLine("Nie wybrano zadnej opcji, badz podano zly znak.");
                }
            } while (checkForExit);
        }

        /// <summary>
        /// Download and save file form given url to given file name
        /// </summary>
        /// <param name="urlPath">Url path to file.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="decision">Defines if user wants to download file from internet</param>
        public static void GetFile(string fileName)
        {
            
            Console.WriteLine("Pobrać plik z internetu? [T/N]");
            char decision = Convert.ToChar(Console.ReadLine());
            if (decision == 'T')
            {
                Console.WriteLine("Podaj adres pliku tekstowego");                
                try
                {
                    urlPath = Console.ReadLine();
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(urlPath, fileName);
                }
                catch (WebException err)
                {
                    Console.WriteLine(string.Format("Error code:{0}", err));
                }
            }
            else
            {
                try
                {
                    Console.WriteLine("Podaj nazwę pliku znajdującego się na dysku:");
                    fileName = Console.ReadLine();
                    Console.WriteLine(File.Exists(fileName) ? "File exists." : "File does not exist.");
                    Console.ReadKey();
                }
                catch (Exception err)
                {
                    Console.WriteLine(string.Format("Error code:{0}", err));
                    Console.ReadKey();
                }
                
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static string GetNumberOfLetters(string fileName)
        {
            if (CheckIfFileExists(filePath))
            {
                string textFromFile = File.ReadAllText(fileName);
                Console.WriteLine("Ten plik zawiera: " + textFromFile.Count(char.IsLetter) + " liter");

                //Zmiana wymagania związanego z liczeniem liter. Osobne zliczanie samogłosek i spółgłosek
                int vowel = 0;
                int cons = 0;
                for (int i = 0; i < textFromFile.Length; i++)
                {
                    if (textFromFile[i] == 'a' || textFromFile[i] == 'e' || textFromFile[i] == 'i' || textFromFile[i] == 'o' || textFromFile[i] == 'u' || textFromFile[i] == 'A' || textFromFile[i] == 'E' || textFromFile[i] == 'I' || textFromFile[i] == 'O' || textFromFile[i] == 'U')
                    {
                        vowel++;
                    }
                    else if ((textFromFile[i] >= 'a' && textFromFile[i] <= 'z') || (textFromFile[i] >= 'A' && textFromFile[i] <= 'Z'))
                    {
                        cons++;
                    }
                }
                Console.WriteLine("Ten plik zawiera: " + vowel.ToString() + " samogłosek");
                Console.WriteLine("Ten plik zawiera: " + cons.ToString() + " spółgłosek");
            }

            return string.Empty;
        }

        /// <summary>
        /// Count number of words in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static string CountNumberOfWords(string fileName)
        {
            if (CheckIfFileExists(filePath))
            {
                string[] file = File.ReadAllText(fileName).Split(' ');
                return (string.Format("Liczba słow wynosi:{0}", file.Where(x => Regex.IsMatch(x, "[a-z]", RegexOptions.IgnoreCase)).Count()));
            }

            return string.Empty;
        }

        /// <summary>
        /// Count number of punctation marks in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static string CountNumberOfPunctationMarks(string fileName)
        {
            if (CheckIfFileExists(filePath))
            {
                return (string.Format("Liczba znakow interpunkcyjnych wynosi: {0}", (Regex.Matches(File.ReadAllText(fileName), @"[\p{P}]").Count)));
            }

            return string.Empty;
        }

        /// <summary>
        /// Count number of sentences in given file.
        /// </summary>
        /// <param name="fileName"></param>
        public static string CountNumberOfSentences(string fileName)
        {
            if (CheckIfFileExists(filePath))
            {
                return (string.Format("Liczba zdan wynosi: {0}", (Regex.Matches(File.ReadAllText(fileName), @"(?<=[.!?])\s?([A-Z]?)").Count)));
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates report of number of each letter that given file contains.
        /// </summary>
        /// <param name="fileName"></param>
        public static string GenerateLetterCountReport(string fileName)
        {
            if (CheckIfFileExists(filePath))
            {
                int[] arrayOfLetters = new int[(int)char.MaxValue];
                string textFromFile = File.ReadAllText(fileName);

                foreach (char letter in textFromFile)
                {
                    if (letter >= 'A' && letter <= 'z')
                    {
                        arrayOfLetters[(int)letter]++;
                    }
                }

                string textToDisplay = "";

                for (char letter = 'A'; letter <= 'z'; letter++)
                {
                    if (Char.IsLetter(letter))
                    {
                        textToDisplay += ((char)letter + " : " + arrayOfLetters[letter] + "\n");
                    }
                }

                return textToDisplay;
            }

            return null;
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
                return false;
            }
        }
        /// <summary>
        /// Creates/overwrite a statystyki.txt file with information from points 2-5.
        /// </summary>
        public static void StatisticsFile()
        {
            if (CheckIfFileExists(filePath))
            {
                string statisticsPath = "statystyki.txt";
                using (StreamWriter sw = File.CreateText(statisticsPath))
                {
                    sw.WriteLine(GetNumberOfLetters(filePath));
                    sw.WriteLine(CountNumberOfWords(filePath));
                    sw.WriteLine(CountNumberOfPunctationMarks(filePath));
                    sw.WriteLine(CountNumberOfSentences(filePath));
                }
                Console.WriteLine("Informacje z punktow 2-5 zostaly zapisane/nadpisane do pliku statystyki.txt");
            }
        }
        /// <summary>
        /// This function will close the program.
        /// </summary>
        /// <returns>Return true for closing app</returns>
        public static void CloseProgram()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (File.Exists("statystyki.txt"))
            {
                File.Delete("statystyki.txt");
            }
            try
            {
                Environment.Exit(0);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}