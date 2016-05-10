using System;

namespace DynamicConfigurator.Common.Domain
{
    public class FormattedException : Exception
    {
        public FormattedException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
