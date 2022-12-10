using MassTransit;

using MassTransitSandbox.Api.Models;

using Microsoft.AspNetCore.Mvc;

namespace MassTransitSandbox.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoggerController : ControllerBase
{
    private readonly IBus _bus;

    public LoggerController(IBus bus) => _bus = bus;

    /// <summary>
    /// Отправка сообщения ILogMessageCreated в шину RabbitMQ.
    /// </summary>
    /// <param name="level">Уровень/уровни логированияю. Может принимать значения: info, error, info.error, error.info</param>
    /// <param name="message">Сообщение выводимое в лог</param>
    [HttpPost(Name = "CreateLogMessage")]
    public async Task<IActionResult> CreateLogMessage(string level, string message)
    {
        await _bus.Publish<ILogMessageCreated>(new LogMessageCreated { Message = message }, context => context.SetRoutingKey(level));

        return Accepted();
    }
}
