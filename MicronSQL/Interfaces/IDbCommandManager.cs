namespace MicronSQL.Interfaces;

public interface IDbCommandManager
{
    ValueTask<string> ExecuteQuery(string database, string query);
    ValueTask<string> GetData(string database, string table);
    ValueTask<DatabasesModel?> GetDatabases();
    ValueTask<TablesModel?> GetTables(string database);
}