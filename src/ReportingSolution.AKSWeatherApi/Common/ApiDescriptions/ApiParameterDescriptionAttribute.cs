namespace ReportingSolution.AKSWeatherApi.Common.ApiDescriptions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ApiParameterDescriptionAttribute : Attribute
    {
        public string Name { get; set; } = null!;
        public bool IsRequired { get; set; }
        public Type Type { get; set; } = null!;
        public string? Source { get; set; }
    }
}
