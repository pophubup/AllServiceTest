using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using zGoogleCloudStorageClient;
using zModelLayer;
using zPostgreSQLRepository.Entities;
using zPostgreSQLRepository.Entities_jsonb;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _Configuration;
        public LabelController(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            _serviceProvider = serviceProvider;
            _Configuration = Configuration;
        }
        /// <summary>
        /// 取得所有標籤
        /// </summary>
        /// <returns></returns>
        /// <remarks>取得所有標籤</remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AssignCategory>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public List<AssignCategory> GetData()
        {
            return _serviceProvider.GetService<Test2Context>().AssignCategory.ToList();
        }
        /// <summary>
        /// 排除ImageFile 自身 label 取剩餘的label
        /// </summary>
        /// <returns></returns>
        /// <remarks>取得所有標籤</remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Label>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IEnumerable<AssignCategory> GetData([FromBody] int id)
        {
            var data = _serviceProvider.GetService<Test2Context>().Images.Include(g=>g.AssignCategory)
                .Where(g => g.AssignCategory.LId != id).Select(g => g.AssignCategory);
            return data;
        }
        /// <summary>
        /// 取得單筆 Label
        /// </summary>
        /// <param name="id">LabelId QueryString</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignCategory))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public AssignCategory GetSingleData(int id)
        {
            return _serviceProvider.GetService<Test2Context>().AssignCategory.FirstOrDefault(g=>g.LId == id);
        }
        /// <summary>
        /// 確認名稱是否重複
        /// </summary>
        /// <param name="name">Label 名稱 QueryString</param>
        /// <remarks>確認名稱是否重複</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{name}")]
        public bool CheckbyName(string name)
        {
            return _serviceProvider.GetService<Test2Context>().AssignCategory.Any(g => g.LabelName == name);
        }
        
        /// <summary>
        /// 刪除特地 Label 將其相關 ImageFile 資料刪除
        /// </summary>
        /// <param name="LabelId">Label ID QueryString</param>
        /// <remarks>刪除特地 Label 將其相關 ImageFile 資料刪除</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public ResponseModel DeleteLabel([FromQuery] int LabelId)
        {
          
           var context = _serviceProvider.GetService<Test2Context>();
           bool isSuccess = false;
           string msg = string.Empty;
           using (var tran = context.Database.BeginTransaction())
           {
                var data = context.Images.Include(g => g.AssignCategory).Where(g => g.AssignCategory.LId == LabelId).ToList();
                List<ImageFile> imageFiles = new List<ImageFile>();
                   try
                   {
                        context.AssignCategory.Remove(data.FirstOrDefault().AssignCategory);
                        context.Images.RemoveRange(data);
                        context.SaveChanges();
                        tran.Commit();
                        isSuccess = true;

                    }
                   catch (Exception ex)
                   {

                        msg = ex.Message;
                   }

                    return new ResponseModel()
                    {
                        isSuccess = isSuccess,
                        Message = msg
                    };
           };

            

        }

    }
}
