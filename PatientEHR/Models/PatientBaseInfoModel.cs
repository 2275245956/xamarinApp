using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientEHR.Models
{
    public class PatientBaseInfoModel
    {
       
        public int PatientID { get; set; }
        public string FaceImgDownLoadUrl { get; set; }
        public int PatientNo { get; set; }

        public string PatientName { get; set; }

        public string PatientKana { get; set; }

        public DateTime? Birthday { get; set; }

        public int Sex { get; set; }

        public string BloodTypeABO { get; set; }

        public string BloodTypeRH { get; set; }

        public string PostCode { get; set; }

        public string StreetAddress { get; set; }

        public string Tel { get; set; }

        public string Tel2 { get; set; }
        public string Mobile { get; set; }

        public string Mail { get; set; }
         

        public int OfficeID { get; set; }

        public string OfficeName { get; set; }

        public string OfficePatientID { get; set; }

        public string CreateOfficeName { get; set; }

        public int ManageOfficeID { get; set; }

        public string SexName
        {
            get
            {
                switch (Sex)
                {
                    case 1:
                        return "男";
                    case 2:
                        return "女";
                    default:
                        return "";
                }
            }
        }

        public string BirthDayJapanFormat { get; set; }

        public string BirthdayStr { get; set; }
        /// <summary>
        /// 年齢
        /// </summary>
        public string Age { get; set; }
    }



    [Table("M_PatientInfo")]
    public class BaseInfoModel {

        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// 患者属性項目ID
        /// </summary>
        public int PatientAttributeID { get; set; }

        /// <summary>
        /// 患者共通ID
        /// </summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// 事業所ID
        /// </summary>
        public int OfficeID { get; set; }

        public string OfficeName { get; set; }

        /// <summary>
        /// 属性項目ID
        /// </summary>
        public int AttributeItemID { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// 属性グループ
        /// </summary>
        public string AttributeItemGroup { get; set; }

        /// <summary>
        /// 属性グループ名
        /// </summary>
        public string AttributeItemGroupName { get;set; }

        public string AttributeItemName { get; set; }

        public string IdentifyingCD { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int LastUserID { get; set; } 
    }
}
