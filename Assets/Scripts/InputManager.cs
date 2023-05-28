using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public delegate void OnMouseAim(Transform TargetPos);
    public static event OnMouseAim onMouseAim;

    public delegate void OnFireButtonPress(Transform TargetPos);
    public static event OnFireButtonPress onFireButtonPress;

    public delegate void OnMouseAimLoose();
    public static event OnMouseAimLoose onMouseAimLoose;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        MousePositionAim();
    }

    // Set fire target as mouse position on a 3d object
    public void MousePositionAim()
    {
        if (!IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            Transform hitTrans = hit.collider.transform;
            if (hit.collider.gameObject.CompareTag("Env"))
            {
                onMouseAim?.Invoke(hitTrans);
                FireOnClick(hitTrans);
            }
            else
                onMouseAimLoose?.Invoke();
        }
        else
            onMouseAimLoose?.Invoke();
    }

    void FireOnClick(Transform TargetTrans)
    {
        if (IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0))
            {
                onFireButtonPress?.Invoke(TargetTrans);
            }
        }
    }

    public bool IsPointerOverGameObject()
    {
        bool IsPointingGO = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        //Debug.Log(IsPointingGO);
        return !IsPointingGO;
    }
}
