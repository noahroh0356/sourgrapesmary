using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipBoxCanvas : MonoBehaviour
{
    //현재 팁 정보 업그레이드 기능

    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text capacityText;
    public TMP_Text amountText;

    public TMP_Text descriptionText;

    public GameObject upgradeGameobject;

    TipBoxData tipBoxData;

    public void Open(TipBoxData tipBox)
    {


        tipBoxData = tipBox;
        nameText.text = tipBoxData.name;
        descriptionText.text = tipBoxData.description;
        capacityText.text = tipBoxData.capacity.ToString();
        Debug.Log("썸네일 스프라이트: " + (tipBoxData.thum == null ? "null" : tipBoxData.thum.name));
        thumImage.sprite = tipBoxData.thum;
        thumImage.SetNativeSize();
        UpdateCanvas();


    }


    public void OnClickedUpgrade()
    {
     TipBoxData nextTipBoxData = GetComponentInParent<TipBoxManager>().GetTipBoxData(tipBoxData.nextKey);

        if (User.Instance.userData.coin < nextTipBoxData.price)
        {
            Debug.Log(tipBoxData.nextKey + "재화부족");
            return;
        }

        else
        {
            User.Instance.UpgradeTipBox(tipBoxData.key, tipBoxData.nextKey);
            MainQuestManager.Instance.DoQuest(MainQuestType.UpgradeTipBox);

            TipBoxManager.Instance.UpdateTipBoxVisual(nextTipBoxData.key);
            User.Instance.AddCoin(-tipBoxData.price);

            Open(nextTipBoxData);

        }

    }

    public void UpdateCanvas()
    {
        amountText.text = GetComponentInParent<TipBoxManager>().aconAmount.ToString();

        if (!string.IsNullOrEmpty(tipBoxData.nextKey))
        {
            upgradeGameobject.SetActive(true);
        }

        else
        {
            upgradeGameobject.SetActive(false);
        }
    }


    //현재까지 충전된 도토리를 얻는 코드
    public void OnClickedReceive()
    {
        User.Instance.AddCoin((int)TipBoxManager.Instance.aconAmount);
        TipBoxManager.Instance.ClearTipBox();
        amountText.text = GetComponentInParent<TipBoxManager>().aconAmount.ToString();
        //**받는 사운드 추가
        UpdateCanvas();
    }

    public void OnClickedAd()
    {
        //광고보고 두배로 리워드 받는 기능
        AdsMgr.Instance.ShowAd(AdUnitType.RV, AdResult);                                             
    }
    //광고에 대한 결과
    void AdResult(bool success)
    {
        if (success)
        {
            int reward = (int)TipBoxManager.Instance.aconAmount * 2;
            User.Instance.AddCoin(reward);
            TipBoxManager.Instance.ClearTipBox();
        }
    }

}

//nextTipBoxData.price; 보유한 코인 보다 적으면 업그레이드 되도록 하는데에 활용 
//tipBoxData.nextKey
//
//User.Instance.AddFurniture()
//구매 이후 화면 갱신



