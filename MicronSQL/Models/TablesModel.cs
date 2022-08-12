namespace MicronSQL.Models;

public class TablesModel
{
    public TablesModel(
        string database,
        List<string> tables, 
        long elapsedTime)
    {
        Database = database;
        Tables = tables;
        ElapsedTime = elapsedTime.ToString() + "ms";
    }

    public string Database { get; set; }
    public List<string> Tables { get; set; }
    public string ElapsedTime { get; set; }
}
