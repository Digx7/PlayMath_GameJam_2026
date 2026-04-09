using UnityEngine;

namespace DevonLively.Attributes
{
    public class HideIfFalseAttribute : PropertyAttribute
    {
        public string ConditionalSourceField { get; private set; }

        public HideIfFalseAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
        }
    }
}