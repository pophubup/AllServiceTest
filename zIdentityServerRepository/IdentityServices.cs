using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace zIdentityServerRepository
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection service, string conn)
        {
            service.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(conn);

            });
            service.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders();

            
           
            var builder = service.BuildServiceProvider();
          
            //var task00 = Task.Run(async () => {
            //    return await builder.GetService<ApplicationDbContext>().Database.EnsureCreatedAsync();
            //});
            //bool result = task00.GetAwaiter().GetResult();
            //註冊使用者
           // var task0 = Task.Run(async () => {
           //     ApplicationUser user = new ApplicationUser()
           //     {
           //         Email = "yyyyyy@gmail.com",
           //         SecurityStamp = Guid.NewGuid().ToString(),
           //         UserName = "test123",

           //     };
           //     return await builder.GetService<UserManager<ApplicationUser>>().CreateAsync(user, "yyyCCCGGG@123");
           // });
           //IdentityResult identityResult = task0.GetAwaiter().GetResult();


           // //尋找使用者名稱
           // var task1 = Task.Run(async () => {
           //     return await builder.GetService<UserManager<ApplicationUser>>().FindByNameAsync("FFFFCCCCfdf");
           // });
         
           //ApplicationUser check = task1.GetAwaiter().GetResult(); 
           // //去認使用者登入密碼
           // var task2 = Task.Run(async () =>
           // {
           //     return await builder.GetService<UserManager<ApplicationUser>>().CheckPasswordAsync(check, "Home7996@");
           // });
           // var check2 = task2.GetAwaiter().GetResult();


            return service;
        }
   }
}
