using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace LevelSystem
{
    public class Level : MonoBehaviour
    {
        public int current = 0;
        public bool IsTimerOn => current != 0;

        public List<int> levelIDs;
        public int currentLevelID = 0;

        public List<GameObject> Walls = new List<GameObject>();

        private Scene _currentLoadedScene;

        public bool IsGameRunning { get; private set; } = false;

        void Start()
        {
            StartCoroutine(StartGame()); // TODO: This should be moved to the UI
        }
        
        public IEnumerator StartGame()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(levelIDs[currentLevelID], LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            _currentLoadedScene = SceneManager.GetSceneAt(levelIDs[currentLevelID]);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
            IsGameRunning = true;

            StartCoroutine(Lose());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static IEnumerable<Object> GatherSuckables()
        {
            return Resources.FindObjectsOfTypeAll(typeof(Suck)).ToList();
        }
        
        
        public IEnumerator Lose()
        {
            foreach (var objs in Walls)
            {
                objs.transform.SetParent(null);
                objs.GetComponent<Suck>().enableForce = true;
            }
            
            yield return new WaitForSecondsRealtime(0.2f);
            
            var suckables = GatherSuckables();
            bool isDone = false;
            while (!isDone)
            {
                foreach (var objs in suckables)
                {
                    var rb = objs.GetComponent<Rigidbody>();

                    rb.velocity = Vector3.right * 25;
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.None;

                    objs.GetComponent<Suck>().enableForce = true;
                }

                isDone = true; // TODO: Move this
                yield return null;
            }

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