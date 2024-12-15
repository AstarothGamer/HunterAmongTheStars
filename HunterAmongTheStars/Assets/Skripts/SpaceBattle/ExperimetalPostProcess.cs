using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ExperimetalPostProcess : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField, Min(0)] private float minExposure = -2f; // Adjust to your range
    [SerializeField, Min(0)] private float maxExposure = 2f;  // Adjust to your range
    [SerializeField, Min(0)] private float minWeight = 0f;
    [SerializeField] private Volume volume;

    private ColorAdjustments colorAdjustments;
    private bool stop = false;
    public bool triggerFromStart = false;

    private void Start()
    {
        volume.weight = 0;

        // Try to get the Color Adjustments effect from the Volume
        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.overrideState = true;
        }
        else
        {
            Debug.LogWarning("Color Adjustments are not present in the Volume Profile.");
        }

        if (triggerFromStart)
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
        if (colorAdjustments == null)
        {
            Debug.LogWarning("No Color Adjustments found. Effect will not run.");
            yield break;
        }

        int direction = 1;

        while (!stop || (stop && colorAdjustments.postExposure.value > minExposure))
        {
            colorAdjustments.postExposure.value += direction * speed * Time.deltaTime;

            if (colorAdjustments.postExposure.value <= minExposure)
            {
                colorAdjustments.postExposure.value = minExposure;
                direction = 1;
            }
            else if (colorAdjustments.postExposure.value >= maxExposure)
            {
                colorAdjustments.postExposure.value = maxExposure;
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