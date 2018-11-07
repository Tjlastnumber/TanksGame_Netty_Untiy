using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[ProtoContract]
public class UserLoginData : ProtocalBase
{
    ////UserId
    [ProtoMember(1)]
    public string UserId { get; set; }
    ////用户名
    [ProtoMember(2)]
    public string UserName { get; set; }
    ////密码
    [ProtoMember(3)]
    public string PassWord { get; set; }

    public UserLoginData(string UserName, string PassWord)
    {
        this.UserName = UserName;
        this.PassWord = PassWord;
    }

    public UserLoginData() { }
}
