using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    public UnityEvent OnOkay = new UnityEvent();

    public void PressOkayButton()
    {
        OnOkay.Invoke();
        this.gameObject.SetActive(false);
    }
   
}
