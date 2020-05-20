using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyReportSystem.Models
{
    public class Likes
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("ReportId")]
        public int ReportId { get; set; }

        [DisplayName("EmployeeId")]
        public string  EmployeeId { get; set; }
    }
}