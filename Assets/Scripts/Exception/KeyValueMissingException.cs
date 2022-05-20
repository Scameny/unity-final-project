using System;

public class KeyValueMissingException : Exception
{

    public KeyValueMissingException(string value, string className)
    {
        this.value = value;
        this.className = className;
    }

    public string value;
    public string className;
}
