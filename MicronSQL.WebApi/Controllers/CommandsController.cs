namespace MicronSQL.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    protected IDbCommandManager _manager;
    public CommandsController(IDbCommandManager manager)
    {
        _manager = manager;
    }

    [HttpGet("GetDatabases")]
    public async Task<IActionResult> GetDatabases()
    {
        var res = await _manager.GetDatabases();
        return Ok(res);
    }

    [HttpGet("{database}/GetTables")]
    public async Task<IActionResult> GetTables(string database)
    {
        var res = await _manager.GetTables(database);
        return Ok(res);
    }

    [HttpGet("{database}/{table}/Data")]
    public async Task<IActionResult> GetDatas(string database, string table)
    {
        var res = await _manager.GetData(database, table);
        return Ok(res);
    }

    [HttpPost("{database}/RunQuery")]
    public async Task<IActionResult> RunQuery(string database, [FromBody] string query)
    {
        var res = await _manager.ExecuteQuery(database, query);
        return Ok(res);

    }
}
