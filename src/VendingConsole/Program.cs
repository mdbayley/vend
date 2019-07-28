using System;
using System.Collections.Generic;
using VendOMatic;

namespace VendingConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var cashFloat = new List<Denomination>
                {
                    new Denomination {Value = 0.01m, Count = 5},
                    new Denomination {Value = 0.02m, Count = 5},
                    new Denomination {Value = 0.05m, Count = 5},
                    new Denomination {Value = 0.10m, Count = 5},
                    new Denomination {Value = 0.20m, Count = 5},
                    new Denomination {Value = 0.50m, Count = 5}
                };

                const decimal requiredTotal = 1.23M;

                if (cashFloat.TryMakeChange(requiredTotal, out var change))
                {
                    foreach (var denomination in change)
                    {
                        Console.WriteLine(denomination);
                    }
                }
                else
                {
                    Console.WriteLine("Cannot make change.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.Write("Done.");
                Console.ReadKey();
            }
        }
    }
}
