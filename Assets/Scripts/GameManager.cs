using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private GameObject _gameOverUI;

    private void Start()
    {
        Time.timeScale = 1f;
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
        Time.timeScale = 0f;

        if (FindObjectOfType<PlayerHealth>().CurrentHealth > 0)
            _winText.SetActive(true);
        else
            _loseText.SetActive(true);

        _gameOverUI.SetActive(true);
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
