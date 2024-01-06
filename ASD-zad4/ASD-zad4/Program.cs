using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;
using System.ComponentModel.Design;
using System;

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

        public Node(string name, string address, string number)
        {
            _name = name;
            _address = address;
            _number = number;
        }
    }
    public class AVL
    {
        public Node? root;

        // Inserting
        public void Insert(Node newNode)
        {
            root = InsertNode(newNode, root!);
        }
        private static Node InsertNode(Node newNode, Node node)
        {
            if (node == null)
                return newNode;

            int compare = string.Compare(newNode._name, node._name);
            if (compare == 0)
                compare = string.Compare(newNode._address, node._address);

            if (compare < 0)
                node._left = InsertNode(newNode, node._left!);
            else if (compare > 0)
                node._right = InsertNode(newNode, node._right!);
            else
            {
                Console.WriteLine($"Same data for {newNode._name}, {newNode._address}, {newNode._number}");
                throw new InvalidOperationException("Duplicate node not allowed");
            }

            node = Rebalance(node);

            return node;
        }

        // Balancing
        private static Node Rebalance(Node node)
        {
            int balance = Height(node._left) - Height(node._right);

            if (balance > 1)
            {
                if (Height(node._left!._left) >= Height(node._left._right))
                {
                    node = RotateRight(node);
                }
                else
                {
                    node = RotateLeftRight(node);
                }
            }
            else if (balance < -1)
            {
                if (Height(node._right!._right) >= Height(node._right._left))
                {
                    node = RotateLeft(node);
                }
                else
                {
                    node = RotateRightLeft(node);
                }
            }

            return node;
        }
        private static int Height(Node? node)
        {
            if (node == null)
                return -1;

            return 1 + Math.Max(Height(node._left), Height(node._right));
        }


        // Rotations
        private static Node RotateRight(Node node)
        {
            Node pivot = node._left!;
            node._left = pivot._right;
            pivot._right = node;
            return pivot;
        }
        private static Node RotateLeft(Node node)
        {
            Node pivot = node._right!;
            node._right = pivot._left;
            pivot._left = node;
            return pivot;
        }
        private static Node RotateRightLeft(Node node)
        {
            node._right = RotateRight(node._right!);
            return RotateLeft(node);
        }
        private static Node RotateLeftRight(Node node)
        {
            node._left = RotateLeft(node._left!);
            return RotateRight(node);
        }


        // Printing tree
        public static void PrintInOrder(Node? node)
        {
            if (node != null)
            {
                PrintInOrder(node._left);
                Console.WriteLine($"{node._name}, {node._address}, {node._number}");
                PrintInOrder(node._right);
            }
        }
        public void PrintTree(Node node, string indent, bool last)
        {
            if (node != null)
            {
                Console.Write(indent);

                if (last)
                {
                    Console.Write("R----");
                    indent += "     ";
                }
                else
                {
                    Console.Write("L----");
                    indent += "|    ";
                }

                Console.WriteLine(node._name);

                PrintTree(node._left, indent, false);
                PrintTree(node._right, indent, true);
            }
        }
        public void PrintTree()
        {
            PrintTree(root, "", true);
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            AVL avl = new();
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad4\ASD-zad4\";
            string pathIn = location + "tempDane.txt";
            string pathOut = location + "wynik.txt";

            /// ========================= READING INPUT DATA ========================
            StreamReader input = new StreamReader(pathIn);

            string line = input.ReadLine()!;

            while (line != null)
            {
                var lineSplit = line.Split(",");
                var name = lineSplit[0];
                var address = lineSplit[1];
                var number = lineSplit[2];
                var newNode = new Node(name, address, number);

                avl.Insert(newNode);

                line = input.ReadLine()!;
            }

            input.Close();


            /// ========================= DISPLAY RESULT DATA ========================
            //AVL.PrintInOrder(avl.root);
            avl.PrintTree();

            Console.ReadLine(); // Program written by Michał Grochowski
        }
    }
}
