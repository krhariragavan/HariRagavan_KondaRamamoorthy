using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Shake")]
    public float CamShakeDuration = 0.5f;
    public float CamShakeMagnitude = 0.1f;

    public static CameraManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    private void OnEnable()
    {
        GameManager.onRocketHit += GameManager_onRocketHit;    
    }

    private void GameManager_onRocketHit(GameManager.RocketType type)
    {
        CameraShake(type);
    }

    private void OnDisable()
    {
        
    }
    
    void Update()
    {
        
    }

    #region Camera Shake Effect
    public void TEST_CamShake() // Applied on button for testing.
    {
        //int range = Random.Range(1, 3);
        int range = 3;

        switch (range)
        {
            case 1:
                Debug.Log("Small Rocket");
                CameraShake(GameManager.RocketType.SmallRocket);
                break;
            case 2:
                Debug.Log("Medium Rocket");
                CameraShake(GameManager.RocketType.MediumRocket);
                break;
            case 3:
                Debug.Log("Large Rocket");
                CameraShake(GameManager.RocketType.LargeRocket);
                break;
        }
    }


    public void CameraShake(GameManager.RocketType type)
    {
        StartCoroutine(Shake(CamShakeDuration, CamShakeMagnitude, type));        
    }
    // Camera shake effect
    IEnumerator Shake(float duration, float magnitude, GameManager.RocketType type)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        int TypeMultiplier = (int)type;
        
        Debug.Log(TypeMultiplier);
        
        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude * TypeMultiplier;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude * TypeMultiplier;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
    #endregion
    // Camera blast effect

}
