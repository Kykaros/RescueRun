using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CatPositionsSO", menuName = "LevelDesign/CatPositionsSO", order = 3)]
    public class FixedPositionCatsSO : ScriptableObject
    {
        public List<Vector2> CatPositions;
    }
}

