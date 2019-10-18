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
                Console.WriteLine("Usage: binhue url");
                return;
            }

            ECDCScrape scraper = new ECDCScrape();
            await scraper.scrape(args[0]);
        }
    }
}
