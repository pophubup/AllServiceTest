using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer
{
    public class ImageContainer
    {
        public string objName { get; set; }
        public Stream stream { get; set; }
        public string FileExtension {
            get 
            {
                switch (Path.GetExtension(objName).Split('.')[1])
                {
                    case "png":
                    case "jpeg":
                    case "jpg":
                        return $"image/{Path.GetExtension(objName).Split('.')[1]}";
                    default:
                        return "application/octet-stream";
                }
            } 
 
         }
    }
}
