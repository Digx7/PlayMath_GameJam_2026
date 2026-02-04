using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ToolUIButtonHelper : MonoBehaviour {
    
    [Header("Incoming Channels")]
    public ToolChannel OnUseEmptyToolChannel;
    public DigDataChannel OnDigChannel;
    
    [Header("References")]
    public Image icon;
    public TextMeshProUGUI count;

    [Header("Events")]
    public ToolEvent TryChangeTool;
    public UnityEvent OnUseEmpty;
    public UnityEvent OnDecrease;

    private int m_currentCount;
    private Tool m_tool;

    #region Channels

    private void OnEnable() {
        OnUseEmptyToolChannel.channelEvent.AddListener(OnRecieve_OnUseEmptyToolChannel);
        OnDigChannel.channelEvent.AddListener(OnReceive_OnDigChannel);
    }

    private void OnDisable() {
        OnUseEmptyToolChannel.channelEvent.RemoveListener(OnRecieve_OnUseEmptyToolChannel);
        OnDigChannel.channelEvent.RemoveListener(OnReceive_OnDigChannel);
    }

    public void OnRecieve_OnUseEmptyToolChannel(Tool tool)
    {
        if(m_tool != null && m_tool == tool)
        {
            OnUseEmpty.Invoke();
        }
    }

    public void OnReceive_OnDigChannel(DigData digData)
    {
        if(m_tool != null && m_tool == digData.toolUsed && (digData.result == DigResult.FOUND_NEW_TREASURE || digData.result == DigResult.FOUND_NEW_EMPTY))
        {
            Decrease();
        }
    }

    #endregion

    #region Public Functions
    public void Setup(CountToolPair countToolPair)
    {
        m_tool = countToolPair.tool;
        m_currentCount = countToolPair.count;
        RefreshIcon();
        RefreshCount();
    }

    public void OnClick()
    {
        if(m_tool != null) TryChangeTool.Invoke(m_tool);
    }

    #endregion

    #region Private Functions

    private void Decrease()
    {
        m_currentCount--;
        RefreshCount();
    }

    private void RefreshIcon()
    {
        if(m_tool != null && m_tool.sprite != null) icon.sprite = m_tool.sprite;
    }

    private void RefreshCount()
    {
        count.text = m_currentCount.ToString();
    }

    #endregion
}