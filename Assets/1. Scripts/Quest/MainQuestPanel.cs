using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class MainQuestPanel : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text processText;
    public TMP_Text reward1Text;
    public TMP_Text reward2Text;
    MainQuestData mainQuestdata;
    public Button questButton; // 버튼을 수동으로 할당할 변수

    public AudioSource clickSound;


    private bool isQuestCompleted = false;
    private bool isInitialized = false; // 초기화 여부 플래그


    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnPanelClicked);
        isInitialized = true; // 초기화 완료

    }


    public void CompleteQuest()
    {
        titleText.text = "퀘스트 완료! (터치하세요)";
        isQuestCompleted = true;
    }

    public void OnPanelClicked()
    {
        if (isInitialized && isQuestCompleted)
        {
            clickSound.Play();

            Debug.Log("가챠코인 지급!");
            MainQuestManager.Instance.CompleteCurrentQuest();
            //User.Instance.AddGatchaCoin(1); // 가챠코인 지급
            //MainQuestManager.Instance.StartQuest(); // 다음 퀘스트 시작
        }
    }



    public void StartQuest(MainQuestData data)
    {

        mainQuestdata = data;
        titleText.text = data.title.ToString();
        isQuestCompleted = false;
        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
        reward1Text.text = "+"+data.aconReward1.ToString();
        reward2Text.text = "+"+data.gatchaCoinReward2.ToString();
    }

    public void UpdatePanel()
    {
        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
    }


}
