using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Mouse_Button{
    Mouse_Left = 0,
    Mouse_Right = 1,
    Mouse_Midden = 2
}

public class Cam_Control : MonoBehaviour
{
    public Transform fixed_X_Rotate_point;  // 利用此點做拖拉平移 (旋轉點)

    public float rotateSpeed = 0.2f;

    public float minVertical = -80f;
    public float maxVertical = 80f;

    private float rotationX;
    private float rotationY;

    // 手指按下時的位置
    private Vector2 lastTouchPosition;

    void Start()
    {
        Vector3 angle = transform.eulerAngles;

        rotationX = angle.x;
        rotationY = angle.y;

        if (rotationX > 180f)
            rotationX -= 360f;

        rotationX = Mathf.Clamp(
            rotationX,
            minVertical,
            maxVertical
        );
    }

    void Update()
    {
        // =========================
        // PC 滑鼠
        // =========================

        if (Input.GetMouseButtonDown(0))
        {
            // 按下時記錄滑鼠位置
            lastTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentPosition = Input.mousePosition;

            Vector2 delta = currentPosition - lastTouchPosition;

            RotateCamera(delta.x, delta.y);

            // 更新位置
            lastTouchPosition = currentPosition;
        }


        // =========================
        // 手機觸控
        // =========================

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 手指剛按下
            if (touch.phase == TouchPhase.Began)
            {
                // 只記錄位置
                // 這裡絕對不旋轉 Camera
                lastTouchPosition = touch.position;
            }

            // 手指移動
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 currentPosition = touch.position;

                Vector2 delta = currentPosition - lastTouchPosition;

                RotateCamera(delta.x, delta.y);

                // 更新手指位置
                lastTouchPosition = currentPosition;
            }
        }
    }

    void RotateCamera(float x, float y)
    {
        rotationY += x * rotateSpeed;
        rotationX -= y * rotateSpeed;

        rotationX = Mathf.Clamp(
            rotationX,
            minVertical,
            maxVertical
        );

        fixed_X_Rotate_point.rotation = Quaternion.Euler(
            rotationX,
            rotationY,
            0f
        );
    }

    //public Transform fixed_X_Rotate_point;  // 利用此點做拖拉平移 (旋轉點)
    //public Text text;
    //public float Cam_rotate_speed = 1.0f;

    //private float rotate_x = 0;
    //private float rotate_y = 0;

    //void Start()
    //{
    //    // 一開始先記錄目前鏡頭的旋轉角度
    //    Vector3 startRotation = transform.eulerAngles;

    //    rotate_x = startRotation.y;
    //    rotate_y = -startRotation.x;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    text.text = transform.eulerAngles.ToString();
    //    if (Input.GetMouseButton(0))
    //    {
    //        Rotate(
    //            Input.GetAxis("Mouse X"),
    //            Input.GetAxis("Mouse Y")
    //        );
    //    }

    //    // 手機觸控
    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);

    //        if (touch.phase == TouchPhase.Moved)
    //        {
    //            Rotate(
    //                touch.deltaPosition.x,
    //                touch.deltaPosition.y
    //            );
    //        }
    //    }
    //}

    //void Rotate(float move_x, float move_y)
    //{
    //    rotate_x += move_x * Cam_rotate_speed;
    //    rotate_y -= move_y * Cam_rotate_speed;

    //    rotate_y = Mathf.Clamp(rotate_y, -90f, 90f);

    //    // 利用滑鼠移動變量計算旋轉角度
    //    var rotate = Quaternion.Euler(-rotate_y, -rotate_x, 0);

    //    // 旋轉鏡頭
    //    transform.rotation = rotate;

    //    // 以 y 軸旋轉 fixed_X_rotate ( 旋轉基準點 ) 
    //    rotate = Quaternion.Euler(0, -rotate_x, 0);

    //    fixed_X_Rotate_point.rotation = rotate;
    //}
}
