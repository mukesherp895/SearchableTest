[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SearchableTest.WebApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SearchableTest.WebApp.App_Start.NinjectWebCommon), "Stop")]

namespace SearchableTest.WebApp.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using SearchableTest.WebApp.DataAccess;
    using SearchableTest.WebApp.Models;
    using SearchableTest.WebApp.Services;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region Services
            kernel.Bind<IService<Models.District>>().To<Service<Models.District>>();
            kernel.Bind<IContestantService>().To<ContestantService>();
            kernel.Bind<IContestantRatingService>().To<ContestantRatingService>();
            #endregion
            #region Repository
            kernel.Bind<IRepository<District>>().To<Repository<District>>();
            kernel.Bind<IRepository<Contestant>>().To<Repository<Contestant>>();
            kernel.Bind<IRepository<ContestantRating>>().To<Repository<ContestantRating>>();
            #endregion
            kernel.Bind<ApplicationDbContext>().ToMethod(ctx => new ApplicationDbContext()).InRequestScope();
            kernel.Bind<IDBFactory>().To<DBFactory>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
        }
    }
}