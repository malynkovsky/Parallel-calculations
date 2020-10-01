using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace IntegralTrapecia
{
    public struct OneTrap
    {
        public int n;
        public double x1;
        public double x2;
        public OneTrap(int n, double x1, double x2)
        {
            this.n = n;
            this.x1 = x1;
            this.x2 = x2;
        }
        
    }
    class Program
    {
        static double ParallelSquare(OneTrap z)
        {
            double[] Summa = new double[z.n];
            double x1 = z.x1;
            double x2 = z.x2;
            double dx = (x2 - x1) / z.n;
            List<OneTrap> s = new List<OneTrap>(z.n);
            for (int i = 0; i < z.n; i++)
            {
                s.Add(new OneTrap(1, (x1 + (dx * i)), (x1 + ((i + 1) * dx))));
                Summa[i] = ((Math.Tan(s[i].x1) + (1.0 / Math.Tan(s[i].x1)) + Math.Tan(s[i].x2) + (1.0 / Math.Tan(s[i].x2))) * ((s[i].x2 - s[i].x1))) / 2; 
            }
            return Summa.Sum();
        }
        static void Main(string[] args)
        {
            //на вход даём a-начало отрезка ,b- конец отрезка,n- разбиение,p- кол-во потоков(задач)
            // tg(x)+ctg(x),   особые точки pi*k, pi/2 +pi*k
            double a, b;
            int n, p;
            do
            {
                Console.Write("Введите начало отрезка интегрирования: ");
                a = double.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("Введите конца отрезка интегрирования: ");
                b = double.Parse(Console.ReadLine());
                Console.WriteLine(); 
                Console.Write("Введите количество шагов: ");
                n = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("Введите количество потоков: ");
                p = int.Parse(Console.ReadLine());
                Console.WriteLine();
                if ((Math.Abs(b - a) < Math.PI / 2.0) && (n > 1) && (p > 1) && (a < b))
                    break;
                else
                    Console.WriteLine("Введённые данные некорректные(в отрезок попадают особые точки)");
            } while (true);
            
            //начинаем считать интеграл
            Stopwatch sw = new Stopwatch();
            sw.Start();//для отслеживания времени на подсчёты
            
            double dy = (b - a) / p;//шаг интегрирования
            double[] y = new double[n];
            Task<double>[] task1 = new Task<double>[p];
            List<OneTrap> trapecia = new List<OneTrap>(p);
            double[] FirstSum = new double[n];

            for (int i = 0; i < p; i++)
            {
                trapecia.Add(new OneTrap(n, a + (i * dy), a + ((i + 1) * dy)));
                task1[i] = new Task<double>(x => (ParallelSquare((OneTrap)x)),trapecia[i]);
                task1[i].Start();
                FirstSum[i] = task1[i].Result;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            Console.WriteLine("Значение интеграла = {0}", FirstSum.Sum());
            Console.ReadKey();
        }
    }
}
