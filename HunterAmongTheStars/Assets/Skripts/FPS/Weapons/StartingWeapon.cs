using UnityEngine;

public class StartingWeapon : MonoBehaviour
{
    [SerializeField] GameObject WeaponMenu;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject shotgun;
    [SerializeField] GameObject rifle;
    void Start()
    {
        // freeze time
        WeaponMenu.SetActive(true);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        //rifle.SetActive(false);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Pistol()
    {
        AudioManager.PlayMusic(SoundType.ShooterMusic, 0.8f);
        WeaponMenu.SetActive(false);
        pistol.SetActive(true);
        shotgun.SetActive(false);
        rifle.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Shotgun()
    {
        AudioManager.PlayMusic(SoundType.ShooterMusic, 0.8f);
        WeaponMenu.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(true);
        rifle.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Rifle()
    {
        AudioManager.PlayMusic(SoundType.ShooterMusic, 0.8f);
        WeaponMenu.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        rifle.SetActive(true);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
}
