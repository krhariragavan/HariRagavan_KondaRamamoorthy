using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RocketSettings", menuName = "Game Settings/RocketSettings")]
public class RocketSettings : ScriptableObject
{
    public GameObject RocketObj;
    public float FlightHeight;
    [Range(0.1f, 3f)]
    public float FlightDuration;
    [Header("Accuracy - 0.1 lowest and 1 is the highest")]
    [Range(0.1f, 1f)]
    public float Accuracy = 0.1f;
    public enum RocketType
    {
        SmallRocket = 1,
        MediumRocket = 2,
        LargeRocket = 3
    }
    public RocketType rocketType;
}
