using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using UnityEngine;

public class MainQuestManager : MonoBehaviour
{
    public static MainQuestManager Instance;
    public MainQuestData[] mainQuestDatas;
    public UserMainQuest userMainQuest;

    public string[] purchaseFurnitureQuestKeys;
    public string[] purchaseKitchenQuestKeys;

    public MainQuestPanel mainQuestPanel;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        TextAsset tAsset = Resources.Load<TextAsset>("Json/MainQuest");
        JSONObject jObj = JSONObject.Parse(tAsset.text);

        JSONArray arr = jObj.GetArray("MyQuest");
        mainQuestDatas = new MainQuestData[arr.Length];

        for (int i = 0; i < arr.Length; i++)
        {
            mainQuestDatas[i] = new MainQuestData();

            mainQuestDatas[i].mainQuestType = System.Enum.Parse<MainQuestType>(arr[i].Obj.GetString("MainQuestType"));
            mainQuestDatas[i].goal = int.Parse(arr[i].Obj.GetString("Goal"));
            mainQuestDatas[i].goalString = arr[i].Obj.GetString("GoalString");
            mainQuestDatas[i].title = arr[i].Obj.GetString("Title");
            mainQuestDatas[i].aconReward1 = int.Parse(arr[i].Obj.GetString("AconReward1"));
            mainQuestDatas[i].gatchaCoinReward2 = int.Parse(arr[i].Obj.GetString("GatchaCoinReward2"));
        }

        userMainQuest = SaveMgr.LoadData<UserMainQuest>("UserMainQuest");

        if (userMainQuest == null)
        {
            userMainQuest = new UserMainQuest
            {
                curQuestIndex = 0,
                process = 0,
                processing = true
            };
            Debug.Log("[새로운 UserMainQuest 생성]");
            StartQuest();
        }
        else
        {
            Debug.Log($"[UserMainQuest 로드] 인덱스: {userMainQuest.curQuestIndex}, 타입: {userMainQuest.mainQuestType}, 진행도: {userMainQuest.process}, 진행 중: {userMainQuest.processing}");
            SafeCallToPanel(panel => panel.StartQuest(mainQuestDatas[userMainQuest.curQuestIndex]));
            CheckClear();
        }

        SaveMgr.SaveData("UserMainQuest", userMainQuest);
    }

    public void StartQuest()
    {
        if (mainQuestDatas.Length <= userMainQuest.curQuestIndex)
            return;

        MainQuestData data = mainQuestDatas[userMainQuest.curQuestIndex];
        userMainQuest.mainQuestType = data.mainQuestType;


        userMainQuest.process = 0;
        userMainQuest.processing = true;
        SaveMgr.SaveData("UserMainQuest", userMainQuest);

        SafeCallToPanel(panel => panel.StartQuest(data));
        CheckClear();
    }

    public void DoQuest(MainQuestType type)
    {
        Debug.Log("DoQuest 호출, type: " + type + ", userMainQuest.mainQuestType: " + userMainQuest.mainQuestType);

        if (userMainQuest.mainQuestType == type)
        {
            MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

            if (type == MainQuestType.PurchaseKitchen ||
                type == MainQuestType.PurchaseFurniture ||
                type == MainQuestType.PurchaseFox)
            {
                userMainQuest.process = 1;
                CheckClear();
                SafeCallToPanel(panel => panel.UpdatePanel());
            }
            else if (userMainQuest.process < curQuestData.goal)
            {
                userMainQuest.process++;
                SaveMgr.SaveData("UserMainQuest", userMainQuest);
                CheckClear();
                SafeCallToPanel(panel => panel.UpdatePanel());
            }
        }
    }

    public bool CheckClear()
    {
        if (!userMainQuest.processing)
            return false;

        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

        if (userMainQuest.mainQuestType == MainQuestType.PurchaseFurniture)
        {
            string key = curQuestData.goalString;
            UserFurniture data = User.Instance.GetUserFurniture(key);
            if (data != null && data.purchased)
            {
                SafeCallToPanel(panel => panel.CompleteQuest());
                return true;
            }
        }
        else if (userMainQuest.mainQuestType == MainQuestType.PurchaseFox)
        {
            string key = curQuestData.goalString;
            UserFox data = User.Instance.GetUserFox(key);
            if (data != null && data.purchased)
            {
                SafeCallToPanel(panel => panel.CompleteQuest());
                return true;
            }
        }
        else if (userMainQuest.mainQuestType == MainQuestType.PurchaseKitchen)
        {
            string key = curQuestData.goalString;
            UserKitchen data = User.Instance.GetUserKitchen(key);
            if (data != null && data.purchased)
            {
                SafeCallToPanel(panel => panel.CompleteQuest());
                return true;
            }
        }
        else
        {
            if (userMainQuest.process >= curQuestData.goal)
            {
                SafeCallToPanel(panel => panel.CompleteQuest());
                return true;
            }
        }

        return false;
    }

    public void CompleteCurrentQuest()
    {
        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

        Debug.Log("퀘스트 완료!");
        userMainQuest.curQuestIndex++;
        userMainQuest.processing = false;
        User.Instance.AddGatchaCoin(curQuestData.gatchaCoinReward2);
        User.Instance.AddCoin(curQuestData.aconReward1);

        SaveMgr.SaveData("UserMainQuest", userMainQuest);
        StartQuest();
    }

    private void SafeCallToPanel(System.Action<MainQuestPanel> callback)
    {
        callback(mainQuestPanel);
    }

    public MainQuestData GetMainQuestData(MainQuestType type)
    {
        foreach (var data in mainQuestDatas)
        {
            if (data.mainQuestType == type)
                return data;
        }
        return null;
    }
}

[System.Serializable]
public class UserMainQuest
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
    public int aconReward1;
    public int gatchaCoinReward2;


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
    TakeOrder,
    PickUpAcon,
    PurchaseFurniture,
    PurchaseKitchen,
    PlayGatcha,
    PurchaseFox,
    UpgradeTipBox
}
