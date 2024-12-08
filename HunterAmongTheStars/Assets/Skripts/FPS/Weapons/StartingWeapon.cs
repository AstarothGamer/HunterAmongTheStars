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
        WeaponMenu.SetActive(false);
        pistol.SetActive(true);
        shotgun.SetActive(false);
        rifle.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Shotgun()
    {
        WeaponMenu.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(true);
        rifle.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Rifle()
    {
        WeaponMenu.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        rifle.SetActive(true);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
}
