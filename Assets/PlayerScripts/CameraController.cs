using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayerCharacter;
    void Start()
    {
        PlayerCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(PlayerCharacter.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
