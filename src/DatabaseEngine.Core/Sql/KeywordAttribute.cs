
namespace DatabaseEngine.Core.Sql
{
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    internal class KeywordAttribute : Attribute
    {
        internal string Value { get; }
        public KeywordAttribute(string value)
        {
            Value = value;
        }
    }
}
