using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class No_VR_RList_Control : MonoBehaviour
{
    public GameObject Right_List;
    public GameObject Open_Button;

    public Animator right_list_anim;

    // Start is called before the first frame update
    void Start()
    {
        right_list_anim.GetComponent<Animator>();
    }

    public void Open_list()
    {
        Open_Button.SetActive(false);
        right_list_anim.SetBool("List_Open", true);
    }

    public void Close_list()
    {
        right_list_anim.SetBool("List_Open", false);
    }
}
