using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        static double InCircle(int points)
        {
            int inC = 0;
            double x, y;
            double Pi;
            Random random = new Random();
            for (int i = 0; i < points; i++)
            {
                x = random.NextDouble();
                y = random.NextDouble();
                if (((x * x) + (y * y)) < 1.0)
                    inC++;
            }
            Pi = (4.0 * inC) / points;
            return Pi;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int flow = int.Parse(Console.ReadLine());
            int points = int.Parse(Console.ReadLine());
            int total = flow * points;
            double[] Pi = new double[flow];
            foreach (int i in Pi)
                Pi[i] = 0.0;
            Task<double>[] task = new Task<double>[flow];
            for (int i = 0; i < flow; i++)
            {
                //Pi[i] = InCircle(points);
                task[i] = new Task<double>(x => InCircle((int)x), points);
                task[i].Start();

            }
            for (int i = 0; i < flow; i++)
                Pi[i] = task[i].Result;
            double fullPi = Pi.Sum() / flow;
            Console.WriteLine("Pi = {0}",fullPi);
            Console.Write("Time: {0}",sw.ElapsedMilliseconds.ToString());
            Console.ReadKey();
        }
    }
}
