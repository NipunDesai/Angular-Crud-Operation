using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular.web.App_Start
{
    public class AutoMapperConfig
    {
        public static IMapper RegisterAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //Register model
                //cfg.CreateMap<DeliveryLoads, DeliveryLoadsAc>();
                //.ForMember(vm => vm.TransportModeCode, m => m.MapFrom(u => (u.TransportModeCode)));

                //AutoMapper.Mapper.CreateMap<Foo, string>().ConvertUsing(x => x.DisplayName());

                //GetType().GetField(vm.TransportModeCode.ToString())

            });
            return config.CreateMapper();
        }
    }
}