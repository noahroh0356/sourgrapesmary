using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public bool isOccupied = false; // 테이블 사용 여부
    public CustomerManager customerManager; // CustomerManager 스크립트 참조
    public Transform sitPointTr;
    public string key;
    public Customer customer;
    public MenuData data;
    public Image tableMenuImage;


    public void Awake()
    {
        tableMenuImage = GetComponentInChildren<Image>(true);
        tableMenuImage.gameObject.SetActive(false);
    }

    public void Taken(Customer cus)
    {
        customer = cus;

    }


    public void OccupyTable()
    {
        isOccupied = true;
    }

    public void VacateTable()
    {
        isOccupied = false;
        customerManager.AssignCustomerToTable(); // CustomerManager에 테이블 비었음을 알림
        //대기 중인 손님이 있으면 들어
    }


    public void SetTableMenuImage(MenuData data)
    {
        this.data = data; // ** 다른애가 주문하는거랑 섞이면 안뜨는 경우가 있는 버그!
        tableMenuImage.gameObject.SetActive(true);
        tableMenuImage.sprite = data.menuImage;
        RectTransform rectTransform = tableMenuImage.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0.5f); // 테이블의 위쪽으로 이동
        tableMenuImage.enabled = true; // 이미지 활성화
        Debug.Log("SetTableMenuImage3");
        StartCoroutine(OffTableMenuImage(data));
    }

    IEnumerator OffTableMenuImage(MenuData data)
    {
       yield return new WaitForSeconds(5f);
      tableMenuImage.gameObject.SetActive(false);
    }


}