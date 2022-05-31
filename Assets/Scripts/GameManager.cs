using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool testing = true;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private List<Color> playerColors = new List<Color>();

    private void Start()
    {
        Events.onLevelLoaded += SpawnAllPlayers;
    }

    public void SpawnAllPlayers(LevelSO so)
    {
        var devices = Gamepad.all;

        int spawnedPlayersIndex = 0;
        for (int i = 0; i < devices.Count; i++)
        {
            var playerinput = playerInputManager.JoinPlayer(i, -1, "Gamepad", devices[i]);
            playerinput.gameObject.transform.position = so.SpawnPoints[spawnedPlayersIndex];
            SetupColor(playerinput.gameObject, spawnedPlayersIndex);
            spawnedPlayersIndex++;
        }

        if (testing)
        {
            var keyboardMouse = Keyboard.current;
            var playerinput =  playerInputManager.JoinPlayer(spawnedPlayersIndex, -1, "", keyboardMouse);
            playerinput.gameObject.transform.position = so.SpawnPoints[spawnedPlayersIndex];
            SetupColor(playerinput.gameObject, spawnedPlayersIndex);
        }

    }

    public void SetupColor(GameObject go, int spawnedPlayersIndex)
    {
        var playercontroller = go.GetComponent<PlayerController>();
        
        playercontroller.color = playerColors[spawnedPlayersIndex];
        playercontroller.SetupColor(playercontroller.gameObject);
    }
}
