namespace PGSyncro.Utils
{
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            return tail_length >= source.Length ? source : source.Substring(source.Length - tail_length);
        }
    }
}