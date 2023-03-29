using MassTransit;

using Microsoft.AspNetCore.Mvc;

using Shared.Contracts.Models.Rpc.ServiceB.TestCreate;

namespace ServiceA.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateController : ControllerBase
{
    private readonly IRequestClient<ServiceBTestCreateRequest> _requestClient;

    public CreateController(IRequestClient<ServiceBTestCreateRequest> requestClient) => _requestClient = requestClient;

    [HttpPost]
    public async Task<IActionResult> Create(string name, CancellationToken cancellationToken)
    {
        Response<ServiceBTestCreateResponse> response = await _requestClient.GetResponse<ServiceBTestCreateResponse>(
            new ServiceBTestCreateRequest { Name = name },
            cancellationToken
        );

        return Ok(response.Message);
    }
}
