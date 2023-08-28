using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerAction PlayerAction;
    public static event Action<InputActionMap> ActionMapChange;

    private void Awake()
    {
        PlayerAction = new PlayerAction();
        ToggleActionMap(PlayerAction.Gameplay);
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;
        PlayerAction.Disable();
        ActionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
}
