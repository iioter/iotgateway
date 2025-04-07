using Microsoft.Extensions.Localization;

namespace WalkingTec.Mvvm.Mvc
{
    public class MvcProgram
    {
        public static IStringLocalizer _localizer = Core.CoreProgram._localizer;
    }
}