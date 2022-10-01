using UnityEngine;

public class MovingTilemap : MonoBehaviour
{
    private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private float speed = 5f;
    private bool displacing = false;

    void Awake()
    {
        Timer.instance.OnTimerEnd += Reset;
    }

    void FixedUpdate()
    {
        Vector2 targetPosition = displacing ? endPosition : startPosition;
        transform.position += (Vector3) (targetPosition - (Vector2) transform.position) * speed * Time.fixedDeltaTime;
    }

    public void Activate() => displacing = true;
    public void Deactivate() => displacing = false;

    private void Reset()
    {
        displacing = false;
        transform.position = startPosition;
    }
}
