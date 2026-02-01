using UnityEngine;
using UnityEngine.Events;

public class SFXChannelRaiser : MonoBehaviour
{
    [SerializeField] private SFXChannel channelToRaise;
    [SerializeField] private string m_SfxName;
    [SerializeField] private Vector3 m_SfxLocation;

    public void Raise(string m_SfxName, Vector3 m_SfxLocation)
    {
        channelToRaise.Raise(m_SfxName, m_SfxLocation);
    }
}
