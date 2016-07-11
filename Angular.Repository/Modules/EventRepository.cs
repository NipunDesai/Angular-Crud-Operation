using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Angular.DomainModel.Models;
using Angular.Repository.Logger;
using Angular.Repository.DataRepository;

namespace Angular.Repository.Modules
{
    public class EventRepository : IEventRepository
    {
        private readonly ILogger _iLoggerRepository;
        private readonly IDataRepository<EventRegistration> _eventDataContext;
        public EventRepository(ILogger iLoggerRepository, IDataRepository<EventRegistration> eventDataContext)
        {
            _iLoggerRepository = iLoggerRepository;
            _eventDataContext = eventDataContext;
        }

        public List<EventRegistration> GetAllEventList()
        {
            return _eventDataContext.GetAll().ToList();
        }
    }
}
