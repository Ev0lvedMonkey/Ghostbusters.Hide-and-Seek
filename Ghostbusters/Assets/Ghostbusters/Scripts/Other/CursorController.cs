using UnityEngine;

public static class CursorController 
{
    public static void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("EnableCursor !!!!!!!!!!!!!!");
    }

    public static void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("DisableCursor !!!!!!!!!!!!!!");
    }
}
