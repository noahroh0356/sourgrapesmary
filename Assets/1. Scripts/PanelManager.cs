using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public GameObject questCanvas; // 실제 내용 패널
    public GameObject questIcon; // 실제 내용 패널
    public GameObject backgroundCloseArea; // 투명 버튼

    void Awake()
    {
        Instance = this;
    }

    public void OpenPanel()
    {
        questCanvas.SetActive(true);
        backgroundCloseArea.SetActive(true);
        backgroundCloseArea.transform.SetAsLastSibling(); // 항상 위로 보내기

    }

    public void ClosePanel()
    {
        questIcon.SetActive(true);
        questCanvas.SetActive(false);
        backgroundCloseArea.SetActive(false);
    }




}
