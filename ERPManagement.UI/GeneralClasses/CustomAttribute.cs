using ERPManagement.UI.DataModels.Interfaces;

namespace ERPManagement.UI.GeneralClasses
{

    public class CustomAttribute : Attribute, ISingletonService
    {
        public bool IsDateonly { get; set; }
    }
}
