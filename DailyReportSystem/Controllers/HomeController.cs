﻿using DailyReportSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DailyReportSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            string UserId = User.Identity.GetUserId();
            //自分の日報だけのリストを作る。
            var myReports = db.Reports
                .Where(r => r.EmployeeId == UserId)
                .OrderByDescending(r => r.ReportDate)
                .ToList();

            //自分の日報リストから表示用のModelデータ(ReportIndexViewModel)のリストを作成
            List<ReportsIndexViewModel> indexViewModels = new List<ReportsIndexViewModel>();
            foreach (var report in myReports)
            {
                ReportsIndexViewModel indexViewModel = new ReportsIndexViewModel
                {
                    Id = report.Id,
                    EmployeeName = db.Users.Find(report.EmployeeId).EmployeeName,
                    ReportDate = report.ReportDate,
                    CliantCompany = report.CliantCompany,
                    Title = report.Title,
                    Content = report.Content
                };
                indexViewModels.Add(indexViewModel);
            }
            return View(indexViewModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Dispose(disposing);
            }
            base.Dispose(disposing);
        }
    }
}
