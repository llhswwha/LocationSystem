using BLL;
using DbModel.Location.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TModel.Location.Manage;

namespace WebLocation.Controllers
{
    public class UserController : Controller
    {
        private Bll bll = new Bll();
        // GET: User
        public ActionResult Index()
        {
            List<User> userList = bll.Users.ToList();
            return View(userList);
        }

        public LoginInfo Login(LoginInfo info)
        {
            bll.Users.Login(info);
            return info;
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = bll.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var result = bll.Users.Add(user);
                if (result)
                {                   
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, errors = bll.Users.ErrorMessage });
                }
            }
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = bll.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }           
            return PartialView(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var result = bll.Users.Edit(user);
                if (result)
                {                  
                    return Json(new { success = result });
                }
                else
                {
                    return Json(new { success = result, erroes = bll.Users.ErrorMessage });
                }
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = bll.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView(user);         
        }

        // POST: User/Delete/5   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var user = bll.Users.Find(id);
            bll.Users.Remove(user);

            return RedirectToAction("Index");
        }         
    }
}
