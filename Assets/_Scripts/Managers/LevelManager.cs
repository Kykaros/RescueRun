using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

namespace Game.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Transform catParent;
        [SerializeField] private List<LevelSO> levels;

        [Space]
        [Header("Support test")]
        [SerializeField] private int levelIndex;

        private List<GameObject> listCat = new List<GameObject>();
        private GameObject player;

        public void SetupLevel(int index)
        {

        }

        public void SetupCurrentLevel()
        {

        }

        [ContextMenu("ShowLevelOnScene")]
        private void ShowLevelOnScene()
        {
            LevelSO levelSO = levels[levelIndex];

            for (int i = 0; i < levelSO.CatPositions.Count; i++)
            {
                var radius = levelSO.SpawnRadius;
                var minX = levelSO.CatPositions[i].x - radius;
                var maxX = levelSO.CatPositions[i].x + radius;
                var minY = levelSO.CatPositions[i].y - radius;
                var maxY = levelSO.CatPositions[i].y + radius;
                var x = Random.Range(minX, maxX);
                var y = Random.Range(minY, maxY);

                var position = new Vector3(x, 0, y);
                Debug.Log($"[CatPosition]: {i} | {position}");

                GameObject cat = Instantiate(levelSO.CatPrefabs[0], position, Quaternion.identity,catParent);
                listCat.Add(cat);
            }

            Debug.Log($"Weight: {levelSO.Weight}");
            Debug.Log($"SpawnRadius: {levelSO.SpawnRadius}");

            Debug.Log($"PlayerPosition: {levelSO.PlayerPosition}");
            var playerPosition = new Vector3(levelSO.PlayerPosition.x, 0, levelSO.PlayerPosition.y);
            player = Instantiate(levelSO.PlayerPrefab, playerPosition, Quaternion.identity);
            
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
    }
}

