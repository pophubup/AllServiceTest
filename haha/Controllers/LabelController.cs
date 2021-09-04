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
        public ResponseModel InsertData([FromBody]List<CreateLabelDataModel> value)
        {
            var context = _serviceProvider.GetService<MYDBContext>();
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var today = DateTime.Now;
                   IEnumerable<Label> labels= value.Select(g => {
                      
                      Guid buckid = Guid.NewGuid();
                       _serviceProvider.GetService<IGoogleStorageRepository>().CreateFolder(buckid.ToString());
                       return new Label()
                       {
                           CreateDate = today,
                           LabelName = g.LabelName,
                           BucketId = buckid.ToString(),
                           GroupId = g.GroupID
                       };
                   });
                    context.Labels.AddRange(labels);
                    context.SaveChanges();
                    tran.Commit();
                    return new ResponseModel() { isSuccess = true, Message = $"{string.Join(",", value.Select(x => x).ToList())} 新增成功" };
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new ResponseModel() { isSuccess = false, Message = ex.Message };

                }

            }
        }

    }
}
