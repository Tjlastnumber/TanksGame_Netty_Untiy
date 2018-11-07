using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ProtocalBase
{
    public ProtocalBase() { }

    public virtual byte[] Encode()
    {
        return ProtoBufSerializable.Encode(this);
    }
}
