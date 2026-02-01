using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnInfoChannelRaiser : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInfoChannel channelToRaise;
    [SerializeField] private PlayerSpawnInfo m_data;

    public void Raise(PlayerSpawnInfo m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
