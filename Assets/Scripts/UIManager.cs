using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject AimObj;
    public GameObject HitObj;

    public GameObject SettingsMenuObj;

    int Score;
    public TMP_Text TimerText;
    public TMP_Text ScoreText;

    [Header("Slider")]
    public Slider SmallRocket_FlightDurationSlider;
    public Slider MediumRocket_FlightDurationSlider;
    public Slider LargeRocket_FlightDurationSlider;

    public Slider SmallRocket_AccuracySlider;
    public Slider MediumRocket_AccuracySlider;
    public Slider LargeRocket_AccuracySlider;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DeactivateHitAimObj();
        SettingsMenuObj.SetActive(false);
    }

    void DeactivateHitAimObj()
    {
        HitObj.SetActive(false);
        AimObj.SetActive(false);
    }

    void Update()
    {
        SetTimerText();
    }

    void SetTimerText()
    {
        int minutes = (int)(Time.timeSinceLevelLoad / 60f);
        int seconds = (int)(Time.timeSinceLevelLoad % 60f);

        TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
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
        Score += 10;
        ScoreText.text = "Score: " + Score.ToString();
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

    public void OnPressSettingsButton()
    {
        SettingsMenuObj.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseSettingsMenu()
    {
        SettingsMenuObj.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnPressTabButton(GameObject Obj)
    {
        Obj.transform.SetAsLastSibling(); // Set as last sibling to bring it to front
    }    

    public void OnSmallRocketSliderChange_FlightDuration()
    {
        GameManager.Instance.SmallRocketSettings.FlightDuration = SmallRocket_FlightDurationSlider.value;
        Debug.Log(GameManager.Instance.SmallRocketSettings.FlightDuration);
    }

    public void OnMedRocketSliderChange_FlightDuration()
    {
        GameManager.Instance.MediumRocketSettings.FlightDuration = MediumRocket_FlightDurationSlider.value;
    }

    public void OnLargeRocketSliderChange_FlightDuration()
    {
        GameManager.Instance.LargeRocketSettings.FlightDuration = LargeRocket_FlightDurationSlider.value;
    }

    public void OnSmallRocketSliderChange_Accuracy()
    {
        GameManager.Instance.SmallRocketSettings.Accuracy = SmallRocket_AccuracySlider.value;
    }

    public void OnMedRocketSliderChange_Accuracy()
    {
        GameManager.Instance.MediumRocketSettings.Accuracy = MediumRocket_AccuracySlider.value;
    }

    public void OnLargeRocketSliderChange_Accuracy()
    {
        GameManager.Instance.LargeRocketSettings.Accuracy = LargeRocket_AccuracySlider.value;
    }

    public void ChangeMasterVolume(Slider slider)
    {
        Camera.main.GetComponent<AudioSource>().volume = slider.value;
    }
    #endregion
}
