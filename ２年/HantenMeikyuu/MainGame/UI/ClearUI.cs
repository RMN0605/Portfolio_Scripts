using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    private void Start()
    {
        GeneralManager.Instance.soundManager.StopBGM();
        GeneralManager.Instance.soundManager.PlayBGM(SoundManager.BgmName.bgm_03);
    }
}
