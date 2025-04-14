using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FurnitureProductPanel : MonoBehaviour
{

    public string key;
    public TextMeshProUGUI PorUtext;
    public TextMeshProUGUI Pricetext;
    public Image tableImage;


    FurnitureData furnitureData;
    

    public void Start()
    {
        FurnitureManager mgr = FindObjectOfType<FurnitureManager>();
        furnitureData = mgr.GetFurnitureData(key);
        Pricetext.text = furnitureData.price.ToString();

        //버튼 컴포넌트를 동적으로(런타임 실행 중) 추가하기
        Button button = gameObject.AddComponent< Button > ();
        button.onClick.AddListener(OnClickedOpenFurniture);
    }

    public void OnClickedOpenFurniture()
    {
        Debug.Log("furnitureProductPanel OnClickedOpenFurniture" + key);
        FurnitureCanvas.Instance.Open(key);

    }


    public void OnClickPurchased()
    {

        Debug.Log(key + "구매시도");
        if (User.Instance.userData.coin < furnitureData.price)
        {
            Debug.Log(key + "재화부족");
            return;
        }

        else
        {
            User.Instance.AddFurniture(key);
            FurnitureManager.Instance.PurchaseFurniture(key);


            MainQuestManager.Instance.DoQuest(MainQuestType.PurchaseFurniture);

            GetComponentInParent<TablePlaceProducts>().UpdateTablePlace();
            User.Instance.userData.coin -= furnitureData.price;
            //User.Instance.UpdateCoinText();
            FurnitureManager.Instance.UpdateFurniture();
            PorUtext.text = "업그레이드";

        }
    }
}