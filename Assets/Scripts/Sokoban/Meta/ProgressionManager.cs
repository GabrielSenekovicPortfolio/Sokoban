using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

/*This code handles callbacks from the puzzle objects to activate effects, as well as determine when the win point activates.
*It also takes care of the loading of the next level. So it technically breaks Single-Responsibility
*Furthermore, it also deals with the win game screen. This is asymmetrical, since the Game Over screen is taken care of by its own script.
*I will fix that if I have time.
*TODO:
*Break out Win Game functionality
*Break out Puzzle management
*/
public class ProgressionManager : MonoBehaviour
{
    [SerializeField] List<GameObject> rooms;
    [SerializeField] LevelTimer levelTimer;
    [SerializeField] int currentIndex;

    [SerializeField] TextMeshProUGUI targetText;
    [SerializeField] TextMeshProUGUI levelText;

    [SerializeField] string gemstonePutSound;
    [SerializeField] string winPointActivate;
    [SerializeField] string winGameSound;

    [SerializeField] CanvasGroup winGameCanvas;
    [SerializeField] TextMeshProUGUI winGameCanvasScoreText;

    GameObject currentLevel;
    WinPoint winPoint;
    List<GemstonePodium> podiums = new List<GemstonePodium>();
    List<Platform> platforms = new List<Platform>();
    int activatedPodiums;

    public static ProgressionManager instance;
    public static ProgressionManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        LoadLevel();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) //Debug button
        {
            LoadLevel();
        }
    }
    public void ProgressToNextLevel()
    {
        currentIndex++;
        levelTimer.SaveTimerScore();
        ScoreManager.Instance.SaveScore();
        LoadLevel();
    }
    public void LoadLevel()
    {
        if(currentIndex < rooms.Count)
        {
            podiums.Clear();
            Destroy(currentLevel);
            currentLevel = Instantiate(rooms[currentIndex]);
            levelText.text = (currentIndex + 1).ToString();
            activatedPodiums = 0;
            levelTimer.ResetTimer();
        }
        else
        {
            WinGame();
        }
    }
    public void SetWinTile(WinPoint winPoint) //The win tile sets itself, which is a bit clunky to do in this script
    {
        this.winPoint = winPoint;
    }
    public void AddPodiums(GemstonePodium podium) //All podiums add themselves, which is a bit clunky to do in this script
    {
        podiums.Add(podium);
        targetText.text = "0 / " + podiums.Count;
    }
    public void AddPlatform(Platform platform) //All platforms add themselves, which is a bit clunky to do in this script
    {
        platforms.Add(platform);
    }
    public void ActivatePodium(ColorCode color)
    {
        activatedPodiums++;
        platforms.Where(p => (ColorCode)p.GetEnumValue() == color)?.ToList().ForEach(p => p.Activate());
        targetText.text = activatedPodiums + " / " + podiums.Count;
        if(activatedPodiums == podiums.Count)
        {
            winPoint.Activate();
            AudioManager.Instance.PlaySound(winPointActivate);
        }
        else
        {
            AudioManager.Instance.PlaySound(gemstonePutSound);
        }
    }
    public void DeactivatePodium()
    {
        activatedPodiums--;
    }
    public void WinGame()
    {
        Time.timeScale = 0;
        AudioManager.Instance.PlaySound(winGameSound);
        StartCoroutine(OnWinGame());
    }
    public IEnumerator OnWinGame()
    {
        //This function makes the game over screen appear. I would have prefered to do this through an animation,
        //but since I'm pressed for time I will make it using an IEnumerator instead.
        winGameCanvasScoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
        while (winGameCanvas.alpha < 1)
        {
            winGameCanvas.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
        yield return new WaitForSeconds(4.0f);
        yield return SceneManager.LoadSceneAsync(0);
    }
}
