using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnTimerEnd : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Timer.instance.OnTimerEnd += PlaySound;
    }

    private void PlaySound() => audioSource.Play();
}
