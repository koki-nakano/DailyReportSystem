using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DailyReportSystem.Models
{
    public enum LikeStatus { 
        UnLike = 0,
        Like = 1
    }
    public class LikesIndexViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("氏名")]
        public string EmployeeName { get; set; }
        [DisplayName("日付")]
        public DateTime? ReportDate { get; set; }
        [DisplayName("取引先会社名")]
        public string CliantCompany { get; set; }
        [DisplayName("タイトル")]
        public string Title { get; set; }
        [DisplayName("内容")]
        public string Content { get; set; }

    }
    public class LikesCreateViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("ReportId")]
        public int ReportId { get; set; }

        [DisplayName("EmployeeId")]
        public string EmployeeId { get; set; }

        public LikeStatus? LikeFlg { get; set; }
    }
    public class LikesDetailViewModel
    {
    }
    public class LikesDeleteViewModel
    {
        public int Id { get; set; }

        [DisplayName("レポートID")]
        public int ReportId { get; set; }

        [DisplayName("従業員ID")]
        public string EmployeeId { get; set; }

        public LikeStatus? LikeFlg { get; set; }
    }
}