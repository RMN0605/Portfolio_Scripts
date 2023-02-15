using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyLayerMask;


    private Color _nomalColor;
    private Image _crossHairImage;

    private void Awake()
    {
        _crossHairImage = GetComponent<Image>();
    }

    private void Start()
    {
        _nomalColor = _crossHairImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        //int mask = 1 << 3;
        Vector3 rayPosition = transform.position;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(rayPosition, transform.forward * 1000, Color.red, 0.1f, true);
        RaycastHit _hitinfo;
        Physics.Raycast(ray, out _hitinfo, Mathf.Infinity, enemyLayerMask);

        if(_hitinfo.collider== null)
        {
            return;
        }
        else if(_hitinfo.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _crossHairImage.color = _nomalColor;
            return;
        }
        else if(_hitinfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _crossHairImage.color = Color.red;
        }
    }
}
