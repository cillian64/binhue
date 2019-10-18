using System;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace binhue
{
    class ECDCScrape
    {
        HttpClient client = new HttpClient();

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
                Console.WriteLine("Parse failed");
                return;
            }

            // Print out row contents
            foreach (var row in rows)
            {
                var contents = row.InnerText.Trim();
                if (contents == "")
                {
                    continue;
                }
                Console.WriteLine(contents);
            }
        }
    }
}
