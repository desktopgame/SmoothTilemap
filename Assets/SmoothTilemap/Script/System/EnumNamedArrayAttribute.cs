// https://answers.unity.com/questions/1589226/showing-an-array-with-enum-as-keys-in-the-property.html
using UnityEngine;
using System;

namespace SmoothTilemap.System
{
    public class EnumNamedArrayAttribute : PropertyAttribute
    {
        public string[] names;
        public EnumNamedArrayAttribute(Type names_enum_type)
        {
            this.names = Enum.GetNames(names_enum_type);
        }
    }
}