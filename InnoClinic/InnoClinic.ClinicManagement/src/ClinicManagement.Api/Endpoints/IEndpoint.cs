using Microsoft.AspNetCore.Routing;

namespace ClinicManagement.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
