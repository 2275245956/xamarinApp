using SQLite;
using System;
using System.Collections.Generic;

namespace PatientEHR.Models
{

    class GetBBSParam
    {
        public int UserRole { get; set; }
        public int OfficeID { get; set; }
        //public DateTime LastUpdateTime { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 10;
    }

    public class GetBBSResult
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<BBSItem> Rows { get; set; }
    }

    
    public class BBSItem
    {
        public int NoticeID { get; set; }
        public string Title { get; set; }
        public string ContentTxt { get; set; }
        public List<string> DownloadUrls { get; set; }
        public string UserName { get; set; }
        public string NoticeDateTime { get; set; }
    }
    //[Table("M_BBS")]
    //public class BBSTable
    //{

    //    [PrimaryKey, AutoIncrement]
    //    public int NoticeID { get; set; }
    //    public string Title { get; set; }
    //    public string ContentTxt { get; set; }
    //    public string DownloadUrls { get; set; }
    //    public string UserName { get; set; }
    //    public string NoticeDateTime { get; set; }
    //}
}
