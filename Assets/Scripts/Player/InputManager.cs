using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance => _instance;
    private static InputManager _instance;

    public List<PlayerMovement> players = new List<PlayerMovement>();
    private int currentPlayerIndex = 0;
    private PlayerMovement currentPlayer => players[currentPlayerIndex];

    private bool movementDisabled = false;

    void Awake() => _instance = this;

    void Start() => currentPlayer.Activate();

    void OnMove(InputValue value)
    {
        if (!movementDisabled)
        {
            currentPlayer.OnMove(value);
        }
    }
    void OnJump()
    {
        if (!movementDisabled)
        {
            currentPlayer.OnJump();
        }
    }

    void OnNext()
    {
        if (!Timer.instance.active)
        {
            currentPlayer.Deactivate();
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
            {
                currentPlayerIndex = 0;
            }
            currentPlayer.Activate();
        }
    }
    void OnPrevious()
    {
        if (!Timer.instance.active)
        {
            currentPlayer.Deactivate();
            currentPlayerIndex--;
            if (currentPlayerIndex < 0)
            {
                currentPlayerIndex = players.Count - 1;
            }
            currentPlayer.Activate();
        }
    }

    public void DisableMovement(float duration)
    {
        if (!movementDisabled)
        {
            movementDisabled = true;
            StartCoroutine(EnableMovement(duration));
        }
    }

    private IEnumerator EnableMovement(float duration)
    {
        yield return new WaitForSeconds(duration);
        movementDisabled = false;
    }
}
