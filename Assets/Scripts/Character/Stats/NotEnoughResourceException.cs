using Character.Stats;
using System;

public class NotEnoughResourceException : Exception
{

    public ResourceType resource;
    public NotEnoughResourceException(ResourceType resource)
    {
        this.resource = resource;
    }
}
