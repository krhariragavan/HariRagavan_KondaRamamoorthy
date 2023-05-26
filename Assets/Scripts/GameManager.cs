using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Weapon Type - Small, Medium, Large Rockets
    public enum RocketType
    {
        SmallRocket = 1,
        MediumRocket = 2,
        LargeRocket = 3
    }

    // Object Pooling
    public GameObject SmallRocketPrefabObj;
    public GameObject MediumRocketPrefabObj;
    public GameObject LargeRocketPrefabObj;
    
    List<GameObject> SmallRocketObjs;
    List<GameObject> MediumRocketObjs;
    List<GameObject> LargeRocketObjs;

    public Transform DefaultRocketTransform;
    
    public delegate void OnRocketHit(RocketType type);
    public static event OnRocketHit onRocketHit;
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        // Object Pooling for all rockets
        CreateObjectPooling_AllRockets(); 
    }

    void Update()
    {
        
    }    


    #region Object Pooling
    void CreateObjectPooling_AllRockets()
    {
        CreatePoolObject(SmallRocketPrefabObj, 10, ref SmallRocketObjs);
        CreatePoolObject(MediumRocketPrefabObj, 10, ref MediumRocketObjs);
        CreatePoolObject(LargeRocketPrefabObj, 10, ref LargeRocketObjs);
    }
    // Create Object pooling objects
    void CreatePoolObject(GameObject PrefabObj, int Count, ref List<GameObject> RocketObjs)
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Instantiate(PrefabObj, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            RocketObjs.Add(obj);
        }
    }

    // Get pooled object
    public GameObject GetPooledObject(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }
    #endregion

    // Fire Rocket
    public void FireRocket(RocketType type, Vector3 target, float height, float duration)
    {
        GameObject rocketObj = null;
        switch (type)
        {
            case RocketType.SmallRocket:
                rocketObj = GetPooledObject(SmallRocketObjs);
                break;
            case RocketType.MediumRocket:
                rocketObj = GetPooledObject(MediumRocketObjs);
                break;
            case RocketType.LargeRocket:
                rocketObj = GetPooledObject(LargeRocketObjs);
                break;
        }
        if (rocketObj != null)
        {
            rocketObj.transform.position = DefaultRocketTransform.position;
            rocketObj.transform.rotation = DefaultRocketTransform.rotation;
            rocketObj.SetActive(true);
            
            StartCoroutine(ParabolicMoveCoroutine(rocketObj, type, target, height, duration));
        }
    }

    IEnumerator ParabolicMoveCoroutine(GameObject obj, RocketType type, Vector3 target, float height, float duration)
    {
        Vector3 originalPos = obj.transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            obj.transform.position = Vector3.Lerp(originalPos, target, t) + new Vector3(0.0f, Mathf.Sin(Mathf.PI * t) * height, 0.0f);
            yield return null;
        }

        if (onRocketHit != null)
            onRocketHit(type);
        obj.transform.position = target;
    }
}
