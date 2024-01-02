using System.Diagnostics.Metrics;
using System.Numerics;

namespace ASD_zad3_B
{
    class Note
    {
        public int value; // Value of notes
        public int number; // Number of notes
        public Note(int v, int n)
        {
            this.value = v;
            this.number = n;
        }
        public override string ToString()
        {
            return $"{value}\t{number}";
        }
    }
    internal class Program
    {
        /// ============================= Variables =============================
        public static int n = 0;
        public static List<int> valuesOfNotes = new();
        public static List<int> numberOfNotes = new();
        public static List<Note> notes = new();
        public static int k = 0;

        public static int[]? dp;
        public static List<List<int>>? chosenNotes;


        public static int numberOfChangeNotes = 0;
        public static List<int> changeNotes = new();
        public static Dictionary<int, int> counts = new();
        public static BigInteger mainAlgorythmCounter = 0;
        public static BigInteger totalCounter = 0;

        /// ============================= Functions =============================
        static void Change()
        {
            dp = Enumerable.Repeat(k + 1, k + 1).ToArray();
            dp[0] = 0;
            chosenNotes = new List<List<int>>();
            for (int i = 0; i < k + 1; i++)
            {
                chosenNotes.Add(new List<int>());
            }
            chosenNotes[0].Add(0);

            for (int i = 1; i < k + 1; i++)
            {
                foreach (Note note in notes) {
                    if (note.value == i)
                    {
                        chosenNotes[i] = new();
                        chosenNotes[i].Add(note.value);
                        dp[i] = 1;
                    }
                    else if (note.value <= i && i - note.value > 0)
                    {
                        int tempIndex = i - note.value;
                        if (chosenNotes[tempIndex].Count != 0)
                        {
                            List<int> tempChosenNotes = new();
                            tempChosenNotes.Add(note.value);
                            foreach (int value in chosenNotes[tempIndex])
                            {
                                tempChosenNotes.Add(value);
                            }

                            var counts = tempChosenNotes.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                            bool isValid = true;
                            foreach (var count in counts)
                            {
                                int valueCount = count.Value;
                                int inputValueCount = notes.First(n => n.value == count.Key).number;
                                if (valueCount > inputValueCount)
                                {
                                    isValid = false;
                                    break;
                                }
                                mainAlgorythmCounter++;
                            }

                            if (isValid && tempChosenNotes.Count < dp[i])
                            {
                                dp[i] = tempChosenNotes.Count;
                                chosenNotes[i] = tempChosenNotes;
                            }
                            mainAlgorythmCounter++;
                        }
                        mainAlgorythmCounter++;
                    }
                    mainAlgorythmCounter++;
                }
            }
        }
        static void CountResultNotes()
        {
            foreach (var value in valuesOfNotes)
            {
                counts[value] = 0;
            }
            foreach (var note in changeNotes)
            {
                if (counts.ContainsKey(note))
                {
                    counts[note]++;
                }
                totalCounter++;
            }   
        }

        /// ============================= Main =============================
        static void Main(string[] args)
        {
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad3\ASD-zad3-B\ASD-zad3-B\";
            string pathIn = location + "PKO_in_ps7_Grochowski.txt";
            string pathOut = location + "PKO_out_ps7_Grochowski.txt";

            /// ============================= READING INPUT DATA =============================
            StreamReader srIn = new(pathIn);

            ///n                number of values 1 <= n <= 200
            ///b1 b2 b3 ...     values 1 <= b1 <= b2 <= ... <= bn <= 20000
            ///c1 c2 c3 ...     number 1 <= ci <= 20000
            ///k                money cashier is supposed to give as change 1 <= k <= 20000 (Always possible)

            // Reading n
            if (!int.TryParse(srIn.ReadLine(), out n))
            {
                throw new ArgumentException("ERROR: 'n' has to be type 'integer'");
            }
            else if (!(1 <= n && n <= 200))
            {
                throw new ArgumentException("ERROR: 'n' must be in range [1; 200]");
            }

            // Reading values of notes
            string valuesOfNotesLine = srIn.ReadLine()!;
            if (string.IsNullOrEmpty(valuesOfNotesLine))
            {
                throw new ArgumentException("ERROR: Line with values of notes cannot be empty");
            }
            string[] valuesOfNotesLineSplit = valuesOfNotesLine.Split();
            if (valuesOfNotesLineSplit.Length != n)
            {
                throw new ArgumentException("ERROR: The count of note values must be equal to 'n'");
            }
            foreach (string value in valuesOfNotesLineSplit)
            {
                if (int.TryParse(value, out int parsedValue))
                {
                    valuesOfNotes.Add(parsedValue);
                }
                else
                {
                    throw new ArgumentException($"ERROR: Invalid value '{value}' for note");
                }
                totalCounter++;
            }

            // Reading number of notes
            string numberOfNotesLine = srIn.ReadLine()!;
            if (string.IsNullOrEmpty(numberOfNotesLine))
            {
                throw new ArgumentException("ERROR: Line with number of notes cannot be empty");
            }
            string[] numberOfNotesLineSplit = numberOfNotesLine.Split();
            if (numberOfNotesLineSplit.Length != n)
            {
                throw new ArgumentException("ERROR: The count of notes should match the expected number 'n'");
            }
            foreach (string number in numberOfNotesLineSplit)
            {
                if (int.TryParse(number, out int parsedValue))
                {
                    numberOfNotes.Add(parsedValue);
                }
                else
                {
                    throw new ArgumentException($"ERROR: Invalid value '{number}' for number of notes");
                }
                totalCounter++;
            }

            // Reading k
            if (!int.TryParse(srIn.ReadLine(), out k))
            {
                throw new ArgumentException("ERROR: 'k' has to be type 'integer'");
            }
            else if (!(1 <= k && k <= 20000))
            {
                throw new ArgumentException("ERROR: 'k' must be in range [1; 20000]");
            }

            // Creating list of notes
            for (int i = 0; i < n; i++)
            {
                Note addNote = new Note(valuesOfNotes[i], numberOfNotes[i]);
                notes.Add(addNote);
            }

            totalCounter += 6;
            srIn.Close();

            /// ============================= CALCULATING RESULST =============================
            Change();
            numberOfChangeNotes = dp![k];
            changeNotes = chosenNotes![k];

            /// ============================= SHOWING RESULTS =============================
            // Displaying input data in terminal
            Console.WriteLine("===============INPUT===============");
            Console.WriteLine($"Different notes: {n}\nValue\tNumber");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(notes[i]);
            }
            Console.WriteLine($"Change to give: {k}");

            // Saving results to output file
            StreamWriter srOut = new StreamWriter(pathOut);
            if (dp[k] != k + 1)
            {
                CountResultNotes();
                srOut.WriteLine($"{numberOfChangeNotes}");
                foreach (var value in valuesOfNotes)
                {
                    srOut.Write($"{counts[value]} ");
                }
            }
            else
            {
                srOut.WriteLine("0");
            }
            totalCounter++;
            srOut.Close();

            // Displaying the results in terminal
            totalCounter += mainAlgorythmCounter + 1;
            Console.WriteLine("\n\n=============OUTPUT============");
            Console.WriteLine($"Main algorythm operations: {mainAlgorythmCounter}");
            Console.WriteLine($"Total operations: {totalCounter}\n");
            Console.WriteLine($"Number of notes: {numberOfChangeNotes}\nNotes:");
            if (dp[k] != k + 1)
            {
                foreach (var note in changeNotes)
                {
                    Console.Write($"{note} ");
                }
            }
            else
            {
                Console.WriteLine("Wydanie reszty nie jest możliwe");
            }


            Console.ReadLine(); // Program written by Michał Grochowski
        }
    }
}