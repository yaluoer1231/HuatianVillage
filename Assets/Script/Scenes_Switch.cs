using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public enum Scene_Num
{
    Home = 0,
    ControlsGuide = 1,
    WanglongPond = 2,
    MapGuide = 3,
    VR_Mode = 4,
    No_VR_Mode = 5
}

public class Scenes_Switch : MonoBehaviour
{
    public Scene_Num scene_num;

    // Start is called before the first frame update
    void Start()
    {
        scene_num = (Scene_Num)SceneManager.GetActiveScene().buildIndex;
        switch (scene_num)
        {
            case Scene_Num.VR_Mode:
            case Scene_Num.No_VR_Mode:
                Screen.orientation = ScreenOrientation.Landscape;
                Screen.autorotateToLandscapeRight = true;
                break;

            case Scene_Num.ControlsGuide:
            case Scene_Num.WanglongPond:
            case Scene_Num.MapGuide:
            case Scene_Num.Home:
                Screen.orientation = ScreenOrientation.Portrait;
                break;

            default: break;
        }
        if (scene_num == Scene_Num.VR_Mode)
            XRSettings.enabled = true;
        else
            XRSettings.enabled = false;
    }

    void Update()
    {
        UpPage();
    }

    public void ScenceSwitch(string Scene)
    {
        if (Enum.IsDefined(typeof(Scene_Num), Scene))
        {
            Scene_Num sceneNum = (Scene_Num)Enum.Parse(typeof(Scene_Num), Scene);
            SceneManager.LoadScene((int)sceneNum +"_" + Scene);
        }
        else
            Debug.LogError("找不到「" + Scene + "」，無法跳轉");
    }

    public void UpPage()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (scene_num)
            {
                case Scene_Num.VR_Mode: case Scene_Num.No_VR_Mode: 
                    SceneManager.LoadSceneAsync((int)Scene_Num.MapGuide);
                    break;

                case Scene_Num.ControlsGuide: case Scene_Num.WanglongPond: case Scene_Num.MapGuide:
                    SceneManager.LoadSceneAsync((int)Scene_Num.Home);
                    break;

                case Scene_Num.Home: break;

                default: break;
            }
        }
    }
}
