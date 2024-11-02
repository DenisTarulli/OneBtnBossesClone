using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _finalStatsUI;
    [SerializeField] private GameObject _newRecordText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _bestTimeText;
    [SerializeField] private TextMeshProUGUI _finalTimeText;
    private float _time;
    private bool _gameIsOver;

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
        SceneManager.LoadScene("GameScene");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        _gameIsOver = true;
        Time.timeScale = 0f;

        if (FindObjectOfType<PlayerHealth>().CurrentHealth > 0)
        {
            _winText.SetActive(true);
            _finalStatsUI.SetActive(true);
            UpdateScores();
        }
        else
            _loseText.SetActive(true);

        _gameOverUI.SetActive(true);
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
