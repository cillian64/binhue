using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Q42.HueApi.ColorConverters;  // for RGBColor

namespace binhue
{
    class ECDC : Council
    {
        private HttpClient client = new HttpClient();
        private string url;

        private class BinCollection
        {
            public Bin bin;
            public DateTime collectionDate;

            public BinCollection(string binDesc, string dateStr)
            {
                bin = new ECDCBin(binDesc);
                collectionDate = DateTime.Parse(dateStr);
            }
        }

        private class ECDCBin : Bin
        {
            // Construct a bin from text description off website
            public ECDCBin(string text)
            {
                if (text == "Black Bag")
                {
                    contents = "General waste";
                    color = new RGBColor("000000");
                }
                else if (text == "Green Bin")
                {
                    contents = "Composting";
                    color = new RGBColor("00FF00");
                }
                else if (text == "Blue Bin")
                {
                    contents = "Recycling";
                    color = new RGBColor("0000FF");
                }
                else
                {
                    throw new ParseException("Unrecognised bin string");
                }
            }
        }

        public ECDC(string _url)
        {
            url = _url;
        }

        // Scrape the ECDC webpage to get a list of bin collections
        private async Task<List<BinCollection>> scrape()
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

            // Parse each row in the table
            var collections = new List<BinCollection>();
            foreach (var row in rows)
            {
                var contents = row.InnerText.Trim();
                if (contents == "")
                {
                    continue;  // Skip mystery empty rows in table
                }

                // Each entry is a bin description, newline, collection date.
                string[] lines = contents.Split('\n');
                string binDesc = lines[0];
                string dateStr = lines[1].Split('-')[1].Trim();

                collections.Add(new BinCollection(binDesc, dateStr));
            }
            return collections;
        }

        override public async Task<List<Council.Bin>> getTomorrowBins()
        {
            var collections = await scrape();
            var tomorrow = DateTime.Today.AddDays(1.0);

            var tomorrowCollections =
                collections.Where(x => x.collectionDate.Date.Equals(tomorrow));
            return tomorrowCollections.Select(x => x.bin).ToList();
        }
    }
}
