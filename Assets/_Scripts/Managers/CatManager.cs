using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

namespace Game.Managers
{
    public class CatManager : Singleton<CatManager>
    {
        [SerializeField] private Transform parent;
        [SerializeField] private List<GameObject> CatListPrefabs;

        private Vector3 CalculatePostionToSpawn(int indexCat)
        {
            return Vector3.zero;
        }

        private void SpawnCatAt(Vector3 position)
        {

        }
    }
}