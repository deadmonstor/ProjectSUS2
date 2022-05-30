using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

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
        public float GameTime { get; private set; } = 0;
        public float GameTimeBeforeLose = 120f;

        public TextMeshProUGUI TimeText;
        
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

            GameTime = 0;

            Events.OnLevelLoaded(levelIDs[currentLevelID]);
            
            yield return new WaitForSecondsRealtime(GameTimeBeforeLose);
            StartCoroutine(Lose());
        }

        private void Update()
        {
            GameTime += Time.deltaTime;
            TimeText.SetText("Time: " + (int)GameTime);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static IEnumerable<Suck> GatherSuckables()
        {
            return GameObject.FindObjectsOfType<Suck>().ToList();
        }
        
        
        public IEnumerator Lose()
        {
            foreach (var objs in Walls)
            {
                var rb = objs.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                
                objs.transform.SetParent(null);
                objs.GetComponent<Suck>().enableForce = true;
            }
             
            yield return new WaitForSecondsRealtime(0.2f);
            
            var suckables = GatherSuckables();
            int i = 0;
            while (i != 10)
            {
                foreach (Suck objs in suckables)
                {
                    var rb = objs.GetComponent<Rigidbody>();

                    objs.transform.SetParent(null);
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.None;

                    objs.GetComponent<Suck>().enableForce = true;
                }

                i++;
            }

            IsGameRunning = false;
        }

        public void Win()
        {
            IsGameRunning = false;
            
            // TODO: Save the high-score
            
            // TODO: Wait and reset the level
        }
    }
}