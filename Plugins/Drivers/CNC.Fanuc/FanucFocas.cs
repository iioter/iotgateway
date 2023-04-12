namespace CNC.Fanuc
{

    public class FanucFocas : Focas1
    {
        public static ushort h;

        //下载程序  5-27
        //开始
        private static short dwnstart(ushort handle, short type)
        {
            return FanucFocas.cnc_dwnstart3(handle, type);
        }
        //结束
        private static short dwnend(ushort handle)
        {
            return FanucFocas.cnc_dwnend3(handle);
        }
        //下载
        private static short dwnload(ushort handle, ref int datalength, string data)
        {
            //开始下载程序  datalength将会被返回，实际的输出的字符数量
            return FanucFocas.cnc_download3(handle, ref datalength, data);
        }
        //获取详细的错误信息
        private static short getdtailerr(ushort handle, FanucFocas.ODBERR odberr)
        {
            return FanucFocas.cnc_getdtailerr(handle, odberr);
        }
        //下载程序的入口点
        /// <summary>
        /// 向CNC下载指定类型的程序
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="type">程序类型</param>
        /// <param name="data">程序的内容</param>
        /// <param name="odberr">保存返回错误信息的详细内容,为null不返回</param>
        /// <returns>错误码</returns>
        public static short download(ushort handle, short type, string data, FanucFocas.ODBERR odberr)
        {
            int datalength = data.Length;
            short ret = dwnstart(handle, type);
            if (ret == 0)
            {
                int olddata = datalength;
                while (true)
                {
                    ret = dwnload(handle, ref datalength, data);
                    //说明缓存已满或为空，继续尝试
                    if (ret == (short)FanucFocas.focas_ret.EW_BUFFER)
                    {
                        continue;
                    }
                    if (ret == FanucFocas.EW_OK)
                    {
                        //说明当前下载完成,temp记录剩余下载量
                        int temp = olddata - datalength;
                        if (temp <= 0)
                        {
                            break;
                        }
                        else
                        {
                            data = data.Substring(datalength, temp);
                            datalength = temp; olddata = temp;
                        }
                    }
                    else
                    {
                        //下载出现错误，解析出具体的错误信息
                        if (odberr != null)
                        {
                            getdtailerr(handle, odberr);
                        }
                        //下载出错
                        break;
                    }
                }
                //判断是哪里出的问题
                if (ret == 0)
                {
                    ret = dwnend(handle);
                    //结束下载出错
                    return ret;
                }
                else
                {
                    dwnend(handle);
                    //下载出错
                    return ret;
                }
            }
            else
            {
                dwnend(handle);
                //开始下载出错
                return ret;
            }
        }
        //下载程序  5-27

        //上传程序 5-28
        //开始
        private static short upstart(ushort handle, short type, int startno, int endno)
        {
            return cnc_upstart3(handle, type, startno, endno);
        }
        //上传
        private static short uplod(ushort handle, int length, char[] databuf)
        {
            return cnc_upload3(handle, ref length, databuf);
        }
        //结束
        private static short upend(ushort handle)
        {
            return cnc_upend3(handle);
        }
        //上传程序的入口
        /// <summary>
        /// 根据程序号读取指定程序
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="type">类型</param>
        /// <param name="no">程序号</param>
        /// <param name="odberr">详细错误内容，null不返回</param>
        /// <param name="data">返回的程序内容</param>
        /// <returns>错误码</returns>
        public static short upload(ushort handle, short type, int no, ref string data, FanucFocas.ODBERR odberr)
        {
            int startno = no; int endno = no;
            int length = 256; char[] databuf = new char[256];
            short ret = upstart(handle, type, startno, endno);
            if (ret == FanucFocas.EW_OK)
            {
                string temp = "";
                while (true)
                {
                    //上传
                    ret = uplod(handle, length, databuf);
                    temp = new string(databuf);
                    int one = temp.Length;
                    if (ret == (short)FanucFocas.focas_ret.EW_BUFFER)
                    {
                        continue;
                    }
                    if (ret == FanucFocas.EW_OK)
                    {
                        temp = temp.Replace("\0", "");
                        data += temp;
                        string lastchar = temp.Substring(temp.Length - 1, 1);
                        if (lastchar == "%")
                        {
                            break;
                        }
                    }
                    else
                    {
                        //下载出现错误，解析出具体的错误信息
                        if (odberr != null)
                        {
                            getdtailerr(handle, odberr);
                        }
                        //下载出错
                        break;
                    }
                }
                //判断是哪里出的问题
                if (ret == 0)
                {
                    ret = upend(handle);
                    //结束上传出错
                    return ret;
                }
                else
                {
                    upend(handle);
                    //上传出错
                    return ret;
                }
            }
            else
            {
                //开始出错
                upend(handle);
                return ret;
            }
        }
        //上传程序 5-28

        //根据alm_grp 编号 返回 提示内容 简
        public static string getalmgrp(short no)
        {
            string ret = "";
            switch (no)
            {
                case 0:
                    ret = "SW";
                    break;
                case 1:
                    ret = "PW";
                    break;
                case 2:
                    ret = "IO";
                    break;
                case 3:
                    ret = "PS";
                    break;
                case 4:
                    ret = "OT";
                    break;
                case 5:
                    ret = "OH";
                    break;
                case 6:
                    ret = "SV";
                    break;
                case 7:
                    ret = "SR";
                    break;
                case 8:
                    ret = "MC";
                    break;
                case 9:
                    ret = "SP";
                    break;
                case 10:
                    ret = "DS";
                    break;
                case 11:
                    ret = "IE";
                    break;
                case 12:
                    ret = "BG";
                    break;
                case 13:
                    ret = "SN";
                    break;
                case 14:
                    ret = "reserved";
                    break;
                case 15:
                    ret = "EX";
                    break;
                case 19:
                    ret = "PC";
                    break;
                default:
                    ret = "Not used";
                    break;
            }
            return ret;
        }
        //根据alm_grp 编号 返回 提示内容 简

        //2016-6-2 根据地址码和地址号，返回完整的显示信息
        public static string getpmcadd(short a, ushort b)
        {
            string tempa = "";
            switch (a)
            {
                case 0:
                    tempa = "G";
                    break;
                case 1:
                    tempa = "F";
                    break;
                case 2:
                    tempa = "Y";
                    break;
                case 3:
                    tempa = "X";
                    break;
                case 4:
                    tempa = "A";
                    break;
                case 5:
                    tempa = "R";
                    break;
                case 6:
                    tempa = "T";
                    break;
                case 7:
                    tempa = "K";
                    break;
                case 8:
                    tempa = "C";
                    break;
                case 9:
                    tempa = "D";
                    break;
                default:
                    tempa = "n";
                    break;
            }
            string tempb = b.ToString().PadLeft(4, '0');
            return tempa + tempb;
        }
        //2016-6-2 根据地址码和地址号，返回完整的显示信息



    }
}
