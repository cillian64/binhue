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

            ECDCScrape scraper = new ECDCScrape();
            await scraper.scrape(args[0]);

//            Hue hue = new Hue(args[0]);
//            await hue.Connect();
//            await hue.TestBlue();
        }
    }
}
