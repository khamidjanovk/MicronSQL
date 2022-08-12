namespace MicronSQL.Repositories;

public class DbCommandManager : IDbCommandManager
{
    protected IDbConnectionManager _connectionManager;
    protected NpgsqlCommand _command;
    public DbCommandManager(IDbConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
        _command = new NpgsqlCommand();
        _command.Connection = _connectionManager.Connection;
    }

    public async ValueTask<DatabasesModel?> GetDatabases()
    {
        if (_connectionManager.State != ConnectionState.Open)
        {
            throw ErrorCodes.ConnectionIsNotOpen.Throw();
        }
        var databases = new List<string>();
        Stopwatch sw = new Stopwatch();
        _command.CommandText = "SELECT datname FROM pg_database";

        sw.Start();
        NpgsqlDataReader dr = await _command.ExecuteReaderAsync();
        while (dr.Read())
        {
            if (dr[0] != null)
                databases.Add((string)dr[0]);
        }
        sw.Stop();

        await dr.CloseAsync();
        return new DatabasesModel(databases, sw.ElapsedMilliseconds);
    }

    public async ValueTask<TablesModel?> GetTables(string database)
    {
        if (_connectionManager.State != ConnectionState.Open)
        {
            throw ErrorCodes.ConnectionIsNotOpen.Throw();
        }

        await ChangeDB(database);

        var tables = new List<string>();
        Stopwatch sw = new Stopwatch();
        _command.CommandText = "SELECT table_name " +
            "FROM information_schema.tables WHERE " +
            "table_schema = 'public' ORDER BY table_name; ";

        sw.Start();
        NpgsqlDataReader dr = await _command.ExecuteReaderAsync();
        while (dr.Read())
        {
            if (dr[0] != null)
                tables.Add((string)dr[0]);
        }
        sw.Stop();

        await dr.CloseAsync();
        return new TablesModel(database, tables, sw.ElapsedMilliseconds);
    }

    public async ValueTask<string> GetData(string database, string table)
    {
        await ChangeDB(database);

        if (!await IsExistTable(database, table))
        {
            throw ErrorCodes.TableNotFound.Throw();
        }

        var res = SelectCommandToJson($"SELECT * FROM {table}");
        if (res == null)
        {
            throw ErrorCodes.QueryInvalid.Throw();
        }
        return res;

    }

    public async ValueTask<string> ExecuteQuery(string database, string query)
    {
        await ChangeDB(database);

        if (query.Contains("SELECT", StringComparison.OrdinalIgnoreCase))
        {
            var res = SelectCommandToJson(query);
            if (res == null)
            {
                throw ErrorCodes.QueryInvalid.Throw();
            }
            return res;
        }

        Stopwatch sw = new Stopwatch();
        _command.CommandText = query;

        sw.Start();
        int result = await _command.ExecuteNonQueryAsync();
        sw.Stop();

        var message = result > 0 ?
            "Query completed successfully"
            :
            "Query failed";

        return JsonConvert.SerializeObject(new
        {
            Result = message,
            ElapsedTime = sw.ElapsedMilliseconds.ToString() + "ms"
        }, Formatting.Indented);
    }

    private async ValueTask<bool> IsExistDatabase(string database)
    {
        bool exists = false;
        _command.CommandText = $"SELECT datname FROM pg_database WHERE datname = '{database}' ";
        NpgsqlDataReader dr = await _command.ExecuteReaderAsync();
        if (dr.HasRows && dr.FieldCount == 1)
        {
            dr.Read();
            var res = dr.GetString(0);
            if (res == database)
            {
                exists = true;
            }
        }
        await dr.CloseAsync();
        return exists;
    }

    private async ValueTask<bool> IsExistTable(string database, string table)
    {
        var tables = await GetTables(database);
        if (tables == null)
        {
            throw ErrorCodes.DatabaseNotFound.Throw();
        }
        return tables.Tables.Contains(table) ? true : false;
    }

    private string? SelectCommandToJson(string query)
    {
        Stopwatch sw = new Stopwatch();
        DataSet _ds = new DataSet();
        DataTable _dt = new DataTable();
        NpgsqlDataAdapter dr = new NpgsqlDataAdapter(_command);

        sw.Start();
        _command.CommandText = query;
        dr.Fill(_ds);
        sw.Stop();

        try
        {
            _dt = _ds.Tables[0];
        }
        catch
        {
            return null;
        }

        var json = JsonConvert.SerializeObject(new
        {
            Data = _dt,
            ElapsedTime = sw.ElapsedMilliseconds.ToString() + "ms"
        }, Formatting.Indented);

        return json;
    }

    private async Task ChangeDB(string database)
    {
        if (_connectionManager.Connection.Database != database)
        {
            if (await IsExistDatabase(database))
                _connectionManager.Connection.ChangeDatabase(database);
            else
                throw ErrorCodes.DatabaseNotFound.Throw();
        }
    }
}
