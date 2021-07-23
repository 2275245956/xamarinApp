using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientEHR.Models
{
    [Table("M_Notice")]
    public class NoticeModel
    {

        [PrimaryKey, AutoIncrement]
        public int NoticeID { get; set; }
        public string Title { get; set; }
        public string ContentTxt { get; set; }
        public string DownloadUrls { get; set; }
        public string UserName { get; set; }
        public DateTime NoticeDateTime { get; set; }

    }
}
