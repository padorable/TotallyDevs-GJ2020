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
        GameManager.instance.OnChangedActionPoints.AddListener(() => 
        {
            if (GameManager.instance.ActionPoints > 0)
            {
                TextAmount.text = "AP: " + GameManager.instance.ActionPoints;
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                TextAmount.text = "END WEEK";
                this.GetComponent<Button>().interactable = true;
            }
        });

        TextAmount.text = "AP: " + 3;
    }
}
