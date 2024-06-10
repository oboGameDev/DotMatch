using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using Misc;
using UnityEngine;
using UnityEngine.UI;

public class DotFinder : MonoBehaviour
{
    public int index;
    public Vector3 DefaultScale;
    public float DelayBeforeInit = 0.2f;
    private Dot _dot;
    private bool _canUpdate = true;

    private void Start()
    {
        _canUpdate = false;
        GetComponent<UITargetFollower>().enabled = false;
        GetComponent<RawImage>().enabled = false;
        DOVirtual.DelayedCall(DelayBeforeInit, () =>
        {
            _dot = DragController.Instance.GetCircle.Dots[index];
            _dot.GetComponent<MeshRenderer>().enabled = false;

            GetComponent<RawImage>().enabled = true;
            GetComponent<UITargetFollower>().Target = _dot.transform;
            transform.localScale = DefaultScale * DragController.Instance.GetCircle.transform.localScale.x;
            DragController.Instance.OnPlacement += OnPlacement;
            GetComponent<UITargetFollower>().enabled = true;
            _canUpdate = true;
        });
    }

    private void Update()
    {
        if (_canUpdate)
            transform.localScale = DefaultScale * DragController.Instance.GetCircle.transform.localScale.x;
    }

    private void OnPlacement()
    {
        _canUpdate = false;
        DragController.Instance.OnPlacement -= OnPlacement;
        if (_dot != null)
            _dot.GetComponent<MeshRenderer>().enabled = true;
        gameObject.SetActive(false);
    }
}