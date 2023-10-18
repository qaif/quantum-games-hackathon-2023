using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpinState : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI spinStateText;
    public float spinStateDown = 1;
    public float spinStateUp = 0;
    public Material look;
    private void Start()
    {
        look = this.gameObject.GetComponent<Renderer>().material;
    }
    private void Update()
    {
        spinStateText.text = "Spin State:\n"+spinStateDown.ToString("F2")+"|0> + "+spinStateUp.ToString("F2")+ "|1>";
        if (spinStateUp==1f)
        {
            look.color = new Color(0f, 1f, 0f, 1f);
        }
        else if(spinStateUp==0f)
        {
            look.color = new Color(1f, 0f, 0f, 1f);
        }
        else
        {
            look.color = new Color(Mathf.Pow(spinStateDown,2f), Mathf.Pow(spinStateUp,2f), 0f, 1f);
        }
        //print(spinStateDown+","+spinStateUp);
    }
}

