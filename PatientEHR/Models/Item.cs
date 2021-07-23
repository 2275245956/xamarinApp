using System;
using SQLite;

namespace PatientEHR.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        //public int DBId { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}