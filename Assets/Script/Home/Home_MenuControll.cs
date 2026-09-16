using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home_MenuControll : MonoBehaviour
{
    public GameObject Reel;
    public GameObject Reel_Background;

    public GameObject HomeList;
    private RectTransform ButtonList_Rect;

    public GameObject NoticeText;

    public GameObject TouchArea;
    

    // Start is called before the first frame update
    void Start()
    {
        ButtonList_Rect = HomeList.GetComponent<RectTransform>();
        ButtonList_Rect.localScale = new Vector2(1f, 0f);

        HomeList.SetActive(false);
        StartCoroutine(Notive_BreaveAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchReel()
    {
        StartCoroutine(ReelOpen_Anim());
    }

    float PanelMax_Scale = 0.9f;
    IEnumerator ReelOpen_Anim()
    {
        bool IsOpen = !HomeList.activeSelf;

        if (IsOpen)
        {
            HomeList.SetActive(true);
            NoticeText.SetActive(false);
        }

        float duration = 0.1f;
        float currentTime = 0f;

        float Start_Scale = IsOpen ? 0f : PanelMax_Scale;
        float End_Scale = IsOpen ? PanelMax_Scale : 0f;
        float elaspValue = 0f;

        ButtonList_Rect.localScale = Vector2.one * Start_Scale;// new Vector2(0.8f, Start_Scale);

        while (currentTime <duration)
        {
            currentTime += Time.deltaTime;
            elaspValue = Mathf.Lerp(Start_Scale, End_Scale, currentTime / duration);

            ButtonList_Rect.localScale = Vector2.one * elaspValue;// new Vector2(0.8f, elaspValue);

            yield return null;
        }

        ButtonList_Rect.localScale = Vector2.one * End_Scale;//new Vector2(0.8f, End_Scale);

        if (!IsOpen)
        {
            HomeList.SetActive(false);
            NoticeText.SetActive(true);
            StartCoroutine(Notive_BreaveAnim());
        }
    }

    IEnumerator Notive_BreaveAnim()
    {
        Image Notice_Img = NoticeText.GetComponent<Image>();
        Color Notive_Color = Notice_Img.color;

        float Breave_duration = 1f;
        float Stop_duration = 0.2f;

        int BreaveStep = 0;
        float TimerCount = 0f;

        while (NoticeText.activeSelf)
        {
            TimerCount += Time.deltaTime;
            if (BreaveStep == 0)
            {
                Notive_Color.a = Mathf.Lerp(1f, 0f, TimerCount / Breave_duration);
                Notice_Img.color = Notive_Color;

                if (TimerCount >= Breave_duration)
                {
                    TimerCount = 0;
                    BreaveStep++;
                }
            }
            else if (BreaveStep == 1 || BreaveStep == 3)
            {
                if (TimerCount >= Stop_duration)
                {
                    TimerCount = 0;

                    if (BreaveStep == 3)
                        BreaveStep = 0;
                    else
                        BreaveStep++;
                }
            }
            else if (BreaveStep == 2)
            {
                Notive_Color.a = Mathf.Lerp(0f, 1f, TimerCount / Breave_duration);
                Notice_Img.color = Notive_Color;

                if (TimerCount >= Breave_duration)
                {
                    TimerCount = 0;
                    BreaveStep++;
                }
            }

            yield return null;
        }
    }
}
