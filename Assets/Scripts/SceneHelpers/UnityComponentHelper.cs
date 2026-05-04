using UnityEngine;
using System.Collections;

public class UnityComponentHelper : MonoBehaviour 
{

    public Behaviour unityComponent;

    public void DisableComponetForSeconds(float seconds)
    {
        StartCoroutine(DisableComponentCoroutine(seconds));
    }

    private IEnumerator DisableComponentCoroutine(float seconds)
    {
        unityComponent.enabled = false;
        yield return new WaitForSeconds(seconds);
        unityComponent.enabled = true;
        yield return new WaitForSeconds(seconds);
        unityComponent.enabled = false;
        yield return new WaitForSeconds(seconds);
        unityComponent.enabled = true;
    }

}