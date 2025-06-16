using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdsMgr : MonoBehaviour
{

    public static AdsMgr Instance;
    private void Awake()
    {
        Instance = this;
    }

    public AdMob adMob;

    private void Start()
    {
        adMob.Init();
    }

    public void ShowAd(AdUnitType adUnitType, Action<bool> callback)
    {
        adMob.ShowAd(adUnitType, callback);
    }
}

public enum AdUnitType
{
    RV, //리워드
    IS, //전면
    BN //배너 
}