namespace PGExporter.Tools
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            if (source == null || toCheck == null)
                throw new ArgumentNullException(source == null ? nameof(source) : nameof(toCheck));
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool ContainsIgnoreCase(this string[] sourceArray, string toCheck)
        {
            if (sourceArray == null || toCheck == null)
                throw new ArgumentNullException(sourceArray == null ? nameof(sourceArray) : nameof(toCheck));
            foreach (string source in sourceArray)
            {
                if (source.ContainsIgnoreCase(toCheck))
                    return true;
            }
            return false;
        }
    }
}
