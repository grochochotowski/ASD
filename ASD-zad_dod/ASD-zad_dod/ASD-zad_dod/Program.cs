using System.Collections.Generic;
using System.Numerics;

namespace ASD_zad_dod
{
    public class PriorityQueue<T>
    {
        private readonly SortedDictionary<int, Queue<T>> _dictionary = new();

        public void Insert(T item, int priority)
        {
            if (!_dictionary.ContainsKey(priority))
            {
                _dictionary[priority] = new Queue<T>();
            }

            _dictionary[priority].Enqueue(item);
        }

        public T DeleteMax()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Priority queue is empty.");
            }

            var firstPair = _dictionary.First();
            var item = firstPair.Value.Dequeue();

            if (firstPair.Value.Count == 0)
            {
                _dictionary.Remove(firstPair.Key);
            }

            return item;
        }

        public void Display()
        {
            foreach (var pair in _dictionary)
            {
                foreach (var item in pair.Value)
                {
                    Console.Write($"{item} ");
                }
            }
            Console.WriteLine();
        }

        public bool IsEmpty => _dictionary.Count == 0;
        public bool IsMoreThan1 => _dictionary.Count > 1;
    }
    internal class Program
    {
        /// ============================= Variables =============================
        public static int n = 0;
        public static int n10 = 10;
        public static int n100 = 100;
        public static int n1000 = 1000;
        public static int n10000 = 10000;
        public static int n100000 = 100000;
        public static int[]? weights;

        public static BigInteger mainCounter = 0;

        /// ============================= Functions =============================
        public static int[] GenerateData(int num)
        {
            Random rnd = new Random();

            if (num == 0)
            {
                do
                {
                    num = rnd.Next(1, 100000);
                } while (num % 10 == 0);
                n = num;
            }

            weights = new int[num];
            for (int i = 0; i < num; i++)
            {
                weights[i] = rnd.Next(1, 11);
            }
            return weights;
        }

        public static void DisplayInput(int num, int[] snowballWeights)
        {
            Console.WriteLine($"========== n = {num} ==========\nSnowball weights [g]:");
            if (num <= 100)
            {
                for (int i = 0; i < num; i++)
                {
                    Console.Write(i == num-1 ? $"{snowballWeights[i]}\n\n" : $"{snowballWeights[i]}, ");
                }
            }
            else
            {
                Console.WriteLine("\ttoo many to display");
            }
        }

        public static int CalculateCost(int num, int[] snowballWeights)
        {
            mainCounter = 0;
            int calculatedEffort = 0;
            PriorityQueue<int> pq = new();

            foreach (var weight in snowballWeights)
            {
                pq.Insert(weight, weight);
            }
            if (num <= 10)
            {
                Console.Write("PQ: ");
                pq.Display();
            }

            while (pq.IsMoreThan1)
            {
                int snowball1 = pq.DeleteMax();
                int snowball2 = pq.DeleteMax();
                mainCounter += 4;

                int mergedWeight = snowball1 + snowball2;
                calculatedEffort += mergedWeight;

                pq.Insert(mergedWeight, mergedWeight);
                mainCounter++;

                if (num <= 10)
                {
                    Console.Write($"COST: {calculatedEffort - mergedWeight} + ({snowball1} + {snowball2}): ");
                    pq.Display();
                }
                else if (num <= 100)
                {
                    Console.WriteLine($"COST: {calculatedEffort - mergedWeight} + ({snowball1} + {snowball2})");
                }
            }

            Console.WriteLine();
            return calculatedEffort;
        }

        public static void DisplayResult(int numOfSnowballs, int totalEffort)
        {
            Console.WriteLine($"Total effort: {totalEffort}");
            Console.WriteLine($"Operation counter: {mainCounter}\n\n");
        }

        /// ============================= Main =============================
        static void Main()
        {
            /// ============================= n = 10 =============================
            int[] weightsN10 = GenerateData(n10);
            DisplayInput(n10, weightsN10);
            int effortN10 = CalculateCost(n10, weightsN10);
            DisplayResult(n10, effortN10);


            /// ============================= n = 100 =============================
            int[] weightsN100 = GenerateData(n100);
            DisplayInput(n100, weightsN100);
            int effortN100 = CalculateCost(n100, weightsN100);
            DisplayResult(n100, effortN100);


            /// ============================= n = 1000 =============================
            int[] weightsN1000 = GenerateData(n1000);
            DisplayInput(n1000, weightsN1000);
            int effortN1000 = CalculateCost(n1000, weightsN1000);
            DisplayResult(n1000, effortN1000);


            /// ============================= n = 10000 =============================
            int[] weightsN10000 = GenerateData(n10000);
            DisplayInput(n10000, weightsN10000);
            int effortN10000 = CalculateCost(n10000, weightsN10000);
            DisplayResult(n10000, effortN10000);


            /// ============================= n = 100000 =============================
            int[] weightsN100000 = GenerateData(n100000);
            DisplayInput(n100000, weightsN100000);
            int effortN100000 = CalculateCost(n100000, weightsN100000);
            DisplayResult(n100000, effortN100000);


            /// ============================= random n =============================
            int[] weightsN = GenerateData(n);
            DisplayInput(n, weightsN);
            int effortN = CalculateCost(n, weightsN);
            DisplayResult(n, effortN);





            Console.ReadLine(); // Program made by Michał Grochowski
        }
    }
}