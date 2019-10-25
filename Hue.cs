using System;
using System.Threading.Tasks;
using System.Linq;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;

namespace binhue
{
    class Hue
    {
        LocalHueClient client;
        string appKey;

        public Hue(string appKey)
        {
            this.appKey = appKey;
        }

        public async Task Connect()
        {
            var locator = new HttpBridgeLocator();
            var bridgeIPs =
                await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
            var bridgeCount = bridgeIPs.Count();
            Console.WriteLine("Found " + bridgeCount + " Hue bridge" +
                              (bridgeCount == 1 ? "." : "s."));
            if (bridgeIPs.Count() == 0)
            {
                Console.WriteLine("Error: No Hue bridges found");
                return;
            }
            var bridge = bridgeIPs.ToArray()[0];
            Console.WriteLine("Connecting to bridge " + bridge.BridgeId + "...");

            client = new LocalHueClient(bridge.IpAddress);
            client.Initialize(this.appKey);

            Console.WriteLine("Hue client initialized.");
        }

        public async Task TestBlue()
        {
            var command = new LightCommand();
            // ColorConverter by default assumes a LCT001 fixture, the gamut
            // of which is very limited in the green direction.  Specify a
            // "Richer Colours" fixture so we can get green instead of lime.
            command.TurnOn().SetColor(new RGBColor("0000FF"), "LCT015");
            var results = await client.SendCommandAsync(command);
            if (results.HasErrors())
            {
                Console.WriteLine("Errors encountered in setting lights:");
                foreach (var res in results.Errors)
                {
                    Console.WriteLine("Address: " + res.Error.Address + ", " +
                                      "Type: " + res.Error.Type + ", " +
                                      "Description: " + res.Error.Description);
                }
            } else {
                Console.WriteLine("Light test successful.");
            }
        }
    }
}
