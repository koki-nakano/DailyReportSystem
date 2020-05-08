﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DailyReportSystem.Models;
using Microsoft.Owin.Logging;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DailyReportSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //このアプリケーション用のユーザのサインインを管理するSignInManager
        private ApplicationSignInManager _signInManager;

        //このアプリケーション用のユーザ情報を管理するUserManager
        private ApplicationUserManager _userManager;

        public EmployeesController() { }

        public EmployeesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            set {
                _userManager = value;
            }
        }


        // GET: Employees
        public ActionResult Index()
        {
            //ビューに送る為のEmployeesIndexViewModelのリストを作成
            List<EmployeesIndexViewModel> employees = new List<EmployeesIndexViewModel>();
            //ユーザ一覧を、作成日時が最近のものから順にしてリストとして取得
            List<ApplicationUser> users = db.Users.OrderByDescending(u => u.CreatedAt).ToList();
            //ユーザのリストをEmployeeIndexViewModelのリストに変換
            foreach (ApplicationUser applicationUser in users) {
                //EmployeeIndexViewModelをApplicationUsersから必要なプロパティだけ抜き出して作成
                EmployeesIndexViewModel employee = new EmployeesIndexViewModel
                {
                    Email = applicationUser.Email,
                    EmployeeName = applicationUser.EmployeeName,
                    DeleteFlg = applicationUser.DeleteFlg,
                    Id = applicationUser.Id
                };
                //作成したEmployeesIndexViewModelをリストに追加
                employees.Add(employee);
            }
            //作成したリストをIndexビューに送る
            return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View(new EmployeesCreateViewModel());
        }

        // POST: Employees/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Create([Bind(Include = "EmployeeName,Email,Password,AdminFlag")] EmployeesCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //ビューから受け取ったEmployeesCreateViewModelからユーザ情報を作成
                ApplicationUser applocationUser = new ApplicationUser
                {
                    //IdentityアカウントのUserNameにはメールアドレスを入れる必要がある
                    UserName = model.Email,
                    Email = model.Email,
                    EmployeeName = model.EmployeeName,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    DeleteFlg = 0
                };

                //ユーザ情報をDBに登録
                var result = await UserManager.CreateAsync(applocationUser, model.Password);
                //DB登録に成功した場合
                if (result.Succeeded)
                {
                    TempData["flash"] = String.Format($"{applocationUser.EmployeeName}さんを登録しました。");
                    return RedirectToAction("Index", "Employees");
                }
                //DB登録に失敗したらエラー登録
                AddErrors(result);
            }
            return View(model);
         }
        //エラーがある発生した場合エラーメッセージを追加する
        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }
     

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            EmployeeEditViewModel employee = new EmployeeEditViewModel
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                EmployeeName = applicationUser.EmployeeName
            };
            return View(employee);
        }

        // POST: Employees/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Edit([Bind(Include = "Id,Email,EmployeeName,Password,AdminFlag")]EmployeeEditViewModel employee) {
            if (ModelState.IsValid) {
                //DBからidのユーザを取得し検索、そのユーザに対し変更をする
                ApplicationUser applicationUser = db.Users.Find(employee.Id);
                //IdentityアカウントのUserNameにはメールアドレスを入れる必要がある
                applicationUser.UserName = employee.Email;
                applicationUser.Email = employee.Email;
                applicationUser.EmployeeName = employee.EmployeeName;
                applicationUser.UpdatedAt = DateTime.Now;
                //Passwordが空でなければパスワードを変更する
                if (!String.IsNullOrEmpty(employee.Password)) {
                    //Passwordの入力検証
                    var result = await UserManager.PasswordValidator.ValidateAsync(employee.Password);
                    //Passwordの検証に失敗したら、エラーを追加しEditビューをもう一度描画
                    if (!result.Succeeded) {
                        AddErrors(result);
                        return View(employee);
                    }
                    //Passwordはハッシュ化したものをDBに登録する必要があるのでPasswordHasherでハッシュ化する
                    applicationUser.PasswordHash = UserManager.PasswordHasher.HashPassword(employee.Password);
                }
                //StateをModifiedにしてUPDATE文を行うように設定
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();

                //TempDataにフラッシュメッセージを入れておく。TempDataは現在のリクエストと次のリクエストまで存在
                TempData["flush"] = String.Format("{0}さんの情報を更新しました", applicationUser.EmployeeName);

                return RedirectToAction("Index", "Employees");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
