using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Select : MonoBehaviour
{
    public static Map_Select map_select;

    public GameObject Select_Mode;

    public string VRScene_Name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        map_select = this;
    }

    //儲存該地點的名稱
    public void GetScene(string VRSc_name)
    {
        VRScene_Name = VRSc_name;
        Select_Mode.SetActive(true);
    }
}
