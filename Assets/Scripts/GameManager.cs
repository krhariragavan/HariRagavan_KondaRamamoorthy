using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Weapon Type - Small, Medium, Large Rockets
    //public enum RocketType
    //{
    //    SmallRocket = 1,
    //    MediumRocket = 2,
    //    LargeRocket = 3
    //}

    public RocketSettings SmallRocketSettings;
    public RocketSettings MediumRocketSettings;
    public RocketSettings LargeRocketSettings;

    public List<GameObject> SmallRocketObjs;
    public List<GameObject> MediumRocketObjs;
    public List<GameObject> LargeRocketObjs;

    RocketSettings.RocketType CurrentRocketType;

    public delegate void OnRocketLaunch(Transform StartTrans, RocketSettings.RocketType type);
    public static event OnRocketLaunch onRocketLaunch;

    public delegate void OnRocketTravel(Transform CurrentTrans, RocketSettings.RocketType type);
    public static event OnRocketTravel onRocketTravel;

    public delegate void OnRocketHit(Transform TargetTrans, RocketSettings.RocketType type);
    public static event OnRocketHit onRocketHit;

    public delegate void OnChangeRocketType(RocketSettings.RocketType type);
    public static event OnChangeRocketType onChangeRocketType;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Object Pooling for all rockets
        CreateObjectPooling_AllRockets();
        SetRocketType(RocketSettings.RocketType.SmallRocket);
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        InputManager.onFireButtonPress += InputManager_onFireButtonPress;
    }
    private void OnDisable()
    {
        InputManager.onFireButtonPress -= InputManager_onFireButtonPress;
    }

    #region Event Functions
    private void InputManager_onFireButtonPress(Transform TargetTrans)
    {
        FireRocket(CurrentRocketType, TargetTrans);
    }

    #endregion

    #region Helper
    public RocketSettings.RocketType GetRocketType()
    {
        return CurrentRocketType;
    }

    public void SetRocketType(RocketSettings.RocketType type)
    {
        CurrentRocketType = type;
        onChangeRocketType?.Invoke(type);
    }
    #endregion

    #region Object Pooling
    void CreateObjectPooling_AllRockets()
    {
        CreatePoolObject(SmallRocketSettings, 10, out SmallRocketObjs);
        CreatePoolObject(MediumRocketSettings, 10, out MediumRocketObjs);
        CreatePoolObject(LargeRocketSettings, 10, out LargeRocketObjs);
    }

    void CreateRockets()
    {

    }
    // Create Object pooling objects
    void CreatePoolObject(RocketSettings rocketSettings, int Count, out List<GameObject> RocketObjs)
    {
        ObjectPoolingManager.Instance.CreatePoolObject(rocketSettings.RocketObj, Count, out RocketObjs);
        foreach (GameObject obj in RocketObjs)
        {
            Rocket rocket = obj.GetComponent<Rocket>();
            rocket.SetRocketSettings(rocketSettings);
        }
        
        //RocketObjs = new List<GameObject>();
        //for (int i = 0; i < Count; i++)
        //{
        //    GameObject obj = Instantiate(rocketSettings.RocketObj, Vector3.zero, Quaternion.identity);
        //    Rocket rocket = obj.GetComponent<Rocket>();
        //    rocket.SetRocketSettings(rocketSettings);
        //    obj.SetActive(false);
        //    RocketObjs.Add(obj);
        //}
    }

    // Get pooled object
    public GameObject GetPooledObject(List<GameObject> pool, Transform TargetTrans)
    {
        GameObject Inst = ObjectPoolingManager.Instance.GetObjectFromPool(pool);
        Rocket rocket = Inst.GetComponent<Rocket>();
        rocket.SetTarget(TargetTrans);
        return Inst;
        
        //for (int i = 0; i < pool.Count; i++)
        //{
        //    if (!pool[i].activeInHierarchy)
        //    {
        //        Rocket rocket = pool[i].GetComponent<Rocket>();
        //        rocket.SetTarget(TargetTrans);
        //        return pool[i];
        //    }
        //}
        //return null;
    }
    #endregion

    #region Rocket Fire
    // Fire Rocket
    public void Test_FireRocket()
    {
        //FireRocket(RocketSettings.RocketType.SmallRocket, new Vector3(0, 2.5f, 16.6f));
    }

    void FireRocket(RocketSettings.RocketType type, Transform TargetTrans)
    {
        GameObject rocketObj = null;
        switch (type)
        {
            case RocketSettings.RocketType.SmallRocket:
                rocketObj = GetPooledObject(SmallRocketObjs, TargetTrans);
                break;
            case RocketSettings.RocketType.MediumRocket:
                rocketObj = GetPooledObject(MediumRocketObjs, TargetTrans);
                break;
            case RocketSettings.RocketType.LargeRocket:
                rocketObj = GetPooledObject(LargeRocketObjs, TargetTrans);
                break;
        }
        if (rocketObj != null)
        {
            Vector3 CamPos = Camera.main.transform.position;
            rocketObj.transform.position = new Vector3(CamPos.x, CamPos.y + 20, CamPos.z);
            rocketObj.transform.LookAt(TargetTrans);
            rocketObj.SetActive(true);

            //StartCoroutine(FireRocketCoroutine(rocketObj, type, settings, target));
            StartCoroutine(FireRocketAboveCoroutine(rocketObj, type, TargetTrans));
        }
    }

    IEnumerator FireRocketCoroutine(GameObject obj, RocketSettings.RocketType type, Transform TargetTrans)
    {
        //Vector3 centerPos = (obj.transform.position + target) * 0.5f;
        //obj.transform.position = new Vector3(centerPos.x, settings.FlightHeight, centerPos.z);

        Vector3 originalPos = obj.transform.position;
        float elapsed = 0.0f;
        onRocketLaunch?.Invoke(obj.transform, type); // Rocket Launch from ground

        RocketSettings settings = GetRocketSettings();

        while (elapsed < settings.FlightDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / settings.FlightDuration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            onRocketTravel?.Invoke(obj.transform, type); // On Rocket flight or travel

            obj.transform.position = Vector3.Lerp(originalPos, TargetTrans.position, t) + new Vector3(0.0f, Mathf.Sin(Mathf.PI * t) * settings.FlightHeight, 0.0f);
            yield return null;
        }

        onRocketHit?.Invoke(TargetTrans, type); // Rocket hit the target
        obj.transform.position = TargetTrans.position;
    }

    IEnumerator FireRocketAboveCoroutine(GameObject obj, RocketSettings.RocketType type, Transform TargetTrans)
    {
        Vector3 originalPos = obj.transform.localPosition;
        float elapsed = 0.0f;
        onRocketLaunch?.Invoke(obj.transform, type); // Rocket Launch

        RocketSettings settings = GetRocketSettings();

        while (elapsed < settings.FlightDuration)
        {
            elapsed += Time.deltaTime;

            onRocketTravel?.Invoke(obj.transform, type); // On Rocket flight or travel

            obj.transform.localPosition = Vector3.Lerp(originalPos, TargetTrans.position, elapsed);
            yield return null;
        }

        obj.transform.localPosition = TargetTrans.position;
        //obj.SetActive(false);
    }

    public void OnRocketHitTarget(Transform TargetTrans, RocketSettings.RocketType rocketType)
    {
        onRocketHit?.Invoke(TargetTrans, rocketType); // Rocket hit the target
    }

    // Get Rocket settings based on current type
    public RocketSettings GetRocketSettings()
    {
        RocketSettings settings = null;
        switch (CurrentRocketType)
        {
            case RocketSettings.RocketType.SmallRocket:
                settings = SmallRocketSettings;
                break;
            case RocketSettings.RocketType.MediumRocket:
                settings = MediumRocketSettings;
                break;
            case RocketSettings.RocketType.LargeRocket:
                settings = LargeRocketSettings;
                break;
        }
        return settings;
    }
    #endregion
}
