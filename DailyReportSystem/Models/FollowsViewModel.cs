using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace DailyReportSystem.Models
{
    public class FollowsIndexViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("社員ID")]
        public string EmployeeId { get; set; }
        [DisplayName("氏名")]
        public string EmployeeName { get; set; }
        [DisplayName("日付")]
        public DateTime? ReportDate { get; set; }
        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DisplayName("取引先会社名")]
        public string CliantCompany { get; set; }
        [DisplayName("内容")]
        public string Content { get; set; }
        [DisplayName("フォローID")]
        public string FollowId { get; set; }
    }

    public class FollowsCreateViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("社員ID")]
        public string EmployeeId { get; set; }

        [DisplayName("氏名")]
        public string EmployeeName { get; set; }

        [DisplayName("フォローID")]
        public string FollowId { get; set; }
    }
}