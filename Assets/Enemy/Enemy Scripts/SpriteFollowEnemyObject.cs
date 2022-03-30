using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollowEnemyObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject parentEnemyObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(parentEnemyObject.transform.position.x, parentEnemyObject.transform.position.y + 1.8f, 0);
    }
}
