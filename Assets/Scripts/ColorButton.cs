using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public class ColorButton : MonoBehaviour
{
    [SerializeField] private Sprite unpressed;
    [SerializeField] private Sprite pressed;

    public UnityEvent OnPress = new UnityEvent();
    public UnityEvent OnRelease = new UnityEvent();

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        sr.sprite = pressed;
        OnPress.Invoke();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        sr.sprite = unpressed;
        OnRelease.Invoke();
    }
}
