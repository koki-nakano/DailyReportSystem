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
            //ログインユーザIDの取得
            string MyId = User.Identity.GetUserId();

            //ログインユーザがフォローしてる従業員IDのリスト
            //SELECT * FROM Follows WHERE EmployeeID == MyID
            var follows = db.Follows
                .Where(f => f.EmployeeId == MyId)
                .Select(f => f.FollowId)
                .ToList();

            follows.Add(MyId);

            //reportsにフォローしているIDのレポートをリストで保持
            //判定はfollowsのEmployeeIDと一致した件
            var reports = db.Reports
                .Where(r => follows.Contains(r.EmployeeId))
                .OrderByDescending(r => r.UpdatedAt)
                .ToList();

            List<FollowsIndexViewModel> indexViewModels = new List<FollowsIndexViewModel>();
            //reportsを分解してリストに追加
            foreach (Report report in reports) {
                FollowsIndexViewModel indexViewModel = new FollowsIndexViewModel
                {
                    //日報情報をindexViewModelに格納
                    Id = report.Id,
                    EmployeeName = db.Users.Find(report.EmployeeId).EmployeeName,
                    ReportDate = report.ReportDate,
                    CliantCompany = report.CliantCompany,
                    Title = report.Title,
                    Content = report.Content,
                };
                //レポートIDが自分でなければ
                if (MyId != report.EmployeeId)
                {
                    //上記格納したデータをModelsに追加
                    indexViewModels.Add(indexViewModel);
                }

            }
            //Modelsをビューに戻す【向こう側のループで全件出力】
            return View(indexViewModels);
        }

        // GET: Follows/Create
        public ActionResult Create()
        {
            //ビューに送る為のFollowsCreateViewModelのリストを作成
            List<FollowsCreateViewModel> createViewModels = new List<FollowsCreateViewModel>();
            //ユーザ一覧を、IDが若い順に登録
            List<ApplicationUser> users = db.Users.OrderBy(u => u.Id).ToList();
            //ログインユーザのIDを保持
            string MyId = User.Identity.GetUserId();

            //ログインユーザがフォローしてる従業員IDのリスト
            var follows = db.Follows
                .Where(f => f.EmployeeId == MyId)
                .Select(f => f.FollowId)
                .ToList();

            follows.Add(MyId);

            //ユーザのリストをFollowsCreateViewModelのリストに変換
            foreach (ApplicationUser applicationUser in users)
            {
                
                //FollowCreateViewModelをApplicationUsersから必要なプロパティだけ抜き出して作成
                FollowsCreateViewModel createViewModel = new FollowsCreateViewModel
                {
                    FollowId = applicationUser.Id,
                    EmployeeName = applicationUser.EmployeeName,

               };
                //followsにapplicationUserのIDがあった場合
                if (follows.Contains(applicationUser.Id))
                {
                    //フォロー済みなのでフォローフラグを立てておく
                    createViewModel.FollowFlg = FollowStatus.Followed;
                }
                //IDがなかった場合
                //【フォローしていないユーザのみを一覧で出したい為】
                else
                {
                    //フォローフラグを非に設定し、Modelを追加する
                    createViewModel.FollowFlg = FollowStatus.UnFollow;
                    //作成したEmployeesIndexViewModelをリストに追加
                    createViewModels.Add(createViewModel);
                }

            }
            //作成したリストをIndexビューに送る
            return View(createViewModels);
        }

        // POST: Follows/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ReportDate")] ReportsCreateViewModel createViewModel)
        public ActionResult Create([Bind(Include = "EmployeeName,FollowId")] FollowsCreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                //フォローボタンを押下した時ログインしていたユーザのIDと
                //フォローするユーザIDをfollowに格納
                Follow follow = new Follow()
                {
                    EmployeeId =User.Identity.GetUserId(),
                    FollowId = createViewModel.FollowId
                };
                //Followsデータベースに追加
                db.Follows.Add(follow);
                //更新情報を保存
                db.SaveChanges();
                TempData["flush"] = createViewModel.EmployeeName + "さんをフォローしました。";
                return RedirectToAction("Index");
            }

            return View(createViewModel);
        }

        // GET: Follows/Delete/5
        public ActionResult Delete()
        {
            //ビューに送る為のFollowsCreateViewModelのリストを作成
            List<FollowsDeleteViewModel> deleteViewModels = new List<FollowsDeleteViewModel>();
            //ユーザ一覧を、IDが若い順に登録
            List<ApplicationUser> users = db.Users.OrderBy(u => u.Id).ToList();
            string MyId = User.Identity.GetUserId();
            //ログインユーザがフォローしてる従業員IDのリスト
            var follows = db.Follows
                .Where(f => f.EmployeeId == MyId)
                .Select(f =>f.FollowId)
                .ToList();
            follows.Add(MyId);

            //ユーザのリストをFollowsCreateViewModelのリストに変換
            //ApplicationUserはユーザ作成に必要なモデル
            foreach (ApplicationUser applicationUser in users)
            {
                //FollowCreateViewModelをApplicationUsersから必要なプロパティだけ抜き出して作成
                FollowsDeleteViewModel deleteViewModel = new FollowsDeleteViewModel
                {
                    FollowId = applicationUser.Id,
                    EmployeeName = applicationUser.EmployeeName,
                    EmployeeId = MyId
                };
                if (deleteViewModel.FollowId == deleteViewModel.EmployeeId)
                {
                    deleteViewModel.FollowFlg = FollowStatus.MyUser;
                }
                //フォローしていたユーザと一致した
                else if (follows.Contains(applicationUser.Id))
                {
                    deleteViewModel.FollowFlg = FollowStatus.Followed;
                    //Modelに追加する
                    deleteViewModels.Add(deleteViewModel);
                }
                else
                {
                    deleteViewModel.FollowFlg = FollowStatus.UnFollow;
                }

            }
            //作成したリストをIndexビューに送る
            return View(deleteViewModels);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "EmployeeId,EmployeeName,FollowId")] FollowsDeleteViewModel deleteViewModel)
        {
            //フォロー解除するユーザ情報をModelに格納
            deleteViewModel = new FollowsDeleteViewModel
            {
                FollowId = deleteViewModel.FollowId,
                EmployeeId = deleteViewModel.EmployeeId,
                EmployeeName = deleteViewModel.EmployeeName
            };

            //ログインユーザがフォローしてる従業員IDのリスト
            var follows = db.Follows
                .Where(r => r.EmployeeId == deleteViewModel.EmployeeId)
                .ToList();

            //followにfollowsリストのIDと解除要求したIDが一致するデータを格納
            Follow follow = follows.Find(f => f.FollowId ==deleteViewModel.FollowId );

            //データベースからfollowの情報を削除
            db.Follows.Remove(follow);
            //更新データを保存
            db.SaveChanges();
            TempData["flush"] = deleteViewModel.EmployeeName + "さんのフォローを解除しました。";
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
