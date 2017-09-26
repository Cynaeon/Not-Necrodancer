using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

abstract public class MenuScreen : MonoBehaviour
{
    public float rollSpeed = 0.5f;
    public Transform menuList;
    public EventSystem eventSystem;
    public GameObject firstSelected;
    public float minRotation = 0;
    public float maxRotation = 15;

    private Vector3 startPos;
    private Quaternion startRot;
    protected float targetRot;
    protected float step;
    protected bool entered;

    protected virtual void Start()
    {
        startPos = menuList.position;
        startRot = menuList.rotation;
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    protected virtual void OnDisable()
    {
        menuList.position = startPos;
        menuList.rotation = startRot;
        targetRot = 0;
        entered = false;
        step = 0;
    }

    protected virtual void OnEnable()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    protected virtual void Update()
    {
        if (!entered)
        {
            Vector3 target = new Vector3(0, 0, 0);
            if (step < 1)
                step += Time.unscaledDeltaTime * rollSpeed;
            else
                step = 1;
            menuList.eulerAngles = Vector3.Lerp(menuList.rotation.eulerAngles, target, step);
            if (Vector3.Distance(menuList.eulerAngles, target) < 0.2f)
            {
                eventSystem.SetSelectedGameObject(firstSelected);
                entered = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Up"))
            {
                if (targetRot > minRotation)
                {
                    targetRot -= 15;
                    step = 0;
                }
            }
            if (Input.GetButtonDown("Down"))
            {
                if (targetRot < maxRotation)
                {
                    targetRot += 15;
                    step = 0;
                }
            }
            Vector3 target = new Vector3(0, 0, targetRot);
            step += Time.unscaledDeltaTime * rollSpeed * 20;
            menuList.eulerAngles = Vector3.Lerp(menuList.rotation.eulerAngles, target, step);
        }

        if (eventSystem.currentSelectedGameObject == null)
        {
            targetRot = 0;
            eventSystem.SetSelectedGameObject(firstSelected);
        }
    }
}