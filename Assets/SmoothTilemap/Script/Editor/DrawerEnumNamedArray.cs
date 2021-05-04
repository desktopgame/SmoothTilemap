// https://answers.unity.com/questions/1589226/showing-an-array-with-enum-as-keys-in-the-property.html
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using SmoothTilemap.System;

namespace SmoothTilemap.Editor
{
    [CustomPropertyDrawer(typeof(EnumNamedArrayAttribute))]
    public class DrawerEnumNamedArray : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
           EnumNamedArrayAttribute enumNames = attribute as EnumNamedArrayAttribute;
           //propertyPath returns something like component_hp_max.Array.data[4]
           //so get the index from there
           int index = Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));
           //change the label
           label.text = enumNames.names[index];
           //draw field
           EditorGUI.PropertyField( position, property, label, true );
        }
    }
}