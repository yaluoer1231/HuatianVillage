using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_Scene_Jump : MonoBehaviour
{
    private Scene_Num scene_num;

    public GameObject[] VR_Scenes;
    public List<GameObject> All_Arrow;

    public GameObject Select_VRScene;
    public GameObject Local_VRScene;
    public GameObject Home_Scene;
    public GameObject Camera_UI;
    public GameObject Return_UI;

    public GameObject Player;
    public Transform Camera_rect;
    
    public float Timer = 0;
    private bool Time_Start = false;

    public GameObject GuideList;

    // Start is called before the first frame update
    void Start()
    {
        //確認當下場景編號
        scene_num = (Scene_Num)SceneManager.GetActiveScene().buildIndex;
        //儲存選擇的景點名稱
        string SelectViewPoint = Map_Select.map_select != null? Map_Select.map_select.VRScene_Name: GuideMap_Name.Home.ToString();

        if (Enum.IsDefined(typeof(GuideMap_Name), SelectViewPoint))
        {
            GuideMap_Name sceneNum = (GuideMap_Name)Enum.Parse(typeof(GuideMap_Name), SelectViewPoint);
            Local_VRScene = VR_Scenes[(int)sceneNum];
        }
        else
            Local_VRScene = VR_Scenes[0];
        
        //將人物視角移動到選擇的景點位置
        Player.transform.position = Local_VRScene.transform.position;
        Vector3 new_rotation = new Vector3(0, 90, 0);
        Player.transform.eulerAngles = new_rotation;

        if (scene_num == Scene_Num.VR_Mode)
            IsHome();
    }

    public void SceneJump(GameObject VR_Sc)
    {
        Select_VRScene = VR_Sc;
        if (scene_num == Scene_Num.VR_Mode)
            Time_Start = true;
    }

    public void StopJump()
    {
        Select_VRScene = null;
        if (scene_num == Scene_Num.VR_Mode)
            Time_Start = false;
    }

    void Update()
    {
        if (scene_num == Scene_Num.No_VR_Mode)
        {
            if (Select_VRScene != null && Input.GetMouseButton((int)Mouse_Button.Mouse_Left))
            {
                Player.transform.position = Select_VRScene.transform.position;
                PlatRotationSetting();

                Local_VRScene = Select_VRScene;
                Select_VRScene = null;

                IsHome();
            }
        }
        else
        {
            if (Time_Start == true)
                Timer += Time.deltaTime;
            else
                Timer = 0;

            if (Timer > 1)
            {
                Player.transform.position = Select_VRScene.transform.position;
                PlatRotationSetting();

                Local_VRScene = Select_VRScene;
                Select_VRScene = null;

                Timer = 0;
                IsHome();
            }
        }

    }

    public void PlatRotationSetting()
    {
        //取得選取的位置的名稱
        var SwitchSceneName = Select_VRScene.GetComponent<VR_Scene_Info>().SceneName;
        //抓出當前場景前往其他地方的資訊
        var NowSceneInfo = Local_VRScene.GetComponent<VR_Scene_Info>().Other_SceneInfo;

        //確認是否可以旋轉，並記錄角度
        bool HasRotation = false;
        float rotation = 0f;

        for (int i = 0; i < NowSceneInfo.Count; i++)
        {
            if (NowSceneInfo[i].NextScene == SwitchSceneName)
            {
                rotation = NowSceneInfo[i].angle;
                HasRotation = true;
                break;
            }
        }

        if (HasRotation)
        {
            if (scene_num == Scene_Num.No_VR_Mode)
                Player.transform.eulerAngles = new Vector3(0, rotation, 0);
            else
            {
                //抓到相機目前的旋轉角度(世界角度)
                Vector3 Camera_rotation = Camera_rect.eulerAngles;
                //Player本身已經轉的角度
                float Player_RotatoY = Player.transform.eulerAngles.y;
                //計算還需要轉幾度
                float New_Rotato = rotation - Camera_rotation.y;

                Player.transform.eulerAngles = new Vector3(0, Mathf.Repeat(Player_RotatoY + New_Rotato, 360f), 0);
            }
        }
    }


    public void IsHome()
    {
        if (Player.transform.position == Home_Scene.transform.position)
        {
            Camera_UI.SetActive(true);
            Return_UI.SetActive(false);
        }
        else
        {
            Camera_UI.SetActive(false);
            Return_UI.SetActive(true);
        }
        Time_Start = false;
    }

    public void SelectJump(int Location_Num)
    {
        if (Location_Num < 0)
            Player.transform.position = Home_Scene.transform.position;
        else
            Player.transform.position = VR_Scenes[Location_Num].transform.position;
    }
}

public enum GuideMap_Name
{
    Home,
    WestTrail_01,
    WestTrail_02,
    ToWestBridge,
    WestTrail_03,
    LargeTree,
    Plaza,
    FeilongTrail_Start,
    WoodlandTrail_01,
    WoodlandTrail_02,
    SteppedBridge,
    TheIsland,
    ObservationDeck_01,
    WoodlandTrail_03,
    WoodlandTrail_04,
    FeilongTrail_Exit,
    ArchBridge,
    ObservationDeck_02
}

public enum Scene_MoveType
{
    Next,
    Prev,
    Middle,
    Prev_Left,
    Prev_Right,
}


[System.Serializable]
public class SceneRoute
{
    public GuideMap_Name NextScene;
    public Scene_MoveType moveType;

    public float angle;
}