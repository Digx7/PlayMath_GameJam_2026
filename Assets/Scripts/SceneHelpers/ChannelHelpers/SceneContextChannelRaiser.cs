using UnityEngine;
using UnityEngine.Events;

public class SceneContextChannelRaiser : MonoBehaviour
{
    [SerializeField] private SceneContextChannel channelToRaise;
    [SerializeField] private SceneContext m_data;

    public void Raise(SceneContext m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
