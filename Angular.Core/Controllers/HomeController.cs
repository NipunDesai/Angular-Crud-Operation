using Angular.Core.Global;
using Angular.Repository.Logger;
using Angular.Repository.Modules;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Angular.Core.Controllers
{
    public class HomeController : Controller
    {
        private CurrentUserContext _currentUser;
        private readonly ILogger _iLoggerRepository;
        private readonly IEventRepository _eventRepository;
        private ApplicationUserManager _userManager;
        public HomeController(ILogger iLoggerRepository, IEventRepository eventRepository, ApplicationUserManager userManager)
        {
            _iLoggerRepository = iLoggerRepository;
            _eventRepository = eventRepository;
            _userManager = userManager;
        }

        protected CurrentUserContext CurrentUserContext
        {
            get
            {
                if (_currentUser == null && System.Web.HttpContext.Current != null)
                {
                    _currentUser = new CurrentUserContext(_iLoggerRepository,_eventRepository);
                }
                return _currentUser;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public JavaScriptResult EventList()
        {
            try
            {
                var eventList = CurrentUserContext.EventList;
                const string js = @"var eventList= {0};";
                return JavaScript(String.Format(js, JsonConvert.SerializeObject(eventList)));
            }
            catch (Exception ex)
            {
                _iLoggerRepository.LogException(ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            return View(CurrentUserContext.EventList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}