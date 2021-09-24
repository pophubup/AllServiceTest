using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace zFluentAPIRepository
{
    public class LabelVaildators : AbstractValidator<CreateLabelDataModel>
    {
        public LabelVaildators()
        {
            RuleFor(x => x.LabelName).NotNull();
            RuleFor(x => x.GroupID).NotEqual(0);
        }
    }
}
