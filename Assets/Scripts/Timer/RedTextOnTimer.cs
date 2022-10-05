using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RedTextOnTimer : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        Timer.instance.OnTimerEnd += ResetColor;
        Timer.instance.OnTimerStart += RedColor;
    }

    private void ResetColor() => text.color = Color.black;
    private void RedColor() => text.color = Color.red;
}
