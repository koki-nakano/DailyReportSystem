using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace DailyReportSystem.Models
{
    public enum AcceptStatus
    {
        [Display(Name = "否認")]
        UnAuthorize = 0,
        [Display(Name = "承認")]
        Auhorized = 1
    }
    public class ReportsIndexViewModel
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

        public LikeStatus? LikeFlg { get; set; }

        public RolesEnum? AdminFlag { get; set; }

        [DisplayName("承認状況")]
        public int Accepting { get; set; }

        public AcceptStatus? AcceptFlg { get; set; }

        [DisplayName("ログインユーザID")]
        public string LoginUserId { get; set; }

    }
    public class ReportsCreateViewModel
    {
        [DisplayName("日付")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "日報の日付を入力してください")]
        public DateTime? ReportDate { get; set; }

        [DisplayName("出勤時刻")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "出勤時刻を入力してください")]
        public DateTime? WorkTime { get; set; }

        [DisplayName("退勤時刻")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "退勤時刻を入力してください")]
        public DateTime? LeaveTime { get; set; }

        [DisplayName("取引先会社名")]
        //[Required(ErrorMessage = "取引先会社名を入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string CliantCompany { get; set; }

        [DisplayName("取引先担当者")]
        //[Required(ErrorMessage = "取引先担当者を入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string CliantPIC { get; set; }

        [DisplayName("取引状況")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessage = "取引状況を入力してください")]
        public string CliantStatus { get; set; }

        [DisplayName("タイトル")]
        [Required(ErrorMessage = "タイトルを入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string Title { get; set; }


        [DisplayName("内容")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "内容を入力してください")]
        public string Content { get; set; }

        [DisplayName("コメント")]
        public string Comment { get; set; }
        [DisplayName("承認状況")]
        public int Accepting { get; set; }

        public LikeStatus? LikeFlg { get; set; }

    }

    public class ReportsDetailsViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("氏名")]
        public string EmployeeName { get; set; }
        [DisplayName("日付")]
        public DateTime? ReportDate { get; set; }

        [DisplayName("出勤時刻")]
        public DateTime? WorkTime { get; set; }
        [DisplayName("退勤時刻")]
        public DateTime? LeaveTime { get; set; }
        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DisplayName("内容")]
        public string Content { get; set; }

        [DisplayName("取引先会社名")]
        public string CliantCompany { get; set; }

        [DisplayName("取引先担当者")]
        public string CliantPIC { get; set; }

        [DisplayName("取引状況")]
        public string CliantStatus { get; set; }

        [DisplayName("作成日付")]
        public DateTime? CreatedAt { get; set; }
        [DisplayName("更新日付")]
        public DateTime? UpdatedAt { get; set; }

        //このレポートを作成したいとなら「この日報を編集する」リンクを出すためのフラグ作成
        public bool isReportCreater { get; set; }

        public LikeStatus? LikeFlg { get; set; }

        [DisplayName("コメント")]
        public string Comment{ get; set; }
        [DisplayName("承認状況")]
        public int Accepting { get; set; }
        //test
        [DisplayName("承認権限")]
        public AcceptStatus? AcceptFlg { get; set; }

        [DisplayName("ログインユーザID")]
        public string LoginUserId { get; set; }
    }

    public class ReportEditViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("日付")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "日報の日付を入力してください")]
        public DateTime? ReportDate { get; set; }

        [DisplayName("出勤時刻")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "出勤時刻を入力してください")]
        public DateTime? WorkTime { get; set; }

        [DisplayName("退勤時刻")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "退勤時刻を入力してください")]
        public DateTime? LeaveTime { get; set; }

        [DisplayName("取引先会社名")]
        // [Required(ErrorMessage = "取引先会社名を入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string CliantCompany { get; set; }

        [DisplayName("取引先担当者")]
        // [Required(ErrorMessage = "取引先担当者を入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string CliantPIC { get; set; }

        [DisplayName("取引状況")]
        [DataType(DataType.MultilineText)]
        //  [Required(ErrorMessage = "取引状況を入力してください")]
        public string CliantStatus { get; set; }

        [DisplayName("タイトル")]
        [Required(ErrorMessage = "タイトルを入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string Title { get; set; }

        [DisplayName("内容")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "内容を入力してください")]
        public string Content { get; set; }

        public int Accepting { get; set; }
    }

    public class ReportsAcceptViewModel
    {
        public int Id { get; set; }

        public string Comment  { get; set; }

        public string Accepting { get; set; }
    }
}