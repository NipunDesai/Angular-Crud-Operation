using Angular.Core.Global;
using Angular.Repository.Logger;
using Angular.Repository.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Angular.Core
{
   public class BaseController : ApiController
    {
        private readonly ILogger _iLoggerRepository;
        private readonly IEventRepository _eventRepository;
        private  CurrentUserContext _currentUser;
        public BaseController(ILogger iLoggerRepository, IEventRepository eventRepository)
        {
            _iLoggerRepository = iLoggerRepository;
            _eventRepository = eventRepository;
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
    }
}
