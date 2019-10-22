using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            int option = 0; //it needs to be assigned from the beginnig
            bool checkForEnding = true;
            bool checkForException = true;

            while (checkForEnding)
            {
                // menu displayed in console
                Console.WriteLine("1. Pobierz plik z internetu.");
                Console.WriteLine("2. Zlicz liczbę liter w pobranym pliku.");
                Console.WriteLine("3. Zlicz liczbę wyrazów w pliku.");
                Console.WriteLine("4. Zlicz liczbę znaków interpunkcyjnych w pliku.");
                Console.WriteLine("5. Zlicz liczbę zdań w pliku.");
                Console.WriteLine("6. Wygeneruj raport o użyciu liter (A-Z)");
                Console.WriteLine("7. Zapisz statystyki z punktów 2-5 do pliku statystyki.txt.");
                Console.WriteLine("8. Wyjście z programu.");
                do // checking if user gave correct input
                {
                    try
                    {
                        option = int.Parse(Console.ReadLine());
                        Console.Clear();
                    }
                    catch (FormatException)
                    {
                        // checking if exception occured
                    }
                    if (option >= 1 && option <= 8)
                    {
                        // if the condition is met change value of checkForException to false to exit loop
                        checkForException = false;
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy format bądź liczba z poza menu. Prosze sprobowac jeszcze raz.");
                    }
                } while (checkForException);

                switch (option)
                {
                    case 1:
                        getFileFromInternet(urlPath, filePath);
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8: //  exit from program
                        checkForEnding = false;
                        break;
                }
            }

        }
        /// <summary>
        /// Download and save file form given url to given file name
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="fileName"></param>
        public static void getFileFromInternet(string urlPath, string fileName)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(urlPath, fileName);
        }
    }
}