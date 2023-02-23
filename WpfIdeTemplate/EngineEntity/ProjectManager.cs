using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCompany.SampleProduct.EngineEntity
{
    public class ProjectManager
    {
        private readonly ConcurrentDictionary<Guid, Project> _projects;
        public ProjectManager()
        {
            _projects = new ConcurrentDictionary<Guid, Project>();
        }
        public Guid CreateProject()
        {
            var project = new Project();
            while (true)
            {
                var guid = Guid.NewGuid();
                if (_projects.TryAdd(guid, project))
                {
                    return guid;
                }
            }
            
        }
        public void DestroyProject(Guid guid)
        {
            _projects.TryRemove(guid, out var _);
        }
    }
}
