using System.Numerics;

namespace ASD_zad3_C
{
    public class Hotel
    {
        public int id;
        public int dist;
        public int cost;
        public Hotel(int id, int dist, int cost)
        {
            this.id = id;
            this.dist = dist;
            this.cost = cost;
        }
        public Hotel(Hotel cpy)
        {
            this.id = cpy.id;
            this.dist = cpy.dist;
            this.cost = cpy.cost;
        }
        public override string ToString()
        {
            return $"ID: {id}, D:{dist}, C:{cost}";
        }
    }
    internal class Program
    {
        /// ============================= Variables =============================
        public static int totalDistance;
        public static int numberOfHotels;
        public static List<Hotel> allHotels = new();

        public static List<Tuple<int, int>>[]? adjacencyList;
        public static Tuple<int, int>? minHeap;
        public static int[]? distances;
        public static bool[]? visited;
        public static int[]? previous;

        public static List<Hotel> resultHotels = new();
        public static BigInteger adjacencyCounter = 0;
        public static BigInteger dijkstraCounter = 0;
        public static BigInteger totalCounter = 0;

        /// ============================= Functions =============================
        public static void CreateAdjacencyList()
        {
            adjacencyList = new List<Tuple<int, int>>[allHotels.Count];
            for (int i = 0; i < allHotels.Count; i++)
            {
                adjacencyList[i] = new List<Tuple<int, int>>();
                for (int j = i+1; j < allHotels.Count; j++)
                {
                    if (Math.Abs(allHotels[i].dist - allHotels[j].dist) <= 800)
                    {
                        int id = allHotels[j].id;
                        int cost = allHotels[j].cost;
                        adjacencyList[i].Add(new Tuple<int, int>(id, cost));
                    }
                    adjacencyCounter++;
                }
            }
        }

        public static void DijkstraAlgoythm()
        {
            distances = new int[allHotels.Count];
            visited = new bool[allHotels.Count];
            previous = new int[allHotels.Count];
            for (int i = 0; i < allHotels.Count; i++)
            {
                distances[i] = int.MaxValue;
                visited[i] = false;
                previous[i] = -1;
            }
            distances[0] = 0;

            for (int count = 0; count < allHotels.Count - 1; count++)
            {
                int u = MinDistance();
                visited[u] = true;

                foreach (var neighbor in adjacencyList![u])
                {
                    int v = neighbor.Item1;
                    int cost = neighbor.Item2;

                    if (!visited[v] && distances[u] != int.MaxValue && distances[u] + cost < distances[v])
                    {
                        distances[v] = distances[u] + cost;
                        previous[v] = u;
                    }
                    dijkstraCounter++;
                }
            }
        }
        
        private static int MinDistance()
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < allHotels.Count; v++)
            {
                if (visited![v] == false && distances![v] <= min)
                {
                    min = distances[v];
                    minIndex = v;
                }
                dijkstraCounter++;
            }

            return minIndex;
        }

        private static void CreateResult()
        {
            int hotelID = allHotels.Count() - 1;
            do
            {
                resultHotels.Add(new Hotel(allHotels[hotelID]));
                hotelID = previous![hotelID];

            } while (hotelID != -1);
        }

        /// ============================= Main =============================
        static void Main(string[] args)
        {
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad3\ASD-zad3-C\ASD-zad3-C\";
            string pathIn = location + "SW_in_ps7_Grochowski.txt";
            string pathOut = location + "SW_out_ps7_Grochowski.txt";

            /// ============================= READING INPUT DATA =============================
            StreamReader srIn = new(pathIn);

            ///
            /// d h                 d - total distance to commute, h - number of hotels on route
            /// (dist cost) x h     dist - distance from beginning, cost - price of each hotel
            /// 
            /// There are h lines with hotels
            /// Hotels connects with each other when distance between them is <= 800
            /// 
            /// d in Z && d <= 16 000
            /// h in Z && h <= 1000
            /// 

            // Reading total distance and number of hotels (d, h)
            string mainInfo = srIn.ReadLine()!;
            if (string.IsNullOrEmpty(mainInfo))
            {
                throw new ArgumentException("ERROR: Line with main route info cannot be empty");
            }
            string[] mainInfoSplit = mainInfo.Split();
            if (mainInfoSplit.Length != 2)
            {
                throw new ArgumentException("ERROR: Number of arguments in the first line is incorrect. There should be 2 values.");
            }
            if (int.TryParse(mainInfoSplit[0], out int parsedDistance))
            {
                if (parsedDistance < 0 || parsedDistance > 16000)
                {
                    throw new ArgumentException("Error: Distance must be in <1;16 000>");
                }
                totalDistance = parsedDistance;
                totalCounter++;
            }
            else
            {
                throw new ArgumentException("ERROR: 'TotalDistance' has to be type 'integer'");
            }
            if (int.TryParse(mainInfoSplit[1], out int parsedNoHotels))
            {
                if (parsedNoHotels < 0 || parsedNoHotels > 1000)
                {
                    throw new ArgumentException("Error: Number of hotels must be in <1;1000>");
                }
                numberOfHotels = parsedNoHotels;
                totalCounter++;
            }
            else
            {
                throw new ArgumentException("ERROR: 'NumberOfHotels' has to be type 'integer'");
            }
            totalCounter += 4;

            // Reading hotels
            Hotel startPoint = new(0, 0, 0);
            allHotels.Add(startPoint);
            for (int i = 0; i < numberOfHotels; i++)
            {
                string hotelInfo = srIn.ReadLine()!;
                if (string.IsNullOrEmpty(hotelInfo))
                {
                    throw new ArgumentException($"ERROR: There are too few hotels - there are {i + 1}/{numberOfHotels} hotels given");
                }
                string[] hotelInfoSplit = hotelInfo.Split();
                if (mainInfoSplit.Length != 2)
                {
                    throw new ArgumentException($"ERROR: Number of arguments in the line: {i + 1} is incorrect. There should be 2 values.");
                }
                if (int.TryParse(hotelInfoSplit[0], out int parsedHotelDistance))
                {
                    if (parsedHotelDistance < 0 || parsedHotelDistance > 16000)
                    {
                        throw new ArgumentException("Error: Distance from start must be in <1;16 000>");
                    }
                    totalCounter++;
                }
                else
                {
                    throw new ArgumentException("ERROR: 'dist' has to be type 'integer'");
                }
                if (!int.TryParse(hotelInfoSplit[1], out int parsedHotelCost))
                {
                    throw new ArgumentException("ERROR: 'cost' has to be type 'integer'");
                }
                Hotel hotelToAdd = new Hotel(i+1, parsedHotelDistance, parsedHotelCost);
                allHotels.Add(hotelToAdd);
                totalCounter += 4;
            }
            Hotel endPoint = new(allHotels.Count, totalDistance, 0);
            allHotels.Add(endPoint);

            srIn.Close();

            /// ============================= CALCULATING RESULST =============================
            CreateAdjacencyList();
            DijkstraAlgoythm();
            CreateResult();

            /// ============================= SHOWING RESULTS =============================
            // Displaying input data in terminal
            Console.WriteLine("===============INPUT===============");
            Console.WriteLine($"Total distance: {totalDistance}\nNumber of hotels: {numberOfHotels}\nPoints on route:");
            for (int i = 0; i < allHotels.Count; i++)
            {
                Console.Write($"{i}. {allHotels[i]}");
                if (i == 0) Console.Write("\t----------- Start Point");
                else if (i == allHotels.Count-1) Console.Write("\t----------- End Point");
                Console.Write('\n');
                totalCounter++;
            }

            // Displaying adjacency list in terminal
            Console.WriteLine("\n\n=========ADJACENCY LIST========");
            for (int i = 0; i < adjacencyList!.Length; i++)
            {
                Console.Write($"{i}: ");
                for (int j = 0; j < adjacencyList[i].Count; j++)
                {
                    var tuple = adjacencyList[i][j];
                    if (j == adjacencyList[i].Count - 1) Console.Write($"{tuple.Item1}({tuple.Item2})");
                    else Console.Write($"{tuple.Item1}({tuple.Item2}), ");
                    totalCounter++;
                }
                Console.WriteLine();
            }

            // Displaying Dijkstra algorythm result in terminal
            Console.WriteLine("\n\n=======DIJKSTRA ALGORYTM=======");
            for (int i = 0; i < allHotels.Count; i++)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                if (distances![i] == int.MaxValue) Console.WriteLine($"{i}: cost: \u221E, prev: {previous![i]}");
                else Console.WriteLine($"{i}: cost: {distances![i]}, prev: {previous![i]}");
                totalCounter++;
            }

            // Displaying results in terminal
            totalCounter += adjacencyCounter + dijkstraCounter + 2;
            Console.WriteLine("\n\n=============OUTPUT============");
            Console.WriteLine($"Creating adjacency list operations: {adjacencyCounter}");
            Console.WriteLine($"Dijkstra algorythm operations: {dijkstraCounter}");
            Console.WriteLine($"Total operations: {totalCounter}");
            if (resultHotels.Count > 1)
            {
                Console.WriteLine($"\nNumber of hotels to stay: {resultHotels.Count-2}\nStops:");
                for (int i = 0; i < resultHotels.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {resultHotels[resultHotels.Count - i - 1]}");
                }
            }
            else
            {
                Console.WriteLine("There are no possible routes");
            }

            // Saving results to output file
            StreamWriter srOut = new StreamWriter(pathOut);
            if (resultHotels.Count > 1)
            {
                srOut.WriteLine($"{resultHotels.Count - 2}");
                for (int i = resultHotels.Count - 2; i > 0; i--)
                {
                    srOut.Write($"{resultHotels[i].dist} ");
                }
            }
            else
            {
                srOut.WriteLine("0");
            }
            srOut.Close();



            Console.ReadLine(); // Program written by Michał Grochowski
        }

    }
}
