using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance => _instance;
    private static Timer _instance;
    
    private float startTime;

    private float ttl => _ttl;
    private float _ttl = 10.0f;

    public bool active => _active;
    private bool _active = false;

    public event System.Action OnTimerEnd;

    void Awake() => _instance = this;

    public void StartTime()
    {
        if (!_active)
        {
            startTime = Time.time;
            _active = true;
            _ttl = 10.0f;
        }
    }

    void FixedUpdate()
    {
        _ttl = 10.0f - (Time.fixedTime - startTime);
        if (_ttl <= 0f)
        {
            _active = false;
            OnTimerEnd?.Invoke();
        }
    }
}
