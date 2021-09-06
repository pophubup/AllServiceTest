using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SQLClientRepository.Entities;
using SQLClientRepository.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using zGoogleCloudStorageClient;
using zModelLayer;
using zModelLayer.ViewModels;
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
        /// <summary>
        /// 取得每個團體的第一筆 ImageFile 資料
        /// </summary>
        /// <remarks>取得每個團體的第一筆 ImageFile 資料</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImageViewModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public List<ImageViewModel> GetFirstDataEveryGroup()
        {
            
            List<ImageViewModel> imageFiles = _serviceProvider.GetService<MYDBContext>().ImageFiles.Include(x => x.Label).Include(g => g.Label.Group).ToList().GroupBy(g => g.Label.Group.Id).Select(x => {

                return new ImageViewModel()
                {
                    id = x.FirstOrDefault().Id.ToString(),
                    name = x.FirstOrDefault().Name,
                    image = $"{_Configuration["imageurl"]}/{x.FirstOrDefault().Label.BucketId}/{x.FirstOrDefault().FileName}",
                    description = x.FirstOrDefault().Description,
                    LabelId = x.FirstOrDefault().Label.Id,
                    GroupId = x.FirstOrDefault().Label.GroupId,
              
                };
                }).ToList();

            
            return imageFiles; 
        }

        // GET api/<ImageFileController>/5
        /// <summary>
        /// 取得單筆資料 by ImageFile ID 
        /// </summary>
        /// <param name="id">GUID </param>
        /// <remarks>取得單筆資料 by ImageFile ID </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageViewModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ImageViewModel GetSingleData(Guid id)
        {
           var data = _serviceProvider.GetService<MYDBContext>().ImageFiles.Include(x => x.Label).Include(g => g.Label.Group).FirstOrDefault(g => g.Id == id);
            return new ImageViewModel()
            { 
                id = data.Id.ToString(),
                name = data.Name,
                description = data.Description,
                image = $"{_Configuration["imageurl"]}/{data.Label.BucketId}/{data.FileName}",
                LabelId = data.Label.Id,
                GroupId = data.Label.GroupId,

            };
        }

        /// <summary>
        ///  multipart/form-data 方式回存圖片、名稱、標籤及內容
        /// </summary>
        /// <param name="id">GUID </param>
        /// <remarks>  multipart/form-data 方式回存圖片、名稱、標籤及內容</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ResponseModel InsertData([FromForm] ImageFileDataModel dataModel)
        {
            
            var context = _serviceProvider.GetService<MYDBContext>();
           
            var createtime =  DateTime.Now;
            var service = _serviceProvider.GetService<ILabel>();
            var (isSuccess, msg) = service.CreateLabel(new List<CreateLabelDataModel>() { 
               new CreateLabelDataModel()
               {
                    LabelName = dataModel.labelName,
               }
            
            });
            var imagesData = new List<ImageContainer>();
            List<ImageFile> imageFiles = dataModel.files.Select(x =>
            {

                var guid = Guid.NewGuid();
                imagesData.Add(new ImageContainer()
                {
                    objName = x.FileName,
                    stream = x.OpenReadStream()

                });
         
                return new ImageFile()
                {
                    Id = guid,
                    FileName = x.FileName,
                    CreateDate = createtime,
                    Description = dataModel.description,
                    Name = dataModel.name,
                    LabelId=Convert.ToInt32(msg)
                };

            }).ToList();
         
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var BucketId =  context.Labels.FirstOrDefault(g => g.Id == Convert.ToInt32(msg)).BucketId;
                    _serviceProvider.GetService<IGoogleStorageRepository>().CreateFiles(imagesData, BucketId);
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

        /// <summary>
        ///  multipart/form-data 方式修改 圖片、名稱、標籤及內容
        /// </summary>
        /// <param name="id">GUID Querystring</param>
        /// <param name="dataModel">ImageFileDataModel</param>
        /// <remarks>multipart/form-data 方式修改 圖片、名稱、標籤及內容</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageFileDataModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// 刪除單筆 ImageFile 資料
        /// </summary>
        /// <param name="id">ImageFileID( GUID )</param>
        /// <remarks>刪除單筆 ImageFile 資料</remarks>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseModel DeleteData(Guid id)
        {
            var context = _serviceProvider.GetService<MYDBContext>();
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    ImageFile obj = context.ImageFiles.Include(x => x.Label).Include(g => g.Label.Group).FirstOrDefault(x => x.Id == id);
                    _serviceProvider.GetService<IGoogleStorageRepository>().DeleteFile(obj.Label.BucketId, obj.FileName);
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
