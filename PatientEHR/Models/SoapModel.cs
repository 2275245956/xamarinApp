using SQLite;
using System;
namespace PatientEHR.Models
{
    [Table("M_Soap")]
    public class SoapModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public  int MedicalInfoSoapID { get; set; }
        public  string PatientNo { get; set; }
        public  int OfficeID { get; set; }
        public  int MedicalHistoryID { get; set; }
        public string S { get; set; }
        public string O { get; set; }
        public string A { get; set; }
        public string P { get; set; }
        public string Memo { get; set; }
        public DateTime  ConsultDate { get; set; }
        public DateTime InsertDateTime { get; set; }
    }
    [Table("M_SoapFile")]
    public class SoapFileModel {
        [PrimaryKey,AutoIncrement]
        public int SoapFileId { get; set; }
        public int MedicalInfoFileID { get; set; }
        public string url  { get; set; }
    }
}
