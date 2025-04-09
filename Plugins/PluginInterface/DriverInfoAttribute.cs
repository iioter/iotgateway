namespace PluginInterface
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DriverInfoAttribute : Attribute
    {
        public string Name { get; }
        public string Copyright { get; }
        public string Version { get; }

        public DriverInfoAttribute(string name, string version, string copyRight)
        {
            Name = name;
            Version = version;
            Copyright = copyRight;
        }
    }
}