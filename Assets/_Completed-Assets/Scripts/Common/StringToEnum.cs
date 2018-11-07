using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringToEnum
{
    public static T ToEnum<T>(this string v)
    {
        return (T) Enum.Parse(typeof(T), v, true);
    }
}
