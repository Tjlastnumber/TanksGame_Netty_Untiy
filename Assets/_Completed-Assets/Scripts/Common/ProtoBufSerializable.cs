using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class ProtoBufSerializable
{
    /// <summary>
    /// 编码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="structData"></param>
    /// <returns></returns>
    public static byte[] Encode<T>(T structData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            Serializer.Serialize(ms, structData);
            byte[] data = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(data, 0, data.Length);
            return data;
        }
    }

    /// <summary>
    /// 解码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="b"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static T Decode<T>(byte[] b, int offset, int length)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(b, offset, length);
            ms.Position = 0;
            T t = Serializer.Deserialize<T>(ms);
            return t;
        }
    }

    public static T Decode<T>(byte[] b)
    {
        return Decode<T>(b, 0, b.Length);
    }
}
