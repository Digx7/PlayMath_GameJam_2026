using UnityEngine;
using UnityEngine.Events;

public class Vector2IntChannelRaiser : MonoBehaviour
{
    [SerializeField] private Vector2IntChannel channelToRaise;
    [SerializeField] private Vector2Int m_data;

    public void Raise(Vector2Int data)
    {
        channelToRaise.Raise(data);
    }

    public void RaiseMData()
    {
        channelToRaise.Raise(m_data);
    }
}
