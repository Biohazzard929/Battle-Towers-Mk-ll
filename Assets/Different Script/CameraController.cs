using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerViewTransform;
    public Transform battleViewTransform;
    public Transform cameraTransform;

    public void SwitchToPlayerView()
    {
        StartCoroutine(SwitchView(playerViewTransform));
    }

    public void SwitchToBattleView()
    {
        StartCoroutine(SwitchView(battleViewTransform));
    }

    private IEnumerator SwitchView(Transform targetView)
    {
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;
        Vector3 endPosition = targetView.position;
        Quaternion endRotation = targetView.rotation;

        float transitionTime = 1.0f; // Duration of the transition
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionTime);
            cameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = endPosition;
        cameraTransform.rotation = endRotation;
    }
}
