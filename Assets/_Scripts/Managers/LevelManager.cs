using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEditor;

namespace Game.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Transform catParent;
        [SerializeField] private List<LevelSO> levels;

        [Space]
        [Header("Number level auto generate")]
        [SerializeField] private int numberLevelAutoGenerate;

        [Space]
        [Header("Support test")]
        [SerializeField] private int levelTestIndex;

        private int currentLevel;
        public int CurrentLevel => this.currentLevel;

        private List<GameObject> listCat = new List<GameObject>();
        private GameObject player;

        private const float WIDTH_ROAD = 100f;

        public void SetupLevel()
        {
            LevelSO levelSO = levels[currentLevel];
            Setup(levelSO);
        }

        public void NextLevel()
        {
            currentLevel++;
        }

        public void SetLevel(int value)
        {
            if (value < 0 || value >= levels.Count)
                return;

            currentLevel = value;
        }

        private void Setup(LevelSO levelSO)
        {
            if (levelSO.CatPrefabs.Count <= 0)
                return;

            if (levelSO.PlayerPrefab == null)
                return;

            //Handle Weight
            Debug.Log($"Weight: {levelSO.Difficulty}");
            int difficulty = levelSO.Difficulty;

            //Handle cats
            for (int i = 0; i < difficulty; i++)
            {
                for (int index = 0; index < levelSO.CatPositions.Count; index++)
                {
                    var radius = levelSO.SpawnRadius;
                    var minX = levelSO.CatPositions[index].x - radius;
                    var maxX = levelSO.CatPositions[index].x + radius;
                    var minY = levelSO.CatPositions[index].y - radius;
                    var maxY = levelSO.CatPositions[index].y + radius;
                    var x = Random.Range(minX, maxX);
                    var y = Random.Range(minY, maxY);

                    var position = new Vector3(x, 0, y);
                    Debug.Log($"[CatPosition]: {index} | {position}");

                    var indexCat = Random.Range(0, levelSO.CatPrefabs.Count);
                    GameObject cat = Instantiate(levelSO.CatPrefabs[indexCat], position, Quaternion.identity, catParent);
                    listCat.Add(cat);
                }
            }
            
            //Handle Player
            Debug.Log($"PlayerPosition: {levelSO.PlayerPosition}");
            var playerPosition = new Vector3(levelSO.PlayerPosition.x, 0, levelSO.PlayerPosition.y);
            player = Instantiate(levelSO.PlayerPrefab, playerPosition, Quaternion.identity);

            //Handle Obstacles

        }

        [ContextMenu("ShowLevelOnScene")]
        private void ShowLevelOnScene()
        {
            LevelSO levelSO = levels[levelTestIndex];
            Setup(levelSO);
        }

        [ContextMenu("Clear level on scene")]
        private void ClearLevelOnScene()
        {
            foreach (var cat in listCat)
            {
                DestroyImmediate(cat);
            }

            DestroyImmediate(player);

            Debug.Log("Clear !!!");
        }

#if UNITY_EDITOR
        [ContextMenu("Generate Levels")]
        private void Generatelevel()
        {
            LevelSO data = ScriptableObject.CreateInstance<LevelSO>();
            string path = "Assets/_Scripts/LevelSO/AutoGenLevel/NewData.asset";
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}

