using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestManager : MonoBehaviour
{
    public static MainQuestManager Instance;
    public MainQuestData[] mainQuestDatas; // 메인 퀘스트 데이터 특정 퀘스트 정보
    public UserMainQuest userMainQuest;
    //public int curQuestIndex = 0; // 퀘스트가 차례대로 나타나도록 설정=진행 중인 퀘스트
   

    public string[] purchaseFurnitureQuestKeys;
    public string[] purchaseKitchenQuestKeys;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {

        userMainQuest = SaveMgr.LoadData<UserMainQuest>("UserMainQuest");
        if (userMainQuest == null)
        {
            userMainQuest = new UserMainQuest();
            userMainQuest.curQuestIndex = 0;
            userMainQuest.process = 0; // 이걸 지금까지 진행도로 업데이트 해야하?
            userMainQuest.processing = true;
            Debug.Log("[새로운 UserMainQuest 생성]"); // 추가된 로그

            StartQuest();
        }
        else
        {
            Debug.Log($"[UserMainQuest 로드] 인덱스: {userMainQuest.curQuestIndex}, 타입: {userMainQuest.mainQuestType}, 진행도: {userMainQuest.process}, 진행 중: {userMainQuest.processing}"); // 추가된 로그

            FindObjectOfType<MainQuestPanel>().StartQuest(mainQuestDatas[userMainQuest.curQuestIndex]);
            CheckClear();

        }
        SaveMgr.SaveData("UserMainQuest", userMainQuest);
    }


    public void StartQuest()
    {
        //퀘스트가 차례대로 나타나도록 설

        if (mainQuestDatas.Length <= userMainQuest.curQuestIndex)
        {
            return;
        }

        MainQuestData data = mainQuestDatas[userMainQuest.curQuestIndex];
        //MainQuestData data = mainQuestDatas[Random.Range(0, mainQuestDatas.Length)];

        //userMainQuest = new UserMainQuest();

        userMainQuest.mainQuestType = data.mainQuestType;
        Debug.Log($"MainQuestManager StartQuest() userQuest.mainQuestType{userMainQuest.mainQuestType}");
        userMainQuest.process = 0;
        userMainQuest.processing = true;
        SaveMgr.SaveData("UserMainQuest", userMainQuest);

        //Debug.Log($"[퀘스트 시작] 인덱스: {userMainQuest.curQuestIndex}, 타입: {userMainQuest.mainQuestType}, 목표: {data.title} ({data.goalString ?? data.goal.ToString()})"); // 추가된 로그


        FindObjectOfType<MainQuestPanel>().StartQuest(data);

        CheckClear();

    }


public void DoQuest(MainQuestType type)
    {
        Debug.Log("DoQuest 호출, type: " + type + ", userMainQuest.mainQuestType: " + userMainQuest.mainQuestType);

        if (userMainQuest.mainQuestType == type)
        {
            MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex]; // <-- 이거 추가!

            if (userMainQuest.mainQuestType == MainQuestType.PurchaseKitchen ||
                userMainQuest.mainQuestType == MainQuestType.PurchaseFurniture ||
                userMainQuest.mainQuestType == MainQuestType.PurchaseFox)
            {
                //string KitchenKey = GetMainQuestData(type).goalString;
                //Debug.Log("KitchenKey: " + KitchenKey);
                userMainQuest.process=1;
                CheckClear();
                FindObjectOfType<MainQuestPanel>().UpdatePanel();
            }
            //else if (userMainQuest.process < GetMainQuestData(type).goal)
            else if (userMainQuest.process < curQuestData.goal)
                    {
                        userMainQuest.process++;
                SaveMgr.SaveData("UserMainQuest", userMainQuest);
                Debug.Log("구매 목표랑 매칭");
                CheckClear();
                FindObjectOfType<MainQuestPanel>().UpdatePanel();
            }
        }
        //if (userMainQuest.mainQuestType == type)
        //{
        //    if (userMainQuest.process < GetMainQuestData(type).goal)
        //    {
        //        userMainQuest.process++;
        //        Debug.Log("주방 구매 목표랑 매칭");
        //        CheckClear();
        //        FindObjectOfType<MainQuestPanel>().UpdatePanel();
        //    }
        //}
        //""현재 진행중인 퀘스트(=만들어야함)""가 타입과 같다면 퀘스트 진행도 1추가
    }

    public bool CheckClear() // **차례대로 클리어
    {
        if (!userMainQuest.processing) // 퀘스트가 진행 중이 아닐 경우 바로 false 반환
        {
            return false;
        }

        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex]; // <-- 인덱스 기준으로 수정!
        if (userMainQuest.mainQuestType == MainQuestType.PurchaseFurniture)
        {
            string furnitureKey = curQuestData.goalString;
            UserFurniture userFurnitureData = User.Instance.GetUserFurniture(furnitureKey);

            if (userFurnitureData != null)
            {
                if (userFurnitureData.purchased)
                {
                    FindObjectOfType<MainQuestPanel>().CompleteQuest();
                    return true;
                }
            }
            else
            {
                Debug.Log("FurnitureData 찾지 못함: " + furnitureKey);
            }

        }

        else if (userMainQuest.mainQuestType == MainQuestType.PurchaseFox)
        {
            string foxKey = curQuestData.goalString;
            UserFox userFoxData = User.Instance.GetUserFox(foxKey);

            if (userFoxData != null)
            {
                if (userFoxData.purchased)
                {
                    FindObjectOfType<MainQuestPanel>().CompleteQuest();
                    return true;
                }
            }
            else
            {
                Debug.Log("FoxData 찾지 못함: " + foxKey);
            }

        }


        else if (userMainQuest.mainQuestType == MainQuestType.PurchaseKitchen)
        {
            string KitchenKey = curQuestData.goalString;
            UserKitchen userKitchen = User.Instance.GetUserKitchen(KitchenKey);
       
            if (userKitchen != null)
            {
                if (userKitchen.purchased)
                {
                    //if (userMainQuest.process < 1)
                    //{
                    //    // 이미 완료된 경우가 아니라면 process만 증가
                    //    userMainQuest.process = 1;
                    //}
                    FindObjectOfType<MainQuestPanel>().CompleteQuest();
                    return true;
                }
            }
            else
            {
                Debug.Log("KitchenData 찾지 못함: " + KitchenKey);

            }
            
        }

        else
        {
            int goal = curQuestData.goal;
            int progress = userMainQuest.process;
            Debug.Log($"[CheckClear] 일반 퀘스트 확인: type = {userMainQuest.mainQuestType}, process = {progress}, goal = {goal}");

            if (progress >= goal) // 혹시라도 값이 넘쳐도 방어
            {
                FindObjectOfType<MainQuestPanel>().CompleteQuest();
                return true;
            }
        }
        //else
        //{
        //    if (userMainQuest.process == curQuestData.goal)
        //    {
        //        Debug.Log($"퀘스트 완료! {userMainQuest.mainQuestType}");
        //        FindObjectOfType<MainQuestPanel>().CompleteQuest();
        //        return true;
        //    }
        //}


        //}

        return false;
    }

    public void CompleteCurrentQuest()
    {
        Debug.Log("퀘스트 완료!");


        userMainQuest.curQuestIndex++;  // 다음 퀘스트로 인덱스 증가
        userMainQuest.processing = false; // 퀘스트 진행 상태 초기화
        User.Instance.AddGatchaCoin(1); // 가챠코인 지급

        SaveMgr.SaveData("UserMainQuest", userMainQuest); // 데이터 저장

        //다음 퀘스트 시작
        StartQuest();
    }
    //public bool CheckClear()
    //{
    //    MainQuestData curQuestData = null;

    //    for (int i = 0; i < mainQuestDatas.Length; i++)
    //    {
    //        if (mainQuestDatas[i].mainQuestType == userMainQuest.mainQuestType)
    //        {
    //            curQuestData = mainQuestDatas[i];
    //        }
    //    }

    //    if (userMainQuest.process >= curQuestData.goal)
    //    {
    //        return true;
    //    }

    //    return false;

    //    //현재 진행중인 퀘스트 데이
    //    //현재 진행 중인 퀘스트를 완료할 수 있으면 true아니면 false

    public MainQuestData GetMainQuestData(MainQuestType type)

    {
        for (int i = 0; i < mainQuestDatas.Length; i++)
        {
            if (mainQuestDatas[i].mainQuestType == type)
            {
                return mainQuestDatas[i];
            }
        }
        return null;

    }
        

}

[System.Serializable]
public class UserMainQuest // 현재 유저가 진행 중인 퀘스트에 대한 정보
{
    public MainQuestType mainQuestType;
    public int curQuestIndex;
    public int process;

    public bool processing;
    public int clearPurchaseFurnitureCount;
    public int clearPurchaseKitchenCount;
}


[System.Serializable]
public class MainQuestData
{
    public MainQuestType mainQuestType;
    public int goal;
    public string goalString;
    public string title;
    //public int exp;
    public string GetGoal()
    {
        if (mainQuestType == MainQuestType.PurchaseFurniture)
        {
            int idx = MainQuestManager.Instance.userMainQuest.clearPurchaseFurnitureCount;

            if (idx >= MainQuestManager.Instance.purchaseFurnitureQuestKeys.Length)
                return null;

            return MainQuestManager.Instance.purchaseFurnitureQuestKeys[idx];
        }

        else if (mainQuestType == MainQuestType.PurchaseKitchen)
        {
            int idx = MainQuestManager.Instance.userMainQuest.clearPurchaseKitchenCount;

            if (idx >= MainQuestManager.Instance.purchaseKitchenQuestKeys.Length)
                return null;

            return MainQuestManager.Instance.purchaseKitchenQuestKeys[idx];
        }

        return null;
    }

}

public enum MainQuestType
{
CallCustomer,
TakeOrder, // 설정
PickUpAcon, //설정 
PurchaseFurniture,
PurchaseKitchen,
PlayGatcha, // 설정
PurchaseFox
}
