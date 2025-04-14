using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{

    Transform cameraTr;
    public static CameraManager Instance;

    public void Awake()
    {
        cameraTr = Camera.main.transform;
        Instance = this;
    }

    public void MoveTo(string areaName)
    {
        if (areaName == "Restaurant")
        {
            KitchenManager.Instance.EndArea();
            StartCoroutine(CoMoveTo(RestaurantManager.Instance.center.position, areaName));
        }

        else if (areaName == "Kitchen")
        {
            RestaurantManager.Instance.EndArea();
            StartCoroutine(CoMoveTo(KitchenManager.Instance.center.position, areaName));

        }

    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        //Input.mousePosition
    //    }

    //    else if (Input.GetMouseButtonUp(0))
    //    {
    //    }
    //}
    //현재 지역 확인, 무슨 지역에서 무슨 지역으로 이동할지 설정 


    IEnumerator CoMoveTo(Vector2 targetPoint,string areaName)
    {
        while (true)
        {

            if (Vector2.Distance(cameraTr.position, targetPoint) < 0.1f)
            {
                break;
            }
            cameraTr.position = Vector2.MoveTowards(cameraTr.position, targetPoint, Time.deltaTime * 30);

            yield return null;
        }
        cameraTr.position = targetPoint;

        if (areaName == "Restaurant")
        {
            RestaurantManager.Instance.StartArea();//각 함수에 버튼 온오프 넣기

        }
        else if (areaName == "Kitchen")
        {
            KitchenManager.Instance.StartArea();//각 함수에 버튼 온오프 넣기

        }

    }

}
