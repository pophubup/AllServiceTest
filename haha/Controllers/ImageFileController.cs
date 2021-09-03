using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQLClientRepository.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using zModelLayer;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageFileController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _Configuration;
        public ImageFileController(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            _serviceProvider = serviceProvider;
            _Configuration = Configuration;
        }
        // GET: api/<ImageFileController>
        [HttpGet]
        public IEnumerable<ImageFile> GetData()
        {
            return _serviceProvider.GetService<MYDBContext>().ImageFiles.AsEnumerable();
        }

        // GET api/<ImageFileController>/5
        /// <summary>
        /// 取得資料 by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ImageFile GetSingleData(Guid id)
        {
            return _serviceProvider.GetService<MYDBContext>().ImageFiles.FirstOrDefault(g=>g.Id == id);
        }

        // POST api/<ImageFileController>
        [HttpPost]
        public ResponseModel InsertData([FromForm] ImageFileDataModel dataModel)
        {
            
            var context = _serviceProvider.GetService<MYDBContext>();
            var guid = Guid.NewGuid();
            var createtime =  DateTime.Now;
          
            string LabelName = context.Labels.FirstOrDefault(x => x.Id == dataModel.labeId).LabelName;
            List<ImageFile> imageFiles = dataModel.files.Select(x =>
            {

                string filePath = $"{_Configuration["imgpath"]}\\{LabelName}\\{x.FileName}";
                Stream fileStream = new FileStream(filePath, FileMode.Create);
                x.CopyToAsync(fileStream).GetAwaiter().GetResult();
                return new ImageFile()
                {
                    Id = guid,
                    FileName = x.FileName,
                    CreateDate = createtime,
                    Description = dataModel.description,
                    Name = dataModel.name,
                    LabelId= dataModel.labeId
                };

            }).ToList();

            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    context.ImageFiles.AddRange(imageFiles);
                    context.SaveChanges();
                    tran.Commit();
                    return new ResponseModel() { isSuccess = true, Message = $"{string.Join(",", imageFiles.Select(x => x.FileName).ToList())} 新增成功" };
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new ResponseModel() { isSuccess = false, Message =ex.Message };
                }

            }
        }

        // PUT api/<ImageFileController>/5
        [HttpPut("{id}")]
        public ResponseModel EditData(Guid id, [FromForm] ImageFileDataModel dataModel)
        {
            var context = _serviceProvider.GetService<MYDBContext>();
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    ImageFile obj = context.ImageFiles.FirstOrDefault(x => x.Id == id);
                    string filePath = $"{_Configuration["imgpath"]}\\{obj.Label.LabelName}\\{dataModel.files.FirstOrDefault().FileName}";
                    Stream fileStream = new FileStream(filePath, FileMode.Create);
                    obj.Name = dataModel.name;
                    obj.Description = dataModel.description;
                    dataModel.files.FirstOrDefault().CopyToAsync(fileStream).GetAwaiter().GetResult();
                    context.SaveChanges();
                    tran.Commit();
                    return new ResponseModel() { isSuccess = true, Message = $"{obj.Name} 修改成功" };
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new ResponseModel() { isSuccess = false, Message = ex.Message };
                }
            }
        }

        // DELETE api/<ImageFileController>/5
        [HttpDelete("{id}")]
        public ResponseModel DeleteData(Guid id)
        {
            var context = _serviceProvider.GetService<MYDBContext>();
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    ImageFile obj = context.ImageFiles.FirstOrDefault(x => x.Id == id);
                    context.ImageFiles.Remove(obj);
                    context.SaveChanges();
                    tran.Commit();
                    return new ResponseModel() { isSuccess = true, Message = $"{obj.Name } 刪除成功" };
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
