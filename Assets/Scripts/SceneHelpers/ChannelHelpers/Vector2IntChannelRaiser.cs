using UnityEngine;
using UnityEngine.Events;

public class Vector2IntChannelRaiser : MonoBehaviour
{
    [SerializeField] private Vector2IntChannel channelToRaise;
    [SerializeField] private Vector2Int m_data;
    public Vector2Int Data
    {
        get
        {
            return m_data;
        }
        set
        {
            m_data = value;
        }
    }

    public void Raise(Vector2Int data)
    {
        channelToRaise.Raise(data);
    }

    public void RaiseMData()
    {
        channelToRaise.Raise(m_data);
    }
}
