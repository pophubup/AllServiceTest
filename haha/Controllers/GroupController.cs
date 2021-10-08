using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using zModelLayer;
using zModelLayer.ViewModels;
using zPostgreSQLRepository.Entities;
using zPostgreSQLRepository.Entities_jsonb;

namespace haha.Controllers
{
    public class temp
    {
        public Image image { get; set; }
        public AssignCategory label { get; set; }
        public List<AssignGroup> groups { get; set; }
    }
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _Configuration;
        public GroupController(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            _serviceProvider = serviceProvider;
            _Configuration = Configuration;
        }
        /// <summary>
        /// 取得所有群體
        /// </summary>
        /// <remarks>取得所有群體</remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignGroup>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<AssignGroup> GetGroups()
        {

            return _serviceProvider.GetService<Test2Context>().AssignGroups;
        }
        
        /// <summary>
        /// 取得樹狀圖資料
        /// </summary>
        /// <remarks>取得樹狀圖資料</remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GroupData>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public List<GroupData> GetOrganizationViewModels()
        {
            List<GroupData> organizationViewModels = new List<GroupData>();
            var raw = _serviceProvider.GetService<Test2Context>().Images
                .Include(x => x.AssignCategory).ToList();
            List<AssignGroup> assignGroups = new List<AssignGroup>();
            List<temp> dynamics = new List<temp>();
            raw.ForEach(g =>
            {
                assignGroups.AddRange(g.AssignCategory.assignGroups);
                dynamics.Add(new temp()
                {
                    image = g,
                    label = g.AssignCategory,
                    groups =  g.AssignCategory.assignGroups
                });
            });
            var groupby = assignGroups.GroupBy(g => g.Id).ToList();

             
                groupby.ForEach(g => {
                   
                   var imagsLabels =   dynamics.Where(x => x.groups.Any(x => x.Id == g.Key));
                    List<ImageData> imageDatas = new List<ImageData>();
                    imagsLabels.ToList().ForEach(g =>
                    {
                       
                       imageDatas.Add( new ImageData
                       {
                           fileName = g.image.FileName,
                           url = $"{_Configuration["imageurl"]}/{g.label.BucketId}/{ g.image.FileName}",
                           createDate = g.image.CreateDate,
                           description = g.image.Description,
                           labelId = g.label.LId,
                           id = g.image.Id.ToString()
                       });

                    });
                   var images=  imageDatas.GroupBy(x => x.labelId).ToList();

                    List<LableViewModel> lableViewModels = new List<LableViewModel>();
                    raw.Select(g => g.AssignCategory).ToList().ForEach(x =>
                    {

                        var data3 = imageDatas.Where(y => y.labelId == x.LId).Select(x => x).ToList();
                        if (data3.Count() != 0)
                        {
                            lableViewModels.Add(new LableViewModel()
                            {
                                createDate = x.CreateDate,
                                labelId = x.LId,
                                labelName = x.LabelName,
                                imageDatas = data3
                            });
                        }
                    });
                    




                    organizationViewModels.Add(new GroupData()
                    {
                    groupID = g.Key,
                    groupName = g.FirstOrDefault().GroupName,
                    CreateDate = g.FirstOrDefault().CreateDate,
                    LableViewModels = lableViewModels

                    });

               });

        

          
            return organizationViewModels;

        }
        /// <summary>
        /// 取得個別 Group 
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <remarks>取得個別 Group </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignGroup))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public AssignGroup GetSingleGroup(int id)
        {
           
            return _serviceProvider.GetService<Test2Context>().AssignGroups.FirstOrDefault(g=>g.Id == id);
        }

        /// <summary>
        ///  新增多筆Group
        /// </summary>
        /// <param name="groups">string[] Group 名稱</param>
        /// <remarks >增多筆Group</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ResponseModel InsertData([FromBody] List<string> groups)
        {
            var context = _serviceProvider.GetService<Test2Context>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var current  = DateTime.Now;
                    context.AssignGroups.AddRange(groups.Select(g => new AssignGroup { GroupName = g, CreateDate = current }));
                    context.SaveChanges();
                    tran.Commit();
                    isSuccess = true;
                    msg =  $"{string.Join(",", groups)} 新增成功";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.Message;
                }
                return new ResponseModel() { isSuccess = isSuccess, Message = msg };
            }
           
        }
        /// <summary>
        /// 單筆修改特定的 group
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <param name="value">需要修改成的 group 名稱</param>
        /// <remarks>單筆修改特定的 group</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public ResponseModel EditData(int id, [FromBody] string value)
        {
            var context = _serviceProvider.GetService<Test2Context>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var group = context.AssignGroups.FirstOrDefault(g => g.Id == id);
                    group.GroupName = value;
                    context.SaveChanges();
                    tran.Commit();
                    isSuccess = true;
                    msg = $"{value} 修改成功";
                  
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.Message;
                }
                return new ResponseModel() { isSuccess = isSuccess, Message = msg };
            }
        }

        /// <summary>
        /// 刪除特定的 Group 及其 Label 和 ImageFile
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <remarks>刪除特定的 Group 及其 Label 和 ImageFile</remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public ResponseModel DeleteData(int id)
        {
            var context = _serviceProvider.GetService<Test2Context>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var group = context.AssignGroups.FirstOrDefault(x => x.Id == id);
                    var raw = context.Images.Include(g => g.AssignCategory).Where(g => g.AssignCategory.assignGroups.Any(y=>y.Id == group.Id));
                
                    context.AssignGroups.Remove(group);
                    context.AssignCategory.RemoveRange(raw.Select(g=>g.AssignCategory).ToList());
                    context.Images.RemoveRange(raw);
                    context.SaveChanges();
                    tran.Commit();
                    isSuccess = true;
                     
                    msg = $"{group.GroupName}\n\r 刪除成功";

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.Message;
                }
                return new ResponseModel() { isSuccess = isSuccess, Message = msg };
            }
        }
    }
}
