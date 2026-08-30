using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Destroy : MonoBehaviour
{
    public void CloseObject()
    {
        gameObject.SetActive(false);
    }
}
