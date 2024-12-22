using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName ="LevelSO", menuName ="LevelDesign/LevelSO", order = 1)]
    public class LevelSO : ScriptableObject
    {
        [Header("Cats")]
        public List<Vector2> CatPositions;
        public List<GameObject> CatPrefabs;

        [Space]
        [Header("Wave")]
        public float WaveVelocity = 10;
        public GameObject WavePrefab;

        [Space]
        [Header("Radius spawn cats")]
        public float SpawnCatRadius;

        [Space]
        [Header("Player")]
        public Vector2 PlayerPosition;
        public GameObject PlayerPrefab;

        [Space]
        [Header("Obstacle")]
        public List<GameObject> ObstaclesPrefab;
        public List<Vector2> ObstaclePositions;

        [Space]
        [Header("Radius spawn obstacles")]
        public float SpawnObstacleRadius;
    }
}