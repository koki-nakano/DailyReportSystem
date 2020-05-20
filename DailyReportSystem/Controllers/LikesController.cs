using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DailyReportSystem.Models;

namespace DailyReportSystem.Controllers
{
    [Authorize]
    public class LikesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Follows
        public ActionResult Index()
        {
            //ログインユーザIDの取得
            string MyId = User.Identity.GetUserId();

            //ログインユーザがいいね登録しているレポートIDのリスト
            var likes = db.Likes
                .Where(l => l.EmployeeId == MyId)
                .Select(l => l.ReportId)
                .ToList();

            //いいねしているレポートIDが含む日報情報をReportsDBからreportsリストに格納
            var reports = db.Reports
                .Where(r => likes.Contains(r.Id))
                .OrderByDescending(r => r.UpdatedAt)
                .ToList();

            //indexViewModelsを作成
            List<LikesIndexViewModel> indexViewModels = new List<LikesIndexViewModel>();
            //reportsにある件数分実行
            foreach (Report report in reports)
            {
                //Modelsに追加する1件分のデータを格納する
                LikesIndexViewModel indexViewModel = new LikesIndexViewModel
                {
                    Id = report.Id,
                    EmployeeName = db.Users.Find(report.EmployeeId).EmployeeName,
                    ReportDate = report.ReportDate,
                    CliantCompany = report.CliantCompany,
                    Title = report.Title,
                    Content = report.Content,
                };
                //Modelsに追加する
                indexViewModels.Add(indexViewModel);
            }
            //ビューにリターンする
            return View(indexViewModels);
        }

        // GET: Likes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Likes/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] LikesCreateViewModel createViewModel)
        {
            //検証の成否チェック
            if (ModelState.IsValid)
            {
                //likeにレポートIDといいねした自分のIDを格納
                Likes like = new Likes
                {
                    ReportId = createViewModel.Id,
                    EmployeeId = User.Identity.GetUserId()
                };
                //いいね情報をLikesDBに保存
                db.Likes.Add(like);
                //DB更新
                db.SaveChanges();
                TempData["flush"] = String.Format("いいねしました");
                return RedirectToAction("Index", "Reports");
            }
            return View(createViewModel);
        }

        // GET: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "Id")]LikesDeleteViewModel deleteViewModel) 
        {
            //いいねを破棄するModelにReportIDとログインIDを格納
            deleteViewModel = new LikesDeleteViewModel
            {
                ReportId = deleteViewModel.Id,
                EmployeeId = User.Identity.GetUserId()
            };
            //LikesDBのReportIdと削除ボタン押下時のReportIdが一致したものをlikesリストに保存
            var likes = db.Likes
                .Where(r => r.ReportId == deleteViewModel.ReportId)
                .ToList();
            
            //likesリストから削除するレコードを抽出
            Likes like = likes.Find(l => l.ReportId == deleteViewModel.ReportId);
            //DBから削除
            db.Likes.Remove(like);
            //DBを更新
            db.SaveChanges();
            TempData["flush"] = String.Format("いいねを解除しました");
            return RedirectToAction("Index", "Reports");

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
