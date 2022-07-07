using System;

public class NotValidOperationException : Exception
{
    public NotValidOperationException(string method, string className)
    {
        this.className = className;
        this.method = method;
    }

    public string method;
    public string className;
}
