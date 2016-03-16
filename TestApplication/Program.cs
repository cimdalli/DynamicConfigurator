namespace ClientApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            var result1 = ConfigurationClient.Instance.GetConfiguration("test1");
            var result2 = ConfigurationClient.Instance.GetConfiguration("test2");
            var result3 = ConfigurationClient.Instance.GetConfiguration("system");
        }
    }
}
