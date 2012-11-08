using System.ComponentModel.Composition;

namespace Infrastructure.Modularity
{
    [Export(typeof(IModule))]
    public abstract class DefaultModule : IModule
    {
        public void Initialize(ModuleLoader moduleLoader)
        {
            //var baseControllerRoute = GetBaseControllerRoute();
            //moduleLoader.MapCodeRoutes(baseControllerRoute, typeof(TBaseControllerType));
        }
    }
}
