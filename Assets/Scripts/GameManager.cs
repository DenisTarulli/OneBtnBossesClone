using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _timerText;
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
            _winText.SetActive(true);
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
