using Angular.Repository.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Angular.DomainModel.Models;
using Angular.Repository.Modules;

namespace Angular.Core.Global
{
   public class CurrentUserContext
    {
        private readonly ILogger _iloggerRepository;
        private readonly IEventRepository _eventRepository;
        public List<EventRegistration> EventList { get; private set; }
        public CurrentUserContext(ILogger iloggerRepository, IEventRepository eventRepository)
        {
            _iloggerRepository = iloggerRepository;
            _eventRepository = eventRepository;
            InitiateCurrentUser();
        }

        private void InitiateCurrentUser()
        {
            try
            {
                _iloggerRepository.LogInfo("Current user initiation");
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    var userId = HttpContext.Current.User.Identity.GetUserId();
                    EventList = _eventRepository.GetAllEventList();
                }
            }
            catch(Exception ex)
            {
                _iloggerRepository.LogException(ex);
                throw;

            }
        }
    }
}
