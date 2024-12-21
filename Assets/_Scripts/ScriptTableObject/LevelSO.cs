using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Managers;
using Unity.VisualScripting;

namespace Game
{
    [CreateAssetMenu(fileName ="LevelSO", menuName ="LevelDesign/LevelSO", order = 1)]
    public class LevelSO : ScriptableObject
    {
        [Header("List Position Cats")]
        public List<Vector2> CatPositions;

        [Header("Cats Prefab")]
        public List<GameObject> CatPrefabs;

        [Space]
        [Header("Weight for difficulty")]
        public int Difficulty = 1;

        [Space]
        [Header("Radius spawn cats")]
        public float SpawnRadius;

        [Space]
        [Header("Player start position")]
        public Vector2 PlayerPosition;
        [Header("Player Prefab")]
        public GameObject PlayerPrefab;

        [Space]
        [Header("Obstacle")]
        public List<GameObject> Obstacles;
    }
}