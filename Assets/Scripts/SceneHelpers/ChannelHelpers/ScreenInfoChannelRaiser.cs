using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class ScreenInfoChannelRaiser : MonoBehaviour
    {
        [SerializeField] private ScreenInfoChannel channelToRaise;
        [SerializeField] private ScreenInfo _data;

        public void Raise(ScreenInfo data)
        {
            channelToRaise.Raise(data);
        }

        public void Raise()
        {
            channelToRaise.Raise(_data);
        }
    }
}
