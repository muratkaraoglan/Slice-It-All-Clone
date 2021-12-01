using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class PopupText : MonoBehaviour
{
    [HideInInspector]
    public string displayText;
    void Start()
    {
        TMP_Text tmp_text = GetComponent<TMP_Text>();
        tmp_text.text = displayText;
        tmp_text.DOFade(0f, 0.7f);
        transform.DOMove(transform.position + Vector3.up * .5f, 1f).OnComplete(() => Destroy(gameObject));
    }

}
