using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] int waitInSeconds = 2;
    [SerializeField] float levelExitSlowMotionFactor = 0.2f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()) { 
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        Time.timeScale = levelExitSlowMotionFactor;
        yield return new WaitForSecondsRealtime(waitInSeconds);
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
