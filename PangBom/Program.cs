using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangBom
{
    class Program
    {
        static void Main(string[] args)
        {
            // 39st elever
            for (int i = 1; i <= 39; i++)
            {
                if (i % 3 == 0)
                {
                    if (i % 5 == 0)
                    {
                        Console.WriteLine("PangBom");
                    }
                    else
                    {
                        Console.WriteLine("Pang");
                    }
                }
                else if (i % 5 == 0)
                {
                    Console.WriteLine("Bom");
                }
                else
                {
                    Console.WriteLine(i);
                }
            }

            Console.ReadKey();
        }
    }
}
