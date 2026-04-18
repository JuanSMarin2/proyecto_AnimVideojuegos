using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CameraShakeController : MonoBehaviour
{

    public AnimationCurve curveLight; public AnimationCurve curveHeavy;



    public void Shake(float customDuration, bool isLight)
    {
        StopAllCoroutines();
        StartCoroutine(Shaking(customDuration, isLight));
    }

    IEnumerator Shaking(float customDuration, bool isLight)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < customDuration)
        {
            elapsedTime += Time.deltaTime;
            if (isLight)
            {
                float strength = curveLight.Evaluate(elapsedTime / customDuration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
            }
            else
            {
                float strength = curveHeavy.Evaluate(elapsedTime / customDuration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
            }
            yield return null;
        }

        transform.position = startPosition;
    }
}
