namespace MicronSQL.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionsController : ControllerBase
{
    protected IDbConnectionManager _manager;
    public ConnectionsController(IDbConnectionManager manager)
    {
        _manager = manager;
    }

    [HttpPost("Connect")]
    public async Task<IActionResult> Connect(ConnectionModel model)
    {
        await _manager.Connect(model);
        var state = _manager.State;
        return Ok(state.ToString());
    }

    [HttpGet("Disconnect")]
    public async Task<IActionResult> Disconnect()
    {
        await _manager.Disconnect();
        var state = _manager.State;
        return Ok(state.ToString());
    }
}
