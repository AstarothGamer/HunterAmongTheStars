using UnityEngine;

public class UISounds : MonoBehaviour
{
    public void ButtonSound()
    {
        AudioManager.PlaySound(SoundType.Button, 0.7f);
    }
    public void ButtonSound2()
    {
        AudioManager.PlaySound(SoundType.Button2, 0.7f);
    }
    public void SwitchSound()
    {
        AudioManager.PlaySound(SoundType.Switch, 0.7f);
    }
}
