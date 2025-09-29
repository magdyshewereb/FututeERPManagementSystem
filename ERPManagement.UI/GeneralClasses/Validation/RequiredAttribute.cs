namespace ERPManagement.UI.GeneralClasses.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public RequiredAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
