using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FurnitureCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text autoSpawnAcon;
    public TMP_Text autoSpawnSec;

    public TMP_Text abilityText;

    private static FurnitureCanvas instance; // 정적 변수
    public static FurnitureCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<FurnitureCanvas>(FindObjectsInactive.Include);

            return instance;
        }
    }



    public void Open(string key)
    {
        gameObject.SetActive(true);

        FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(key);
        thumImage.sprite = furnitureData.thum;
        //nameText.text = furnitureData.name;
        //abilityText.text = "도토리 획득량 +" + furnitureData.abilityLv;

        FurnitureDetail furnitureDetail = FurnitureManager.Instance.GetFurnitureDetail(furnitureData.tableKey);
        nameText.text = furnitureDetail.name;
        description.text = furnitureDetail.description;
        autoSpawnAcon.text = furnitureDetail.autoSpawnAcon.ToString();
        autoSpawnSec.text = furnitureDetail.autoSpawnSec.ToString();



    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

}
