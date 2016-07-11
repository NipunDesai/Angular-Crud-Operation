using Angular.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular.Repository.Modules
{
  public  interface IEventRepository
    {
        List<EventRegistration> GetAllEventList();
    }
}
