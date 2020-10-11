using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    public UnityEvent OnOkay = new UnityEvent();
    public Text ConfirmationText;

    public int Cost = 1;

    private void OnEnable()
    {
        ConfirmationText.text = "This will consume\n\n" + Cost + " AP";
    }

    public void PressOkayButton()
    {
        OnOkay.Invoke();
        this.gameObject.SetActive(false);
    }
   
}
