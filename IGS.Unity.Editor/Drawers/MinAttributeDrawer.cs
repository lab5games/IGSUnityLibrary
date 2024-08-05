using UnityEngine;
using UnityEditor;

namespace IGS.Unity.Editor
{
    [CustomPropertyDrawer(typeof(MinAttribute))]
    internal class MinAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Max(property.floatValue, (attribute as MinAttribute).min);

                EditorGUI.PropertyField(position, property, label);
            }
            else if(property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = (int)Mathf.Max(property.intValue, (attribute as MinAttribute).min);

                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label, "Use MinAttrubte for Float or Integer");
            }
        }
    }
}
