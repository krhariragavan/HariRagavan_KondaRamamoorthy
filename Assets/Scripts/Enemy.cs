using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public GameObject TargetParentObj;
    GameObject[] TargetObj;

    void Start()
    {
        InvokeRepeating("SetRandomDestination", 1f, 4f);
    }

    void Update()
    {
        
    }

    // Get randome target obj  
    void SetRandomDestination()
    {
        TargetObj = new GameObject[TargetParentObj.transform.childCount];
        for (int i = 0; i < TargetParentObj.transform.childCount; i++)
        {
            TargetObj[i] = TargetParentObj.transform.GetChild(i).gameObject;
        }

        int RandomTarget = Random.Range(0, TargetObj.Length);
        navMeshAgent.SetDestination(TargetObj[RandomTarget].transform.position);

        CameraManager.Instance.SetEnemyTransform(this.transform);
    }

}
