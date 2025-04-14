using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FoxManager : MonoBehaviour
{
    public FoxData[] foxDatas; // 게임 상 존재하는 직원에 대한 게임 데이터
    public Fox[] foxes;
    public List<Fox> currentfoxes = new List<Fox>(); // 씬상에에 존재하는 직원

    public static FoxManager Instance;


    void Awake()
    {
        Instance = this;
        foxes = FindObjectsOfType<Fox>(true);
    }

    void Start()
    {
        UpdateFox();
    }

    void Update()
    {

    }

    public FoxData GetFoxData(string key)
    {
        for (int i = 0; i < foxDatas.Length; i++)

        {
            if (foxDatas[i].key == key)
            {
                return foxDatas[i];
            }
        }
        return null;

    }


    //현재 여우를 획득시 호출
    public void AddFox(string key)

   {
        Fox fox = GetFox(key);
        fox.Enter();
        currentfoxes.Add(fox);
        
        //FoxData data = GetFoxData(key);
        //Fox fox = Instantiate(data.foxPrefab);

        
        //fox.Enter();
        //foxes.Add(fox);

        //key에 해당하는 fox를 씬상에 생
    }

    public Fox GetFox(string key)

    {
        for (int i = 0; i < foxes.Length; i++)
        {

            if (foxes[i].key == key)
            {
                return foxes[i];
            }
        }
        return null;

    }

    //유저가 보유한 직원에 따라 씬상에 fox 생성하기
    public void UpdateFox()
    {
        if (User.Instance != null && User.Instance.userData != null && User.Instance.userData.userFoxes != null)
        {

            for (int i = 0; i < User.Instance.userData.userFoxes.Count; i++)
            {
                if (User.Instance.userData.userFoxes[i].purchased)
                {
                    Fox fox = GetFox(User.Instance.userData.userFoxes[i].key);
                    fox.Enter();
                    currentfoxes.Add(fox);
                }
            }
        }


    }

    public float GetFoxAbility(FoxAbilityType abilityType)
    {
        float total = 0;
        //어빌리티 타입의 능력치의 합산값을 토탈에 담기

        for (int i = 0; i < currentfoxes.Count; i++)
        {
            FoxData data = GetFoxData(currentfoxes[i].key);
            for (int j = 0; j < data.abilities.Length; j++)
            {
                if (data.abilities[j].abilityType == abilityType)
                {
                    total = +data.abilities[j].abiltyValue;
                }
            }
        }

        return total;
    }


}

[System.Serializable]

public class FoxData
{
    public string key;
    public int price;
    public Sprite thum;
    public Fox foxPrefab;

    public FoxAbility[] abilities;

}

[System.Serializable]
public class FoxAbility
{
    public FoxAbilityType abilityType;
    public float abiltyValue;

}

public enum FoxAbilityType
{

    KitchenSpeedUp,
    MoreTip,
    CustomerSpeedUp

}