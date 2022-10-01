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

    void Awake() => _instance = this;

    void Start() => currentPlayer.Activate();

    void OnMove(InputValue value) => currentPlayer.OnMove(value);
    void OnJump() => currentPlayer.OnJump();
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
}
