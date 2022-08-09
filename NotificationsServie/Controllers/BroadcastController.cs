using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationsServie.SignalR;

namespace NotificationsServie.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BroadcastController : ControllerBase
{
    private readonly IHubContext<EmployeesHub> _hub;

    public BroadcastController(IHubContext<EmployeesHub> hub)
    {
        _hub = hub;
    }
    [HttpGet]
    public async Task<IActionResult> Update()
    {
        await _hub.Clients.All.SendAsync("Update", "test");        
        return Ok("Update message sent.");
    }

}