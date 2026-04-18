using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Domain.Repositories.Abstract;
using Yummyanime.Domain.Repositories.EntityFramework;
using Yummyanime.Infrastructure;
using Serilog;

namespace Yummyanime
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //Подключаем в конфигурацию файл appsettings.json
            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            // Подключаем контекст БД
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
                //На момент создания приложения в данной версии EF был баг, хотя ошибки нет, поэтому подавляем предупреждения
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            builder.Services.AddTransient<IServiceCategoriesRepository, EFServiceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFServicesRepository>();
            builder.Services.AddTransient<DataManager>();
            //Настраиваем Identity систему
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //Настраиваем Auth cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "YummyanimeAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //Подключаем функционал контроллеров
            builder.Services.AddControllersWithViews();

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            //Собираем конфигурацию
            WebApplication app = builder.Build();

            app.UseSerilogRequestLogging();

            //Далее подключаем обработку исключение
            if (app.Environment.IsDevelopment())
                    app.UseDeveloperExceptionPage();
               

            //! Порядок следования middleware очень важен, так как они выполняются согласно нему
            //Подключаем использование статичных файлов(jss,css любых)
            app.UseStaticFiles();
            //Подключаем маршрутизацию
            app.UseRouting();

            //Подключаем аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            //Регистрируем нужные нам маршруты
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


             await app.RunAsync();
        }
    }
}
