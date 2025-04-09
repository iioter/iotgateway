using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace WalkingTec.Mvvm.Core
{
    public class CoreProgram
    {
        public static IStringLocalizer _localizer
        {
            get;
            set;
        }

        public static JsonSerializerOptions DefaultJsonOption
        {
            get; set;
        }

        public static JsonSerializerOptions DefaultPostJsonOption
        {
            get; set;
        }

        public static string[] Buildindll = new string[]
            {
                    "WalkingTec.Mvvm.Core",
                    "WalkingTec.Mvvm.Mvc",
                    "WalkingTec.Mvvm.Admin",
                    "WalkingTec.Mvvm.Taghelpers"
            };
    }
}