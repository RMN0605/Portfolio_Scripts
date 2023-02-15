using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BgmName    //BGM
{
    bgm_01 = 0,
    bgm_02,
    bgm_03,
    bgm_04,
    Silent = 999,
}
public class StartBGM : MonoBehaviour
{
    [SerializeField]
    private BgmName bgm;
    
    void Start()
    {
        GeneralManager.Instance.soundManager.StopBGM();
        GeneralManager.Instance.soundManager.PlayBGM((SoundManager.BgmName)bgm);
    }

}
