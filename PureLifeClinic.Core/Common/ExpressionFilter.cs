using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Common
{
    public class ExpressionFilter
    {
        public string? PropertyName { get; set; }
        public object? Value { get; set; }
        public Comparison Comparison { get; set; }
    }
}
