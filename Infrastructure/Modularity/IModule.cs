namespace Infrastructure.Modularity
{
    public interface IModule
    {
        void Initialize(ModuleLoader moduleLoader);
    }
}
