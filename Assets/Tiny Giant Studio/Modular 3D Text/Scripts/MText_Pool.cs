using System.Collections.Generic;
using UnityEngine;

namespace MText
{  
    [DisallowMultipleComponent]
    public class MText_Pool : MonoBehaviour
    {
        public static MText_Pool Instance;

        public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        public GameObject GetPoolItem(MText_Font font, char c)
        {
            string key = font.name + " " + c;

            if (poolDictionary.ContainsKey(key))
            {
                if (poolDictionary[key].Count > 0)
                {
                    GameObject poolItem = poolDictionary[key].Dequeue();
                    return poolItem;
                }
                else
                {
                    GameObject newPoolItem = new GameObject();
                    Mesh meshPrefab = font.RetrievePrefab(c);
                    if (meshPrefab)
                    {
                        newPoolItem.AddComponent<MeshFilter>();
                        newPoolItem.GetComponent<MeshFilter>().sharedMesh = meshPrefab;
                        newPoolItem.SetActive(false);
                        newPoolItem.transform.SetParent(transform);
                        newPoolItem.AddComponent<MText_PoolItem>().key = key;
                        newPoolItem.name = c.ToString();
                    }
                    else
                    {
                        newPoolItem.name = "Space";
                    }

                    return newPoolItem;
                }
            }
            else
            {
                Queue<GameObject> queue = new Queue<GameObject>();
                poolDictionary.Add(key, queue);

                GameObject newPoolItem = new GameObject();
                Mesh meshPrefab = font.RetrievePrefab(c);
                if (meshPrefab)
                {
                    newPoolItem.AddComponent<MeshFilter>();
                    newPoolItem.GetComponent<MeshFilter>().sharedMesh = meshPrefab;
                    newPoolItem.SetActive(false);
                    newPoolItem.transform.SetParent(transform);
                    newPoolItem.AddComponent<MText_PoolItem>().key = key;
                }
                else newPoolItem = new GameObject();
                return newPoolItem;
            }
        }
        public void returnPoolItem(GameObject poolItem)
        {
            if (poolItem.GetComponent<MText_PoolItem>())
            {
                if (poolDictionary.ContainsKey(poolItem.GetComponent<MText_PoolItem>().key))
                {
                    poolItem.SetActive(false);
                    poolItem.transform.SetParent(transform);
                    poolDictionary[poolItem.GetComponent<MText_PoolItem>().key].Enqueue(poolItem);
                }
                else Destroy(poolItem);
            }
            else
            {
                Destroy(poolItem);
            }
        }
    }
}
