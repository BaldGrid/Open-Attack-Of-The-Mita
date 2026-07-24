using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShakingManager : MonoBehaviour
{
    public bool startShakingOnStart = false;
    private void Start()
    {
        if(startShakingOnStart) StartShaking();
    }
    private Vector3 originalPosition;

    public void StartShaking()
    {
        originalPosition = transform.position;
        StartCoroutine(eStartShaking());
    }
    public void StopShaking()
    {
        StopCoroutine(eStartShaking());
        transform.position = originalPosition;
    }
    
    private IEnumerator eStartShaking()
    {
        while (true)
        {
            var nextPosition = originalPosition;
            Vector3 positionDelta;
            if(Mathf.Abs(transform.forward.y) == 0)
            {
                positionDelta = transform.forward;
            }
            else
            {
                positionDelta = transform.up;
            }
            nextPosition += positionDelta * 0.4f;
            var time = 0f;
            while (time < 1)
            {
                transform.position = Vector3.Lerp(originalPosition, nextPosition, time);
                time += Time.deltaTime * 64;
                yield return null;
            }
            yield return null;
            nextPosition = originalPosition - positionDelta * 0.4f;
            time = 0f;
            while (time < 1)
            {
                transform.position = Vector3.Lerp(originalPosition, nextPosition, time);
                time += Time.deltaTime * 64;
                yield return null;
            }
            yield return null;
        }
    }
}