using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreatePoolObject(GameObject PrefabObj, int Count, out List<GameObject> PooledObjs)
    {
        PooledObjs = new List<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Instantiate(PrefabObj, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            PooledObjs.Add(obj);
        }
    }

    public GameObject GetObjectFromPool(List<GameObject> objList)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if (!objList[i].activeInHierarchy)
            {
                return objList[i];
            }
        }
        return null;
    }
}
