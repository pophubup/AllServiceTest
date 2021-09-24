using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using zModelLayer.ViewModels;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;

namespace zWebCrawlingRepository
{
    public class HtmlContainer
    {
        public string posts_html { get; set; }

    }
    public class DATOWebCrawlingRepossitory : IDATOWebCrawling
    {
        private IConfiguration _configuration;
        public DATOWebCrawlingRepossitory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<EveryPage> GetDataFromWebElement(string value, int endpage)
        {
            try { 
   
              var urls = Enumerable.Range(1, endpage).Select(g => new { page = g, url = $"https://twlolstats.com/moreGames/833fb0e3-eeb4-55a3-9d20-c59a28d19305/game{g}" }).ToList();
                List<int> pags = Enumerable.Range(1, endpage).ToList();
                List<EveryPage> everyPages = new List<EveryPage>();
                Parallel.ForEach(urls, url => {
                    var result = new HttpClient().GetAsync(url.url).GetAwaiter().GetResult();
                    var cotent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    HtmlContainer valus = JsonConvert.DeserializeObject<HtmlContainer>(cotent);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(valus.posts_html);
                    Console.WriteLine(Task.CurrentId);
                    IList<HtmlNode> WinTexts = doc.QuerySelectorAll("td:nth-child(3) div:nth-child(1) b");
                    IList<HtmlNode> KindTexts = doc.QuerySelectorAll($"td:nth-child(3) div:nth-child(2)");
                    IList<HtmlNode> FinshTime = doc.QuerySelectorAll($"td:nth-child(3) div:nth-child(3)");
                    IList<HtmlNode> TotalHour = doc.QuerySelectorAll($"td:nth-child(3) div:nth-child(4)");
                    List<DATOViewModel> dATOViewModels = new List<DATOViewModel>();
                    for (int i = 0; i < WinTexts.Count(); i++)
                    {
                        dATOViewModels.Add(new DATOViewModel()
                        {
                            index = i + 1,
                            isWinOrFail = WinTexts[i].InnerText,
                            finishTime = FinshTime[i].InnerText,
                            totalPlayHour = TotalHour[i].InnerText,
                            type = KindTexts[i].InnerText
                        });

                    }
                    everyPages.Add(new EveryPage()
                    {
                        page = url.page,
                        result = dATOViewModels
                    });
                });

                return everyPages;
            }
            catch (System.Exception ex)
            {

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(_configuration["Exception"], "WriteLines.txt")))
                {
                    outputFile.WriteLine(ex.InnerException);
                    outputFile.WriteLine(ex.Message);

                }
                return null;
            }

        }

   
    }
}
