using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;
using SampleCompany.SampleProduct.EngineEntity;

namespace SampleCompany.SampleProduct.ApplicationEngine.Services
{
    public class ProjectManagementService : ProjectManagement.ProjectManagementBase
    {
        private readonly ProjectManager _projectManager;
        private readonly ILogger<ProjectManagementService> _logger;

        public ProjectManagementService(ProjectManager projectManager, ILogger<ProjectManagementService> logger)
        {
            _projectManager = projectManager;
            _logger = logger;
        }

        public override Task<ProjectInfo> CreateProject(Empty request, ServerCallContext context)
        {
            var guid = _projectManager.CreateProject();
            var info = new ProjectInfo() { Id = guid.ToString() };
            return Task.FromResult(info);
        }
        public override Task<Empty> DestoryProject(ProjectInfo request, ServerCallContext context)
        {
            var guid = new Guid(request.Id);
            _projectManager.DestroyProject(guid);
            return Task.FromResult(new Empty());
        }
    }
}
