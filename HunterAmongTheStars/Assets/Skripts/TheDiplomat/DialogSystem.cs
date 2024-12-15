using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text alienQuestion;

    [SerializeField] private List<QuestionsAndAnswers> questions = new List<QuestionsAndAnswers>();
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject lostGamePanel;
    [SerializeField] private GameObject wonGamePanel;

    private List<string> answers;
    public Button[] answerButtons;

    private QuestionsAndAnswers currentQuestion; 
    public Door door;
    public bool isGameActive;
    public bool isGameOver = false;

    public float timer;

    [SerializeField] private int gameScore;
    private int questionAsked;

    void Awake()
    {
        lostGamePanel.SetActive(false);
        wonGamePanel.SetActive(false);

        questions.Add(new QuestionsAndAnswers()
        {
            question = "Do I use an alarm clock for work?",
            correctAnswer = "Yes",
            wrongAnswer = "No",
            // neutralAnswer = "120 minutes"
        });

        questions.Add(new QuestionsAndAnswers()
        {
            question = "How many players are on the field for one team during the soccer match?",
            correctAnswer = "11 players",
            wrongAnswer = "13 players",
            neutralAnswer = "9 players"
        });

        questions.Add(new QuestionsAndAnswers()
        {
            question = "What is the name of the area where a penalty kick is taken?",
            correctAnswer = "Penalty box",
            wrongAnswer = "Center circle",
            neutralAnswer = "Corner"
        });

        questions.Add(new QuestionsAndAnswers()
        {
            question = "What is my favorite drink when working at the station?",
            correctAnswer = "Hot chocolate",
            wrongAnswer = "Beer",
            neutralAnswer = "Soda water"
        });

        questions.Add(new QuestionsAndAnswers()
        {
            question = "What is my go-to food to eat while on break?",
            correctAnswer = "Soup",
            wrongAnswer = "Steak and fries",
            neutralAnswer = "Nothing"
        });

        questions.Add(new QuestionsAndAnswers()
        {
            question = "Do I like to smoke cigarettes?",
            correctAnswer = "Yes",
            wrongAnswer = "No",
            neutralAnswer = "Sometimes"
        });
    }
    void Start()
    {
        AudioManager.PlayLoopSound(SoundType.BackgroundMusic2, 0.2f);
    }

    void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("0.00");

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                Debug.Log("1");
                SelectAnswer(0); 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                Debug.Log("2");
                SelectAnswer(1); 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SelectAnswer(2); 
                Debug.Log("3");
            }
        }
    }

    public void SetRandomQuestion()
    {
        timer = 30;
        if(questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];

            questions.RemoveAt(randomIndex);

            DisplayQuestion();
        }
        else
        {
            Debug.Log("All the questions are asked. End of the game!");
        }
    }

    void DisplayQuestion()
    {
        alienQuestion.text = currentQuestion.question;

        answers = new List<string> {
            currentQuestion.correctAnswer,
            currentQuestion.wrongAnswer,
            currentQuestion.neutralAnswer
        };

        Shuffle(answers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            int index = i;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answers[index]));
        }
    }

    void CheckAnswer(string selectedAnswer)
    {
        if(selectedAnswer == currentQuestion.correctAnswer)
        {
            gameScore += 2;
            Debug.Log("Correct");
        }
        else if(selectedAnswer == currentQuestion.wrongAnswer)
        {
            gameScore -= 2;
            Debug.Log("Wrong!");
        }
        else
        {
            gameScore += 1;
            Debug.Log("neutral");
        }

        QuestionAsked();       
    }

    void Shuffle(List<string> list)
    {
        for ( int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void WonGamePanel()
    {
        dialoguePanel.SetActive(false);
        wonGamePanel.SetActive(true);
    }

    void LostGamePanel()
    {
        dialoguePanel.SetActive(false);
        lostGamePanel.SetActive(true);
    }
    void QuestionAsked()
    {
        questionAsked += 1;

        if(questionAsked == 3)
        {
            if(gameScore >= 3)
            {
                WonGamePanel();
            }
            else
            {
                LostGamePanel();
            }

            door.isDialogActive = false;
            isGameOver = true;
            StartCoroutine(timerOver());

        }

        else if(questionAsked < 3)
        {
            SetRandomQuestion();
        }
    }

    void SelectAnswer(int answerIndex)
    {
        if (answerIndex >= 0 && answerIndex < answers.Count)
        {
            string selectedAnswer = answers[answerIndex];
            CheckAnswer(selectedAnswer);
        }
        else
        {
            Debug.LogWarning("Invalid answer index: " + answerIndex);
        }
    }

    private IEnumerator timerOver()
    {
        yield return new WaitForSeconds(5f);
        lostGamePanel.SetActive(false);
        wonGamePanel.SetActive(false);
    }
}
