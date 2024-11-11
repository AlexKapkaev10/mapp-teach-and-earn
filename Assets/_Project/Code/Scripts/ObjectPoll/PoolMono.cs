using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts.ObjectPoll
{
    public class PoolMono<T> where T : MonoBehaviour
    {
        public T Prefab { get; }
        public bool AutoExpand { get; set; }
        public Transform Container { get; set; } = null;

        private List<T> _pool;

        public PoolMono(T prefab, int count)
        {
            Prefab = prefab;
            CreatePool(count);
        }

        public PoolMono(T prefab, int count, Transform container)
        {
            Prefab = prefab;
            Container = container;
            CreatePool(count);
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActiveDyDefault = false)
        {
            var createdObject = Object.Instantiate(Prefab, Container);
            createdObject.gameObject.SetActive(isActiveDyDefault);
            _pool.Add(createdObject);
            return createdObject;
        }

        public bool HasFreeElement(out T element)
        {
            foreach (var mono in _pool)
            {
                if (!mono.gameObject.activeInHierarchy)
                {
                    element = mono;
                    mono.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
            {
                return element;
            }

            if (AutoExpand)
            {
                return CreateObject(true);
            }

            throw new Exception($"No free elements {typeof(T)}");
        }
    }
}