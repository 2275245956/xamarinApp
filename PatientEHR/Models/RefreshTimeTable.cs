using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientEHR.Models
{
    [Table("M_RefreshTime")]
    public class RefreshTimeTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string TableKey { get; set; }
        public long LastDataDateTime { get; set; }

        [Ignore]
        public DateTime RefreshDate { get => new DateTime(LastDataDateTime); }
    }
}
