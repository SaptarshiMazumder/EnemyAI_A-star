using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundAngle : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 normalVector;
    public float angle;
    //EnemyLookRotation enemyLookRot;
    void Start()
    {
        //enemyLookRot = GetComponent<EnemyLookRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D footAngleHit = Physics2D.Raycast(transform.position, Vector2.down);
        if (footAngleHit.collider != null)
        {
            normalVector = footAngleHit.normal;
            angle = Vector2.Angle(Vector2.down, normalVector) + 180f;
            //Debug.DrawLine(transform.position, footAngleHit.normal, Color.yellow, 10.0f);
            if (normalVector.x < 0 || normalVector.y < 0)
            {
                angle = -angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
            //if (!enemyLookRot.isFlipped)
            //{
            //    transform.rotation = Quaternion.Euler(0, 0, angle);
            //}
            //else
            //{
                
            //}
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
