using Microsoft.Extensions.DependencyInjection;
using DMS.CORE;
using DMS.BUSINESS.Services.AD;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using DMS.BUSINESS.Common;
using System.Text.RegularExpressions;

namespace DMS.BUSINESS
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, typeof(MappingProfile).Assembly);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Connection")));
            var allProviderTypes = Assembly.GetAssembly(typeof(IAccountService))
             .GetTypes().Where(t => t.Namespace != null).ToList();
            foreach (var intfc in allProviderTypes.Where(t => t.IsInterface))
            {
                var impl = allProviderTypes.FirstOrDefault(c => c.IsClass && !c.IsAbstract && intfc.Name[1..] == c.Name);
                if (impl != null) services.AddScoped(intfc, impl);
            }
            return services;
        }

        public static string SaveBase64ToFile(string base64String)
        {
            try
            {
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }
                byte[] fileBytes = Convert.FromBase64String(base64String);
                string rootPath = "Uploads/Images";
                string datePath = $"{DateTime.Now:yyyy/MM/dd}";
                string fullPath = Path.Combine(rootPath, datePath);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(fullPath, fileName);
                File.WriteAllBytes(filePath, fileBytes);
                return Path.Combine("/Uploads/Images", datePath, fileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }

        public static bool IsBase64String(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            input = input.Trim();
            int commaIndex = input.IndexOf(",");
            if (input.StartsWith("data:", StringComparison.OrdinalIgnoreCase) && commaIndex >= 0)
                input = input[(commaIndex + 1)..];

            if (input.Length % 4 != 0) return false;

            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9\+/]*={0,2}$")) return false;

            try
            {
                Convert.FromBase64String(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
