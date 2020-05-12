﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

namespace DailyReportSystem.Models
{
    public class Report
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("従業員ID")]
        public string EmployeeId { get; set; }

        [DisplayName("日付")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "日報の日付を入力してください。")]
        public DateTime? ReportDate { get; set; }

        [DisplayName("出勤時刻")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage ="出勤時刻を入力してください")]
        public DateTime?  WorkTime { get; set; }

        [DisplayName("退勤時刻")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "退勤時刻を入力してください")]
        public DateTime? LeaveTime { get; set; }

        [DisplayName("タイトル")]
        [Required(ErrorMessage = "タイトルを入力してください。")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません。")]
        public string Title { get; set; }

        [DisplayName("内容")]
        [Required(ErrorMessage = "内容を入力してください。")]
        public string Content { get; set; }

        [DisplayName("取引先会社名")]
        [Required(ErrorMessage ="取引先会社名を入力してください")]
        [StringLength(100,ErrorMessage ="{0}は{1}文字を超えることはできません")]
        public string CliantCompany { get; set; }

        [DisplayName("取引先担当者")]
        [Required(ErrorMessage = "取引先担当者を入力してください")]
        [StringLength(100, ErrorMessage = "{0}は{1}文字を超えることはできません")]
        public string CliantPIC { get; set; }

        [DisplayName("取引状況")]
        [Required(ErrorMessage = "取引状況を入力してください")]
        public string CliantStatus { get; set; }

        [DisplayName("作成日付")]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("更新日付")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}