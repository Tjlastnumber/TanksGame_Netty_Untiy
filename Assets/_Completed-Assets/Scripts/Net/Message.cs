using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Message
{
    /// <summary>
    /// 数据缓存长度
    /// </summary>
    public const int BUFFER_SIZE = 1024;
    public const int headLength = sizeof(int);
    private byte[] data = new byte[BUFFER_SIZE];
    private int startIndex;

    /// <summary>
    /// 数据
    /// </summary>
    public byte[] Data
    {
        get { return data; }
    }

    /// <summary>
    /// 起始索引
    /// </summary>
    public int StartIndex
    {
        get { return startIndex; }
    }

    /// <summary>
    /// 保留长度
    /// </summary>
    public int RemainSize
    {
        get { return data.Length - startIndex; }
    }

    /// <summary>
    /// 解码并执行命令回调
    /// </summary>
    /// <param name="dataLenght">数据流长度</param>
    /// <param name="actionCallback">回调函数</param>
    public void DecodeMessage(int dataLenght, Action<ProtocalData> actionCallback)
    {
        startIndex += dataLenght;
        while (true)
        {
            if (startIndex <= headLength) return;
            int bodyLength = NettyBitConverter.ToInt(data);
            if ((startIndex - headLength) >= bodyLength)
            {
                ProtocalData data = ProtoBufSerializable.Decode<ProtocalData>(Data, headLength, bodyLength);
                actionCallback(data);
                Array.Copy(Data, headLength + bodyLength, Data, 0, startIndex - headLength - bodyLength);
                startIndex -= (headLength + bodyLength);
            } 
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 编码消息
    /// </summary>
    /// <param name="data">消息对象</param>
    /// <returns>序列化结果</returns>
    public static byte[] EncodeMessage(ProtocalData data)
    {
        byte[] body = data.Encode();
        byte[] head = NettyBitConverter.ToBytes(body.Length);
        return head.Concat(body).ToArray();
    }
}
