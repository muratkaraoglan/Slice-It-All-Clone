using System.Collections;
using UnityEngine;
using DG.Tweening;
public class KnifeMovement : MonoBehaviour
{
    public float rotSpeed;
    public float forwardForce;
    public float backForce;
    public bool canRotate;

    Rigidbody rb;
    Vector3 direction = new Vector3(0, 1.5f, 1f);
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if ( GameManager.Instance.gameState != GameManager.GameState.Playing ) return;

        if ( Input.GetMouseButtonDown(0) )
        {
            Force(direction, forwardForce);
        }
    }
    IEnumerator DoRotate()
    {
        yield return new WaitForSeconds(.2f);
        canRotate = true;
        while ( canRotate )
        {
            transform.Rotate(new Vector3(Time.deltaTime * rotSpeed, 0, 0));
            yield return null;
        }
    }

    public void BackForce(Vector3 direction)
    {
        if ( direction.z > 0 )
        {
            direction = new Vector3(direction.x, direction.y, direction.z * -1);
        }
        Force(direction, backForce);
    }

    void Force(Vector3 direction, float force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * force);
        if ( !canRotate )
        {
            StartCoroutine(DoRotate());
        }
    }
}
