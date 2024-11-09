using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamShake : Singleton<CamShake>
{
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;
    public void Shake(float shakeDuration, float frequency)
    {
        StartCoroutine(Shaky(shakeDuration, frequency));
    }
    private IEnumerator Shaky(float shakeDuration, float frequency)
    {
        noise.AmplitudeGain = frequency;

        // Wait for the flash duration
        yield return new WaitForSeconds(shakeDuration);

        noise.AmplitudeGain = 0f;
    }
}
