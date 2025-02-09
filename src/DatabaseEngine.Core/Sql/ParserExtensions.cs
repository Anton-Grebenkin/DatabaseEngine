
namespace DatabaseEngine.Core.Sql
{
    internal static class ParserExtensions
    {
        public static T? NextValue<T>(this Queue<T> queue) where T : class
        {
            return queue.TryDequeue(out var token) ? token : null;
        }

        public static T? PeekValue<T>(this Queue<T> queue) where T : class
        {
            return queue.Count > 0 ? queue.Peek() : null;
        }
    }
}
