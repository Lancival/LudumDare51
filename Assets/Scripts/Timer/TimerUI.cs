using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private Timer timer;

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = Timer.instance;
    }

    void Update()
    {
        if (timer.active && timer.ttl <= 9.99f)
        {
            timerText.text = timer.ttl.ToString("F2");
            timerText.color = Color.Lerp(Color.red, Color.white, timer.ttl / 10f);
        }
        else
        {
            timerText.text = "10.0";
            timerText.color = Color.white;
        }
    }
}
