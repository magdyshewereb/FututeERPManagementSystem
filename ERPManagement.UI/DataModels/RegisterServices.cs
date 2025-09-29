using ERPManagement.UI.DataModels.Interfaces;
using System.Reflection;

namespace ERPManagement.UI.DataModels
{
    public static class RegisterServices
    {
        public static void AddAllGenericServices(this IServiceCollection services)
        {
            //var serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())         
            //    .Where(p => p.IsClass && !p.IsAbstract && p.IsPublic
            //                                  && p.Namespace.Contains("ERPManagement.UI.DataModels"));
            //    //&& p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == serviceType));

            //foreach (var type in serviceTypes)
            //{
            //    //foreach (var @interface in type.GetInterfaces())
            //    //{
            //        services.AddScoped(type);
            //    //}
            //}
            var currentAssembly = Assembly.GetExecutingAssembly();
            var allTypes = currentAssembly.GetTypes().Concat(
                currentAssembly
                .GetReferencedAssemblies()
                .SelectMany(assemblyName => Assembly.Load(assemblyName).GetTypes()))
                .Where(type => !type.IsInterface && !type.IsAbstract);

            var scopedServices = allTypes.Where(type => typeof(IScopedService).IsAssignableFrom(type));

            foreach (var type in scopedServices)
            {
                services.AddScoped(type);
            }

            var transientServices = allTypes.Where(type => typeof(ITransientService).IsAssignableFrom(type));

            foreach (var type in transientServices)
            {
                services.AddTransient(type);
            }

            var singletonServices = allTypes.Where(type => typeof(ISingletonService).IsAssignableFrom(type));

            foreach (var type in singletonServices)
            {
                services.AddSingleton(type);
            }
        }
    }
}
