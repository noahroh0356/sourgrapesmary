using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class KitchenCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text abilityText;

    private static KitchenCanvas instance; // 정적 변수
    public static KitchenCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<KitchenCanvas>(FindObjectsInactive.Include);

            return instance;
        }
    }



    public void Open(string key)
    {
        Debug.Log("KitchenCanvas Open" + key);
        gameObject.SetActive(true);

        KitchenData kitchenData = KitchenManager.Instance.GetKitchenData(key);
        thumImage.sprite = kitchenData.thum;
        nameText.text = kitchenData.name;
        abilityText.text = "도토리 획득량 +" + kitchenData.abilityLv;

    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

}
