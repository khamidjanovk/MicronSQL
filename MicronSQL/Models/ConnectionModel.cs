namespace MicronSQL.Models;
public class ConnectionModel
{
    public ConnectionModel(
        string host, 
        string username, 
        string password)
    {
        Host = host;
        Username = username;
        Password = password;
    }
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string ToConnectionString()
    {
        return $"Host={Host};Username={Username};Password={Password}";
    }
}
