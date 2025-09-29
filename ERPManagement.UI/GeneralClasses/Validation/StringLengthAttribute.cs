namespace ERPManagement.UI.GeneralClasses.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StringLengthAttribute : Attribute
    {
        public int MinimumLength { get; }
        public int MaximumLength { get; }
        public string ErrorMessage { get; }

        public StringLengthAttribute(int min, int max, string errorMessage)
        {
            MinimumLength = min;
            MaximumLength = max;
            ErrorMessage = errorMessage;
        }
    }
}
