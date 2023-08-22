using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Jwap.DAL.Entities
{
    public enum MessegeTypes
    {
        [EnumMember(Value ="text")]
        text,
        [EnumMember(Value = "audio")]
        audio,
        [EnumMember(Value = "image")]
        image,
        [EnumMember(Value = "file")]
        file
    }
}
