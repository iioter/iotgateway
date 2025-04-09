using System;
using System.IO;

namespace WalkingTec.Mvvm.Core.Models
{
    public interface IWtmFile
    {
        string Path { get; set; }

        string FileName { get; set; }
        string FileExt { get; set; }
        long Length { get; set; }

        DateTime UploadTime { get; set; }

        string SaveMode { get; set; }
        string ExtraInfo { get; set; }
        string HandlerInfo { get; set; }

        Stream DataStream { get; set; }

        string GetID();
    }
}