using UnityEngine;
using UnityEngine.Events;

public class StringChannelListener : MonoBehaviour 
{
    public StringChannel channelToListenTo;
    public string dataToListenFor;
    public bool shouldFilterData = true;

    public UnityEvent OnHearData;
    public StringEvent OnHearData_Details;

    private void OnEnable() {
        channelToListenTo.channelEvent.AddListener(OnRecieve_ChannelToListenTo);
    }

    private void OnDisable() {
        channelToListenTo.channelEvent.RemoveListener(OnRecieve_ChannelToListenTo);
    }

    public void OnRecieve_ChannelToListenTo(string data)
    {
        if(shouldFilterData && data == dataToListenFor) OnHearData.Invoke();
        else OnHearData_Details.Invoke(data);
    }
}