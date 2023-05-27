using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject AimObj;
    public GameObject HitObj;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DeactivateHitAimObj();
    }

    void DeactivateHitAimObj()
    {
        HitObj.SetActive(false);
        AimObj.SetActive(false);
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        InputManager.onMouseAim += InputManager_onMouseAim;
        InputManager.onFireButtonPress += InputManager_onFireButtonPress;
        InputManager.onMouseAimLoose += InputManager_onMouseAimLoose;
        GameManager.onRocketHit += GameManager_onRocketHit;
    }

    private void OnDisable()
    {
        InputManager.onMouseAim -= InputManager_onMouseAim;
        InputManager.onFireButtonPress -= InputManager_onFireButtonPress;
        InputManager.onMouseAimLoose -= InputManager_onMouseAimLoose;
        GameManager.onRocketHit -= GameManager_onRocketHit;
    }

    #region Even Functions
    private void InputManager_onMouseAim(Transform TargetTrans)
    {
        AimObj.transform.position = TargetTrans.position;
        AimObj.SetActive(true);
    }
    
    private void InputManager_onFireButtonPress(Transform TargetTrans)
    {
        HitObj.transform.position = TargetTrans.position;
        HitObj.SetActive(true);
    }

    private void InputManager_onMouseAimLoose()
    {
        AimObj.SetActive(false);
    }
    private void GameManager_onRocketHit(Transform TargetTrans, RocketSettings.RocketType type)
    {
        HitObj.SetActive(false);
    }
    #endregion

    #region Buttons

    public void SmallRocketButton()
    {
        GameManager.Instance.SetRocketType(RocketSettings.RocketType.SmallRocket);
    }

    public void MediumRocketButton()
    {
        GameManager.Instance.SetRocketType(RocketSettings.RocketType.MediumRocket);
    }

    public void LargeRocketButton()
    {
        GameManager.Instance.SetRocketType(RocketSettings.RocketType.LargeRocket);
    }
    #endregion
}
