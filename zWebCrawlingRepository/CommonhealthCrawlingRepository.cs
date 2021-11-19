using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using zModelLayer.ViewModels;

namespace zWebCrawlingRepository
{
    public class CommonhealthCrawlingRepository
    {
        public byte[] ConvertToTrainTxt(IFormFile  file)
        {
            string groups = String.Empty;
            using (StreamReader r = new StreamReader(file.OpenReadStream()))
            {
                string jsonString = r.ReadToEnd();
           
                var data = JsonConvert.DeserializeObject<List<MyViewModel2>>(jsonString);
               

                    data.ForEach(g =>
                    {
                        
                        g.appendix.ForEach(x =>
                        {
                            groups += x + "\r\n";
                           
                        });

                        groups += Environment.NewLine;
                    });
                    
               
                 
              
            }
            return Encoding.UTF8.GetBytes(groups);
        }
        public byte[] GetAllDataFromTopic()
        {
            var topics = new string[]{
                "惡性腫瘤",
"心臟疾病",
"肺炎",
"腦血管疾病",
"糖尿病",
"事故傷害",
"慢性下呼吸道疾病",
"高血壓性疾病",
"腎炎、腎病症候群及腎病變",
"慢性肝病及肝硬化","人體骨架",
    "骨骼",
    "腕骨",
    "鎖骨",
    "股骨",
    "腓骨",
    "肱骨",
    "下顎",
    "掌骨",
    "跖骨",
    "聽小骨",
    "髕骨",
    "指頭的骨頭",
    "橈骨",
    "顱骨",
    "跗骨",
    "脛骨",
    "尺骨",
    "肋骨",
    "脊椎",
    "骨盆",
    "胸骨",
    "軟骨",
    "關節",
    "纖維性關節",
    "軟骨性關節",
    "滑液關節",
    "肌肉系統",
    "肌肉",
    "腱",
    "橫膈膜",
    "心血管系統",
    "動脈",
    "靜脈",
    "淋巴管",
    "心臟",
    "淋巴系統",
    "骨髓",
    "胸腺",
    "脾臟",
    "淋巴結",
    "膠淋巴系統",
    "人腦",
    "後腦",
    "延髓",
    "橋腦",
    "小腦",
    "中腦",
    "前腦",
    "間腦",
    "視網膜",
    "視神經",
    "大腦",
    "邊緣系統",
    "脊髓",
    "神經",
    "感覺系統",
    "耳部",
    "人眼",
    "人類皮膚",
    "皮下組織",
    "乳房",
    "乳腺",
    "髓細胞",
    "免疫系統",
    "淋巴細胞",
    "免疫系統",
    "上呼吸道",
    "鼻子",
    "咽",
    "喉",
    "下呼吸道",
    "氣管",
    "支氣管",
    "肺",
    "嘴巴",
    "唾腺",
    "舌部",
    "口咽",
    "喉咽",
    "食道",
    "胃",
    "小腸",
    "闌尾",
    "大腸",
    "直腸",
    "肛門",
    "肝臟",
    "膽道",
    "胰臟",
    "泌尿生殖系統",
    "腎",
    "輸尿管",
    "膀胱",
    "尿道",
    "女性生殖系統",
    "子宮",
    "陰道",
    "女陰",
    "卵巢",
    "胎盤",
    "男性生殖系統",
    "陰囊",
    "陰莖",
    "前列腺",
    "睪丸",
    "精囊",
    "腦下垂體",
    "松果體",
    "甲狀腺",
    "副甲狀腺",
    "腎上腺",
    "胰島",
    "解剖",
    "系統及器官",
    "區域解剖",
    "解剖平面及基準線",
    "中軸表面解剖",
    "附肢表面解剖"};
           
            List<MyViewModel2> myViewModel2s = new List<MyViewModel2>();

          
         
            var objs = topics.Select((g ,i)=>   new { keyword = g, url = $"https://health.tvbs.com.tw/search/{g}/articles/", index = i} ).ToList();
        
            Parallel.ForEach(objs,( obj , state) => {

            
                var result = new HttpClient().GetAsync($"{obj.url}").GetAwaiter().GetResult();
                var cotent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var doc = new HtmlDocument();
                doc.LoadHtml(cotent);
                IList<HtmlNode> WinTexts = doc.QuerySelectorAll(".search_result .list a");

                foreach (var link in WinTexts)
                {

                    var href = link.Attributes["href"].Value;
                    var result2 = new HttpClient().GetAsync($"{href}").GetAwaiter().GetResult();
                    var cotent2 = result2.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var doc2 = new HtmlDocument();
                    doc2.LoadHtml(cotent2);

                    IList<HtmlNode> innerContent = doc2.QuerySelectorAll("div[itemprop='articleBody'] p");
                    IList<HtmlNode> innertitle = doc2.QuerySelectorAll(".title_box");

                    var data = new MyViewModel2()
                    {
                        index = obj.index,
                        keyWords = obj.keyword,
                        linkforContent = obj.url,
                        title = String.Join(String.Empty, innertitle.Select(g => Regex.Replace(g.InnerText, @"\s+", "").Replace("&nbsp;", string.Empty).Replace("／", string.Empty).Replace("&rarr;", string.Empty).Trim()).ToList()),
                        content = String.Join(String.Empty, innerContent.Select(g => Regex.Replace(g.InnerText, @"\s+", "").Replace("&nbsp;", string.Empty).Replace("／", string.Empty).Replace("&rarr;", string.Empty).Trim()).ToList())
                    };
                    myViewModel2s.Add(data);
                }
              


            });
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(myViewModel2s.OrderBy(g => g.index)));
        }

    


    }
}
