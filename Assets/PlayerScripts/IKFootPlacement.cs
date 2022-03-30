using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;

    public LayerMask layerMask;
    public GameObject rightFootIK;
    public GameObject rightFootRoot;

    [Range(0, 1f)]
    public float DistanceToGround;
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rightFootRoot.transform.position, Vector2.down);
       
       if (hit.collider.tag == "Walkable")
            {
                Vector2 footPosition = hit.point;
                footPosition.y += DistanceToGround;

                rightFootIK.transform.position = footPosition;
            }
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //if(anim)
        //{
        //    anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
        //    anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

        //    RaycastHit hit;
        //    Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        //    if(Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
        //    {
        //        if(hit.transform.tag == "Walkable")
        //        {
        //            Vector3 footPosition = hit.point;
        //            footPosition.y += DistanceToGround;
        //            anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
        //        }
        //    }
        //}
    }
}
