using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text abilityText;

    public static WorkerCanvas instance; // 정적 변수
    public static WorkerCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<WorkerCanvas>(FindObjectsInactive.Include);

            return instance;
        }
    }



    public void Open(string key)
    {
        Debug.Log("WorkerCanvas Open" + key);
        gameObject.SetActive(true);

        FoxData foxData = FoxManager.Instance.GetFoxData(key);
        thumImage.sprite = foxData.thum;
        nameText.text = foxData.name;
        //abilityText.text = "도토리 획득량 +" + foxData.abilityLv;

    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

}
