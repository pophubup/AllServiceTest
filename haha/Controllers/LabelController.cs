using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using SQLClientRepository.Entities;
using zModelLayer;
using zGoogleCloudStorageClient;
using Microsoft.EntityFrameworkCore;
using SQLClientRepository.IServices;
using zModelLayer.ViewModels;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Label>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public IEnumerable<Label> GetData()
        {
            return _serviceProvider.GetService<MYDBContext>().Labels.AsEnumerable();
        }
        /// <summary>
        /// 排除ImageFile 自身 label 取剩餘的label
        /// </summary>
        /// <returns></returns>
        /// <remarks>取得所有標籤</remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Label>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IEnumerable<Label> GetData([FromBody] string id)
        {
            return _serviceProvider.GetService<MYDBContext>().Labels.Include(g=>g.ImageFiles).Where(g=>g.ImageFiles.Any(x=>x.Id.ToString() != id)).AsEnumerable();
        }
        /// <summary>
        /// 取得單筆 Label
        /// </summary>
        /// <param name="id">LabelId QueryString</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Label))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public Label GetSingleData(int id)
        {
            return _serviceProvider.GetService<MYDBContext>().Labels.AsQueryable().FirstOrDefault(g=>g.Id == id);
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
            return _serviceProvider.GetService<MYDBContext>().Labels.AsQueryable().Any(g => g.LabelName == name);
        }
        /// <summary>
        /// 插入多筆標籤
        /// </summary>
        /// <param name="value">預設 Group 暫定是 2 未分類</param>
        /// <remarks> 插入多筆標籤</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ResponseModel InsertLabel([FromBody]List<CreateLabelDataModel> value)
        {
            var service = _serviceProvider.GetService<ILabel>();
            var (isSuccess, msg) = service.CreateLabel(value);
            return new ResponseModel() { isSuccess = isSuccess, Message = msg };
          
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
           var data = _serviceProvider.GetService<MYDBContext>().ImageFiles.Include(g=>g.Label).Where(g => g.LabelId == LabelId);
           if (data.Count()  == 0 )
            {
                var context = _serviceProvider.GetService<MYDBContext>();
                bool isSuccess = false;
                string msg = string.Empty;
                using (var tran = context.Database.BeginTransaction())
               {
                   try
                   {
                        var target = context.Labels.FirstOrDefault(g => g.Id == LabelId);
                        var imageFiles = context.ImageFiles.Where(g => g.LabelId == LabelId).ToList();
                         _serviceProvider.GetService<IGoogleStorageRepository>().DeleteFolder(target.BucketId);
                        context.ImageFiles.RemoveRange(imageFiles);
                        context.Labels.Remove(target);
                        context.SaveChanges();
                        tran.Commit();
                        isSuccess = true;
                        msg = $"{data.FirstOrDefault().Label.LabelName} 刪除成功";
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
            else
            {
                return new ResponseModel()
                {
                    isSuccess = false,
                    Message = $"無法刪除標籤 {data.FirstOrDefault().Name}，其資料{string.Join(",", data.Select(g=>g.FileName))}仍是此標籤"

                };
            }
            
        }

    }
}
