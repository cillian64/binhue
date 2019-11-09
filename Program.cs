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

            ECDC council = new ECDC();
            var collections = await council.scrape(args[0]);
            foreach (var collection in collections)
            {
                Console.WriteLine("Which bin: " + collection.bin.contents +
                                  " (" + collection.bin.color.ToHex() + ")");
                Console.WriteLine("Date: " + collection.collectionDate);
            }

//            Hue hue = new Hue(args[0]);
//            await hue.Connect();
//            await hue.TestBlue();
        }
    }
}
