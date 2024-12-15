using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunkEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField, Min(0)] private float minIntensity = 0f;
    [SerializeField, Min(0)] private float maxIntensity = 1f;
    [SerializeField, Min(0)] private float minWeight = 0f;
    [SerializeField] private Volume volume;

    private ChromaticAberration chromaticAberration;
    private bool stop = false;
    public bool trigerFromStart = false;

    private void Start()
    {
        volume.weight = 0;

        // Try to get the effect from the Volume
        if (volume.profile.TryGet(out chromaticAberration))
        {
            chromaticAberration.intensity.overrideState = true;
        }
        else
        {
            Debug.LogWarning("Chromatic Aberration is not present in the Volume Profile.");
        }

        if (trigerFromStart)
        TriggerDizzyness();
    }

    public void StopDizzyness()
    {
        stop = true;
        StartCoroutine(Normal());
    }

    public void TriggerDizzyness()
    {
        stop = false;
        volume.weight = 1;
        StartCoroutine(DizzynessCycle());
    }

    private IEnumerator DizzynessCycle()
    {
        if (chromaticAberration == null)
        {
            Debug.LogWarning("No Chromatic Aberration found. Effect will not run.");
            yield break;
        }

        int direction = 1;

        while (!stop || (stop && chromaticAberration.intensity.value > minIntensity))
        {
            chromaticAberration.intensity.value += direction * speed * Time.deltaTime;

            if (chromaticAberration.intensity.value <= minIntensity)
            {
                chromaticAberration.intensity.value = minIntensity;
                direction = 1;
            }
            else if (chromaticAberration.intensity.value >= maxIntensity)
            {
                chromaticAberration.intensity.value = maxIntensity;
                direction = -1;
            }

            yield return null;
        }
    }
    private IEnumerator Normal()
    {
        if (volume == null)
        yield break;

        while (volume.weight > minWeight)
        {
            volume.weight -= 0.1f * Time.deltaTime;
            volume.weight = Mathf.Max(volume.weight, minWeight);
            yield return null;
        }
    }
}