using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NettyBitConverter
{
    /// <summary>
    /// 转换前4个字节为Int类型
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int ToInt(byte[] data)
    {
        int num = 0;
        for (int i = 0; i < 4; i++)
        {
            num <<= 8;
            num |= (data[i] & 0xff);
        }
        return num;
    }

    /// <summary>
    /// 转换前4个字节为Int类型
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int ToInt(byte[] data, int offset)
    {
        int num = 0;
        for (int i = 0; i < 4; i++)
        {
            num <<= 8;
            num |= (data[offset + i] & 0xff);
        }
        return num;
    }

    public static byte[] ToBytes(int num)
    {
        byte[] bytes = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)(num >> (24 - i * 8));
        }
        return bytes;
    }
}
