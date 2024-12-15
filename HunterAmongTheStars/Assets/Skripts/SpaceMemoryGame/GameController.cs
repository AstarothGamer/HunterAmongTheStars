using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject restartMenuButton;
    [SerializeField] private GameObject startPlanetMemoryMenuButton;

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

    public float timer = 0;
    public bool isGameActive = false;


    void Start()
    {
        AudioManager.PlayMusic(SoundType.BackgroundMusic, 0.2f);

        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        instantiatedObjects = new GameObject[btns.Count];        
    }

    void Update()
    {
        if(isGameActive)
        {
            timer += Time.deltaTime; 
            timerText.text = "Timer: " + timer.ToString("0.00");
        }

        GameLost();
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
                AudioManager.PlaySound(SoundType.CorrectAnswer, 0.7f);
                Debug.Log("the puzzles match");
            }

            else
            {
                AudioManager.PlaySound(SoundType.WrongAnswer, 0.7f);
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

        if(!isGameActive)
        {
            yield return new WaitForSeconds(5);

            SceneManager.LoadScene("MemoryGame");
        }

        // yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;
    }

    void CheckIfTheGameIfFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            isGameActive = false;

            // startMenuPanel.SetActive(true);
            // startPlanetMemoryMenuButton.SetActive(false);
            // restartMenuButton.SetActive(true);
            
            timerText.text = "You won Memory Planet Game!";
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

    public void StartGamePlanetMemory()
    {
        startMenuPanel.SetActive(false);
        isGameActive = true;
    }

    public void StartGameSolarMatch()
    {
        SceneManager.LoadScene("SolarMatch");
    }

    public void StartGameSpaceMinesweeper()
    {
        SceneManager.LoadScene("Minesweeper");
    }

    public void GameLost()
    {
        if(timer > 60)
        {
            isGameActive = false;
            timerText.text = "You lost Memory Planet Game!";
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Bar");
    }
}
