using UnityEngine;
using UnityEditor;
///<summary>
///绘制多选属性
///</summary>
[CustomPropertyDrawer(typeof(EnumFlags))]
public class EnumFlagsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
}