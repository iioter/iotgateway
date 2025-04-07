using System.Collections.Generic;

namespace WalkingTec.Mvvm.Core.Support.Json
{
    public class ApiListModel<T>
    {
        public List<T> Data { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
    }
}