using System;


namespace CardSystem
{
    public class EmptyCardContainerException : Exception
    {

        public EmptyCardContainerException( string className)
        {
            this.className = className;
        }

        public string className;
    }
}
