using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerPlanets; 
    [SerializeField] private TMP_Text outcomeText;
    [SerializeField] private RandomPlanetSpawner spawner; 

    public int wrongAnswerCount = 0;

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
                            wrongAnswerCount += 1;

                            if(wrongAnswerCount > 2)
                            {                               
                                spawner.isGameActive = false;

                                outcomeText.text = "You pressed wrong 3 times. You lost.";
                            }
                            Debug.Log("Wrong!");
                        }

                        break;
                    }
                }
            }
        }
    }
}

