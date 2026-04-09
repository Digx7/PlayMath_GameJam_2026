using UnityEngine;

namespace DevonLively.Attributes
{
    public class HideIfTrueAttribute : PropertyAttribute
    {
        public string ConditionalSourceField { get; private set; }

        public HideIfTrueAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
        }
    }
}