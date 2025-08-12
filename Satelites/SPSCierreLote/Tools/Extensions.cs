using SPSCierreLote.Models;

namespace SPSCierreLote.Tools
{
    public static class Extensions
    {
        public static string GetFileExtension(string filename) => Path.GetExtension(filename);

        public static FileInfoModel GetFileInfo(FileInfo archivo)
        {
            return new FileInfoModel()
            {
                Extension = archivo.Extension,
                FullName = archivo.FullName,
                Name = archivo.Name
            };
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
