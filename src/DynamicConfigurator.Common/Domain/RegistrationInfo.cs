namespace DynamicConfigurator.Common.Domain
{
    public class RegistrationInfo
    {
        public string Application { get; set; }
        public string Environment { get; set; }
        public string ClientAddress { get; set; }

        public override string ToString()
        {
            return $"{Application}|{Environment}|{ClientAddress}";
        }

        protected bool Equals(RegistrationInfo other)
        {
            return ToString().Equals(other.ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((RegistrationInfo)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
