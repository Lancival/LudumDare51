using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Timer.instance.Stop();
            InputManager.instance.DisableMovement(3f);

            if (gameObject.name == "Secret Goal")
            {
                SceneLoader.instance.LoadScene("Bonus Level");
            }
            else
            {
                SceneLoader.instance.LoadScene();
            }
        }
    }
}
