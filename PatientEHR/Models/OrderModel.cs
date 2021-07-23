using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientEHR.Models
{
    [Table("M_OrderTable")]
    public class OrderModel
    {
        [PrimaryKey,AutoIncrement]
        public int OrderId { get; set; }
        public int OtherEHRSystemCode { get; set; }
        public string OfficeCD { get; set; }
        public string OfficePatientID { get; set; }
        /// <summary>
        /// 图片文件  53143  添加属性 2019-7-12
        /// </summary>
        public string FileData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PatientNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OfficeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MedicalHistoryID { get; set; }
        public DateTime ConsultDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderDiv { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TestResult { get; set; }
        public string LowerValue
        {
            get;
            set;
        }
        public string UpperValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Times { get; set; }

        /// <summary>
        /// 単位
        /// </summary>
        public string TimesUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GroupNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNote { get; set; }

        public string GroupNote { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DrugOrderInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CancelFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CancelDateTime { get; set; }

        public string StaffName { get; set; }
        public string OfficeName { get; set; }
        public string DrName { get; set; }
        public string SectionName { get; set; }


        public string HisTestItemID { get; set; }
        public string AbnormalFlag { get; set; }

        public string LocalGroupNo { get; set; }

    }
}
