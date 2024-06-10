using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HideAfter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.DelayedCall(3f, () => gameObject.SetActive(false));
    }
}