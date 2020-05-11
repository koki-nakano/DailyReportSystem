﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace DailyReportSystem.Models
{
    public class ReportsIndexViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("氏名")]
        public string EmployeeName { get; set; }
        [DisplayName("日付")]
        public DateTime? ReportDate { get; set; }
        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DisplayName("内容")]
        public string Content { get; set; }

    }
    public class ReportsCreateViewModel { 
        [DisplayName("日付")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-mm-dd}",ApplyFormatInEditMode =true)]
        [Required(ErrorMessage ="日報の日付を入力してください")]
        public DateTime? ReportDate { get; set; }

        [DisplayName("タイトル")]
        [Required(ErrorMessage ="タイトルを入力してください")]
        [StringLength(100,ErrorMessage ="{0}は{1}文字を超えることはできません")]
        public string Title { get; set; }

        [DisplayName("内容")]
        [Required(ErrorMessage ="内容を入力してください")]
        public string Content { get; set; }
    }

    public class ReportsDetailsViewModel { 
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("氏名")]
        public string EmployeeName { get; set; }
        [DisplayName("日付")]
        public DateTime? ReportDate { get; set; }

        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DisplayName("内容")]
        public string Content { get; set; }

        [DisplayName("作成日付")]
        public DateTime? CreatedAt { get; set; }
        [DisplayName("更新日付")]
        public DateTime? UpdatedAt { get; set; }

        //このレポートを作成したいとなら「この日報を編集する」リンクを出すためのフラグ作成
        public bool isReportCreater { get; set; }
    }
}