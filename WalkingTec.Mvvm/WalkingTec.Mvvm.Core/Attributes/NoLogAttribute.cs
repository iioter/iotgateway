using System;

namespace WalkingTec.Mvvm.Core
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class NoLogAttribute : Attribute
    {
    }
}