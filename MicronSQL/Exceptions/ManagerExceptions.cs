namespace MicronSQL.Exceptions;

[Serializable]
public class ManagerException : Exception
{
    public string Code { get; set; }
    public ManagerException()
    {

    }
    public ManagerException(string code)
    {
        Code = code;
    }
    public ManagerException(string code, string message) : base(message)
    {
        Code = code;
    }
    public ManagerException(string code, string message, Exception inner) : base(message, inner)
    {
        Code = code;
    }
}
