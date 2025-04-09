using System.Collections.Generic;

namespace WalkingTec.Mvvm.Core.Support
{
    public class NugetInfo
    {
        public List<NugetInfoData> data { get; set; }
    }

    public class NugetInfoData
    {
        public string version { get; set; }
        public List<NugetVersionData> versions { get; set; }
    }

    public class NugetVersionData
    {
        public string version { get; set; }
    }
}