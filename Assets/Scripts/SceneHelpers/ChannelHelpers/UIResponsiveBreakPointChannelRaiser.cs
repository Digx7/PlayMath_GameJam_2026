using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class UIResponsiveBreakPointChannelRaiser : MonoBehaviour
    {
        [SerializeField] private UIResponsiveBreakPointChannel channelToRaise;
        [SerializeField] private UIResponsiveBreakPoint _data;

        public void Raise(UIResponsiveBreakPoint data)
        {
            channelToRaise.Raise(data);
        }

        public void Raise()
        {
            channelToRaise.Raise(_data);
        }
    }
}
