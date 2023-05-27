using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    RocketSettings settings;
    Transform Target;
    
    void Start()
    {

    }

    void Update()
    {

    }

    public void SetRocketSettings(RocketSettings rocketSettings)
    {
        settings = rocketSettings;
    }

    public void SetTarget(Transform TargetTrans)
    {
        Target = TargetTrans;
    }

    // On Trigger Enter call
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            GameManager.Instance.OnRocketHitTarget(Target, settings.rocketType);
            gameObject.SetActive(false);
        }
    }
}
