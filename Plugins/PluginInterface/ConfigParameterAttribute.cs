namespace PluginInterface
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ConfigParameterAttribute : Attribute
    {
        public string Description { get; }

        public ConfigParameterAttribute(string description)
        {
            Description = description;
        }
    }
}