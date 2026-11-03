using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loadingSlider;     
    public float loadingDuration = 1.5f; // Tổng thời gian loading 3 giây)
    private float targetProgress = 0f;
    private float currentProgress = 0f;

    void Start()
    {
        StartCoroutine(LoadGameSmoothly());
    }

    void Update()
    {
        // Làm thanh chạy mượt bằng cách dùng Lerp
        currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * 3f);
        loadingSlider.value = currentProgress;
    }

    IEnumerator LoadGameSmoothly()
    {
        float step = 3f / loadingDuration;

        while (targetProgress < 1f)
        {
            targetProgress += step * Time.deltaTime;
            yield return null;
        }

        targetProgress = 1f;

        // Chờ 0.5s để đảm bảo thanh đầy rồi mới chuyển cảnh
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Play");
    }
}
