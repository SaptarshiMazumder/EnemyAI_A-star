using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookRotation : MonoBehaviour
{
    public Transform player;

    public bool isFlipped = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    private void Update()
    {
        LookAtPlayer();
    }
}
