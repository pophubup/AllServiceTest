using zModelLayer.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace zModelLayer
{
  
    public class ImageFileDataModel
    {
        public string labelName { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        //[AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg"})]
        public List<IFormFile> files { get; set; }
    }
}
