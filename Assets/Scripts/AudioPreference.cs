using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Toggle))]
public class AudioPreference : MonoBehaviour
{
    private static bool _soundEnabled = true;
    public static bool soundEnabled => _soundEnabled;

    private Toggle toggle;
    [SerializeField] private AudioMixer audioMixer;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        UpdateToggle();
        toggle.onValueChanged.AddListener(ToggleAudio);
    }

    public void UpdateToggle() => toggle.isOn = _soundEnabled;

    public void ToggleAudio(bool value)
    {
        if (_soundEnabled != value)
        {
            _soundEnabled = value;
            audioMixer.SetFloat("Volume", _soundEnabled ? 0f : -80f);
        }
    }
}
