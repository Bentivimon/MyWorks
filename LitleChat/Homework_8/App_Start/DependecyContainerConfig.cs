using Homework_8.Abstract;
using Homework_8.Ioc;
using Homework_8.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Homework_8.App_Start
{
    public class DependecyContainerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var uc = new UnityContainer();
            uc.RegisterType<IDataManager, UserDataManager>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(uc);
            var dataManager = uc.Resolve<IDataManager>();
        }
    }
}