using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightInput : MonoBehaviour {

    public static bool moveRight = false;

    public void MoveRight()
    {
        moveRight = true;
    }

    public void StopRight()
    {
        moveRight = false;
    }
}
