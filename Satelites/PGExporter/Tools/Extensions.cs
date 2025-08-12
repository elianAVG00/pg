namespace PGExporter.Tools
{
    public class Extensions
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
    }
}
