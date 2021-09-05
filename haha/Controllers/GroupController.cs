using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SQLClientRepository.Entities;
using zModelLayer;

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        public GroupController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        // GET: api/<GroupController>
        [HttpGet]
        public IEnumerable<Group>  GetGroups()
        {

            return _serviceProvider.GetService<MYDBContext>().Groups.AsEnumerable();
        }
        [HttpGet("{id}")]
        public Group GetSingleGroup(int id)
        {
            return _serviceProvider.GetService<MYDBContext>().Groups.AsQueryable().FirstOrDefault(g=>g.Id == id);
        }

        // POST api/<GroupController>
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
                return new ResponseModel() { isSuccess = false, Message = msg };
            }
           
        }

        // PUT api/<GroupController>/5
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

        // DELETE api/<GroupController>/5
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
