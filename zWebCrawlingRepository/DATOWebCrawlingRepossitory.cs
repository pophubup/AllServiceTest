using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using zModelLayer.ViewModels;

namespace zWebCrawlingRepository
{
    class DATOWebCrawlingRepossitory : IWebCrawling<EveryPage>
    {
        private IConfiguration _configuration;
        public DATOWebCrawlingRepossitory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<EveryPage> GetDataFromWebElement(string value, int endpage)
        {
            var list = new List<int>();
            list.AddRange(Enumerable.Range(2, endpage));
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--no-sandbox");
            IWebDriver driver = new ChromeDriver(chromeDriverDirectory: _configuration["ChromeDriver"], options);
            driver.Navigate().GoToUrl($"https://twlolstats.com/summoner/?summoner={value}");
            List<EveryPage> everyPages = new List<EveryPage>();
            ReadOnlyCollection<IWebElement> isWin;
            ReadOnlyCollection<IWebElement> Kind;
            ReadOnlyCollection<IWebElement> FinshTime;
            ReadOnlyCollection<IWebElement> TotalHour;
            if(endpage != 1)
            {
                foreach (var item in list)
                {
                    List<DATOViewModel> dATOViewModels = new List<DATOViewModel>();
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    Thread.Sleep(5000);
                    driver.FindElements(By.CssSelector($"#game{item} input")).FirstOrDefault().Click();

                    isWin = driver.FindElements(By.CssSelector($"#game{item} table tbody td:nth-child(3) div:nth-child(1) b"));
                    Kind = driver.FindElements(By.CssSelector($"#game{item} table tbody td:nth-child(3) div:nth-child(2)"));
                    FinshTime = driver.FindElements(By.CssSelector($"#game{item} table tbody td:nth-child(3) div:nth-child(3)"));
                    TotalHour = driver.FindElements(By.CssSelector($"#game{item} table tbody td:nth-child(3) div:nth-child(4)"));
                    for (int i = 0; i < isWin.Count; i++)
                    {

                        dATOViewModels.Add(new DATOViewModel()
                        {
                            index = i + 1,
                            finishTime = FinshTime[i].Text,
                            isWinOrFail = isWin[i].Text,
                            totalPlayHour = TotalHour[i].Text,
                            type = Kind[i].Text
                        });
                    }
                    everyPages.Add(new EveryPage()
                    {
                        page = item,
                        result = dATOViewModels
                    });
                }
            }
      
            isWin = driver.FindElements(By.CssSelector("table:nth-child(2) tbody td:nth-child(3) div:nth-child(1) b"));
            Kind = driver.FindElements(By.CssSelector("table:nth-child(2) tbody td:nth-child(3) div:nth-child(2)"));
            FinshTime = driver.FindElements(By.CssSelector("table:nth-child(2) tbody td:nth-child(3) div:nth-child(3)"));
            TotalHour = driver.FindElements(By.CssSelector("table:nth-child(2) tbody td:nth-child(3) div:nth-child(4)"));
            List<DATOViewModel> dATOViewModels_FirstElement = new List<DATOViewModel>();
            for (int i = 0; i < isWin.Count; i++)
            {

                dATOViewModels_FirstElement.Add(new DATOViewModel()
                {
                    index = i + 1,
                    finishTime = FinshTime[i].Text,
                    isWinOrFail = isWin[i].Text,
                    totalPlayHour = TotalHour[i].Text,
                    type = Kind[i].Text
                });
            }
            everyPages.Insert(0, new EveryPage() {

                 page = 1,
                 result = dATOViewModels_FirstElement
            });

            return everyPages;
        }
    }
}
