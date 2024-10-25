using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace LibraryManagementSystem.IntegrationTests
{
    public abstract class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;

        protected BaseControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
    }
}
