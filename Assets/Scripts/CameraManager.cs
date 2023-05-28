using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Shake")]
    public float CamShakeDuration = 0.5f;
    public float CamShakeMagnitude = 0.1f;

    bool IsCameraShake;
    Transform EnemyTransform;
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

    private void OnDisable()
    {
        GameManager.onRocketHit -= GameManager_onRocketHit;
    }

    #region Events
    private void GameManager_onRocketHit(Transform TargetTrans, RocketSettings.RocketType type)
    {
        //CameraShake(type);
    }
    #endregion

    void Update()
    {

    }

    private void LateUpdate()
    {
        CameraFollowEnemy();
    }

    // Camera shake effect without using position



    #region Camera Shake Effect
    public void TEST_CamShake() // Applied on button for testing.
    {
        //int range = Random.Range(1, 3);
        int range = 3;

        switch (range)
        {
            case 1:
                Debug.Log("Small Rocket");
                CameraShake(RocketSettings.RocketType.SmallRocket);
                break;
            case 2:
                Debug.Log("Medium Rocket");
                CameraShake(RocketSettings.RocketType.MediumRocket);
                break;
            case 3:
                Debug.Log("Large Rocket");
                CameraShake(RocketSettings.RocketType.LargeRocket);
                break;
        }
    }


    public void CameraShake(RocketSettings.RocketType type)
    {
        IsCameraShake = true;
        StartCoroutine(Shake(CamShakeDuration, CamShakeMagnitude, type));
    }
    // Camera shake effect
    IEnumerator Shake(float duration, float magnitude, RocketSettings.RocketType type)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        int TypeMultiplier = (int)type;

        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude * TypeMultiplier;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude * TypeMultiplier;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        IsCameraShake = false;
    }
    #endregion

    #region Camera Follow Enemy
    public void SetEnemyTransform(Transform TargetTrans)
    {
        EnemyTransform = TargetTrans;
    }

    void CameraFollowEnemy() // Do particlemanager for explosions
    {
        if (EnemyTransform != null)
        {
            if (!IsCameraShake)
            {
                transform.position = new Vector3(EnemyTransform.position.x - 10f, 35f, EnemyTransform.position.z - 10f);
                transform.LookAt(EnemyTransform);
            }
        }
    }
    #endregion
}
