using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeUI : MonoBehaviour
{
    private GameObject childRope;
    private bool _isHaveRope = false;
    [HideInInspector]
    public bool isHaveRope
    {
        get { return _isHaveRope; }
        set
        {
            _isHaveRope = value;
            if (_isHaveRope)
            {
                GeneralManager.Instance.soundManager.PlaySE(SoundManager.SeName.se_12);
                childRope.SetActive(true);
            }
            else
                childRope.SetActive(false);
        }
    }

    void Start()
    {
        childRope = transform.GetChild(0).gameObject;
    }


}
