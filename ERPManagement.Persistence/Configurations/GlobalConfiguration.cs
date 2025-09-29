using Microsoft.EntityFrameworkCore;

namespace ERPManagement.Persistence.Configurations
{
    public static class GlobalConfiguration
    {
        public static void GlobalConfigure(this ModelBuilder mb)
        {


            foreach (var property in mb.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null)
                    if (property.Name == "Remarks")
                        property.SetMaxLength(256);
                    else
                        property.SetMaxLength(50);
            }

            foreach (var property in mb.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(bool)))
            {
                if (property.Name == "IsActive")
                    property.SetDefaultValue(true);
                else if (property.Name == "IsDefault")
                    property.SetDefaultValue(false);
            }

            foreach (var property in mb.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(3);
            }
        }

    }
}
