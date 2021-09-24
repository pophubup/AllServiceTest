using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace zFluentAPIRepository
{
    public class ProductVaildators: AbstractValidator<ProductViewModel>
    {
        public ProductVaildators()
        {
            RuleFor(g => g.categoryViewModel).NotNull().DependentRules(() => {

                RuleFor(g => g.ProductName).NotNull().DependentRules(() =>
                {
                    RuleFor(g => g.ProductPrice)
                    .Must(g => LagerThen100(g)).WithMessage((x) => {  return "小於一百"; })
                    .When(g => g.ProductID == 1)
                    .Must(g => LagerThen1000(g)).WithMessage((x) => { return "小於一千"; })
                    .When(g => g.ProductID == 2);
                    
                });
              
            });
           
            //RuleFor(g => g.ProductID).Must(g=>g.ToString().Contains("3")).When(g => g.ProductID == 1);
        }
        private bool LagerThen100(decimal val)
        {
            return val >= 100;
        }
        private bool LagerThen1000(decimal val)
        {
            return val >= 1000;
        }
    }
}
