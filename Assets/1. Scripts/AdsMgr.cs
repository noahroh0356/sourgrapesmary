using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdsMgr : MonoBehaviour
{
   
    public void ShowAd(AdUnitType adUnitType, Action<bool> callback)
    {

    }
}

public enum AdUnitType
{
    RV, //리워드
    IS, //전면
    BN //배너 
}