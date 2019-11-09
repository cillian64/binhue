using System;
using System.Threading.Tasks;

namespace binhue
{
    class Program
    {
        async static Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: binhue appkey");
                return;
            }

            ECDC council = new ECDC(args[0]);
            var bins = await council.getTomorrowBins();
            if (bins.Count == 0)
            {
                Console.WriteLine("No collections tomorrow.");
            }
            else
            {
                Console.WriteLine("Collections tomorrow: ");
                foreach (var bin in bins)
                {
                    Console.WriteLine(bin.contents + " ("
                                      + bin.color.ToHex() + ")");
                }
            }

//            Hue hue = new Hue(args[0]);
//            await hue.Connect();
//            await hue.TestBlue();
        }
    }
}
