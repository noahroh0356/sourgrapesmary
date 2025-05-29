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

    TipBoxData tipBoxData;

    public void Open(TipBoxData tipBox)
    {


        tipBoxData = tipBox;
        nameText.text = tipBoxData.name;
        descriptionText.text = tipBoxData.description;
        capacityText.text = tipBoxData.capacity.ToString();
        amountText.text = GetComponentInParent<TipBoxManager>().aconAmount.ToString();
        Debug.Log("썸네일 스프라이트: " + (tipBoxData.thum == null ? "null" : tipBoxData.thum.name));

        thumImage.sprite = tipBoxData.thum;
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
            User.Instance.userData.coin -= tipBoxData.price;

        }
    }


    }

    //nextTipBoxData.price; 보유한 코인 보다 적으면 업그레이드 되도록 하는데에 활용 
    //tipBoxData.nextKey
    //
    //User.Instance.AddFurniture()
    //구매 이후 화면 갱신



