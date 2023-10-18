using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGate : MonoBehaviour
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
            float temp = other.gameObject.GetComponentInChildren<SpinState>().spinStateUp;
            other.gameObject.GetComponentInChildren<SpinState>().spinStateUp = other.gameObject.GetComponentInChildren<SpinState>().spinStateDown;
            other.gameObject.GetComponentInChildren<SpinState>().spinStateDown = temp;
            //print(other.gameObject.GetComponentInChildren<SpinState>().spinStateUp+","+ other.gameObject.GetComponentInChildren<SpinState>().spinStateDown);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        collision.gameObject.GetComponentInChildren<SpinState>().spinState = !collision.gameObject.GetComponentInChildren<SpinState>().spinState;
    //        print(collision.gameObject.GetComponentInChildren<SpinState>().spinState);
    //    }
    //}
}
