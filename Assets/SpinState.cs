using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinState : MonoBehaviour
{
    public float spinStateDown = 1;
    public float spinStateUp = 0;
    public Material look;
    private void Start()
    {
        look = this.gameObject.GetComponent<Renderer>().material;
    }
    private void Update()
    {
        if(spinStateUp==1f)
        {
            look.color = new Color(0f, 1f, 0f, 1f);
        }
        else if(spinStateUp==0f)
        {
            look.color = new Color(1f, 0f, 0f, 1f);
        }
        else
        {
            look.color = new Color(Mathf.Abs(spinStateDown), Mathf.Abs(spinStateUp), 0f, 1f);
        }
        print(spinStateDown+","+spinStateUp);
    }
}
