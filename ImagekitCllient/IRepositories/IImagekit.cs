using Imagekit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagekitCllient.IRepositories
{
    public interface IImagekit
    {
        ServerImagekit serverImagekitClient { get; }
        public (bool, string )CreateLabelPath(string labelname, byte[] filearr);
        public bool SaveImageToTheFolder(string Image);
    }
}
