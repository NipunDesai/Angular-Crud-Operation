using Angular.Core.Controllers;
using Angular.Core.Global;
using Angular.DomainModel.DataContext;
using Angular.Repository.DataRepository;
using Angular.Repository.Logger;
using Angular.Repository.Modules;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Angular.web.App_Start
{
    public class AutofacConfig
    {
        public static IContainer RegisterDependencies()
        {
            var containerBuilder = new ContainerBuilder();

            //Register all Controller within current assembly
            containerBuilder.RegisterControllers(typeof(AccountController).Assembly);
            containerBuilder.RegisterApiControllers(typeof(AccountController).Assembly);

            //Register DbContext
            containerBuilder.RegisterType<AngularDbContext>().As<DbContext>().InstancePerDependency();

            //This will set dependency resolver for MVC
            containerBuilder.RegisterType<UserStore<IdentityUser>>().As<IUserStore<IdentityUser>>().InstancePerRequest();
            containerBuilder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            containerBuilder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            containerBuilder.RegisterType<EmailService>();
            containerBuilder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            containerBuilder.Register<IDataProtectionProvider>(c => Startup.DataProtectionProvider).InstancePerRequest();

            //Registration of Generic DataRepository
            containerBuilder.RegisterGeneric(typeof(DataRepository<>)).As(typeof(IDataRepository<>)).InstancePerDependency();

       
            /*Register Loggers*/
            containerBuilder.RegisterType<Logger>().As<ILogger>().InstancePerDependency();

            /*Register Event Repository*/
            containerBuilder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerDependency();
            containerBuilder.Register(x => AutoMapperConfig.RegisterAutoMapper()).As<IMapper>().SingleInstance();


            var container = containerBuilder.Build();

            //container.ActivateGlimpse();

            //This will set dependency resolver for MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //This will set dependency resolver for WebAPI
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;


            return container;
        }
    }
}