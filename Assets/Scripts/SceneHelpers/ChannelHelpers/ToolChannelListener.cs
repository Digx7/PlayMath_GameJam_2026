using UnityEngine;
using UnityEngine.Events;

public class ToolChannelListener : MonoBehaviour 
{
    public ToolChannel channelToListenTo;
    public Tool dataToListenFor;
    public bool onlyHearDataToListenFor = true;

    public ToolEvent OnHear;

    private void OnEnable() {
        channelToListenTo.channelEvent.AddListener(OnRecieve_ChannelToListenTo);
    }

    private void OnDisable() {
        channelToListenTo.channelEvent.RemoveListener(OnRecieve_ChannelToListenTo);
    }

    public void OnRecieve_ChannelToListenTo(Tool data)
    {
        if(onlyHearDataToListenFor && data == dataToListenFor) OnHear.Invoke(data);
        else OnHear.Invoke(data);
    }
}