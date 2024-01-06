﻿using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;
using System.ComponentModel.Design;

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

        public int _balance;

        public Node? _left;
        public Node? _right;

        public Node(string name, string address, string number, int balance)
        {
            _name = name;
            _address = address;
            _number = number;

            _balance = balance;
        }
    }
    public class AVL
    {
        public Node? root;

        public void Insert(Node newNode, Node prevNode)
        {

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            AVL avl = new();
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad4\ASD-zad4\";
            string pathIn = location + "dane.txt";
            string pathOut = location + "wynik.txt";

            /// ========================= READING INPUT DATA ========================
            StreamReader input = new StreamReader(pathIn);

            string line = input.ReadLine()!;
            int index = 0;

            while (line != null)
            {
                var lineSplit = line.Split(",");
                var name = lineSplit[0] + " " + lineSplit[1];
                var address = lineSplit[2] + " " + lineSplit[3] + " " + lineSplit[4];
                var newNode = new Node(name, address, lineSplit[5], 0);

                if (index == 0) avl.root = newNode;
                else avl.Insert(newNode, avl.root!);

                line = input.ReadLine()!;
            }
            //Console.WriteLine($"Same data for {name}, {address}, {lineSplit[5]}");
            //string.Compare(avl[index - 1]._name, name

            input.Close();
            /// ============================= CALCULATING RESULST =============================


            /// ============================= SHOWING RESULTS =============================



            Console.ReadLine(); // Program written by Michał Grochowski
        }
    }
}
