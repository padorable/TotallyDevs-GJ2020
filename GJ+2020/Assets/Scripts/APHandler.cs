using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APHandler : MonoBehaviour
{
    public Text TextAmount;
    public Sprite NeutralImage;
    public Sprite EndImage;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnChangedActionPoints.AddListener(() => 
        {
            if (GameManager.instance.ActionPoints > 0)
            {
                this.GetComponent<Image>().sprite = NeutralImage;
                TextAmount.text = "AP: " + GameManager.instance.ActionPoints;
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.GetComponent<Image>().sprite = EndImage;
                TextAmount.text = "END WEEK";
                this.GetComponent<Button>().interactable = true;
            }
        });

        TextAmount.text = "AP: 2";
        this.GetComponent<Button>().interactable = false;
    }
}
