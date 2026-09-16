using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VR_Scene_Jump : MonoBehaviour
{
    [SerializeField] Scenes_Switch ScenesSwitch;
    bool IsReturnPage = false;

    /// <summary> 當前場景編號 </summary>
    private Scene_Num scene_num;

    /// <summary> 所有場景物件 </summary>
    public GameObject[] VR_Scenes;
    /// <summary> 所有箭頭物件 </summary>
    public List<GameObject> All_Arrow;

    public List<GameObject> MapSelect_Btn;
    public GameObject OnSelect_Btn;

    public Sprite NoSelete;
    public Sprite OnSelete;

    /// <summary> 選擇的場景 </summary>
    public GameObject Select_VRScene;
    /// <summary> 當前的場景 </summary>
    public GameObject Local_VRScene;
    /// <summary> 預設的起始場景 </summary>
    public GameObject Home_Scene;
    /// <summary> 跟著相機旋轉的UI_Canvas(VR模式下無法點擊) </summary>
    public GameObject Camera_UI;
    /// <summary> 回到選擇畫面的的UI_Canvas(觸控模式時不顯示) </summary>
    public GameObject Return_UI;
    /// <summary> 回到選擇畫面的的UI_Canvas(觸控模式時不顯示) </summary>
    public GameObject StopOrPlay_UI;

    [SerializeField] Sprite Play_Btn;
    [SerializeField] Sprite Stop_Btn;

    /// <summary> 玩家物件 </summary>
    public GameObject Player;
    /// <summary> 相機的位置訊息 </summary>
    public Transform Camera_rect;

    /// <summary> 計時器 </summary>
    public float Timer = 0;
    /// <summary> 是否開始計時 </summary>
    private bool Time_Start = false;

    /// <summary> 觸控模式用的地圖清單 </summary>
    public GameObject GuideList;
    public Cam_Control FPS_Ctrl;

    #region 自動模式用
    /// <summary> 自動模式用場景順序 </summary>
    public GuideMap_Name LocalScene_Num;
    /// <summary> 自動模式用遮罩 </summary>
    public GameObject Mask;

    /// <summary> 自動模式停止計持按鈕 </summary>
    public GameObject StopCount_Btn;
    /// <summary> 自動模式繼續計持按鈕 </summary>
    public GameObject ContinueCount_Btn;

    /// <summary> 場景自動轉換時間 </summary>
    private float SceneTransitionDelay = 10f;
    /// <summary> 場景轉換時花費的總時長 </summary>
    private float SceneTransitionDuration = 1f;

    /*private readonly GuideMap_Name[] Auto_Scenes
                                = new GuideMap_Name[18]
                                {
                                   GuideMap_Name.Home,
                                   GuideMap_Name.WestTrail_01,
                                   GuideMap_Name.WestTrail_02,
                                   GuideMap_Name.ToWestBridge,
                                   GuideMap_Name.WestTrail_03,
                                   GuideMap_Name.LargeTree,
                                   GuideMap_Name.Plaza,
                                   GuideMap_Name.FeilongTrail_Start,
                                   GuideMap_Name.WoodlandTrail_01,
                                   GuideMap_Name.WoodlandTrail_02,
                                   GuideMap_Name.SteppedBridge,
                                   GuideMap_Name.TheIsland,
                                   GuideMap_Name.ObservationDeck_01,
                                   GuideMap_Name.WoodlandTrail_03,
                                   GuideMap_Name.WoodlandTrail_04,
                                   GuideMap_Name.FeilongTrail_Exit,
                                   GuideMap_Name.ArchBridge,
                                   GuideMap_Name.ObservationDeck_02,
                                };*/
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //確認當下場景編號
        scene_num = (Scene_Num)SceneManager.GetActiveScene().buildIndex;
        //儲存選擇的景點名稱
        string SelectViewPoint = Map_Select.map_select.VRScene_Name;

        if (Enum.IsDefined(typeof(GuideMap_Name), SelectViewPoint))
        {
            GuideMap_Name sceneNum = (GuideMap_Name)Enum.Parse(typeof(GuideMap_Name), SelectViewPoint);
            LocalScene_Num = sceneNum;
            Local_VRScene = VR_Scenes[(int)sceneNum];

            if (scene_num == Scene_Num.Touch_Mode && MapSelect_Btn[(int)sceneNum] != null)
            {
                OnSelect_Btn = MapSelect_Btn[(int)sceneNum];
                OnSelect_Btn.transform.GetChild(1).GetComponent<Image>().sprite = OnSelete;
            }
        }
        else
        {
            LocalScene_Num = GuideMap_Name.Home;
            Local_VRScene = VR_Scenes[0];
        }

        //將人物視角移動到選擇的景點位置
        Player.transform.position = Local_VRScene.transform.position;
        Vector3 new_rotation = new Vector3(0, 90, 0);
        Player.transform.eulerAngles = new_rotation;

        if (scene_num == Scene_Num.Auto_Mode)
        {
            Timer = 0f;
            Time_Start = true;

            Mask.SetActive(false);
            MeshRenderer Mask_Renderer = Mask.GetComponent<MeshRenderer>();
            Color Mask_Color = Mask_Renderer.material.color;
            Mask_Color.a = 0;
            Mask_Renderer.material.color = Mask_Color;

            StopOrPlay_UI.GetComponent<Image>().sprite = Play_Btn;
        }

        IsReturnPage = false;
        /*if (scene_num == Scene_Num.VR_Mode)
            IsHome();*/
    }

    public void ReturnToMapScene()
    {
        IsReturnPage = true;

        if (scene_num != Scene_Num.Touch_Mode)
            Time_Start = true;
    }

    public void SceneJump(GameObject VR_Sc)
    {
        Select_VRScene = VR_Sc;
        if (scene_num == Scene_Num.VR_Mode)
            Time_Start = true;
        else
        {
            Player.transform.position = Select_VRScene.transform.position;
            //PlayRotationSetting();

            if (GuideList.activeSelf) GuideMap_Switch(false);

            if (OnSelect_Btn != null)
                OnSelect_Btn.transform.GetChild(1).GetComponent<Image>().sprite = NoSelete;

            int SceneNum = -1;
            for (int i = 0; i < VR_Scenes.Length; i++)
            {
                if (VR_Scenes[i] == VR_Sc)
                {
                    SceneNum = i;
                    break;
                }
            }

            OnSelect_Btn = MapSelect_Btn[SceneNum];

            if (OnSelect_Btn != null)
                OnSelect_Btn.transform.GetChild(1).GetComponent<Image>().sprite = OnSelete;
        }
        Local_VRScene = Select_VRScene;
        Select_VRScene = null;

    }

    public void StopJump()
    {
        Select_VRScene = null;
        IsReturnPage = false;

        if (scene_num == Scene_Num.VR_Mode)
            Time_Start = false;
    }

    void Update()
    {
        if (scene_num != Scene_Num.Touch_Mode)
        {
            if (Time_Start == true)
                Timer += Time.deltaTime;
            else
                Timer = 0;

            if (IsReturnPage)
            {
                if (Timer > 1f)
                    ScenesSwitch.ScenceSwitch(Scene_Num.MapGuide.ToString());
            }
            else
            {
                if (scene_num == Scene_Num.Auto_Mode)
                {
                    if (SceneTransitionDelay <= Timer && Time_Start == true)
                    {
                        Time_Start = false;
                        StartCoroutine(AutoMode_SwitchScene());
                    }
                }
                else
                {
                    if (Timer > 1)
                    {
                        Player.transform.position = Select_VRScene.transform.position;
                        PlayRotationSetting();

                        Local_VRScene = Select_VRScene;
                        Select_VRScene = null;

                        Timer = 0;
                    }
                }
            }
        }
    }

    public void SwitchTimeCount()
    {
        if (Time_Start)
        {
            StopOrPlay_UI.GetComponent<Image>().sprite = Stop_Btn;
            Time_Start = false;
            Timer = 0;
        }
        else
        {
            StopOrPlay_UI.GetComponent<Image>().sprite = Play_Btn;
            Time_Start = true;
        }
    }

    IEnumerator AutoMode_SwitchScene()
    {
        GuideMap_Name Next_Scene = LocalScene_Num == GuideMap_Name.ObservationDeck_02 ? GuideMap_Name.Home : LocalScene_Num + 1;
        Select_VRScene = VR_Scenes[(int)Next_Scene];

        Mask.SetActive(true);
        MeshRenderer Mask_Renderer = Mask.GetComponent<MeshRenderer>();
        Color Mask_Color = Mask_Renderer.material.color;
        Mask_Color.a = 0;
        Mask_Renderer.material.color = Mask_Color;

        float duration = SceneTransitionDuration / 2;
        float elaspTime = 0;
        bool IsSwitch = false;

        while (elaspTime < SceneTransitionDuration)
        {
            elaspTime += Time.deltaTime;
            if (elaspTime < duration)
                Mask_Color.a = Mathf.Lerp(0, 1, elaspTime / duration);
            else
                Mask_Color.a = Mathf.Lerp(1, 0, (elaspTime - 0.5f) / duration);

            if (elaspTime >= 0.5f && !IsSwitch)
            {
                IsSwitch = true;
                SelectJump((int)Next_Scene);
                PlayRotationSetting();
            }

            Mask_Renderer.material.color = Mask_Color;
            yield return null;
        }

        Mask_Color.a = 0;
        Mask_Renderer.material.color = Mask_Color;
        Mask.SetActive(false);

        LocalScene_Num = Next_Scene;
        Local_VRScene = Select_VRScene;
        Select_VRScene = null;

        Timer = 0f;
        Time_Start = true;
    }

    //設定到場景後的面對方向
    public void PlayRotationSetting()
    {
        //取得選取的位置的名稱
        var SwitchSceneName = Select_VRScene.GetComponent<VR_Scene_Info>().SceneName;
        //抓出當前場景前往其他地方的資訊
        var NowSceneInfo = Local_VRScene.GetComponent<VR_Scene_Info>().Other_SceneInfo;

        //確認是否可以旋轉，並記錄角度
        bool HasRotation = false;
        float rotation = 0f;

        //找到對應的名稱與角度
        for (int i = 0; i < NowSceneInfo.Count; i++)
        {
            if (NowSceneInfo[i].NextScene == SwitchSceneName)
            {
                rotation = NowSceneInfo[i].angle;
                HasRotation = true;
                break;
            }
        }

        //如果有找到，就轉動，沒有就維持原角度
        if (HasRotation)
        {
            if (scene_num == Scene_Num.Touch_Mode)
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

    public void GuideMap_Switch(bool IsOpen)
    {
        GuideList.SetActive(IsOpen);
        FPS_Ctrl.IsStop = IsOpen;
    }

    /*public void IsHome()
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
    }*/

    public void SelectJump(int Location_Num)
    {
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
    ObservationDeck_02,
    FeilongTrail,
    Exit,
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