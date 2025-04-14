using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaButton : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject activePanel;
    public GatchaManager gatchaManager;

    public GameObject bgmObject; // "BGM" 오브젝트
    public GameObject gatchaBgmObject; // "gatchabgm" 오브젝트

    public void OnClickButton()
    {


        if (User.Instance.userData.gatchaCoin < 1)
        {
            Debug.Log("가챠코인부족");
            activePanel.gameObject.SetActive(false);

            return;
        }
        else
        {
            User.Instance.userData.gatchaCoin -= 1;
            activePanel.gameObject.SetActive(true);
            gatchaManager.StartGacha();

        }


        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }



        if (bgmObject != null)
        {
            bgmObject.SetActive(false);
        }

        if (gatchaBgmObject != null)
        {
            gatchaBgmObject.SetActive(true);
        }

    }

}

