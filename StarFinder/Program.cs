using System;
using System.Threading.Tasks;
using NBitcoin;

namespace StarFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GroundStation gs = new GroundStation();

            gs.Run();

            Console.ReadLine();
        }
    }
}
