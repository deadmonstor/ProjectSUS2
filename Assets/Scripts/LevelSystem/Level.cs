using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    public class Level : MonoBehaviour
    {
        public int current = 0;
        public bool IsTimerOn => current != 0;

        public List<int> LevelIDs;
        public int CurrentLevelID = 0;

        private Scene currentLoadedScene;

        public bool IsGameRunning { get; private set; } = false;

        void Start()
        {
            StartCoroutine(StartGame()); // TODO: This should be moved to the UI
        }
        
        public IEnumerator StartGame()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(LevelIDs[CurrentLevelID], LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            currentLoadedScene = SceneManager.GetSceneAt(LevelIDs[CurrentLevelID]);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
            IsGameRunning = true;
        }
        
        public void Lose()
        {
            // TODO: Break wall
            
            // TODO: Apply velocity
            
            // TODO: End the game
            IsGameRunning = false;
        }

        public void Win()
        {
            // TODO: Stop the game
            IsGameRunning = false;
            
            // TODO: Save the high-score
            
            // TODO: Wait and reset the level
        }
    }
}