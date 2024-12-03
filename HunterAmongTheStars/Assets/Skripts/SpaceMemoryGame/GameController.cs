using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;

    public GameObject[] puzzles;
    private GameObject[] instantiatedObjects;
    public List<GameObject> gamePuzzles = new List<GameObject>();
    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    public int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;


    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        instantiatedObjects = new GameObject[btns.Count];
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for(int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i <looper; i ++)
        {
            if(index == looper / 2)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);

            index++;
        }
    }
    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }
   
    public void PickPuzzle()
    {
        if(!firstGuess)
        {
            firstGuess = true;

            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            ReplaceButtonWith3DObject(firstGuessIndex);
        }
        else if(!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            ReplaceButtonWith3DObject(secondGuessIndex);

            countGuesses++;

            StartCoroutine(CheckIfThePuzzlesMatch());

            if(firstGuessPuzzle == secondGuessPuzzle)
            {
                Debug.Log("the puzzles match");
            }

            else
            {
                Debug.Log("The puzzles do not match");
            }
        }
    }

    void ReplaceButtonWith3DObject(int buttonIndex)
    {
        MakeButtonInvisible(btns[buttonIndex]);

        Vector3 position = btns[buttonIndex].transform.position;

        instantiatedObjects[buttonIndex] = Instantiate(gamePuzzles[buttonIndex], position, Quaternion.identity);
    }

     public void Destroy3DObject(int buttonIndex)
    {
        if (instantiatedObjects[buttonIndex] != null)
        {
            Destroy(instantiatedObjects[buttonIndex]);
            instantiatedObjects[buttonIndex] = null; 
        }
    }

    private void MakeButtonInvisible(Button button)
    {
        if (button.image != null)
        {
            Color buttonColor = button.image.color;
            buttonColor.a = 0; 
            button.image.color = buttonColor;
        }
    }

    private void MakeButtonVisible(Button button)
    {
        if (button.image != null)
        {
            Color buttonColor = button.image.color;
            buttonColor.a = 255; 
            button.image.color = buttonColor;
        }
    }

    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);

        if(firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            MakeButtonInvisible(btns[firstGuessIndex]);
            MakeButtonInvisible(btns[secondGuessIndex]);

            Destroy3DObject(firstGuessIndex);
            Destroy3DObject(secondGuessIndex);

            CheckIfTheGameIfFinished();
        }

        else
        {
            yield return new WaitForSeconds(.5f);

            MakeButtonVisible(btns[firstGuessIndex]);
            MakeButtonVisible(btns[secondGuessIndex]);

            Destroy3DObject(firstGuessIndex);
            Destroy3DObject(secondGuessIndex);
        }

        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;
    }

    void CheckIfTheGameIfFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game finished");
            Debug.Log("It took you " + countGuesses + " many guesses to finish the game");
        }
    }

    void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
