using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APHandler : MonoBehaviour
{
    public Text TextAmount;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnChangedActionPoints.AddListener(() => TextAmount.text = "AP: " + GameManager.instance.ActionPoints);
        TextAmount.text = "AP: " + 3;
    }
}
