using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChallangeMovement : MonoBehaviour
{
    void Update()
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            transform.DORotate(new Vector3(45, 0, 0), .5f).OnComplete(() => transform.DORotate(new Vector3(-45, 0, 0), .5f));
        }
    }
}
