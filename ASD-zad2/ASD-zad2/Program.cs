using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

/*
    Hamak już kupiony, tylko gdzie go powiesi? Hamak nie za długi, ale na szczęcie w lesie
    sporo drzew rośnie. Trzeba znaleźć dwa drzewa, które w lesie są połoąone najbliżej siebie.
    Napisz program, który:
    • wczyta współrzdne wszystkich drzew w lesie z pliku wejściowego GH_in_grupa_nazwisko.txt,
    • wyznaczy dwa drzewa położone najbliżej siebie,
    • zapisze wynik (numery drzew) do pliku wynikowego GH_out_grupa_nazwisko.txt

    Wejście:
    W pierwszej linii pliku GH_in_grupa_nazwisko.txt jest zapisana jedna liczba naturalna n
    oznaczająca liczbę drzew (1 <= n <= 10^7). W kolejnych n linijkach pliku znajdują się pary liczb
    całkowitych x, y oznaczające współrzdne kartezjańskie położenia poszczególnych drzew 1 <= x, y <= 10^7

    Wyjście:
    W pierwszej linii pliku GH_out_grupa_nazwisko.txt znajduje się para liczb oddzielonych
    spacją oznaczająca współrzdne pierwszego drzewa z pary najbliższych drzew. W drugiej
    linii pliku znajduje się para liczb oddzielonych spacją oznaczająca współrzdne drugiego
    drzewa z pary najbliższych drzew. Jeśeli istnieje wicej niż jedna para najbliższych drzew, to
    wypisywana jest ta para, w której numery drzew (w sensie kolejności podawania ich
    współrzdnych w pliku wejściowym) są wcześniejsze leksykograficznie.
*/


namespace ASD_zad2
{
    class Tree
    {
        public int x, y, id;
        public Tree()
        {
            x = 0;
            y = 0;
            id = int.MaxValue;
        }
        public Tree(int xx, int yy, int idd)
        {
            x = xx;
            y = yy;
            id = idd;
        }
        public Tree(Tree tree)
        {
            x = tree.x;
            y = tree.y;
            id = tree.id;
        }
        public override string ToString()
        {
            return $"[{x},{y}]";
        }
    }

    internal class Program
    {
        public static int recCounter = 0;
        public static int itCounter = 0;
        public static int sortCounter = 0;
        public static int xtraSortCounter = 0;
        public static int baseCounter = 0;

        /// ========================================================= Recursive Method =========================================================
        // Finding closest trees with 'Divide and Conquer'
        static (Tree treeOne, Tree treeTwo, double delta) RecMethod(List<Tree> xTrees, List<Tree> yTrees)
        {
            int n = xTrees.Count;
            Tree treeOne = new Tree(int.MaxValue, int.MaxValue, -1);
            Tree treeTwo = new Tree(int.MinValue, int.MinValue, -1);
            double delta = double.MaxValue;

            // Base cases for 'n == 2' and 'n == 3'
            if (n == 2) // >>> O(1)
            {
                treeOne = xTrees[0];
                treeTwo = xTrees[1];
                delta = CalculateDistance(treeOne.x, treeOne.y, treeTwo.x, treeTwo.y);
                return (treeOne, treeTwo, delta);
            }
            recCounter++;
            if (n == 3) // >>> O(1)
            {
                (treeOne, treeTwo, delta) = BaseCases(xTrees);
                return (treeOne, treeTwo, delta);
            }
            recCounter++;
            // Dividing trees by two >>> O(1)
            Tree middle = xTrees[n / 2];

            List<Tree> leftXTrees = xTrees.Take(n / 2).ToList();
            List<Tree> rightXTrees = xTrees.Skip(n / 2).ToList();

            List<Tree> leftYTrees = yTrees.Take(n / 2).ToList();
            List<Tree> rightYTrees = yTrees.Skip(n / 2).ToList();

            // Finding closest trees in left and right parts of list
            (Tree treeOneLeft, Tree treeTwoLeft, double deltaLeft) = RecMethod(leftXTrees, leftYTrees);
            (Tree treeOneRight, Tree treeTwoRight, double deltaRight) = RecMethod(rightXTrees, rightYTrees);
            if (deltaLeft < deltaRight)
            {
                treeOne = treeOneLeft;
                treeTwo = treeTwoLeft;
                delta = deltaLeft;
            }
            else
            {
                treeOne = treeOneRight;
                treeTwo = treeTwoRight;
                delta = deltaRight;
            }
            recCounter++;

            // Finding band points in area of delta x 2*delta >>> O(n)
            List<Tree> inBandTrees = new List<Tree>();
            for (int i = 0; i < n; i++)
            {
                if (yTrees[i].x >= middle.x - delta && yTrees[i].x <= middle.x + delta)
                {
                    inBandTrees.Add(yTrees[i]);
                }
            }

            // Sorting band points by y-coordinate >>> O(n log n)
            inBandTrees = MergeSort(inBandTrees);
            recCounter += xtraSortCounter;

            // Checking if band points are closer than non-band points
            int bl = inBandTrees.Count;
            for (int i = 0; i < bl - 1; i++)
            {
                for (int j = i + 1; j < Math.Min(i + 7, bl); j++) // >>> O(bl^2-bl+1)
                {
                    double bandDelta = CalculateDistance(inBandTrees[i].x, inBandTrees[i].y, inBandTrees[j].x, inBandTrees[j].y);
                    if (bandDelta < delta)
                    {
                        treeOne = inBandTrees[i];
                        treeTwo = inBandTrees[j];
                        delta = bandDelta;
                    }
                    recCounter++;
                }
            }
            return (treeOne, treeTwo, delta);
        } // >>> O(n log n) - O(n^2)


        /// ========================================================= Iterative Method =========================================================
        // Finding closest trees with 'Divide and Conquer'
        static (Tree treeOne, Tree treeTwo, double delta) ItMethod(List<Tree> xTrees, List<Tree> yTrees)
        {
            int n = xTrees.Count;

            Tree treeOne = new Tree(int.MaxValue, int.MaxValue, -1);
            Tree treeTwo = new Tree(int.MinValue, int.MinValue, -1);
            double delta = double.MaxValue;

            // Base cases for 'n ==2' and 'n == 3' >>> O(1)
            if (n <= 3)
            {
                (treeOne, treeTwo, delta) = BaseCases(xTrees);
                itCounter += baseCounter;
                return (treeOne, treeTwo, delta);
            }
            itCounter++;
            int left = 0;
            int right = n - 1;

            while (left < right) // >>> O(n)
            {
                for (int i = left; i < right; i++)
                {
                    for (int j = i + 1; j <= right; j++) // >>> O(n^2)
                    {
                        double currentDelta = CalculateDistance(xTrees[i].x, xTrees[i].y, xTrees[j].x, xTrees[j].y);
                        if (currentDelta < delta || (currentDelta == delta && (xTrees[i].id < treeOne.id || xTrees[j].id < treeTwo.id)))
                        {
                            treeOne = xTrees[i];
                            treeTwo = xTrees[j];
                            delta = currentDelta;
                            itCounter += 3;
                        }
                        itCounter++;
                    }
                }

                if (right - left + 1 > 2)
                {
                    int middle = (left + right) / 2;

                    for (int i = left; i <= middle; i++)
                    {
                        for (int j = middle + 1; j <= right; j++) // >>> O(n^2)
                        {
                            double currentDelta = CalculateDistance(yTrees[i].x, yTrees[i].y, yTrees[j].x, yTrees[j].y);
                            if (currentDelta < delta || (currentDelta == delta && (yTrees[i].id < treeOne.id || yTrees[j].id < treeTwo.id)))
                            {
                                treeOne = yTrees[i];
                                treeTwo = yTrees[j];
                                delta = currentDelta;
                                itCounter+=3;
                            }
                            itCounter++;
                        }
                    }

                    left = right + 1;
                }
                itCounter++;
            }
            return (treeOne, treeTwo, delta);
        } // >>> O(n^3)


        /// ============================================================== Other ===============================================================
        // Calculating distance for 2 trees
        static double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }

        // Base cases for 'n == 2' and 'n == 3'
        static (Tree treeOne, Tree treeTwo, double delta) BaseCases(List<Tree> xTrees)
        {
            baseCounter = 0;
            int n = xTrees.Count;
            Tree treeOne = xTrees[0];
            Tree treeTwo = xTrees[1];
            double delta = CalculateDistance(xTrees[0].x, xTrees[0].y, xTrees[1].x, xTrees[1].y);
            int smaller = Math.Min(treeOne.id, treeTwo.id);

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++) // >>> O(n^2)
                {
                    double tempDelta = CalculateDistance(xTrees[i].x, xTrees[i].y, xTrees[j].x, xTrees[j].y);
                    if (tempDelta < delta || (tempDelta == delta && (xTrees[i].id < smaller || xTrees[j].id < smaller)))
                    {
                        treeOne = xTrees[i];
                        treeTwo = xTrees[j];
                        delta = tempDelta;
                        smaller = Math.Min(treeOne.id, treeTwo.id);
                        baseCounter += 4;
                    }
                    baseCounter++;
                }
            }

            return (treeOne, treeTwo, delta);
        } // O(n^2)

        // Sorting lists
        public static List<Tree> MergeSort(List<Tree> xTrees)
        {
            xtraSortCounter = 0;
            if (xTrees.Count <= 1)
            {
                return xTrees;
            }
            sortCounter++;
            xtraSortCounter++;

            int middle = xTrees.Count / 2;
            List<Tree> left = new List<Tree>();
            List<Tree> right = new List<Tree>();

            for (int i = 0; i < middle; i++)
            {
                left.Add(xTrees[i]);
            }
            for (int i = middle; i < xTrees.Count; i++)
            {
                right.Add(xTrees[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);

            return Merge(left, right);
        }
        private static List<Tree> Merge(List<Tree> left, List<Tree> right)
        {
            List<Tree> result = new List<Tree>();
            int leftPointer = 0, rightPointer = 0;

            while (leftPointer < left.Count && rightPointer < right.Count)
            {
                if (left[leftPointer].x <= right[rightPointer].x)
                {
                    result.Add(left[leftPointer]);
                    leftPointer++;
                    sortCounter++;
                    xtraSortCounter++;
                }
                else
                {
                    result.Add(right[rightPointer]);
                    rightPointer++;
                    sortCounter++;
                    xtraSortCounter++;
                }
                sortCounter++;
                xtraSortCounter++;
            }

            while (leftPointer < left.Count)
            {
                result.Add(left[leftPointer]);
                leftPointer++;
                sortCounter++;
                xtraSortCounter++;
            }
            while (rightPointer < right.Count)
            {
                result.Add(right[rightPointer]);
                rightPointer++;
                sortCounter++;
                xtraSortCounter++;
            }

            return result;
        } // O(n log n)

        // Sort results in order of inputing
        static (Tree treeOneSorted, Tree treeTwoSorted) SortResults(Tree treeOne, Tree treeTwo, List<Tree> xTrees)
        {
            int n = xTrees.Count;
            Tree treeOneSorted = new Tree();
            Tree treeTwoSorted = new Tree();
            for (int i = 0; i < n; i++)
            {
                if (treeOne.id == xTrees[i].id)
                {
                    treeOneSorted = treeOne;
                    treeTwoSorted = treeTwo;
                    break;
                }
                if (treeTwo.id == xTrees[i].id)
                {
                    treeOneSorted = treeTwo;
                    treeTwoSorted = treeOne;
                    break;
                }
            }

            return (treeOneSorted, treeTwoSorted);
        } // O(n)


        /// =============================================================== Main ===============================================================
        static void Main(string[] args)
        {
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad2\ASD-zad2\";
            string pathIn = location + "GH_in_ps7_Grochowski.txt";
            string pathOut = location + "GH_out_ps7_Grochowski.txt";

            // Opening input and output files
            StreamReader srIn = new StreamReader(pathIn);
            StreamWriter srOut = new StreamWriter(pathOut);

            // Reading 'n' value from input file
            string nRaw = srIn.ReadLine();
            int n = 0;
            if (nRaw != null)
            {
                n = int.Parse(nRaw);
                if (n == 1)
                {
                    Console.WriteLine("There is only one tree, you can't hang a hammock");
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Error while reading a file: 'n' cannot be null");
                Environment.Exit(-1);
            }

            // Creating lists of tuples for 'x' coordinate and 'y' coordinate
            List<Tree> xTrees = new List<Tree>();
            List<Tree> yTrees = new List<Tree>();
            for (int i = 0; i < n; i++)
            {
                string line = srIn.ReadLine();
                string[] split = line.Split(" ");

                xTrees.Add(new Tree(int.Parse(split[0]), int.Parse(split[1]), i));
                yTrees.Add(new Tree(int.Parse(split[0]), int.Parse(split[1]), i));
            }

            // Sorting 'xTrees' by 'x' coordinate and 'yTrees' by 'y' coordianate
            List<Tree> xTreesRec = MergeSort(xTrees);
            List<Tree> yTreesRec = xTreesRec;
            List<Tree> xTreesIt = xTreesRec;
            List<Tree> yTreesIt = xTreesRec;

            // Finding the result
            (Tree treeOneRec, Tree treeTwoRec, double deltaRec) = RecMethod(xTreesRec, yTreesRec);    // RECURSIVE
            (Tree treeOneIt, Tree treeTwoIt, double deltaIt) = ItMethod(xTreesIt, yTreesIt);          // ITERATIVE

            // Sort result to be in order of inputs
            (treeOneRec, treeTwoRec) = SortResults(treeOneRec, treeTwoRec, xTrees);
            (treeOneIt, treeTwoIt) = SortResults(treeOneIt, treeTwoIt, xTrees);
            recCounter += 2;
            itCounter += 2;
            
            // Displaying the results in terminal
            Console.WriteLine($"Recursive Method:" +                // RECURSIVE
                $"\n{treeOneRec}" +
                $"\n{treeTwoRec}" +
                $"\nDistance: {deltaRec}" +
                $"\nOperations: {recCounter}" +
                $"\n===================");
            Console.WriteLine($"Iterative Method:" +                // ITERATIVE
                $"\n{treeOneIt}" +
                $"\n{treeTwoIt}" +
                $"\nDistance: {deltaIt}" +
                $"\nOperations: {itCounter}" +
                $"\n===================");
            Console.WriteLine($"Sorting operations: {sortCounter}");

            // Saving results to file
            srOut.WriteLine($"Recursive method:" +                  // RECURSIVE
                $"\n{treeOneRec.x} {treeOneRec.y}" +
                $"\n{treeTwoRec.x} {treeTwoRec.y}");
            srOut.WriteLine($"\n\nIterative method:" +              // ITERATIVE
                $"\n{treeOneIt.x} {treeOneIt.y}" +
                $"\n{treeTwoIt.x} {treeTwoIt.y}");

            // Closing files and ending a program
            srIn.Close();
            srOut.Close();
            Console.ReadLine();
        }
    }
} // >>> O(n log n) - O(n^2)
