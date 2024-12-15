using UnityEngine;
using UnityEngine.UI;

public class TimCalculate : MonoBehaviour
{
    public Text timerText;

    private float timeRemaining = 120f;

    private bool timerRunning = true;

    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                UpdateTimerUI();

            }
            else
            {
                timeRemaining = 0;

                timerRunning = false;

            }
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);

        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
