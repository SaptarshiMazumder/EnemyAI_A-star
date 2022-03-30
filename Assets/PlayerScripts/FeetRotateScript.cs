using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetRotateScript : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject Feet1;
    public Vector3 tloc;
    public float angle;

    void Start()
    {
        RotateBone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateBone()
    {
        this.transform.localEulerAngles = new Vector3(0, 0, 50);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Walkable"))
        {
            Debug.Log("collided");
            tloc = other.gameObject.transform.position - this.transform.position;
            angle = Vector3.Angle(this.transform.forward, tloc);
            Debug.Log(angle);
        }
    }
}
