﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DailyReportSystem.Models;
using Microsoft.AspNet.Identity;

namespace DailyReportSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports
        public ActionResult Index()
        {
            // 日報のリストから、表示用のビューモデルのリストを作成
            List<ReportsIndexViewModel> indexViewModels = new List<ReportsIndexViewModel>();
            var reports = db.Reports
                .OrderByDescending(r => r.ReportDate)
                .ToList();
            foreach (Report report in reports)
            {
                ReportsIndexViewModel indexViewModel = new ReportsIndexViewModel
                {
                    Id = report.Id,
                    // 従業員のリストからこの日報のEmployeeIdで検索をかけて取得した従業員の名前を設定
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

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ReportsDetailsViewModel detailsViewModel = new ReportsDetailsViewModel
            {
                Id = report.Id,
                ReportDate = report.ReportDate,
                WorkTime = report.WorkTime,
                LeaveTime = report.LeaveTime,
                Title = report.Title,
                CliantCompany = report.CliantCompany,
                CliantPIC = report.CliantPIC,
                CliantStatus = report.CliantStatus,
                Content = report.Content,
                CreatedAt = report.CreatedAt,
                UpdatedAt = report.UpdatedAt
            };
            detailsViewModel.EmployeeName = db.Users.Find(report.EmployeeId).EmployeeName;
            detailsViewModel.isReportCreater = User.Identity.GetUserId() == report.EmployeeId;
            return View(detailsViewModel);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportDate,WorkTime,LeaveTime,CliantCompany,CliantPIC,CliantStatus,Title,Content")] ReportsCreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                Report report = new Report()
                {
                    ReportDate = createViewModel.ReportDate,
                    WorkTime = new DateTime(createViewModel.ReportDate.Value.Year, createViewModel.ReportDate.Value.Month, createViewModel.ReportDate.Value.Day, createViewModel.WorkTime.Value.Hour, createViewModel.WorkTime.Value.Minute, createViewModel.WorkTime.Value.Second),
                    LeaveTime = new DateTime(createViewModel.ReportDate.Value.Year, createViewModel.ReportDate.Value.Month, createViewModel.ReportDate.Value.Day, createViewModel.LeaveTime.Value.Hour, createViewModel.LeaveTime.Value.Minute, createViewModel.LeaveTime.Value.Second),
                    Title = createViewModel.Title,
                    Content = createViewModel.Content,
                    CliantCompany = createViewModel.CliantCompany,
                    CliantPIC = createViewModel.CliantPIC,
                    CliantStatus = createViewModel.CliantStatus,
                    //現在ログイン中のUserIDを取得し、EmployeeIdとして登録
                    EmployeeId = User.Identity.GetUserId(),
                    //作成時は現在の時刻に設定
                    CreatedAt = DateTime.Now,
                    //作成時は現在の時刻に設定
                    UpdatedAt = DateTime.Now
                };
                if (createViewModel.CliantCompany == null)
                {
                    report.CliantCompany = "--NO ASSIGN--";
                }
                if (createViewModel.CliantPIC == null)
                {
                    report.CliantPIC = "--NO ASSIGN--";
                }
                if (createViewModel.CliantStatus == null)
                {
                    report.CliantStatus = "--NO ASSIGN--";
                }
                //Contextに新しいオブジェクト追加
                db.Reports.Add(report);
                //実際のDBに反映
                db.SaveChanges();
                //TempDataにフラッシュメッセージを入れておく
                TempData["flush"] = "日報を登録しました";
                //Indexにリダイレクト
                return RedirectToAction("Index");
            }

            return View(createViewModel);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            //本人の日報でなければ表示しないように、それをEdit.cshtmlにTempDataで伝える
            if (report.EmployeeId != User.Identity.GetUserId()) {
                TempData["wrong_person"] = "true";
            }
            ReportEditViewModel editViewModel = new ReportEditViewModel()
            {
                Id = report.Id,
                ReportDate = report.ReportDate,
                WorkTime = report.WorkTime,
                LeaveTime = report.LeaveTime,
                CliantCompany = report.CliantCompany,
                CliantPIC = report.CliantPIC,
                CliantStatus = report.CliantStatus,
                Title = report.Title,
                Content = report.Content
            };
            return View(editViewModel);
        }

        // POST: Reports/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReportDate,WorkTime,LeaveTime,CliantCompany,CliantPIC,CliantStatus,Title,Content")] ReportEditViewModel editViewModel)
        {
            if (ModelState.IsValid)
            {
                Report report = db.Reports.Find(editViewModel.Id);
                report.ReportDate = editViewModel.ReportDate;
                report.WorkTime = new DateTime(report.ReportDate.Value.Year, report.ReportDate.Value.Month, report.ReportDate.Value.Day, editViewModel.WorkTime.Value.Hour, editViewModel.WorkTime.Value.Minute, editViewModel.WorkTime.Value.Second);
                report.LeaveTime = new DateTime(report.ReportDate.Value.Year, report.ReportDate.Value.Month, report.ReportDate.Value.Day, editViewModel.LeaveTime.Value.Hour, editViewModel.LeaveTime.Value.Minute, editViewModel.LeaveTime.Value.Second);
                report.Title = editViewModel.Title;
                report.CliantCompany = editViewModel.CliantCompany;
                report.CliantPIC = editViewModel.CliantPIC;
                report.CliantStatus = editViewModel.CliantStatus;
                report.Content = editViewModel.Content;
                report.UpdatedAt = DateTime.Now;
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();

                //TempDataにフラッシュメッセージを入れておく
                TempData["flush"] = "日報を編集しました";
                return RedirectToAction("Index");
            }
            return View(editViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
