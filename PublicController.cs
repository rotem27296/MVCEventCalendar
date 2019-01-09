using MVCEventCalendar.Models;
using Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCEventCalendar.Controllers
{
    public class PublicController : Controller
    {

        // GET: Public
        
        public ActionResult resetPassword()
        {
            string hash = HttpContext.Request.Params.Get("hash");
            if (hash == null)
            {
                ViewBag.error = "Hash not Found";
                return View("~/Views/Shared/Error.cshtml");
            }
            ResetDal rd = new ResetDal();
            Reset to_reset;
            try
            {
                to_reset = rd.reset.Single(o => o.hash == hash);
                rd.reset.Remove(to_reset);
                rd.SaveChanges();
                return View(new Users() { ID = to_reset.ID });
            }
            catch (Exception)
            {
                ViewBag.error = "Hash not Valide";
                return View("~/Views/Shared/Error.cshtml");
            }


            //return View("~/Views/Shared/Error.cshtml");
        }
        public ActionResult submitResetPassword(Users objuser)
        {
            //update
            UsersDal ud = new UsersDal();
            Users us = ud.User.First(a => a.ID == objuser.ID);
            ud.SaveChanges();
            ViewBag.success = "Password is reseted";
            return View("~/Views/Shared/success.cshtml"); ;
        }
        public ActionResult Login()
        {

            return View(new Users());
        }

        public ActionResult Register()
        {
            return View("Register");
        }
        public ActionResult Logout()
        {
            Session["Log"] = null;
            return RedirectToAction("Login", new Users());
        }
        public ActionResult LoginUser(Users obj)
        {
            if (ModelState.IsValid)
            {
                obj.type = "0";
                UsersDal udal = new UsersDal();
                udal.User.Add(obj);
                udal.SaveChanges();

            }
            return View("Login", obj);
        }

        public ActionResult CheckLogin(Users obj)
        {
            UsersDal udal = new UsersDal();
            List<Users> dbuser = (from x in udal.User where x.ID.Equals(obj.ID) select x).ToList();

            if (dbuser.Count == 0)
            {
                ViewBag.Error = "You arent registered, Register you before!";
                return View("Register");
            }
            else
            {
                if (obj.passworld!= dbuser[0].passworld)
                {
                    ViewBag.Error = "password Wrong";
                    return View("Login", obj);
                }

                Session["idlog"] = dbuser[0].ID;
                Session["Log"] = dbuser[0].type;
                if (dbuser[0].type == "0" || dbuser[0].type == "2")
                {
                    return RedirectToAction("Index", "Home");

                }
                if (dbuser[0].type == "1")
                {
                    return RedirectToAction("AllEvents", "Admin");
                }
               
                return View("Login", obj);

            }
        }
        public ActionResult Reset()
        {
            return View();
        }
    }
}
