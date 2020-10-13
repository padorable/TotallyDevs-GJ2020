using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCheck : MonoBehaviour 
{
    private GameObject currentGameObject;
    private bool isOnPhone = false;

    public static MouseCheck instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (obj != null) Check(obj.tag);
        }
    }

    public void Check(string tagged)
    {
        if(tagged == "Phone")
        {
            if(!isOnPhone)
                DialogueManager.instance.SetDialogue(". . .");
            isOnPhone = true;
        }
        else if (tagged == "World")
        {
            if (PhoneManager.instance.IsStillLocked) return;

            PhoneManager.instance.Home();
            isOnPhone = false;
        }
    }
}