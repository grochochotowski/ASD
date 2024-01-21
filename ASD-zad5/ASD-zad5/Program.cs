using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace ASD_zad5
{
    /// Asd- landia słynie z bogatych złóż złota, dlatego przez lata kwitnie sprzedaż tego kruszcu do
    /// sąsiedniego królestwa, GRAF-landii.Niestety powiększająca się ostatnio dziura budżetowa zmusiła
    /// króla GRAF-landii do wprowadzenia zaporowych ceł na metale i minerały
    /// 
    /// Handlarze złota przekraczający granicę muszą zapłacić 50% wartości przewożonego ładunku.ASDlandzkim grozi bankructwo.
    /// Na szczęście alchemicy tego kraju opracowali sposoby pozwalające zamienić pewne metale na inne.
    /// Kupcy chcą zamieniać przy pomocy chemików złoto w pewne
    /// tanie metale, a następnie po przewiezieniu przez granice i zapłaceniu niewielkiego cła, znowu
    /// otrzymywać z niego złoto.Niestety alchemicy nie znaleźli sposobu na zamianę dowolnego metalu
    /// w dowolny inny.Może się więc zdarzyć, że proces otrzymania danego metalu ze złota musi
    /// przebiegać wielostopniowo i że na każdym etapie i że na każdym etapie będzie uzyskiwany inny
    /// metal. Chemicy na każdy etapie znanego sobie procesu zamiany metalu A na metal B wyznaczyli
    /// cenę za przemianę 1 kg.Handlarze zastanawiają się , w jakiej postaci należy przewieźć złoto przez
    /// granicę oraz jaki ciąg procesów chemicznych należy zastosować aby zyski były możliwie
    /// największe.
    /// 
    /// Napisz program, który:
    ///     1.wczyta tabelę cen wszystkich metali, a także ceny przemian oferowanych przez alchemików.
    ///     
    ///     2.Wyznaczy taki ciąg metali m0, m1, …, mk, że
    ///     -m0= mk to złoto
    ///     -dla każdego i= 1,2,…, k chemicy potrafią otrzymać metal mi z metalu mi-1
    ///     -koszt wykonania całego ciągu procesów chemicznych dla 1 kg złota powiększony o płacone na
    ///     granicy cło (50% ceny 1 kg najtańszego metalu z metali mi, dla i = 1,2,..k) jest najmniejszy z
    ///     możliwych
    ///     Zakładamy, że podczas procesów chemicznych waga metali się nie zmienia.
    /// 
    ///     3. Wypisz koszt wykonania wyznaczonego ciągu procesów chemicznych powiększony o płacone na granicy cło.
    ///     
    /// Wejście:
    /// W pierwszej linii pliku znajduje się jedna dodatnia liczba całkowita n oznaczająca liczbę rodzajów
    /// metali 1<=n<=5000. W wierszu o numerze k+1, dla 1<=k<=n, znajduje się nieujemna parzysta
    /// liczba całkowita pk, cena 1 kg metalu oznaczonego numerem k, 0<=pk<=109.
    /// Przypuśćmy, że złoto ma numer 1. W wierszu o numerze n+2 znajduje się jedna liczba nieujemna całkowita m równa
    /// liczbie procesów przemiany znanych chemikom 0<=m<=10000. W każdym z kolejnych m wierszy
    /// znajdują się po trzy liczby naturalne, podzielone pojedynczymi odstępami, opisujące kolejne
    /// procesy przemiany.Trójka liczb a, b, c oznacza, że chemicy potrafią z metalu o numerze a
    /// otrzymać metal o numerze b i za zamianę 1 kg surowca każą sobie płacić c dolarów, 1<=a, b<=n,
    /// 0<=c<=10000. Uporządkowana para liczb a i b może się pojawić w danych co najwyżej jeden raz.
    /// 
    /// Wyjście:
    /// Twój program powinien w pliku w pierwszym wierszu wypisać jedną liczbę całkowitą- koszt
    /// wykonania ciągu procesów chemicznych powiększony o płacone na granicy cło
    /// 
    /// Przykład
    /// Dla danych wejściowych:
    /// 4
    /// 200
    /// 100
    /// 40
    /// 2
    /// 6
    /// 1 2 10
    /// 1 3 5
    /// 2 1 25
    /// 3 2 10
    /// 3 4 5
    /// 4 1 50
    /// Wyjście:
    /// 60

    internal class Program
    {
        /// ============================= Variables =============================
        public static int num_of_metals;
        public static int num_of_transformations;
        public static List<int> metals_prices = [];
        public static List<(int, int, int)> transformations = [];

        public static int price;
        

        /// ============================= Functions =============================
        public static int Route(int num_of_metals, List<int> metals_prices, List<(int, int, int)> transformations)
        {
            int sum;
            int[] cheapest = new int[num_of_metals];
            Array.Fill(cheapest, int.MaxValue);
            cheapest[0] = 0;
            int final = 0;

            foreach (var transformation in transformations)
            {
                int from_metal = transformation.Item1 - 1;
                int to_metal = transformation.Item2 - 1;
                int price = transformation.Item3;

                if (cheapest[from_metal] != int.MaxValue && cheapest[to_metal] + price < cheapest[to_metal])
                {
                    cheapest[to_metal] = cheapest[from_metal] + price;
                }
            }

            int min = int.MaxValue;
            for(int i = 0; i < num_of_metals; i++)
            {
                if (metals_prices[i] / 2 + cheapest[i] < min)
                {
                    min = (metals_prices[i] / 2 + cheapest[i]);
                    final = i;
                }
            }

            sum = cheapest[final] + (int)(0.5 * metals_prices[final]);

            Array.Fill(cheapest, int.MaxValue);
            cheapest[0] = 0;
            foreach (var transformation in transformations)
            {
                int from_metal = transformation.Item1 - 1;
                int to_metal = transformation.Item2 - 1;
                int price = transformation.Item3;

                if (cheapest[from_metal] != int.MaxValue && cheapest[to_metal] + price < cheapest[to_metal])
                {
                    cheapest[to_metal] = cheapest[from_metal] + price;
                }
            }

            sum += cheapest[0];

            return sum;
        }


        /// =============================== Main ================================
        static void Main(string[] args)
        {
            // Creating paths for input and output files
            string location = @"C:\nonSystem\IT\Code\ASD\ASD-zad5\ASD-zad5\";
            string pathIn = location + "XX_in_ps7_Grochowski.txt";
            string pathOut = location + "XX_out_ps7_Grochowski.txt";

            /// ========================= READING INPUT DATA ========================
            StreamReader reader = new StreamReader(pathIn);

            var line = reader.ReadLine();
            num_of_metals = int.Parse(line!); // number of metals

            for (int i = 0; i < num_of_metals; i++)
            {
                line = reader.ReadLine();
                var pk = int.Parse(line!); // price of metal k

                metals_prices.Add(pk);
            }


            line = reader.ReadLine();
            num_of_transformations = int.Parse(line!); // number of transformations

            for (int i = 0; i < num_of_transformations; i++)
            {
                line = reader.ReadLine();
                string[] lineElements = line!.Split(' ');

                transformations.Add((           //
                    int.Parse(lineElements[0]), //
                    int.Parse(lineElements[1]), // transformation
                    int.Parse(lineElements[2])  //
                ));                             //
            }


            reader.Close();
            /// ============================= CALCULATING RESULST =============================
            price = Route(num_of_metals, metals_prices, transformations);


            /// ============================= SHOWING RESULTS =============================
            Console.WriteLine("========== INPUT DATA ==========");
            Console.WriteLine($"{num_of_metals} metals:");
            for (int i = 0; i < num_of_metals; i++) Console.WriteLine($"\t{metals_prices[i]}");
            Console.WriteLine($"\n{num_of_transformations} transformations:");
            for (int i = 0; i < num_of_transformations; i++) Console.WriteLine($"\t{transformations[i].Item1} + {transformations[i].Item2} -> {transformations[i].Item3}");
            Console.WriteLine("================================");
            Console.WriteLine("============ OUTPUT ============");
            Console.WriteLine($"Price: {price}");
            Console.WriteLine("================================");

            Console.ReadLine(); // Program written by Michał Grochowsk
        }
    }
}
