using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.Pool;

namespace FTKingdom
{
    public static class GenericPool
    {
        private static readonly Dictionary<PoolType, ObjectPool<Component>> pools = new();

        public static void CreatePool<T>(PoolType poolType, T prefab) where T : Component
        {
            if (pools.ContainsKey(poolType))
            {
                return;
            }

            pools.Add(poolType, new ObjectPool<Component>(
            () => { return Create<T>(prefab.gameObject); },
            OnGet, OnRelease, null, false, 10, 20));

            LogHandler.PoolLog($"Create pool: {poolType}.");
        }

        public static void ClearPools()
        {
            foreach (var pool in pools)
            {
                pool.Value.Dispose();
            }

            pools.Clear();
        }

        public static void GetItem<T>(out T poolTarget, PoolType poolType) where T : Component
        {
            Component poolItem = pools[poolType].Get();
            poolTarget = poolItem as T;
            LogHandler.PoolLog($"Get {poolType}. Current pool size: {pools[poolType].CountInactive}");
        }

        // TODO: ExetensionMethods for release item
        public static void ReleaseItem<T>(T item, PoolType poolType) where T : Component
        {
            pools[poolType].Release(item);
            LogHandler.PoolLog($"Release {poolType}. New pool size: {pools[poolType].CountInactive}");
        }

        private static Component Create<T>(GameObject prefab)
        {
            return Object.Instantiate(prefab).GetComponent(typeof(T));
        }

        private static void OnGet(Component c)
        {
            c.gameObject.SetActive(true);
            c.transform.SetAsLastSibling();
        }

        private static void OnRelease(Component c)
        {
            GameObject gO = c.gameObject;

            if (gO.activeSelf)
            {
                gO.SetActive(false);
            }
        }
    }
}