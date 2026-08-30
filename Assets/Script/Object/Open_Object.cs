using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Object : MonoBehaviour
{
    public GameObject Object;

    public void Open()
    {
        Object.SetActive(true);
    }
}
