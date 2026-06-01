using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        
        private static Pool instance = null;

        private Dictionary<string, List<GameObject>> pooledObjects = new();
        
        public static Pool Instance
        {
            get
            {
                if (instance == null)
                {
                    new GameObject("Pool", typeof(Pool));
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                parent = new GameObject("PooledObjects").transform;
                parent.position = Vector3.one * -10000f;
                parent.gameObject.SetActive(false);
            }
            else
            {
                Destroy(this);
            }
        }
        
        public GameObject Spawn(GameObject prefab)
        {
            if (pooledObjects.TryGetValue(prefab.name, out var objects))
            {
                if (objects.Count > 0)
                {
                    var pooledObject = objects[0];
                    objects.RemoveAt(0);
                    return pooledObject;
                }
            }
            
            var instance = Instantiate(prefab, parent);
            instance.name = prefab.name;
            return instance;
        }

        public void Despawn(GameObject pooledObject)
        {
            if (pooledObjects.TryGetValue(pooledObject.name, out var objects))
            {
                if (objects.Contains(pooledObject))
                {
                    Debug.LogWarning("Try multiple despawn.");
                }
                else
                {
                    objects.Add(pooledObject);
                }
            }
            else
            {
                Debug.LogWarning("Try despawn not spawned object");
                var newObjects = new List<GameObject>();
                newObjects.Add(pooledObject);
                pooledObjects.Add(pooledObject.name, newObjects);
            }
            
            pooledObject.transform.SetParent(parent);
            pooledObject.transform.localPosition = Vector3.zero;
        }
    }
}