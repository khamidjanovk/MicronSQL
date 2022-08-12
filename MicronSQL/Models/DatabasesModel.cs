namespace MicronSQL.Models;

public class DatabasesModel
{
    public DatabasesModel(List<string> databases, long elapsedTime)
    {
        Databases = databases;
        ElapsedTime = elapsedTime.ToString() + "ms";
    }

    public List<string> Databases { get; set; }
    public string ElapsedTime { get; set; }
}
