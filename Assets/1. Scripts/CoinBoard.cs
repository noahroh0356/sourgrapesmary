using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CoinBoard : MonoBehaviour
{

    public TMP_Text coinText;
    public TMP_Text coinEffectText;
    public RectTransform startPoint;

    public static CoinBoard Instance;


    void Awake()
    {
        Instance = this;
    }

    //public UserData userData;

    private void Update()
    {
       UpdateCoinText();
    }


    public void AddedCoin(int coin)
    {

        coinEffectText.transform.position = startPoint.position;
        coinEffectText.gameObject.SetActive(true);
        coinEffectText.text = coin.ToString(); // 얻은 코인 값 설정
        coinEffectText.transform.DOKill(); // 이전에 두트윈 기능이 동작하고 있다면 꺼라
        coinEffectText.transform.DOMove(coinEffectText.transform.position + new Vector3(0, 50, 0), 0.4f)
                    .OnComplete(() => {
                        coinEffectText.gameObject.SetActive(false);
                    });

    }


    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = User.Instance.userData.coin.ToString();
        }
        else
        {
            Debug.LogError("coinText가 null입니다.");
        }
    }

}
