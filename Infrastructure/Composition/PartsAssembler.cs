using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Infrastructure.Composition
{
    public class PartsAssembler
    {
        public void ComposeParts(object target)
        {
            var path = PathProvider.BinaryPath;
            var directoryCatalog = new DirectoryCatalog(path);
            var container = new CompositionContainer(directoryCatalog);
            container.ComposeParts(target);
        }
    }
}