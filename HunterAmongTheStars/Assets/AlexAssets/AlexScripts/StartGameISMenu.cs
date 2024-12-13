using UnityEngine;
using UnityEngine.SceneManagement;


public class StartGameISMenu : MonoBehaviour
{

    public GameObject menuIS;

    public void StartIS()   
    {


        menuIS.SetActive(false);

        SceneManager.LoadScene("InterferenceSpace"); 


    }  


     

     





}
