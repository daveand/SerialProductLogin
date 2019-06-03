using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialProductLogin
{
    class Program
    {
        static bool isArticle;

        static List<string> articles = new List<string>(ConfigurationManager.AppSettings["Articles"].Split(new char[] { ';' }));

        static void Main(string[] args)
        {

            while (true)
            {
                SerialComm s = new SerialComm();

                // Declare variables
                //string scannedValue;
                string scan1 = "";
                string scan2;
                string product;

                while (scan1 != "error")
                {
                    scan1 = s.Read();

                    // Check if first scan matched any article number
                    isArticle = GetArticle(scan1);

                    if (isArticle && scan1 != "error")
                    {
                        Console.WriteLine($"Scanned value {scan1} is an article. Scan product ID.");
                        scan2 = s.Read();
                        product = scan2 + scan1;
                        s.Send(product);
                        isArticle = false;
                    }
                    else if (scan1 != "error")
                    {
                        Console.WriteLine("Scanned value is not an article. Sending first scan.");
                        s.Send(scan1);
                        isArticle = false;
                    }

                }

                Console.WriteLine("Is another application using the COM-port?\nPress any key to try again...");
                Console.ReadLine();

            }

        }

        static bool GetArticle(string data)
        {
            foreach (string x in articles)
            {
                if (x.Equals(data))
                {
                    isArticle = true;
                }
            }
            return isArticle;
        }
    }
}
