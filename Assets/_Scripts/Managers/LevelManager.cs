using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEditor;
using System.IO;

namespace Game.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Transform catParent;
        [SerializeField] private Transform obstacleParent;
        [SerializeField] private Transform waveParent;
        [SerializeField] private List<LevelSO> levels;

        [Space]
        private const string FILE_NAME = "LevelSO_";
        [Header("Auto generate levels")]
        [SerializeField] private string pathFolderLevel;
        [SerializeField] private int numberLevelAutoGenerate;
        [SerializeField] private FixedPositionCatsSO fixedCatsPositionSO;
        [SerializeField] private FixedObstaclesPositionSO fixedObstaclesPositionSO;
        [SerializeField] private List<GameObject> catPrefabs;
        [SerializeField] private List<GameObject> obstaclePrefabs;
        [SerializeField] private List<GameObject> wavePrefabs;
        [SerializeField] private GameObject playerPrefab;

        [Space]
        [Header("Support test")]
        [SerializeField] private int levelTestIndex;

        private int currentLevel = 0;
        public int CurrentLevel => this.currentLevel;

        private List<GameObject> listCat = new List<GameObject>();
        private List<GameObject> listObstacle = new List<GameObject>();
        private GameObject player;
        private GameObject wave;

        private WaveBehavior waveBehavior;

        private const float WIDTH_ROAD = 100f;

        private const float DIFFICULTY_RATE_EACH_LEVEL = 0.5f;//TODO: Maybe define in scripttableObject for edit 

        private void Start()
        {
            waveBehavior = FindAnyObjectByType<WaveBehavior>();
        }

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

            //Handle wave
            float waveVelocity = levelSO.WaveVelocity;
            waveBehavior?.SetVelocityBegin(waveVelocity);
            wave = Instantiate(levelSO.WavePrefab, waveParent);
            wave.transform.localPosition = Vector3.zero;

            //Handle cats
            for (int index = 0; index < levelSO.CatPositions.Count; index++)
            {
                var radius = levelSO.SpawnCatRadius;
                var minX = levelSO.CatPositions[index].x - radius;
                var maxX = levelSO.CatPositions[index].x + radius;
                var minY = levelSO.CatPositions[index].y - radius;
                var maxY = levelSO.CatPositions[index].y + radius;
                var x = Random.Range(minX, maxX);
                var y = Random.Range(minY, maxY);

                var position = new Vector3(x, 0, y);

                var indexCat = Random.Range(0, levelSO.CatPrefabs.Count);
                GameObject cat = Instantiate(levelSO.CatPrefabs[indexCat], position, Quaternion.identity, catParent);
                listCat.Add(cat);
            }
            
            //Handle Player
            var playerPosition = new Vector3(levelSO.PlayerPosition.x, 0, levelSO.PlayerPosition.y);
            player = Instantiate(levelSO.PlayerPrefab, playerPosition, Quaternion.identity);

            //Handle Obstacles
            for (int i = 0; i < levelSO.ObstaclePositions.Count; i++)
            {
                var radius = levelSO.SpawnObstacleRadius;
                var minX = levelSO.ObstaclePositions[i].x - radius;
                var maxX = levelSO.ObstaclePositions[i].x + radius;
                var minY = levelSO.ObstaclePositions[i].y - radius;
                var maxY = levelSO.ObstaclePositions[i].y + radius;
                var x = Random.Range(minX, maxX);
                var y = Random.Range(minY, maxY);

                var position = new Vector3(x, 0, y);

                var indexObstacle = Random.Range(0, levelSO.ObstaclesPrefab.Count);
                GameObject obstacle = Instantiate(levelSO.ObstaclesPrefab[indexObstacle], position, Quaternion.identity, obstacleParent);
                listObstacle.Add(obstacle);
            }
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

            foreach (var obstacle in listObstacle)
            {
                DestroyImmediate(obstacle);
            }

            DestroyImmediate(player);
            DestroyImmediate(wave);

            Debug.Log("Clear !!!");
        }

#if UNITY_EDITOR
        [ContextMenu("Generate Levels")]
        private void Generatelevel()
        {
            float waveVelocity = 10;
            int indexWave = 0;
            for (int i = 0; i < numberLevelAutoGenerate; i++)
            {
                LevelSO data = ScriptableObject.CreateInstance<LevelSO>();

                //vi tri cua may con meo
                data.CatPositions = fixedCatsPositionSO.CatPositions;

                //nhung con meo se dc dung trong map
                data.CatPrefabs = catPrefabs;

                //nhung obstacle se duoc dung trong map
                data.ObstaclesPrefab = obstaclePrefabs;

                //vi tri cua nhung obstacle
                data.ObstaclePositions = fixedObstaclesPositionSO.ObstaclePositions;

                //wave se duoc dung trong map
                data.WavePrefab = wavePrefabs[indexWave];
                indexWave = (indexWave + 1) % wavePrefabs.Count;

                //toc do cua wave
                data.WaveVelocity = waveVelocity;
                waveVelocity += DIFFICULTY_RATE_EACH_LEVEL;

                //pham vi spawn meo
                data.SpawnCatRadius = 20f;

                //pham vi spawm obstacle
                data.SpawnObstacleRadius = 5f;

                //nguoi choi
                data.PlayerPosition = new Vector2(50, 0);
                data.PlayerPrefab = playerPrefab;

                //tao file
                var fileName = $"{FILE_NAME}{i}.asset";
                string path = Path.Join(pathFolderLevel, fileName);
                AssetDatabase.CreateAsset(data, path);
                AssetDatabase.SaveAssets();
            }
        }

        [ContextMenu("Preference LevelSO")]
        private void PreferenceLevelSO()
        {
            var files = AssetDatabase.FindAssets(FILE_NAME, new[] { pathFolderLevel});

            foreach (var file in files)
            {
                string path = AssetDatabase.GUIDToAssetPath(file);
                LevelSO levelSO = AssetDatabase.LoadAssetAtPath<LevelSO>(path);
                Debug.Log("Preference: " + levelSO.name);
                levels.Add(levelSO);
            }
        }
#endif
    }
}

