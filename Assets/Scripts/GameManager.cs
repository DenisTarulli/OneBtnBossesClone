using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Members
    [Header("UI elements")]
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _finalStatsUI;
    [SerializeField] private GameObject _newRecordText;
    [SerializeField] private GameObject _gameUI;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _bestTimeText;
    [SerializeField] private TextMeshProUGUI _finalTimeText;

    [Header("Objects")]
    [SerializeField] private GameObject _gameplayObjectsContainter;
    [SerializeField] private GameObject _preFightCanvas;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _nextLevelButton;

    private float _time;
    private bool _gameIsOver;
    #endregion

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (_timerText != null)
            TimeUpdate();
    }       

    public void StartGame()
    {
        _gameplayObjectsContainter.SetActive(true);
        _preFightCanvas.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex) + 1;

        if (SceneManager.sceneCountInBuildSettings <= nextSceneIndex)
            SceneManager.LoadScene("LevelSelector");
        else
            SceneManager.LoadScene(nextSceneIndex);
    }

    private void GameOver()
    {
        _gameIsOver = true;
        Time.timeScale = 0f;

        if (FindObjectOfType<PlayerHealth>().CurrentHealth > 0)
        {
            _winText.SetActive(true);
            _finalStatsUI.SetActive(true);
            _nextLevelButton.SetActive(true);
            UpdateScores();
        }
        else
        {
            _loseText.SetActive(true);
            _restartButton.SetActive(true);
        }

        _gameOverUI.SetActive(true);
        _gameUI.SetActive(false);
    }

    private void TimeUpdate()
    {
        if (_gameIsOver) return;

        _time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateScores()
    {
        float bestTime = PlayerPrefs.GetFloat("bestTime", 0f);

        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);

        _finalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (_time < bestTime || bestTime == 0f)
        {
            bestTime = _time;
            PlayerPrefs.SetFloat("bestTime", bestTime);
            _newRecordText.SetActive(true);
        }

        int bestMinutes = Mathf.FloorToInt(bestTime / 60);
        int bestSeconds = Mathf.FloorToInt(bestTime % 60);

        _bestTimeText.text = string.Format("{0:00}:{1:00}", bestMinutes, bestSeconds);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += GameOver;
        EnemyHealth.OnEnemyKill += GameOver;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= GameOver;
        EnemyHealth.OnEnemyKill -= GameOver;
    }
}
