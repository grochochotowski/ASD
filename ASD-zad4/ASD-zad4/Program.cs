using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

namespace ASD_zad4
{
    // 1.  Napisz program realizujący książkę telefoniczną w strukturze drzewa AVL
    //     Program ma umożliwiać wykonanie następujących operacji:
    //     a)   wstawienie nowego abonenta wraz z numerami telefonów tego abonenta.Porządek
    //          symetryczny drzewa jest wyznaczony przez porządek leksykograficzny na danych
    //          abonenta.Dane abonenta to: nazwisko i imię(albo nazwa firmy), adres abonenta.Jeśli
    //          dwóch abonentów nie rozróżnia nazwisko i imię , to o kolejności danych decyduje adres.
    //          Zakładamy, że nie ma dwóch abonentów o tym samym imieniu i nazwisku oraz adresie,
    //     b)   usunięcie abonenta wraz z jego numerami telefonów
    //     c)   wyszukanie numeru(numerów) telefonu po podaniu danych abonenta(nazwisko i imię
    //          oraz adres), uwzględnienie komunikatu : Brak abonenta.......
    //     d)   zapis aktualnej zawartości słownika do pliku(zapisywanie abonentów wraz z danymi).
    //          Format danych w pliku proszę ustalić samodzielnie.
    //     e)   odczyt danych z pliku i wstawienie danych do drzewa AVL
    // 2.  Przygotuj plik do testów – co najmniej 100 abonentów wraz z danymi
    // 3.  Przetestowanie poprawności zaimplementowanych operacji i ustalenie ich złożoności czasowej

    public class Node
    {
        public string _name;
        public string _address;
        public string _number;

        public int _level;

        public Node? _left;
        public Node? _right;

        public Node(string name, string address, string number, int level)
        {
            _name = name;
            _address = address;
            _number = number;

            _level = level;
        }
    }

    internal class Program
    {
        /// ============================= Variables =============================
        public static List<Node> avl = [];

        /// ============================= Functions =============================
        public static void GoLeft(Node newElement, Node parent)
        {
            if ()
            {

            }
        }
        public static void GoRight()
        {

        }
        
        public static void RotationRL()
        {

        }
        public static void RotationLR()
        {

        }
        public static void RotationR()
        {

        }
        public static void RotationL()
        {

        }

        /// =============================== Main ================================
        static void Main(string[] args)
        {
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad4\ASD-zad4\";
            string pathIn = location + "dane.txt";
            string pathOut = location + "wynik.txt";

            /// ========================= READING INPUT DATA ========================
            StreamReader input = new StreamReader(pathIn);

            string line = input.ReadLine();
            int index = 0;

            while (line != null)
            {
                var lineSplit = line.Split(",");
                var name = lineSplit[0] + " " + lineSplit[1];
                var address = lineSplit[2] + " " + lineSplit[3] + " " + lineSplit[4];
                var newElement = new Node(name, address, lineSplit[5], 0);

                // check if compare parent to new lower or higher
                //      > if compare parent to new == check address
                // go to CLR
                // do this until end of branch
                // put element
                // calculate levels on branch
                // check if levels are ok
                //      > if levels ok do nothing
                //      > if levels not ok do something

                if (avl.Count == 0)
                {
                    avl.Add(newElement);
                }
                else
                {
                    GoLeft(newElement, avl[index-1]);
                    GoRight(newElement, avl[index - 1]);
                    
                    if (string.Compare(avl[index-1]._name, name) == -1)
                    {
                        // go left?
                    }
                    else if (string.Compare(avl[index - 1]._name, name) == 1)
                    {
                        //go right?
                    }
                    else
                    {
                        if (string.Compare(avl[index - 1]._address, address) == -1)
                        {
                            // go left?
                        }
                        else if (string.Compare(avl[index - 1]._address, address) == 1)
                        {
                            //go right?
                        }
                        else
                        {
                            Console.WriteLine($"Same data for {name}, {address}, {lineSplit[5]}");
                        }
                    }
                }

                index++;
                line = input.ReadLine();
            }

            Console.WriteLine(string.Compare("mayo", "text"));

            input.Close();
            /// ============================= CALCULATING RESULST =============================


            /// ============================= SHOWING RESULTS =============================



            Console.ReadLine(); // Program written by Michał Grochowski
        }
    }
}
