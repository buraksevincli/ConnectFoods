using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConnectedFoods.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ConnectedFoods.Game
{
    public class SampleLevelCreator : MonoBehaviour
    {
        [SerializeField] private FoodItem foodItemPrefab;
        [SerializeField, Range(4, 9)] private int size;

#if UNITY_EDITOR
        [Button]
        private void Create()
        {
            Clear();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    FoodItem foodItem = PrefabUtility.InstantiatePrefab(foodItemPrefab, transform) as FoodItem;
                    
                    float scale = foodItem.transform.localScale.x;
                    float x = j - size / 2f + scale / 2f;
                    float y = i - size / 2f + scale / 2f;

                    foodItem.transform.localPosition = new Vector3(x, y, 0);
                    foodItem.FoodType = (FoodType)Random.Range(1, Enum.GetValues(typeof(FoodType)).Length);
                }
            }
        }

        [Button]
        private void Clear()
        {
            FoodItem[] items = GetComponentsInChildren<FoodItem>();

            foreach (FoodItem foodItem in items)
            {
                DestroyImmediate(foodItem.gameObject);
            }
        }
#endif
        
    }
}
