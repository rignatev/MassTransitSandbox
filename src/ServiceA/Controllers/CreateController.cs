using MassTransit;
using MassTransit.Custom.Abstractions.Interfaces;

using Microsoft.AspNetCore.Mvc;

using ServiceA.DomainConsumers.LogConsumer;

using Shared.Contracts.Models.Rpc.ServiceB.TestCreate;

namespace ServiceA.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateController : ControllerBase
{
    private readonly IRequestClient<ServiceBTestCreateRequest> _requestClient;
    private readonly IDomainBus _domainBus;

    public CreateController(IRequestClient<ServiceBTestCreateRequest> requestClient , IDomainBus domainBus)
    {
        _requestClient = requestClient;
        _domainBus = domainBus;
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, CancellationToken cancellationToken)
    {
        await _domainBus.Publish(new LogTextReceived("121212"), cancellationToken);

        var response = await _requestClient.GetResponse<ServiceBTestCreateResponse>(
            new ServiceBTestCreateRequest { Name = name },
            cancellationToken
        );

        return Ok(response.Message);
    }
}
