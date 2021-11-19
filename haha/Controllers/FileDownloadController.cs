using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using zWebCrawlingRepository;

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _Configuration;
        public FileDownloadController(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            _serviceProvider = serviceProvider;
            _Configuration = Configuration;
        }
        /// <summary>
        ///  取得爬蟲後的資料
        /// </summary>
        /// <remarks>  Json file  </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public FileContentResult GetDataFromHealthData()
        {

           byte[] bytearr = _serviceProvider.GetService<CommonhealthCrawlingRepository>().GetAllDataFromTopic();
            return File(bytearr, "application/json", "source2.json");
        }
        /// <summary>
        /// 轉換 Json 檔至 txt 檔
        /// </summary>
        /// <remarks> .txt file </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public FileContentResult ConvertJsonToTrainTxt(IFormFile file)
        {
            byte[] bytearr = _serviceProvider.GetService<CommonhealthCrawlingRepository>().ConvertToTrainTxt(file);
            return File(bytearr, "text/plain" , "train.txt");
        }
    }
}
