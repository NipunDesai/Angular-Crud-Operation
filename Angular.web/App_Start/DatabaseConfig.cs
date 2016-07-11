using System.Data.Entity;
using Autofac;

using System;

namespace Angular.web.App_Start
{
    public class DatabaseConfig
    {
        public static void Initialize(IComponentContext componentContext)
        {

            using (var angularDbContext = componentContext.Resolve<DbContext>())
            {
                try
                {
                    angularDbContext.Database.Initialize(false);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}