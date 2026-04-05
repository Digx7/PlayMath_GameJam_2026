using UnityEngine;
using UnityEngine.Events;

public class IsInEditorHelper : MonoBehaviour {
    
    public BooleanEvent isInEditor;
    public UnityEvent onInEditor;
    public UnityEvent onNotInEditor;

    private void Awake() 
    {
#if UNITY_EDITOR
        isInEditor.Invoke(true);
        onInEditor.Invoke();
#else
        isInEditor.Invoke(false);
        onNotInEditor.Invoke();
#endif
    }

}