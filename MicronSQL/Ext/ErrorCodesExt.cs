using MicronSQL.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace MicronSQL.Ext;

public static class ErrorCodesExt
{
    public static ManagerException Throw(this ErrorCodes error)
    {
        return new ManagerException(((int)error).ToString(), error.ToString());
    }
}


