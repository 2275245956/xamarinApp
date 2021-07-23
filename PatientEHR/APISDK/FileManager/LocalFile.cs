using System;
using SQLite;

namespace PatientEHR.APISDK.FileManager
{
    public class LocalFile
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FileId { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string PreviewFilePath { get; set; }
        public bool NeedUpload { get; set; }
        public bool IsLocal { get; set; }
    }
}
