using UnityEngine;
using UnityEngine.Events;

public class StringChannelListener : MonoBehaviour 
{
    public StringChannel channelToListenTo;
    public string dataToListenFor;

    public UnityEvent OnHearData;

    private void OnEnable() {
        channelToListenTo.channelEvent.AddListener(OnRecieve_ChannelToListenTo);
    }

    private void OnDisable() {
        channelToListenTo.channelEvent.RemoveListener(OnRecieve_ChannelToListenTo);
    }

    public void OnRecieve_ChannelToListenTo(string data)
    {
        if(data == dataToListenFor) OnHearData.Invoke();
    }
}