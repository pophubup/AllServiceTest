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
        // GET: api/<LabelController>
        [HttpGet]
        public IEnumerable<Label> GetData()
        {
            return _serviceProvider.GetService<MYDBContext>().Labels.AsEnumerable();
        }

        // GET api/<LabelController>/5
        [HttpGet("{id}")]
        public Label GetSingleData(int id)
        {
            return _serviceProvider.GetService<MYDBContext>().Labels.AsQueryable().FirstOrDefault(g=>g.Id == id);
        }
        [HttpGet("{name}")]
        public Label CheckbyName(string name)
        {
            return _serviceProvider.GetService<MYDBContext>().Labels.AsQueryable().FirstOrDefault(g => g.LabelName == name);
        }
        // POST api/<LabelController>
        [HttpPost]
        public ResponseModel InsertLabel([FromBody]List<CreateLabelDataModel> value)
        {
            var context = _serviceProvider.GetService<MYDBContext>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var today = DateTime.Now;
                    List<Label> labels = value.Select(g =>
                    {

                        Guid buckid = Guid.NewGuid();
                        _serviceProvider.GetService<IGoogleStorageRepository>().CreateFolder(buckid.ToString());
                        return new Label()
                        {
                            CreateDate = today,
                            LabelName = g.LabelName,
                            BucketId = buckid.ToString(),
                            GroupId = g.GroupID
                        };
                    }).ToList();
                    context.Labels.AddRange(labels);
                    context.SaveChanges();
                    tran.Commit();
                    isSuccess = true;

                    msg = $"{string.Join(",", labels.Select(g => g.Id))}";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.Message;
                }
                return new ResponseModel() { isSuccess = isSuccess, Message = msg };
            }
        }
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
                        bool result = _serviceProvider.GetService<IGoogleStorageRepository>().DeleteFolder(target.BucketId);
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
