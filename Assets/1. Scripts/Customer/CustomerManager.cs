using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public Transform enterance; // 입구 위치
    public CustomerData[] customerDatas;
    public List<UserCustomer> userCustomers = new List<UserCustomer>(); // 손님의 종 리스트
    public List<Customer> customerPrefabs = new List<Customer>(); // 손님의 종 리스트
    public List<Customer> waitingCustomers = new List<Customer>(); //대기 손님

    public RestaurantManager restaurantManager;
    public static CustomerManager Instance;

    public FatJinsangCustomer fatThiefPrefab;
    public JinsangCustomer thiefPrefab;
    public PinkPanda pinkPandaPrefab; // PinkPanda 프리팹 연결
    public KickOutCustomer kickoutPrefab;

    float minsec=5f;
    float maxsec=25f;

    public bool isFastEnterActive = false; // 패스트 엔터 활성화 여부


    public Image inviteGageBar;

    //public Customer normalCustomerPrefab;
    //public Customer obCustomerPrefab;


    private void Awake()
    {
        Instance = this; //this 자신의 객체 = User 스크립
        InvokeRepeating("EnterJinsang", Random.Range(30f, 60f), Random.Range(90f, 150f));
        InvokeRepeating("EnterFatJinsang", Random.Range(40f, 90f), Random.Range(120f, 180f));
        InvokeRepeating("EnterKickoutJinsang", Random.Range(70f, 120f), Random.Range(150f, 210f));



        //Invoke("EnterCustomer", Random.Range(3f, 15f));
        //InvokeRepeating("CallEnterCustomer", 0f, 1f); // 0초 후 시작, 매 1초마다 호출

        ////Invoke("EnterCustomer", Random.Range(minsec, maxsec));
        //CancelInvoke("CallEnterCustomer");
        InvokeRepeating("CallEnterCustomer", 0f, 1f);
    }
    private void CallEnterCustomer()
    {
        // 현재 대기 중인 손님 수가 최대치를 넘지 않았는지 확인
        if (waitingCustomers.Count <= 5)
        {
            // 확률적으로 손님 생성 (호출 빈도를 조절하여 간접적으로 생성 확률 조절)
            float chance = Random.Range(0f, 1f);
            float spawnProbability = 1f / Random.Range(minsec, maxsec); // 현재 간격에 따른 확률 조정

            if (chance <= spawnProbability)
            {
                EnterCustomer();
            }
        }
    }

    public void FastEnter(bool enable)
    {
        isFastEnterActive = enable;
        if (isFastEnterActive)
        {
            minsec = 1f;
            maxsec = 3f;
            Debug.Log("패스트 엔터 활성화!");
        }
        else
        {
            minsec = 3f;
            maxsec = 15f;
            Debug.Log("패스트 엔터 비활성화!");
        }

        // 현재 예약된 EnterCustomer 함수를 취소하고 새로운 간격으로 다시 예약
        //CancelInvoke("EnterCustomer");
        //Invoke("EnterCustomer", Random.Range(minsec, maxsec));
    }



    public void EnterJinsang()
    {
        JinsangCustomer jinsangCustomer = Instantiate(thiefPrefab);
        jinsangCustomer.Enter();

    }


    public void EnterFatJinsang()
    {

        FatJinsangCustomer fatJinsangCustomer = Instantiate(fatThiefPrefab);
         fatJinsangCustomer.Enter();
    }

    public void EnterKickoutJinsang()
    {

        KickOutCustomer kCustomer = Instantiate(kickoutPrefab);
        kCustomer.Enter();

    }



    public void EnterCustomer()
    {
        if (waitingCustomers.Count > 5)
        {
            Invoke("EnterCustomer", Random.Range(minsec, maxsec));

            return;
        }
        MainQuestManager.Instance.DoQuest(MainQuestType.CallCustomer);
        //int randomIndex = Random.Range(0, userCustomers.Count);
        //Customer newCustomerObject = Instantiate(userCustomers[randomIndex], enterance.position, Quaternion.identity);

        List<UserCustomer> activeUserCustomers = User.Instance.userData.userCustomers.FindAll(customer => customer.open);
        if (activeUserCustomers.Count > 0)
        {
            int randomIndex = Random.Range(0, activeUserCustomers.Count);
            string customerKey = activeUserCustomers[randomIndex].key;

            // customerKey에 해당하는 Customer 프리팹을 찾습니다.
            Customer customerPrefab = FindCustomerPrefabByKey(customerKey);

            if (customerPrefab != null)
            {
                Customer newCustomerObject = Instantiate(customerPrefab, enterance.position, Quaternion.identity);
                Customer newCustomer = newCustomerObject.GetComponent<Customer>();
                newCustomer.Enter();
                waitingCustomers.Add(newCustomer);
                SortCustomer();
                //AssignCustomerToTable();
            }
            else
            {
                Debug.LogError("Customer prefab not found for key: " + customerKey);
            }
        }
        else
        {
            Debug.LogWarning("No active customers available.");
        }
        //int randomIndex = Random.Range(0, customerPrefabs.Count);
        //Customer newCustomerObject = Instantiate(customerPrefabs[randomIndex], enterance.position, Quaternion.identity);

        //Customer newCustomer = newCustomerObject.GetComponent<Customer>();
        //newCustomer.Enter();
        //waitingCustomers.Add(newCustomer);
        //SortCustomer();
        //AssignCustomerToTable();

    }
    public Customer FindCustomerPrefabByKey(string key)
        {
        // key에 해당하는 CustomerData를 찾습니다.
        CustomerData foundCustomerData = null;
        for (int i = 0; i < customerDatas.Length; i++)
        {
            if (customerDatas[i].key == key)
            {
                foundCustomerData = customerDatas[i];
                break;
            }
        }

        if (foundCustomerData != null)
        {
            // foundCustomerData를 기반으로 Customer 프리팹을 찾습니다.
            for (int i = 0; i < customerPrefabs.Count; i++)
            {
                // Customer 프리팹에 CustomerData가 없으므로, 다른 방법으로 비교해야 합니다.
                // 예를 들어, Customer 프리팹의 이름이나 태그를 사용하여 비교할 수 있습니다.
                // 여기서는 Customer 프리팹의 이름에 CustomerData의 key가 포함되어 있다고 가정합니다.
                if (customerPrefabs[i].name.Contains(foundCustomerData.key))
                {
                    return customerPrefabs[i];
                }
            }
        }

        return null;
    }

    private void Update()
    {
        if (waitingCustomers.Count > 0)
        {
            AssignCustomerToTable(); // 매 프레임마다 빈 테이블 확인 및 배정
        }
        PinkPanda existingPinkPanda = FindObjectOfType<PinkPanda>();

        if (existingPinkPanda != null && !isFastEnterActive)
        {
            FastEnter(true);
        }
        else if (existingPinkPanda == null && isFastEnterActive)
        {
            FastEnter(false);
        }
    }

    // 빈 테이블 찾기 (RestaurantManager와 연동)
    private Table GetRandomAvailableTable()
    {
        return restaurantManager.GetRandomAvailableTable();
    }

    public void AssignCustomerToTable() 
    {
        if (waitingCustomers.Count > 0)
        {
            Table table = GetRandomAvailableTable();
            if (table != null)
            {
                Customer customer = waitingCustomers[0];
                customer.SetTarget(table);
                waitingCustomers.RemoveAt(0);
            }
        }
    }

    void SortCustomer()
    {
        for (int i = 0; i < waitingCustomers.Count; i++)
        {
            float randomX = Random.Range(-0.3f, 0.3f);
            float plusY = i*0.6f;
            waitingCustomers[i].transform.position = (Vector2)enterance.position + new Vector2(randomX, plusY);
            // **기존 애들 포지션도 바뀌는데 왜 그?
        }

    }
}

[System.Serializable]
public class CustomerData
{
    public string key;
    public int order;
    public Sprite thum;


}
