using System;
using System.Linq;
using System.Threading.Tasks;

namespace binhue
{
    class Program
    {
        async static Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: binhue councilURL appkey");
                return;
            }
            string councilURL = args[0];
            string appKey = args[1];

            // TODO(dwt): Use a config file instead of commandline args.

            ECDC council = new ECDC(councilURL);
            var bins = await council.getTomorrowBins();
            if (bins.Count == 0)
            {
                Console.WriteLine("No collections tomorrow.");
                return;
            }

            Console.WriteLine("Collections tomorrow: ");
            foreach (var bin in bins)
            {
                Console.WriteLine(bin.contents + " ("
                                  + bin.color.ToHex() + ")");
            }

            // Filter out black bins, which we can't display using lights...
            bins = bins.Where(x => x.color.ToHex() != "000000").ToList();

            if (bins.Count == 0)
            {
                Console.WriteLine("Only black bin collection tomorrow.");
                return;
            }
            else if (bins.Count > 1)
            {
                Console.WriteLine("Multiple coloured bin collections " +
                                  "tomorrow.");
            }

            Hue hue = new Hue(appKey);
            await hue.Connect();
            await hue.SetColour(bins[0].color);


        }
    }
}
