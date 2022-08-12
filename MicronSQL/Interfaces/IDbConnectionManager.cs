namespace MicronSQL.Interfaces;

public interface IDbConnectionManager
{
    NpgsqlConnection Connection { get; }
    ConnectionState State { get; }
    ValueTask Disconnect();
    ValueTask Connect(ConnectionModel model);
}