using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CameraShakeController : MonoBehaviour
{

    public AnimationCurve curveLight; public AnimationCurve curveHeavy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
