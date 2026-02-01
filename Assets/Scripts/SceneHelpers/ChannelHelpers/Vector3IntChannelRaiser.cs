using UnityEngine;
using UnityEngine.Events;

public class Vector3IntChannelRaiser : MonoBehaviour
{
    [SerializeField] private Vector3IntChannel channelToRaise;
    [SerializeField] private Vector3Int m_data;

    public void Raise(Vector3Int m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
