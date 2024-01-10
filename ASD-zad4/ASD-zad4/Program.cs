using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;
using System.ComponentModel.Design;
using System;
using System.Runtime.CompilerServices;

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
        public string? _number;

        public Node? _left;
        public Node? _right;

        public Node(string name, string address, string? number)
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
        public void PrintInOrder()
        {
            PrintInOrder(root);
        }
        private static void PrintInOrder(Node? node)
        {
            if (node != null)
            {
                PrintInOrder(node._left);
                Console.WriteLine($"{node._name}, {node._address}, {node._number}");
                PrintInOrder(node._right);
            }
        }
        public void PrintTree()
        {
            PrintTree(root!, "", true);
        }
        private static void PrintTree(Node node, string indent, bool last)
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

                PrintTree(node._left!, indent, false);
                PrintTree(node._right!, indent, true);
            }
        }


        // Find element
        public static Node FindElement(string name, string address, Node node)
        {
            if (node == null)
                return null!;

            int compareName = string.Compare(name, node._name);
            int compareAddress = string.Compare(address, node._address);

            if (compareName == 0 && compareAddress == 0)
                return node;

            if (compareName < 0 || (compareName == 0 && compareAddress < 0))
                return FindElement(name, address, node._left!);
            else
                return FindElement(name, address, node._right!);
        }


        // Delete element
        public void Delete(string name, string address)
        {
            root = DeleteNode(root!, name, address);
        }

        private static Node DeleteNode(Node node, string name, string address)
        {
            if (node == null)
                return node!;

            int compareName = string.Compare(name, node._name);
            int compareAddress = string.Compare(address, node._address);

            if (compareName < 0 || (compareName == 0 && compareAddress < 0))
                node._left = DeleteNode(node._left!, name, address);
            else if (compareName > 0 || (compareName == 0 && compareAddress > 0))
                node._right = DeleteNode(node._right!, name, address);
            else
            {
                if (node._left == null || node._right == null)
                {
                    Node temp = node._left! ?? node._right!;
                    if (temp == null)
                    {
                        temp = node;
                        node = null!;
                    }
                    else
                    {
                        node = temp; // One child case
                    }
                }
                else
                {
                    Node temp = FindMin(node._right);
                    node._name = temp._name;
                    node._address = temp._address;
                    node._number = temp._number;
                    node._right = DeleteNode(node._right, temp._name, temp._address);
                }
            }

            if (node == null)
                return node!;

            node = Rebalance(node);

            return node;
        }

        private static Node FindMin(Node node)
        {
            Node current = node;
            while (current._left != null)
            {
                current = current._left;
            }
            return current;
        }


        // Save book
        public void SaveTreeToFile(Node node, StreamWriter output)
        {
            if (node != null)
            {
                SaveTreeToFile(node._left!, output);
                output.WriteLine($"{node._name},{node._address},{node._number ?? ""}");
                SaveTreeToFile(node._right!, output);
            }
        }

    }



    internal class Program
    {
        static void Main(string[] args)
        {
            
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad4\ASD-zad4\";
            string data = location + "data.txt";
            string book = location + "book.txt";

            // Reading data & creating a tree
            StreamReader input = new(book);

            string line = input.ReadLine()!;
            AVL avl = new();

            while (line != null)
            {
                var lineSplit = line.Split(",");
                var name = lineSplit[0];
                var address = lineSplit[1];
                var number = lineSplit[2];
                if (number == "") number = null;
                var newNode = new Node(name, address, number);

                avl.Insert(newNode);

                line = input.ReadLine()!;
            }

            input.Close();

            // Main menu
            int choice;
            string choices =
                "1. Display phone book\n" +
                "2. Display AVL strucure\n" +
                "3. Find element\n" +
                "4. Add new element\n" +
                "5. Remove element\n" + 
                "6. Read data from file \"data.txt\" --- !WARNING! this will overwrite avl structure\n" +
                "7. Save phone book\n" +
                "0. Exit";
            do
            {
                Console.Clear();
                Console.WriteLine(choices);
                choice = Convert.ToInt32(Console.ReadLine());

                switch(choice)
                {
                    // Exit
                    case 0:
                        return;
                    // Read phone book
                    case 1:
                        Console.Clear();
                        avl.PrintInOrder();
                        Console.ReadLine();
                        break;
                    // Show AVL structure
                    case 2:
                        Console.Clear();
                        avl.PrintTree();
                        Console.ReadLine();
                        break;
                    // Find person
                    case 3:
                        Console.Clear();

                        Console.WriteLine("\nEnter name:");
                        var name = Console.ReadLine();
                        Console.WriteLine("\nEnter address:");
                        var address = Console.ReadLine();

                        var result = AVL.FindElement(name!, address!, avl.root!);
                        if (result != null)
                        {
                            string resultNumber = result._number ?? "NO ABONAMENT";
                            Console.WriteLine($"\nRESULT\n{name}, {address}, {resultNumber}");
                        }
                        else Console.WriteLine($"\nRESULT\nPerson with name: \"{name}\" and address: \"{address}\" does not exist in phone book");

                        Console.ReadLine();
                        break;
                    // Add new element
                    case 4:
                        Console.Clear();

                        Console.WriteLine("\nEnter name:");
                        name = Console.ReadLine();
                        Console.WriteLine("\nEnter address:");
                        address = Console.ReadLine();
                        Console.WriteLine("\nEnter number:");
                        var number = Console.ReadLine();

                        if (name == "" || address == "")
                        {
                            Console.WriteLine("Name and address cannot be null");
                            break;
                        }

                        Node newNode = new(name!, address!, number);
                        avl.Insert(newNode);

                        break;
                    // Remove element
                    case 5:
                        Console.Clear();

                        Console.WriteLine("\nEnter name:");
                        name = Console.ReadLine();
                        Console.WriteLine("\nEnter address:");
                        address = Console.ReadLine();

                        avl.Delete(name!, address!);
                        break;
                    // Read data from file
                    case 6:
                        input = new(data);

                        line = input.ReadLine()!;
                        avl = new();

                        while (line != null)
                        {
                            var lineSplit = line.Split(",");
                            name = lineSplit[0];
                            address = lineSplit[1];
                            number = lineSplit[2];
                            if (number == "") number = null;
                            newNode = new Node(name, address, number);

                            avl.Insert(newNode);

                            line = input.ReadLine()!;
                        }

                        input.Close();

                        Console.Clear();
                        Console.WriteLine("Data imported - avl strucute overwriten");
                        Console.ReadLine();

                        break;
                    // Save phone book
                    case 7:
                        StreamWriter output = new(book);
                        avl.SaveTreeToFile(avl.root!, output);
                        output.Close();

                        Console.Clear();
                        Console.WriteLine("Phone book saved to 'book.txt'");
                        Console.ReadLine();

                        break;
                    // Error
                    default:
                        Console.WriteLine($"There is no option {choice} - press anything to continue");
                        Console.ReadLine();
                        break;
                }
            } while (choice != 0);


            Console.ReadLine(); // Program written by Michał Grochowski
        }
    }
}
