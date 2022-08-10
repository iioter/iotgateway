using System.Text;

namespace DriverModbusMaster
{
    public class ModbusDataConvert
    {
        /// <summary>
        /// 赋值string
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetString(ushort[] src, int start, string? value)
        {
            if (value != null)
            {
                byte[] bytesTemp = Encoding.UTF8.GetBytes(value);
                ushort[] dest = Bytes2Ushorts(bytesTemp);
                dest.CopyTo(src, start);
            }
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetString(ushort[] src, int start, int len)
        {
            ushort[] temp = new ushort[len];
            for (int i = 0; i < len; i++)
            {
                temp[i] = src[i + start];
            }

            byte[] bytesTemp = Ushorts2Bytes(temp);
            string res = Encoding.UTF8.GetString(bytesTemp).Trim(new[] { '\0' });
            return res;
        }

        /// <summary>
        /// 赋值Real类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        public static void SetReal(ushort[] src, int start, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            ushort[] dest = Bytes2Ushorts(bytes);

            dest.CopyTo(src, start);
        }

        /// <summary>
        /// 获取float类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static float GetReal(ushort[] src, int start)
        {
            ushort[] temp = new ushort[2];
            for (int i = 0; i < 2; i++)
            {
                temp[i] = src[i + start];
            }

            byte[] bytesTemp = Ushorts2Bytes(temp);
            float res = BitConverter.ToSingle(bytesTemp, 0);
            return res;
        }

        /// <summary>
        /// 赋值Short类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        public static void SetShort(ushort[] src, int start, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            ushort[] dest = Bytes2Ushorts(bytes);

            dest.CopyTo(src, start);
        }

        /// <summary>
        /// 获取short类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static short GetShort(ushort[] src, int start)
        {
            ushort[] temp = new ushort[1];
            temp[0] = src[start];
            byte[] bytesTemp = Ushorts2Bytes(temp);
            short res = BitConverter.ToInt16(bytesTemp, 0);
            return res;
        }


        public static bool[] GetBools(ushort[] src, int start, int num)
        {
            ushort[] temp = new ushort[num];
            for (int i = start; i < start + num; i++)
            {
                temp[i] = src[i + start];
            }

            byte[] bytes = Ushorts2Bytes(temp);

            bool[] res = Bytes2Bools(bytes);

            return res;
        }

        private static bool[] Bytes2Bools(byte[] b)
        {
            bool[] array = new bool[8 * b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    array[i * 8 + j] = (b[i] & 1) == 1; //判定byte的最后一位是否为1，若为1，则是true；否则是false
                    b[i] = (byte)(b[i] >> 1); //将byte右移一位
                }
            }

            return array;
        }

        private static byte Bools2Byte(bool[]? array)
        {
            if (array != null && array.Length > 0)
            {
                byte b = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (array[i])
                    {
                        byte nn = (byte)(1 << i); //左移一位，相当于×2
                        b += nn;
                    }
                }

                return b;
            }

            return 0;
        }

        private static ushort[] Bytes2Ushorts(byte[] src, bool reverse = false)
        {
            int len = src.Length;

            byte[] srcPlus = new byte[len + 1];
            src.CopyTo(srcPlus, 0);
            int count = len >> 1;

            if (len % 2 != 0)
            {
                count += 1;
            }

            ushort[] dest = new ushort[count];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i] = (ushort)(srcPlus[i * 2] << 8 | srcPlus[2 * i + 1] & 0xff);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i] = (ushort)(srcPlus[i * 2] & 0xff | srcPlus[2 * i + 1] << 8);
                }
            }

            return dest;
        }

        private static byte[] Ushorts2Bytes(ushort[] src, bool reverse = false)
        {
            int count = src.Length;
            byte[] dest = new byte[count << 1];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 8);
                    dest[i * 2 + 1] = (byte)(src[i] >> 0);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 0);
                    dest[i * 2 + 1] = (byte)(src[i] >> 8);
                }
            }

            return dest;
        }
    }
}