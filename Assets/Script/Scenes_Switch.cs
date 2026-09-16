using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public enum Scene_Num
{
    Home = 0,
    WanglongPond = 1,
    ControlsGuide = 2,
    Traffic_Information = 3,
    MapGuide = 4,
    VR_Mode = 5,
    Touch_Mode = 6,
    Auto_Mode = 7,
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
            case Scene_Num.Touch_Mode:
            case Scene_Num.Auto_Mode:
                Screen.orientation = ScreenOrientation.Landscape;

                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;

                break;

            case Scene_Num.ControlsGuide:
            case Scene_Num.WanglongPond:
            case Scene_Num.MapGuide:
            case Scene_Num.Home:
                XRSettings.enabled = false;
                Screen.orientation = ScreenOrientation.Portrait;

                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;

                break;

            default: break;
        }

        if (scene_num == Scene_Num.VR_Mode || scene_num == Scene_Num.Auto_Mode)
            XRSettings.enabled = true;
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
                case Scene_Num.VR_Mode:
                case Scene_Num.Auto_Mode:
                case Scene_Num.Touch_Mode:
                    SceneManager.LoadSceneAsync((int)Scene_Num.MapGuide);
                    break;

                case Scene_Num.ControlsGuide:
                case Scene_Num.WanglongPond:
                case Scene_Num.MapGuide:
                    SceneManager.LoadSceneAsync((int)Scene_Num.Home);
                    break;

                case Scene_Num.Home: break;

                default: break;
            }
        }
    }
}
