using UnityEngine;
using UnityEditor;

namespace IGS.Unity.Editor
{
    [CustomPropertyDrawer(typeof(MaxAttribute))]
    internal class MaxAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Min(property.floatValue, (attribute as MaxAttribute).max);

                EditorGUI.PropertyField(position, property, label);
            }
            else if(property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = (int)Mathf.Min(property.intValue, (attribute as MaxAttribute).max);

                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label, "Use MaxAttrubte for Float or Integer");
            }
        }
    }
}
