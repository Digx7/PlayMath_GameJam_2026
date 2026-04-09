using System;
using UnityEngine;

namespace DevonLively.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonLabel { get; private set; }
        public string GroupLabel { get; private set; }
        public float Spacing { get; private set; } 
        public string HideIfField { get; private set; }
        public string ShowIfField { get; private set; }
        public string ConditionalValue { get; private set; }

        public ButtonAttribute() : this(null, null, 0f, null, null, "") { }

        public ButtonAttribute(string buttonLabel = null, string groupLabel = null, float spacing = 0f, string hideIfField = null, string showIfField = null, string conditionalValue = "")
        {
            ButtonLabel = buttonLabel;
            GroupLabel = groupLabel;
            Spacing = spacing;
            HideIfField = hideIfField;
            ShowIfField = showIfField;
            ConditionalValue = conditionalValue;
        }
    }
}