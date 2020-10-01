using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace parfor
{
    class Program
    {
        //Обычное перемножение матриц
        static double[,] Multiplicate(double[,] A,double[,] B, int width, int height, int n)
        {
            double[,] result = new double[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int s = 0; s < n; s++)
                    {
                        result[i, j] += A[i, s] * B[s, j];
                    }
                }
            }
            return result;
        }

        //Параллельное перемножение матриц
        static double[,] ParallelMultiplicate(double[,] A, double[,] B, int width, int height, int n)
        {
            double[,] result = new double[width, height];

            Parallel.For(0, width, (i) =>
           {
               //Parallel.For(0, height, (j) =>
               //{
               //    Parallel.For(0, n, (s) =>
               //    {
               //        result[i, j] += A[i, s] * B[s, j];
               //    });
               //    for (int s = 0; s < n; s++)
               //    {
               //        result[i, j] += A[i, s] * B[s, j];
               //    }
               //});
               for (int j = 0; j < height; j++)
               {
                   for (int s = 0; s < n; s++)
                   {
                       result[i, j] += A[i, s] * B[s, j];
                   }
               }
           });
            return result;
        }

        static void Main(string[] args)
        {
            Stopwatch sWatchOrdinary = new Stopwatch();
            sWatchOrdinary.Start();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            var lines = File.ReadAllLines(@"W:\Монте-Карло\MultipleMatrix\matrix.csv");
            var lines2 = File.ReadAllLines(@"W:\Монте-Карло\MultipleMatrix\matrix.csv");
            int i1 = lines.Length;
            int n1 = 0;
            int i2 = lines2.Length;
            int n2 = 0;
            

            foreach (string line in lines)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                    j++;
                n1 = j;
            }
            foreach (string line in lines2)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                    j++;
                n2 = j;
            }

            double[,] matrix1 = new double[i1,n1];
            double[,] matrix2 = new double[i2,n2];

            int r = 0;
            foreach (string line in lines)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    matrix1[r, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++r;
            }
            r--;
            int r2 = 0;
            foreach (string line in lines2)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    matrix2[r2, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++r2;
            }
            if (n1 == i2)
            {
                int weight = i1;
                int height = n2;
                int n = n1;
                double[,] result = new double[weight, height];

                
                //result = Multiplicate(matrix1, matrix2, weight, height, n);
                result = ParallelMultiplicate(matrix1, matrix2, weight, height, n);

                StreamWriter sw = new StreamWriter(@"W:\Монте-Карло\MultipleMatrix\answer.csv");
                for (int i = 0; i < weight; i++)
                {
                    for (int j = 0; j < height - 1; j++)
                        sw.Write(result[i, j] + ",");
                    sw.Write(result[i, height - 1]);
                    sw.WriteLine();
                }
                sw.Close();
            }
            else
                Console.WriteLine("Перемножение матриц невозможно");


            sWatchOrdinary.Stop();
            Console.WriteLine("Multiplication time: {0} mc", sWatchOrdinary.ElapsedMilliseconds.ToString());
            Console.ReadKey();
        }
    }
}
