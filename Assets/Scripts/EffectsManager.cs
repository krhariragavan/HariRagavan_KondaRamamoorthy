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
        ObjectPoolingManager.Instance.CreatePoolObject(SmallExplosionPrefab, 20, out SmallExplosionObjs);
        ObjectPoolingManager.Instance.CreatePoolObject(MediumExplosionPrefab, 20, out MediumExplosionObjs);
        ObjectPoolingManager.Instance.CreatePoolObject(LargeExplosionPrefab, 20, out LargeExplosionObjs);
    }

    void Explode(Transform targetTrans, RocketSettings.RocketType rocketType)
    {
        Vector3 ExplosionPos = new Vector3(targetTrans.position.x, targetTrans.position.y + 3f, targetTrans.position.z);
        switch (rocketType)
        {
            case RocketSettings.RocketType.SmallRocket:
                GameObject smallExplosionObj = ObjectPoolingManager.Instance.GetObjectFromPool(SmallExplosionObjs);
                smallExplosionObj.transform.position = ExplosionPos;
                smallExplosionObj.SetActive(true);
                break;
            case RocketSettings.RocketType.MediumRocket:
                GameObject mediumExplosionObj = ObjectPoolingManager.Instance.GetObjectFromPool(MediumExplosionObjs);
                mediumExplosionObj.transform.position = ExplosionPos;
                mediumExplosionObj.SetActive(true);
                break;
            case RocketSettings.RocketType.LargeRocket:
                GameObject largeExplosionObj = ObjectPoolingManager.Instance.GetObjectFromPool(LargeExplosionObjs);
                largeExplosionObj.transform.position = ExplosionPos;
                largeExplosionObj.SetActive(true);
                break;
        }
    }
}
