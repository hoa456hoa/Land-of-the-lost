using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    // Gọi khi bấm nút Settings (răng cưa)
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f; // Tạm dừng game
    }
  

}
