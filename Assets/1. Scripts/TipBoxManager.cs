using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipBoxManager : MonoBehaviour
{

    //신규 유저라면 tipbox 기본 지급
    //현재 보유한 팁박스 중 가장 좋은 팁박스에 해당하는 게임 오브젝트를 활성화 처리하기

    [SerializeField] TipBox[] tipBoxes;

    int capacity = 3000;
    float aconPerSec = 0.5f;
    float aconAmount = 0; // 충전된 양
    
    public void Start()
    {
        tipBoxes = GetComponentsInChildren<TipBox>();

        UserFurniture userFurniture = User.Instance.GetSetUpFurniture(FurniturePlace.tipbox);
        for (int i = 0; i < tipBoxes.Length; i++)
        {
            if (userFurniture.furniturekey == tipBoxes[i].key)
            {
                tipBoxes[i].gameObject.SetActive(true);
            }
            else
            {
                tipBoxes[i].gameObject.SetActive(false);
            }

        }
        aconAmount = PlayerPrefs.GetFloat("TipBoxAcon", 0);
        string lasTimeStr = PlayerPrefs.GetString("TipBoxLastTime", null);

        if (lasTimeStr != null)
        {
            DateTime lastTime = DateTime.Parse(lasTimeStr);
            int pastSec = (int)((DateTime.Now - lastTime)*0.5f).TotalSeconds;
            User.Instance.AddCoin(pastSec);
            // 0.5초에 1씩 확
            // 로그 찍어서 문자열로 확인해보기 
        }

        StartCoroutine(CoFillTipBox());
    }

    IEnumerator CoFillTipBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            aconAmount += aconPerSec;
            PlayerPrefs.SetFloat("TipBoxAcon", aconAmount);
            PlayerPrefs.SetString("TipBoxLastTime", DateTime.Now.ToString());

            if (aconAmount >= capacity)
            {
                aconAmount = capacity;
            }
        }

    }

    private void Update()
    {
        
    }

}
