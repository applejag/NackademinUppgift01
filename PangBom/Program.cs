using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangBom
{
    class Program
    {
        const int PANG = 3;
        const int BOM = 5;

        static void Main(string[] args)
        {

            // 39st elever
            for (int i = 1; i <= 100; i++)
            {
                if (i % PANG == 0)
                {
                    if (i % BOM == 0)
                    {
                        Console.WriteLine("PangBom");
                    }
                    else
                    {
                        Console.WriteLine("Pang");
                    }
                }
                else if (i % BOM == 0)
                {
                    Console.WriteLine("Bom");
                }
                else
                {
                    Console.WriteLine(i);
                }
            }

            ConsoleHelper.WaitBeforeExit();
        }
    }
}
