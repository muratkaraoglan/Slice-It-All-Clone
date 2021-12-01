using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CollisionHelper : MonoBehaviour
{
    public event Action<Collision> OnCollisionEnterAction;
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterAction?.Invoke(collision);
    }
}
