using MVCEventCalendar.DAL;
using MVCEventCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 /*
  0 Worker
  1 admin
  2 client 
     
     */
namespace MVCEventCalendar.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        // CLIENT  
        public JsonResult GetEventsClients()
        {
            if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                Redirect("~/Views/Home/NotLogged.cshtml");
                return null;
            }
            using (EventDal dc = new EventDal())
            {
                String user = Session["idlog"].ToString();
                var events = (from x in dc.Event where x.Invite.Equals(user) select x).ToList();

                //var events = dc.Event.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        // GESTION Worker 
        public ActionResult Index()
        {
            if (Session["Log"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            if (Session["Log"].ToString() == "1")
            {
                
                return RedirectToAction("AllEvents", "Admin");
            }
            if (Session["Log"].ToString() == "2")
            {
                return View("HomeClient");
                //return RedirectToAction("AllEvents", "Admin");
            }
            return View();
        }
        public ActionResult addEvent()
        {
            if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            EventClient ev = new EventClient();
            String user = Session["idlog"].ToString();
            UsersDal ud = new UsersDal();
            ev.UsersList = (from x in ud.User where x.Worker.Equals(user) select x).ToList();
            ev.Event = new Models.Event();
            return View(ev);
        }
        public ActionResult addEventSubmit()
        {
            Models.Event S = new Models.Event();
            if (Request.Form["Clients"] != null)
                ;
            if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }

            S.Start = DateTime.ParseExact(Request.Form["Event.Start"].ToString(), "yyyy-MM-ddTHH:mm", null);
            if(Request.Form["Event.End"] !="")
                S.End = DateTime.ParseExact(Request.Form["Event.End"].ToString(), "yyyy-MM-ddTHH:mm", null);

            S.User = Session["idlog"].ToString();
            S.Description = Request.Form["Event.Description"].ToString();
            S.IsFullDay = Boolean.Parse(Request.Form["IsFullDay"].ToString());
            S.ThemeColor = Request.Form["ThemeColor"].ToString();
            S.Subject= Request.Form["Event.Subject"].ToString();
            S.Location = Request.Form["Event.Location"].ToString();
            EventDal dc = new EventDal();
            if (Request.Form["Clients"] != null)
            {
                foreach (String t in Request.Form["Clients"].ToString().Split(','))
                {
                    S.Invite = t;
                    dc.Event.Add(S);
                    dc.SaveChanges();
                }
            }

            else
            { 
                dc.Event.Add(S);
                dc.SaveChanges();
                //return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            
            return RedirectToAction("Index");
        }
        public JsonResult GetEvents()
        {
            if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                Redirect("~/Views/Home/NotLogged.cshtml");
                return null;
            }
            using (EventDal dc = new EventDal())
            {
                String user = Session["idlog"].ToString();
                var events = (from x in dc.Event where x.User.Equals(user) select x).ToList();

                //var events = dc.Event.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult DeleteEvent()
        {
            if (Session["idlog"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }

            EventDal ud = new EventDal();
            Models.Event test = new Models.Event()
            {
                EventID = int.Parse(Request.Form["EventID"])

            };
            var customer = ud.Event.Single(o => o.EventID == test.EventID);

            ud.Event.Remove(customer);
            ud.SaveChanges();

            return null;
        }
        public ActionResult ClientsList()
        {
            //Session["Log"] = "3";
            if (Session["Log"] == null)
            {
                ViewBag.Error = "not logged";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            if (Session["Log"].ToString() != "0")
            {
                ViewBag.Error = "not Worker ";
                return View("~/Views/Home/NotLogged.cshtml");
            }
            return View();
        }
        public ActionResult SubmitUser()
        {

            Users objusers = new Users();
            UsersDal ud = new UsersDal();
            objusers.ID = Request.Form["ID"].ToString();
            objusers.passworld = Request.Form["passworld"].ToString();
            objusers.email = Request.Form["email"].ToString();
            objusers.type = Request.Form["type"].ToString();
            objusers.Worker = Session["idlog"].ToString();
            if (ModelState.IsValid)
            {
                ud.User.Add(objusers);
                ud.SaveChanges();
            }
            return View("ClientsList");

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
            String user = Session["idlog"].ToString();
            UsersDal ud = new UsersDal();
            var events = (from x in ud.User where x.Worker.Equals(user) select x).ToList();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

    }
}