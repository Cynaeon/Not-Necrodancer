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
    protected SoundEffects soundEffects;

    protected virtual void Start()
    {
        startPos = menuList.position;
        startRot = menuList.rotation;
        eventSystem.SetSelectedGameObject(firstSelected);
        soundEffects = (SoundEffects) FindObjectOfType(typeof(SoundEffects));
    }

    protected virtual void OnDisable()
    {
        soundEffects.MenuValidate();
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
        if (Input.GetButtonDown("Up"))
        {
            if (targetRot > minRotation)
            {
                targetRot -= 15;
                step = 0;
                soundEffects.MenuSelect();
            }
        }
        else if (Input.GetButtonDown("Down"))
        {
            if (targetRot < maxRotation)
            {
                targetRot += 15;
                step = 0;
                soundEffects.MenuSelect();
            }
        }

        /*
        if (!entered)
        {
            Vector3 target = new Vector3(0, 0, targetRot);
            if (step < 1)
                step += Time.unscaledDeltaTime * rollSpeed;
            else
                step = 1;
            menuList.eulerAngles = Vector3.Lerp(menuList.rotation.eulerAngles, target, step);
            if (Vector3.Distance(menuList.eulerAngles, target) < 0.2f)
            {
                entered = true;
            }
        }
        else
        {
            Vector3 target = new Vector3(0, 0, targetRot);
            step += Time.unscaledDeltaTime * rollSpeed * 20;
            menuList.eulerAngles = Vector3.Lerp(menuList.rotation.eulerAngles, target, step);
        }

        if (eventSystem.currentSelectedGameObject == null)
        {
            targetRot = 0;
            eventSystem.SetSelectedGameObject(firstSelected);
        }
        */

        Vector3 target = new Vector3(0, 0, targetRot);
        step += Time.unscaledDeltaTime * rollSpeed;
        menuList.eulerAngles = Vector3.Lerp(menuList.rotation.eulerAngles, target, step);
        

        if (eventSystem.currentSelectedGameObject == null)
        {
            targetRot = 0;
            eventSystem.SetSelectedGameObject(firstSelected);
        }
    }
}