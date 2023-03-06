using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCompany.SampleProduct.EngineEntity
{
    public class ApplicationInstanceManager
    {
        private readonly ConcurrentDictionary<Guid, ApplicationInstance> _projects;
        public ApplicationInstanceManager()
        {
            _projects = new ConcurrentDictionary<Guid, ApplicationInstance>();
        }
        public Task<ProjectInfo> CreateProject(Empty request, ServerCallContext context)
        {
            return base.CreateProject(request, context);
        }
        public Task<Empty> DestoryProject(ProjectInfo request, ServerCallContext context)
        {
            return base.DestoryProject(request, context);
        }
    }
}
