using Infrastructure.Composition;

namespace Infrastructure.Modularity
{
    public class ModuleLoader
    {
        public void LoadAllModules()
        {
            new PartsList<IModule>(module => module.Initialize(this));
        }

        public void Initialize(object container)
        {
        }
    }
}
