using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SQLClientRepository.Entities;
using zModelLayer;
using Microsoft.AspNetCore.Http;
using zModelLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace haha.Controllers
{
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Group>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Group> GetGroups()
        {

            return _serviceProvider.GetService<MYDBContext>().Groups.AsEnumerable();
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
            _serviceProvider.GetService<MYDBContext>().ImageFiles.Include(x => x.Label).Include(x => x.Label.Group).ToList().GroupBy(g=>g.Label.Group.Id).ToList().ForEach(g => {

               
                var labels = g.Select(y => {
                    var images = _serviceProvider.GetService<MYDBContext>().ImageFiles.Where(x => x.LabelId == y.LabelId).Select(x => new ImageData
                    {
                        fileName = x.FileName,
                        url = $"{_Configuration["imageurl"]}/{x.Label.BucketId}/{x.FileName}",
                        createDate = x.CreateDate,
                        description = x.Description,
                        id = x.Id.ToString()


                    }).ToList();
                    return new LableViewModel()
                    {
                        createDate = y.CreateDate,
                        labelId = y.Label.Id,
                        labelName = y.Label.LabelName,
                        imageDatas = images
                    };
                } ).ToList();
                
                organizationViewModels.Add(new GroupData()
                {
                    groupID = g.Key,
                    groupName = g.FirstOrDefault().Label.Group.GroupName,
                    CreateDate = g.FirstOrDefault().Label.Group.CreateDate,
                    LableViewModels = labels

                });;

            });

        

          
            return organizationViewModels;

        }
        /// <summary>
        /// 取得個別 Group 
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <remarks>取得個別 Group </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public Group GetSingleGroup(int id)
        {
           
            return _serviceProvider.GetService<MYDBContext>().Groups.AsQueryable().FirstOrDefault(g=>g.Id == id);
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
            var context = _serviceProvider.GetService<MYDBContext>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var current  = DateTime.Now;
                    context.Groups.AddRange(groups.Select(g => new Group { GroupName = g, CreateDate = current }));
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
            var context = _serviceProvider.GetService<MYDBContext>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var group = context.Groups.FirstOrDefault(g => g.Id == id);
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
            var context = _serviceProvider.GetService<MYDBContext>();
            bool isSuccess = false;
            string msg = string.Empty;
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var group = context.Groups.FirstOrDefault(g => g.Id == id);
                   
                    context.ImageFiles.ToList().ForEach(g=> { g.LabelId = 0; });
                    context.Labels.RemoveRange(context.Labels.Where(g => g.GroupId == id));
                    context.Groups.Remove(group);
                
                    context.SaveChanges();
                    tran.Commit();
                    isSuccess = true;
                    msg = $"{group.GroupName} 移除成功";
               
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
