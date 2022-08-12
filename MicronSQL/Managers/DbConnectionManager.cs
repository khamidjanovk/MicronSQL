namespace MicronSQL.Managers;

public class DbConnectionManager : IDbConnectionManager
{
    protected NpgsqlConnection connection;
    public ConnectionState State => connection.State;
    public NpgsqlConnection Connection => connection;

    public DbConnectionManager()
    {
        connection = new NpgsqlConnection();
    }

    public async ValueTask Disconnect()
    {
        if(State == ConnectionState.Closed)
        {
            throw ErrorCodes.ConnectionIsNotOpen.Throw();
        }
        await connection.CloseAsync();
    }

    public async ValueTask Connect(ConnectionModel model)
    {
        if(State == ConnectionState.Open)
        {
            throw ErrorCodes.ConnectionAlreadyOpened.Throw();
        }
        connection.ConnectionString = model.ToConnectionString();
        await connection.OpenAsync();

    }
}
