using System;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using Q42.HueApi.ColorConverters;  // for RGBColor

namespace binhue
{
    class ECDCScrape : Council
    {
        HttpClient client = new HttpClient();

        private struct BinCollection
        {
            public Bin bin;
            public DateTime collectionDate;
        }

        // Scrape the ECDC webpage to get a list of bin collections
        public async Task scrape(string url)
        {
            // Load webpage
            var response = await client.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();

            // Parse contents to rows
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);
            string xpath = "//div[contains(@class,'collectionsrow')]";
            var rows = pageDocument.DocumentNode.SelectNodes(xpath);
            if (rows == null)
            {
                throw new ParseException("Can't see any bin collections.");
            }

            // Print out row contents
            foreach (var row in rows)
            {
                var contents = row.InnerText.Trim();
                if (contents == "")
                {
                    continue;
                }

                string[] lines = contents.Split('\n');
                Console.WriteLine("Which bin: " + lines[0]);

                string dateStr = lines[1].Split('-')[1].Trim();
                var day = DateTime.Parse(dateStr);
                Console.WriteLine("Date: " + day);
            }
        }

        // Convert the text on the website (e.g. "Green Bin") to a Bin
        private Bin TextToBin(string text)
        {
            Bin bin;
            if (text == "Black Bag")
            {
                bin.contents = "General waste";
                bin.color = new RGBColor("000000");
            }
            else if (text == "Green Bin")
            {
                bin.contents = "Composting";
                bin.color = new RGBColor("00FF00");
            }
            else if (text == "Blue Bin")
            {
                bin.contents = "Recycling";
                bin.color = new RGBColor("0000FF");
            }
            else
            {
                throw new ParseException("Unrecognised bin string");
            }
            return bin;
        }

        override public async Task<Bin> getTomorrowBin()
        {
            // TODO(dwt): Not implemented
            Bin bin;
            bin.contents = "junk";
            bin.color = new RGBColor("FF0000");
            return bin;
        }
    }
}
