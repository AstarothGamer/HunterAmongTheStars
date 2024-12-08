using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerPlanets; 
    [SerializeField] private RandomPlanetSpawner spawner; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < playerPlanets.Count; i++)
                {
                    if (hit.collider.gameObject == playerPlanets[i])
                    {
                        bool isCorrect = spawner.CheckPlanet(i);
                        if (isCorrect)
                        {
                            AudioManager.PlaySound(SoundType.CorrectAnswer, 0.7f);

                            Debug.Log("Correct!");
                        }
                        else
                        {
                            AudioManager.PlaySound(SoundType.WrongAnswer, 0.7f);

                            Debug.Log("Wrong!");
                        }

                        break;
                    }
                }
            }
        }
    }
}

