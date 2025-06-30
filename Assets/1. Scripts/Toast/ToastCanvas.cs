using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToastCanvas : MonoBehaviour
{

    public static ToastCanvas Instance;

    public TMP_Text toastText;
    public GameObject toast;

    public void Awake()
    {
        Instance = this;
    }

    public void ShowToast(string text)
    {
        toast.gameObject.SetActive(true);
        toastText.text = text;
        CancelInvoke();
        Invoke("OffToast", 2);

    }

    public void OffToast()
    {
        toast.SetActive(false);
    }

}
