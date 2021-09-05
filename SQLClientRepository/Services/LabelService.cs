using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQLClientRepository.Entities;
using SQLClientRepository.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using zGoogleCloudStorageClient;
using zModelLayer;

namespace SQLClientRepository.Services
{
    public class LabelService : ILabel
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _Configuration;
        public LabelService(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            _serviceProvider = serviceProvider;
            _Configuration = Configuration;
        }
        public (bool, string) CreateLabel(List<CreateLabelDataModel> value)
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
                            GroupId = Convert.ToInt32(_Configuration["DefaultGroup"])
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
                return (isSuccess, msg);
            } 
        }
    }

}
