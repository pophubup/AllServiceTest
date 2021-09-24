using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using zModelLayer;

namespace zAutoMapperRepository
{
    public static class AutoMapperService
    {
        public static IServiceCollection AddAutoMapperService(this IServiceCollection service)
        {
            service.AddAutoMapper(
            AppDomain.CurrentDomain.GetAssemblies(),
            serviceLifetime: ServiceLifetime.Singleton);

            //var builder = service.BuildServiceProvider();
            //List<MyModel0> myModel0s = new List<MyModel0>();
            //for (int i = 0; i < 10; i++)
            //{
            //    MyModel0 myModel0 = new MyModel0()
            //    {
            //        Id = i,
            //        products = Enumerable.Range(1, 10).Select(x => new Product
            //        {
            //            Id = x,
            //            Name = x.ToString(),
            //            description = x.ToString() + x.ToString(),
            //            categories = Enumerable.Range(1, 10).Select(y => new Category()
            //            {
            //                Id = y,
            //                Name = y.ToString() + y.ToString() + y.ToString()
            //            }).ToList(),
            //            price = x * 10000
            //        }).ToList()

            //    };
            //    myModel0s.Add(myModel0);
            //}

            //List<Container> containers = builder.GetService<IMapper>().Map<List<Container>>(myModel0s);

            //List<MyModel0> containers2 = builder.GetService<IMapper>().Map<List<MyModel0>>(containers);
            return service;
        }
    }
}
