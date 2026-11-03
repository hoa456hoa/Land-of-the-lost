using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public GameObject panelToClose;

    public void ClosePanel()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
            Time.timeScale = 1f;// game tiếp tục chạy 
        }
    }
}
