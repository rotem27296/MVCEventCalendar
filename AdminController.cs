using MVCEventCalendar.DAL;
using MVCEventCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Net.Mail;
using Project.DAL;
using System.Net;


namespace MVCEventCalendar.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AllEvents()
        {
            if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                Redirect("~/Views/Home/NotLogged.cshtml");
                return null;
            }
            if (Session["Log"].ToString() != "1")
            {
                ViewBag.Error = "Admin Only";
                Redirect("~/Views/Home/NotLogged.cshtml");
                return null;
            }

            return View();
        }
        public JsonResult GetEvents()
        {
           if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                Redirect("~/Views/Home/NotLogged.cshtml");
                return null;
            }
            if (Session["Log"].ToString() == "1")
                using (EventDal dc = new EventDal())
                {

                    var events = dc.Event.ToList();

                    //var events = dc.Event.ToList();
                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            else
                return new JsonResult {Data=null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public async System.Threading.Tasks.Task<ActionResult> resetPassword()
        {

            try
            {
                RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                byte[] randomBytes = new byte[10];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                string hashe = Convert.ToBase64String(randomBytes);
                UsersDal ud = new UsersDal();
                var var_id = Request.Form["ID"].ToString();
                Users us = ud.User.Single(x => x.ID == var_id);
                Reset to_reset = new Reset()
                {
                    hash = hashe,
                    ID = us.ID
                };
                ResetDal rd = new ResetDal();
                rd.reset.Add(to_reset);
                rd.SaveChanges();

                var body = "<p>id: {0} ({1})</p><p>To reset Password:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(us.email));  // replace with valid value 
                message.From = new MailAddress("mreouven@rubnet.fr");  // replace with valid value
                message.Subject = "reset password Calendar";
                message.Body = string.Format(body, us.ID, us.email, "https://calendarsce.azurewebsites.net/Public/resetPassword?hash=" + to_reset.hash);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "mreouven@rubnet.fr",  // replace with valid value
                        Password = "Rubnetisrael!09"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.ionos.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = false;
                    await smtp.SendMailAsync(message);

                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }


            ViewBag.success = "mail is send";
            return View("~/Views/Public/Login.cshtml"); ;
        }

        public ActionResult DeleteUsers()
        {
            if (Session["Log"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            if (Session["Log"].ToString() == "1")
            {
                UsersDal ud = new UsersDal();
                Users test = new Users()
                {
                    ID = Request.Form["ID"].ToString()

                };
                var customer = ud.User.Single(o => o.ID == test.ID);

                ud.User.Remove(customer);
                ud.SaveChanges();
            }
            return null;
        }
        public ActionResult getUsersByJson()
        {
            UsersDal ud = new UsersDal();
            return Json(ud.User.ToList<Users>(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SubmitUser()
        {

            Users objusers = new Users();
            UsersDal ud = new UsersDal();
            objusers.ID = Request.Form["ID"].ToString();
            objusers.passworld = Request.Form["passworld"].ToString();
            objusers.email = Request.Form["email"].ToString();
            objusers.type = Request.Form["type"].ToString();
            if (ModelState.IsValid)
            {
                ud.User.Add(objusers);
                ud.SaveChanges();
            }
            return View("UserAdmin");

        }
        public ActionResult UserAdmin()
        {
            //Session["Log"] = "3";
            if (Session["Log"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            if (Session["Log"].ToString() != "1")
            {
                ViewBag.Error = "not Admin ";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            return View();
        }

    }

}