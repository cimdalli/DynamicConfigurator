﻿namespace DynamicConfigurator.Server.Exceptions
{
    public class FormattedException : System.Exception
    {
        public FormattedException(string format, params object[] args) : base(string.Format(format, args))
        {
        }
    }
}
