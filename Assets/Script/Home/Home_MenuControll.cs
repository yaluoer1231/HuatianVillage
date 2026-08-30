using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home_MenuControll : MonoBehaviour
{
    public GameObject Reel;
    public GameObject Reel_Background;

    public Animator Reel_anim;
    public Animator Reel_Background_anim;

    // Start is called before the first frame update
    void Start()
    {
        Reel_anim.GetComponent<Animator>();
        Reel_Background_anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenReel()
    {
        Reel.SetActive(true);
        Reel_Background.SetActive(true);
        
        Reel_anim.SetBool("Open", true);
        Reel_Background_anim.SetBool("Open", true);
    }
    public void CloseReel()
    {
        Reel_anim.SetBool("Open", false);
        Reel_Background_anim.SetBool("Open", false);
    }
}
