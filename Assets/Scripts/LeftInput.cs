using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftInput : MonoBehaviour {

    public static bool moveLeft = false;

    public void MoveLeft()
    {
        moveLeft = true;
    }

    public void StopLeft()
    {
        moveLeft = false;
    }
}
