using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool testing = true;
    [SerializeField] private PlayerInputManager playerInputManager;

    private void Start()
    {
        SpawnAllPlayers();
    }

    public void SpawnAllPlayers()
    {
        var devices = Gamepad.all;

        int spawnedPlayersIndex = 0;
        for (int i = 0; i < devices.Count; i++)
        {
            playerInputManager.JoinPlayer(i, -1, "Gamepad", devices[i]);
            spawnedPlayersIndex++;
        }

        if (testing)
        {
            var keyboardMouse = Keyboard.current;
            playerInputManager.JoinPlayer(spawnedPlayersIndex, -1, "", keyboardMouse);
        }

    }
}
