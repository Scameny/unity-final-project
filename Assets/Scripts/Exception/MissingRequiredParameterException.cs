using System;
public class MissingRequiredParameterException : Exception
{
    public MissingRequiredParameterException(string var, string className)
    {
        this.className = className;
        this.var= var;
    }

    public string var;
    public string className;
}
