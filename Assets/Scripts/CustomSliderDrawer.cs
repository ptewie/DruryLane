using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomSliderAttribute))]
public class CustomSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomSliderAttribute slider = attribute as CustomSliderAttribute;

        if (property.propertyType == SerializedPropertyType.Float)
        {
            EditorGUI.Slider(position, property, slider.min, slider.max, label);
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use CustomSlider with float.");
        }
    }
}

public class CustomSliderAttribute : PropertyAttribute
{
    public float min;
    public float max;

    public CustomSliderAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
