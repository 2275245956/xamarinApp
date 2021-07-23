using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PatientEHR.Models
{
    [Table("M_MedicalHis")]
    public class MedicalHistoryTableModel
    {
        [PrimaryKey]
        public int MedicalHistoryID { get; set; }
        public string PatientNo { get; set; }
        public DateTime ConsultDate { get; set; }
        public string ConsultDateStr { get { return string.Format(new System.Globalization.CultureInfo("ja-JP"), "{0:yyyy/MM/dd}({0:ddd})", ConsultDate); } }
        public DateTime InsertDateTime { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string PrescriptionOfficeName { get; set; }

        public string SectionName { get; set; }
        public string DrName { get; set; }
    }
}
