using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ObstaclePositionsSO", menuName = "LevelDesign/ObstaclePositionsSO", order = 4)]
    public class FixedObstaclesPositionSO : ScriptableObject
    {
        public List<Vector2> ObstaclePositions;
    }
}