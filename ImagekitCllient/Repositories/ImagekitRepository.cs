using Imagekit;
using ImagekitCllient.IRepositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagekitCllient.Repositories
{
    public class ImagekitRepository : IImagekit
    {
        private IConfiguration _configuration;
        public ImagekitRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ServerImagekit serverImagekitClient =>  new ServerImagekit(_configuration["Imagekit:publicKey"], _configuration["Imagekit:privateKey"], _configuration["Imagekit:urlEndPoint"], "path");

        public (bool, string )CreateLabelPath(string labelname, byte[] filearr)
        {
            ImagekitResponse resp = serverImagekitClient
      .FileName(labelname)
      .UploadAsync(filearr).GetAwaiter().GetResult();


            return ( true, resp.URL);
        }

        public bool SaveImageToTheFolder(string Image)
        {
            throw new NotImplementedException();
        }
    }
}
