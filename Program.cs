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
            var collections = await council.scrape();
            foreach (var collection in collections)
            {
                Console.WriteLine("Which bin: " + collection.bin.contents +
                                  " (" + collection.bin.color.ToHex() + ")");
                Console.WriteLine("Date: " + collection.collectionDate);
            }

            Council.Bin bin = await council.getTomorrowBin();
            if (bin != null)
            {
                Console.WriteLine("Collection tomorrow: " + bin.contents +
                                  " (" + bin.color.ToHex() + ")");
            }
            else
            {
                Console.WriteLine("No collection tomorrow.");
            }

//            Hue hue = new Hue(args[0]);
//            await hue.Connect();
//            await hue.TestBlue();
        }
    }
}
