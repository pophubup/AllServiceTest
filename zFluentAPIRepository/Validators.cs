using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System;
using zModelLayer;

namespace zFluentAPIRepository
{
    public static class Validators
    {
        public static IServiceCollection AddCustomizedVaildator(this IServiceCollection service)
        {
            service.AddScoped<IValidator<CreateLabelDataModel>, LabelVaildators>();
            service.AddScoped<IValidator<ProductViewModel>, ProductVaildators>();
           // var builder = service.BuildServiceProvider();
           // var fun0 = builder.GetService<IValidator<CreateLabelDataModel>>();
           // ValidationResult final = fun0.Validate(new CreateLabelDataModel()
           // {
           //      LabelName = "123",
           //      GroupID =3
           // });
           //var func = builder.GetService<IValidator<ProductViewModel>>();
           //var final2 = func.Validate(new ProductViewModel() {
           //    categoryViewModel = new CategoryViewModel()
           //    {
           //        CategoryID = 1,
           //        CategoryName = "123"
           //    },
           //    ProductID = 2,
           //    ProductName = "5566",
           //    ProductPrice = 100
             
           // });
          
            return service;
        }
    }
}
