using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Select : MonoBehaviour
{
    public static Map_Select map_select;
    
    public GameObject MapReel;
    public GameObject Reel_Mask;

    public string VRScene_Name;
    private Animator Reel_anim;

    public Sprite NoSelete;
    public Sprite OnSelete;

    public GameObject Select_Locztion;

    public Sprite[] SceneSP;
    public GameObject Photo;

    // Start is called before the first frame update
    void Start()
    {
        Reel_anim = MapReel.GetComponent<Animator>();
    }

    void Awake()
    {
        map_select = this;
    }

    //儲存該地點的名稱
    public void GetScene(string VRSc_name)
    {
        VRScene_Name = VRSc_name;
        MapReel.SetActive(true);
        Reel_anim.SetBool("Open", true);

        if (Enum.IsDefined(typeof(GuideMap_Name), VRSc_name))
        {
            GuideMap_Name mapName = (GuideMap_Name)Enum.Parse(typeof(GuideMap_Name), VRSc_name);
            int number = (int)mapName;
            Photo.GetComponent<Image>().sprite = SceneSP[number];
        }

        StartCoroutine(AutoMode_SwitchScene(true));
    }

    public void Switch_ButtonImg(GameObject New_Button)
    {
        if (Select_Locztion != null)
            Select_Locztion.GetComponent<Image>().sprite = NoSelete;

        New_Button.GetComponent<Image>().sprite = OnSelete;
        Select_Locztion = New_Button;
    }

    void Destory()
    {
        if (Select_Locztion != null)
            Select_Locztion.GetComponent<Image>().sprite = NoSelete;
    }

    IEnumerator AutoMode_SwitchScene(bool IsStart)
    {
        float Start_Alpha = IsStart ? 0f : 59f /255f;
        float End_Alpha = IsStart ? 59f / 255f : 0f;

        if (IsStart)
            Reel_Mask.SetActive(true);

        Image Mask_Img = Reel_Mask.GetComponent<Image>();

        Color Mask_Color = Mask_Img.color;
        Mask_Color.a = Start_Alpha;
        Mask_Img.color = Mask_Color;

        float duration = 1f;
        float elaspTime = 0;

        while (elaspTime < duration)
        {
            elaspTime += Time.deltaTime;
            Mask_Color.a = Mathf.Lerp(Start_Alpha, End_Alpha, elaspTime / duration);

            Mask_Img.color = Mask_Color;
            yield return null;
        }

        Mask_Color.a = End_Alpha;
        Mask_Img.color = Mask_Color;

        if (!IsStart)
        {
            Reel_Mask.SetActive(false);
            Select_Locztion.GetComponent<Image>().sprite = NoSelete;
            Select_Locztion = null;
        }
    }

    public void CloseReel()
    {
        VRScene_Name = "";
        Reel_anim.SetBool("Open", false);

        //StartCoroutine(WaitingReel_Close());
        StartCoroutine(AutoMode_SwitchScene(false));
    }

    IEnumerator WaitingReel_Close()
    {
        Reel_anim.SetBool("Open", false);

        AnimatorStateInfo stateInfo;

        while (true)
        {
            stateInfo = Reel_anim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Close") && stateInfo.normalizedTime >= 1f && !Reel_anim.IsInTransition(0))
            {
                break;
            }

            yield return null;
        }

        MapReel.SetActive(false);
    }
}
