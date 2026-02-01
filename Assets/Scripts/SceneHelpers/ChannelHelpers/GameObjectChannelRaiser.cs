using UnityEngine;
using UnityEngine.Events;

public class GameObjectChannelRaiser : MonoBehaviour
{
    [SerializeField] private GameObjectChannel channelToRaise;
    [SerializeField] private GameObject m_data;

    public void Raise(GameObject m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
