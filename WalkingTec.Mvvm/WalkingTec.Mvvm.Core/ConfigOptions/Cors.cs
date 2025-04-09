using System.Collections.Generic;

namespace WalkingTec.Mvvm.Core
{
    public class Cors
    {
        public bool EnableAll { get; set; }
        public List<CorsPolicy> Policy { get; set; }
    }

    public class CorsPolicy
    {
        public string Name { get; set; }
        public string Domain { get; set; }
    }
}