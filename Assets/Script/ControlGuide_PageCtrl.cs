using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGuide_PageCtrl : MonoBehaviour
{
    public List<GameObject> Page;

    public GameObject OnPage;

    public GameObject UpPage;
    public GameObject DownPage;

    public int Page_Num;

    // Start is called before the first frame update
    void Start()
    {
        Page[0].SetActive(true);

        for(int i = 1; i < Page.Count; i++)
            Page[i].SetActive(false);

        UpPage.SetActive(false);
        DownPage.SetActive(true);

        Page_Num = 1;

        OnPage = Page[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPage(int Add_Page)
    {
        Page_Num += Add_Page;

        if (Page_Num >= Page.Count)
        {
            Page_Num = Page.Count;

            UpPage.SetActive(true);
            DownPage.SetActive(false);
        }
        else if (Page_Num <= 1)
        {
            Page_Num = 1;

            UpPage.SetActive(false);
            DownPage.SetActive(true);
        }

        OnPage.SetActive(false);
        OnPage = Page[Page_Num - 1];
        Page[Page_Num - 1].SetActive(true);


    }
}
