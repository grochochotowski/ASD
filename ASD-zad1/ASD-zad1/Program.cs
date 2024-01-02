using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.CodeDom.Compiler;

namespace ASD_zad1
{
    internal class Program
    {   /*
            Wariant A – „Symbol Newtona”
            WP: n, m – liczby naturalne takie, e n, m<=10000 i m<=n
            WK: wartosymbolu Newtona dla danego n i m
            Opis wejcia i wyjcia: Na wejciu, w pliku tekstowym o nazwie in_A_grupa_nazwisko.txt
            podawane s w tej samej linii, oddzielone pojedyncz spacj wartoci n i m spełniajce WP.
            Na wyjciu, w pliku tekstowym out_A_grupa_nazwisko.txt podawane s w oddzielnych
            liniach co najmniej dwie liczby, bdce wartoci symbolu Newtona dla n i m o wartoci
            spełniajcej WP, jako wyniki rónych rozwiza zadania.
      
        Przykład
            in_A_1_Kowalski.txt:
            5 3
            out_A_1_Kowalski.txt:
            10
            10       
        */
        static BigInteger[] Sposob1(int n, int m)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int licznikOperacji = 0;
            BigInteger wynik = 1;

            m = Math.Min(m, n - m);

            for (int i = 1; i <= m; i++) ///=============================================== Ilośc operacji: 2*m (sposób bardziej efektywny)
            {
                wynik *= n - i + 1;
                wynik /= i;
                licznikOperacji += 2;
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            BigInteger [] zwrot = { wynik, licznikOperacji, elapsedMs };
            return zwrot;
        }
        static BigInteger[] Sposob2(int n, int m)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int licznikOperacji = 0;
            BigInteger[] wspolczynnik = new BigInteger[m + 1];
            wspolczynnik[0] = 1;

            for (int i = 1; i <= n; i++) ///=============================================== Ilość operacji: n*m
            {
                for (int j = m; j > 0; j--)
                {
                    wspolczynnik[j] = wspolczynnik[j] + wspolczynnik[j - 1];
                    licznikOperacji++;
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            BigInteger[] wynik = { wspolczynnik[m], licznikOperacji, elapsedMs };
            return wynik;
        }
        static void Main(string[] args)
        {
            // ======================= WP =======================
            // Liczby naturalne takie, że n,m < 10000 oraz m <= n
            // ==================================================
            
            StreamReader srIn = new StreamReader("../../in_A_ps7_Grochowski.txt");
            StreamWriter srOut = new StreamWriter("../../out_A_ps7_Grochowski.txt");
        
            // Czytanie danych
            string[] fileInput = srIn.ReadToEnd().Split(' ');
            int n = Int32.Parse(fileInput[0]);
            int m = Int32.Parse(fileInput[1]);
            srIn.Close();


            // SPOSÓB 1:
            //      C(n, m) = ((n-1) * (n-2) * ... * (n-m+1)) / m!
            try
            {
                BigInteger[] wynik1 = Sposob1(n, m);
                Console.WriteLine($"Sposób 1:\nSymbol Newtona dla C({n},{m}) wynosi: {wynik1[0]}" +
                    $"\n\t> Ilość operacji elementarnych: {wynik1[1]}" +
                    $"\n\t> Zajęło to {(wynik1[2] < 1000 ? wynik1[2].ToString() : (wynik1[2] / 1000).ToString())} {(wynik1[2] < 1000 ? "ms" : "s")}");
                srOut.WriteLine($"{wynik1[0]}");
            }
            catch
            {
                Console.WriteLine($"Sposób 1:\nSymbolu Newtona dla C({n},{m}) nie da się obliczyć tym sposobem"); 
            }


            // SPOSÓB 2:
            //      C(n, m) = C(n-1, m-1) + C(n-1, m)
            try
            {
                BigInteger[] wynik2 = Sposob2(n, m);
                Console.WriteLine($"\nSposób 2:\nSymbol Newtona dla C({n},{m}) wynosi: {wynik2[0]}" +
                    $"\n\t> Ilość operacji elementarnych: {wynik2[1]}" +
                    $"\n\t> Zajęło to {(wynik2[2] < 1000 ? wynik2[2].ToString() : (wynik2[2] / 1000).ToString())} {(wynik2[2] < 1000 ? "ms" : "s")}");
                srOut.WriteLine($"{wynik2[0]}");
            }
            catch
            {
                Console.WriteLine($"\nSposób 2:\nSymbolu Newtona dla C({n},{m}) nie da się obliczyć tym sposobem");
            }


            srOut.Close();
            Console.ReadLine();
        }
    }
}
    