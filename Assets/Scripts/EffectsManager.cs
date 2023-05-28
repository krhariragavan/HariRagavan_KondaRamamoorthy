using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    public GameObject SmallExplosionPrefab;
    public GameObject MediumExplosionPrefab;
    public GameObject LargeExplosionPrefab;

    List<GameObject> SmallExplosionObjs;
    List<GameObject> MediumExplosionObjs;
    List<GameObject> LargeExplosionObjs;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        CreateExplosionPool();   
    }

    private void OnEnable()
    {
        GameManager.onRocketHit += GameManager_onRocketHit;    
    }
    
    private void OnDisable()
    {
        GameManager.onRocketHit -= GameManager_onRocketHit;
    }

    private void GameManager_onRocketHit(Transform TargetTrans, RocketSettings.RocketType type)
    {
        Explode(TargetTrans, type);
    }


    void Update()
    {
        
    }

    void CreateExplosionPool()
    {
        ObjectPoolingManager.Instance.CreatePoolObject(SmallExplosionPrefab, 10, out SmallExplosionObjs);
        ObjectPoolingManager.Instance.CreatePoolObject(MediumExplosionPrefab, 10, out MediumExplosionObjs);
        ObjectPoolingManager.Instance.CreatePoolObject(LargeExplosionPrefab, 10, out LargeExplosionObjs);
    }

    void Explode(Transform targetTrans, RocketSettings.RocketType rocketType)
    {
        switch (rocketType)
        {
            case RocketSettings.RocketType.SmallRocket:
                GameObject smallExplosionObj = ObjectPoolingManager.Instance.GetObjectFromPool(SmallExplosionObjs);
                smallExplosionObj.transform.position = targetTrans.position;
                break;
            case RocketSettings.RocketType.MediumRocket:
                GameObject mediumExplosionObj = ObjectPoolingManager.Instance.GetObjectFromPool(MediumExplosionObjs);
                mediumExplosionObj.transform.position = targetTrans.position;
                break;
            case RocketSettings.RocketType.LargeRocket:
                GameObject largeExplosionObj = ObjectPoolingManager.Instance.GetObjectFromPool(LargeExplosionObjs);
                largeExplosionObj.transform.position = targetTrans.position;
                break;
        }
    }
}
