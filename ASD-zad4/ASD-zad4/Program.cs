using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Net;
using System.Xml.Linq;

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

    public class Element
    {
        public string _name;
        public string _city;
        public string _street;
        public string _address;
        public string _number;

        public int _level;

        public Element? _parent;

        public Element(string name, string city, string street, string address, string number, int level, Element parent)
        {
            _name = name;
            _city = city;
            _street = street;
            _address = address;
            _number = number;

            _level = level;

            _parent = parent;
        }
        public Element (Element prevElement, int level, Element parent)
        {
            _name = prevElement._name;
            _city = prevElement._city;
            _street = prevElement._street;
            _address = prevElement._address;
            _number = prevElement._number;

            _level = level;

            _parent = parent;
        }
    }

    internal class Program
    {
        /// ============================= Variables =============================
        public static List<Element> avl = new();

        /// ============================= Functions =============================
        public void RotationRL()
        {

        }
        public void RotationLR()
        {

        }
        public void RotationR()
        {

        }
        public void RotationL()
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
            int index = 1;

            while (line != null)
            {
                var lineSplit = line.Split(",");

                // check if compare parent to new lower or higher
                //      > if compare parent to new == check address
                // go to CLR
                // do this until end of branch
                // put element
                // calculate levels on branch
                // check if levels are ok
                //      > if levels ok do nothing
                //      > if levels not ok do something

                avl.Add(new Element(lineSplit[0] + " " + lineSplit[1], lineSplit[2], lineSplit[3], lineSplit[0], lineSplit[0], 0, null));


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
