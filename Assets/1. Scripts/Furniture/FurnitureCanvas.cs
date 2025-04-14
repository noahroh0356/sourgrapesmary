using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FurnitureCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text abilityText;

    private static FurnitureCanvas instance; // 정적 변수
    public static FurnitureCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<FurnitureCanvas>(FindObjectsInactive.Include);

            return Instance;
        }
    }



    public void Open(string key)
    {
        Debug.Log("FurnitureCanvas Open" + key);
        gameObject.SetActive(true);

        FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(key);
        thumImage.sprite = furnitureData.thum;
        nameText.text = furnitureData.name;
        abilityText.text = "도토리 획득량 +" + furnitureData.abilityLv;

    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

}
