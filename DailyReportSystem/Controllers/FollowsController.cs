using System;
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
    public class FollowsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Follows
        public ActionResult Index()
        {
            List<FollowsIndexViewModel> indexViewModels = new List<FollowsIndexViewModel>();
            var reports = db.Reports
                .OrderByDescending(r => r.UpdatedAt)
                .ToList();
            var follows = db.Follows.ToList();

            foreach (Report report in reports) {
                FollowsIndexViewModel indexViewModel = new FollowsIndexViewModel
                {
                    Id = report.Id,
                    EmployeeName = db.Users.Find(report.EmployeeId).EmployeeName,
                    ReportDate = report.ReportDate,
                    CliantCompany = report.CliantCompany,
                    Title = report.Title,
                    Content = report.Content,
                };
                indexViewModels.Add(indexViewModel);
            }
            return View(indexViewModels);
        }

        // GET: Follows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        // GET: Follows/Create
        public ActionResult Create()
        {
            //ビューに送る為のFollowsCreateViewModelのリストを作成
            List<FollowsCreateViewModel> createViewModels = new List<FollowsCreateViewModel>();
            //ユーザ一覧を、IDが若い順に登録
            List<ApplicationUser> users = db.Users.OrderBy(u => u.Id).ToList();
            //ユーザのリストをFollowsCreateViewModelのリストに変換
            foreach (ApplicationUser applicationUser in users)
            {
                //FollowCreateViewModelをApplicationUsersから必要なプロパティだけ抜き出して作成
                FollowsCreateViewModel createViewModel = new FollowsCreateViewModel
                {
                    FollowId = applicationUser.Id,
                    EmployeeName = applicationUser.EmployeeName,
                };
                //作成したEmployeesIndexViewModelをリストに追加
                createViewModels.Add(createViewModel);
            }
            //作成したリストをIndexビューに送る
            return View(createViewModels);
        }

        // POST: Follows/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "item.FollowId,item.EmployeeName")] FollowsCreateViewModel createViewModel)
        {

            if (ModelState.IsValid)
            {
                Follow follow = new Follow()
                {
                    EmployeeId =User.Identity.GetUserId(),
                    FollowId = createViewModel.FollowId
                };
                db.Follows.Add(follow);
                //db.SaveChanges();
                TempData["flush"] = createViewModel.EmployeeName+"さんをフォローしました。";
                return RedirectToAction("Index");
            }

            return View(createViewModel);
        }

        // GET: Follows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        // POST: Follows/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,FollowId")] Follow follow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(follow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(follow);
        }

        // GET: Follows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Follow follow = db.Follows.Find(id);
            db.Follows.Remove(follow);
            db.SaveChanges();
            return RedirectToAction("Index");
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
