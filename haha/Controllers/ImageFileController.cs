using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zPostgreSQLRepository.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using zGoogleCloudStorageClient;
using zModelLayer;
using zModelLayer.ViewModels;
using zPostgreSQLRepository.Entities_jsonb;
using Newtonsoft.Json;
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
            List<ImageViewModel> imageFiles = new List<ImageViewModel>();
            var raw = _serviceProvider.GetService<Test2Context>().Images.Include(x => x.AssignCategory).ToList();
            List<AssignGroup> assignGroups = new List<AssignGroup>();
            raw.ForEach(x => {
                assignGroups.AddRange(x.AssignCategory.assignGroups.ToList());
            });
            var data = assignGroups.GroupBy(g => g.Id).ToList();
            data.ForEach(x => {
                var data = raw.FirstOrDefault(g => g.AssignCategory.assignGroups.Any(y => y.Id == x.Key));
                imageFiles.Add(new ImageViewModel()
                {
                    description = data.Description,
                    GroupId = new List<int>() { x.Key },
                    LabelId = data.AssignCategory.LId,
                    id = data.Id.ToString(),
                    image = $"{_Configuration["imageurl"]}/{data.AssignCategory.BucketId}/{data.FileName}",

                });
 
            });
            
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
        public ImageViewModel GetSingleData(int id)
        {
           var data = _serviceProvider.GetService<Test2Context>().Images.Include(x => x.AssignCategory).FirstOrDefault(g => g.Id == id);
            return new ImageViewModel()
            { 
                id = data.Id.ToString(),
                description = data.Description,
                image = $"{_Configuration["imageurl"]}/{data.AssignCategory.BucketId}/{data.FileName}",
                LabelId = data.AssignCategory.LId,
                GroupId = data.AssignCategory.assignGroups.Select(g=>g.Id ).ToList(),

            };
        }

        /// <summary>
        ///  multipart/form-data 方式回存圖片、名稱、標籤及內容
        /// </summary>
        /// <remarks>  multipart/form-data 方式回存圖片、名稱、標籤及內容</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ResponseModel InsertData([FromForm] ImageFileDataModel dataModel)
        {
            
            var context = _serviceProvider.GetService<Test2Context>();
            var createtime =  DateTime.Now;
            List<int> groups = JsonConvert.DeserializeObject<List<int>>(dataModel.groupIds);
            var assignGroups =  context.AssignGroups.Where(g => groups.Any(x=> x == g.Id)).ToList();      
            var imagesData = new List<ImageContainer>();
            var guid = Guid.NewGuid().ToString();
            var AssignCategory = new AssignCategory()
            {
                LabelName = dataModel.labelName,
                assignGroups = assignGroups,
                BucketId = guid
            };
            List<Image> imageFiles = dataModel.files.Select(x =>
            {
                imagesData.Add(new ImageContainer()
                {
                    objName = x.FileName,
                    stream = x.OpenReadStream()
                    
                }) ;
         
                return new Image()
                {
                    FileName = x.FileName,
                    CreateDate = createtime,
                    Description = dataModel.description,
                    AssignCategory = AssignCategory
                };

            }).ToList();
         
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    _serviceProvider.GetService<IGoogleStorageRepository>().CreateFolder(guid);
                    _serviceProvider.GetService<IGoogleStorageRepository>().CreateFiles(imagesData, guid);
                    context.AssignCategory.Add(AssignCategory);
                    context.Images.AddRange(imageFiles);
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
        public ResponseModel EditData(int id, [FromForm] ImageFileDataModel dataModel)
        {
            var context = _serviceProvider.GetService<Test2Context>();
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                  
                    Image obj = context.Images.Include(g => g.AssignCategory).FirstOrDefault(g => g.Id == id);
                    Stream fileStream = new FileStream($"{_Configuration["imgpath"]}\\{dataModel.labelName}\\{dataModel.files.FirstOrDefault().FileName}", FileMode.Create); 
                    dataModel.files.FirstOrDefault().CopyToAsync(fileStream).GetAwaiter().GetResult();
                    _serviceProvider.GetService<IGoogleStorageRepository>().DeleteFile(obj.AssignCategory.BucketId, obj.AssignCategory.LabelName);
                    _serviceProvider.GetService<IGoogleStorageRepository>().CreateFiles(new List<ImageContainer>() { new ImageContainer()
                    {
                        objName = dataModel.files.FirstOrDefault().FileName,
                        stream = fileStream

                    }}, dataModel.labelName);
                    obj.AssignCategory = context.AssignCategory.FirstOrDefault(g => g.LabelName == dataModel.labelName);
                    obj.AssignCategory.assignGroups = context.AssignGroups.Where(g => dataModel.groupIds.Any(x => x == g.Id)).ToList();
                    obj.Description = dataModel.description;
                    obj.FileName = dataModel.files.FirstOrDefault().FileName;
                    context.SaveChanges();
                    tran.Commit();
                    return new ResponseModel() { isSuccess = true, Message = $"{obj.FileName} 修改成功" };
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
        public ResponseModel DeleteData(int id)
        {
            var context = _serviceProvider.GetService<Test2Context>();
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var obj = context.Images.Include(g => g.AssignCategory).FirstOrDefault(g => g.Id == id);
                    context.Images.Remove(obj);
                    context.SaveChanges();
                    tran.Commit();
                    return new ResponseModel() { isSuccess = true, Message = $"{obj.FileName } 刪除成功" };
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
