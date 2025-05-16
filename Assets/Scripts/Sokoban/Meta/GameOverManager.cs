using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * The Game Over Manager takes care of the game over screen as well as moving you back to the start menu
 */
public class GameOverManager : MonoBehaviour
{
    [SerializeField] string gameOverSound;

    CanvasGroup gameOverCanvas;

    private void Awake()
    {
        gameOverCanvas = GetComponent<CanvasGroup>();
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        AudioManager.Instance.PlaySound(gameOverSound);
        StartCoroutine(OnGameOver());
    }
    public IEnumerator OnGameOver() 
    {
        //This function makes the game over screen appear. I would have prefered to do this through an animation,
        //but since I'm pressed for time I will make it using an IEnumerator instead.
        while(gameOverCanvas.alpha < 1)
        {
            gameOverCanvas.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
        yield return new WaitForSeconds(4.0f);
        yield return SceneManager.LoadSceneAsync(0);
    }
}
