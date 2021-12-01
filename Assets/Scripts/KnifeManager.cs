using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KnifeManager : MonoBehaviour
{
    /// <summary>
    /////////////////// PUBLIC //////////////////////////// 
    /// </summary>
    public event Action<GameManager.GameState> OnGameStateChanged;
    public CollisionHelper handleColliderHelper;
    public CollisionHelper topColliderHelper;
    public CollisionHelper sharpColliderHelper;
    public MeshRenderer knifeMeshRenderer;
    public LayerMask finishLayer;
    /////////////////// LOCAL /////////////////////////////// 
    private KnifeMovement knifeMovement;
    private BoxCollider boxCollider;
    private Vector3 directionOffset = new Vector3(0, -.5f, 1f);
    List<Color> originalColorList = new List<Color>();

    private void OnEnable()
    {
        handleColliderHelper.OnCollisionEnterAction += TopAndHandleColliderHelper;
        topColliderHelper.OnCollisionEnterAction += TopAndHandleColliderHelper;
        sharpColliderHelper.OnCollisionEnterAction += SharpColliderHelper;
        boxCollider = GetComponent<BoxCollider>();
        knifeMovement = GetComponent<KnifeMovement>();
        foreach ( var material in knifeMeshRenderer.materials )
        {
            originalColorList.Add(material.color);
        }
        GameManager.Instance.BeSubs();
    }

    private void SharpColliderHelper(Collision obj)
    {

        if ( GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.Challange )
        {
            if ( obj.collider.CompareTag("Slicable") )
            {
                boxCollider.isTrigger = true;
                obj.gameObject.GetComponent<MeshSlicer>().Slice(new Plane(Vector3.right, Vector3.zero), Vector3.up * .5f);
                ScoreManager.Instance.PopupText(transform.position, "+1");
            }
            else if ( finishLayer.value == (1 << obj.gameObject.layer) )
            {
                ScoreManager.Instance.CalculateScore(obj.collider.tag);
                OnGameStateChanged?.Invoke(GameManager.GameState.Finish);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    private void TopAndHandleColliderHelper(Collision obj)
    {
        if ( GameManager.Instance.gameState != GameManager.GameState.Playing ) return;
        if ( obj.collider.CompareTag("Slicable") )
        {
            knifeMovement.BackForce(CalculateDirection(obj.contacts[0].point));
            StartCoroutine("KnifeCollisionbyHandleorTop");
        }
        else if ( finishLayer.value == (1 << obj.gameObject.layer) )
        {
            knifeMovement.BackForce(CalculateDirection(obj.contacts[0].point));
            StartCoroutine("KnifeCollisionbyHandleorTop");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( GameManager.Instance.gameState != GameManager.GameState.Playing ) return;
        knifeMovement.canRotate = false;
        if ( collision.collider.CompareTag("Platform") || collision.collider.CompareTag("Finish") )
        {
            boxCollider.isTrigger = false;
        }
        else if ( collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Plane") )
        {
            Debug.Log("Game Over");
            boxCollider.isTrigger = false;
            OnGameStateChanged?.Invoke(GameManager.GameState.GameOver);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if ( GameManager.Instance.gameState != GameManager.GameState.Playing ) return;
        knifeMovement.canRotate = false;
        if ( other.CompareTag("Platform") || other.CompareTag("Finish") )
        {
            if ( other.CompareTag("Finish") ) other.GetComponent<BoxCollider>().enabled = false;
            boxCollider.isTrigger = false;
        }
        else if ( other.CompareTag("Obstacle") || other.CompareTag("Plane") )
        {
            Debug.Log("Game Over");
            boxCollider.isTrigger = false;
            OnGameStateChanged?.Invoke(GameManager.GameState.GameOver);
        }


    }

    private Vector3 CalculateDirection(Vector3 contactPoint) => (contactPoint - directionOffset).normalized;

    IEnumerator KnifeCollisionbyHandleorTop()
    {
        Material[] knifeMaterials = knifeMeshRenderer.materials;
        foreach ( var material in knifeMaterials )
        {
            material.color = Color.white;
        }

        yield return new WaitForSeconds(.4f);

        for ( int i = 0; i < originalColorList.Count; i++ )
        {
            knifeMaterials[i].color = originalColorList[i];
        }
    }
    private void OnDisable()
    {
        handleColliderHelper.OnCollisionEnterAction -= TopAndHandleColliderHelper;
        topColliderHelper.OnCollisionEnterAction -= TopAndHandleColliderHelper;
        sharpColliderHelper.OnCollisionEnterAction -= SharpColliderHelper;
    }
}
