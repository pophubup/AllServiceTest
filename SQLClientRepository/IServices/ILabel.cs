using SQLClientRepository.Entities;
using System.Collections.Generic;
using zModelLayer;

namespace SQLClientRepository.IServices
{
    public interface ILabel
    {
        public (bool, string) CreateLabel(List<CreateLabelDataModel> value);
    }
}
