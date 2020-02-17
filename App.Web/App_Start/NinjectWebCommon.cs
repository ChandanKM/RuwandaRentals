using App.DataAccess;
using App.DataAccess.Interfaces;
using App.UIServices;
using System;
using System.Web;
using System.Web.Http;
using App.Web.App_Start;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Syntax;
using System.Web.Http.Dependencies;
using App.UIServices.InterfaceServices;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace App.Web.App_Start
{

    // Provides a Ninject implementation of IDependencyScope
    // which resolves services using the Ninject container.
    public class NinjectDependencyScope : IDependencyScope
    {
        IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return resolver.TryGet(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return resolver.GetAll(serviceType);
        }

        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
        }
    }

    // This class is the resolver, but it is also the global scope
    // so we derive from NinjectScope.
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }


    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
        
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>();
            kernel.Bind<IDisposable>().To<Disposable>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<ICityMasterService>().To<CityMasterService>();
            kernel.Bind<IConsumerService>().To<ConsumerService>();
            kernel.Bind<IPropertyService>().To<PropertyService>();
            kernel.Bind<IPincodeService>().To<PincodeService>();
            kernel.Bind<ILocationService>().To<LocationService>();
            kernel.Bind<IVendorService>().To<VendorService>();
            kernel.Bind<ICityService>().To<CityService>();
            kernel.Bind<IFacilityService>().To<FacilityService>();
            kernel.Bind<IRoomsService>().To<RoomsService>();
            kernel.Bind<IRoomTypeServices>().To<RoomTypeServices>();
            kernel.Bind<ILoyaltyServices>().To<LoyaltyServices>();
            kernel.Bind<IPromotionServices>().To<PromotionServices>();
     
            kernel.Bind<ISubscribeServices>().To<SubscribeServices>();
            kernel.Bind<IUserProfileServices>().To<UserProfileServices>();
            kernel.Bind<IParamServices>().To<ParamServices>();
            kernel.Bind<ISystemProfileServices>().To<SystemProfileServices>();
            kernel.Bind<IManageLocationServices>().To<ManageLocationServices>();
            kernel.Bind<ILoginServices>().To<LoginServices>();
            kernel.Bind<IApplicationExceptionServices>().To<ApplicationExceptionServices>();
            kernel.Bind<IBookingService>().To<BookingService>();
            kernel.Bind<ICCAvenueServices>().To<CCAvenueServices>();
            kernel.Bind<ICorporateService>().To<CorporateService>();
        }        
    }
}
