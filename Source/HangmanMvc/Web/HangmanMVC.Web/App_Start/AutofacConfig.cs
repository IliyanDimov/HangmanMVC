namespace HangmanMVC.Web.App_Start
{
    using System.Web.Mvc;
    using System.Reflection;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Data;
    using System.Data.Entity;
    using Data.Common;

    public static class AutofacConfig
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Register services
            RegisterServices(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.Register(x => new ApplicationDbContext())
                .As<DbContext>()
                .InstancePerRequest();
            builder.RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<>))
                .InstancePerRequest();
            builder.RegisterGeneric(typeof(DbGenericRepository<,>))
                .As(typeof(IDbGenericRepository<,>))
                .InstancePerRequest();
        }
    }
}