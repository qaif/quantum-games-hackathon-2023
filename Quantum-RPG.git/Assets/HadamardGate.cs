using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadamardGate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float temp = other.gameObject.GetComponentInChildren<SpinState>().spinStateDown;
            float temp1 = other.gameObject.GetComponentInChildren<SpinState>().spinStateUp;
            other.gameObject.GetComponentInChildren<SpinState>().spinStateDown = (temp + temp1) / (Mathf.Sqrt(2f));
            other.gameObject.GetComponentInChildren<SpinState>().spinStateUp = (temp-temp1)/(Mathf.Sqrt(2f));
        }
    }
}
