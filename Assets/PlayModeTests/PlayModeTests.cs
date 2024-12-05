using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeTests : InputTestFixture
{
    Keyboard _keyboard;

    public override void Setup()
    {
        base.Setup();
        _keyboard = InputSystem.AddDevice<Keyboard>();
    }
    #region Unit Tests
    [UnityTest]
    public IEnumerator TestStartGameButton()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(2f);
        
        GameManager.Instance.StartGame();

        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator TestLevelSelectorSingleButton()
    { 
        SceneManager.LoadScene("Scenes/LevelSelector");

        var levelSelector = new GameObject().AddComponent<LevelSelector>();

        levelSelector.LoadLevel("Level01");

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestRestartLevel()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(1f);

        GameManager.Instance.RestartLevel();
    }
    #endregion

    #region Integration Tests
    [UnityTest]
    public IEnumerator TestGameOverOnPlayerDeath()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.StartGame();

        yield return new WaitForSeconds(0.5f);

        PlayerHealth playerHealth = GameObject.FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.TakeDamage(3);

        Assert.AreEqual(0, playerHealth.CurrentHealth);
        Assert.AreEqual(0, Time.timeScale);

        yield return new WaitForSecondsRealtime(0.5f);
    }

    [UnityTest]
    public IEnumerator TestPlayerDashTriggerAndBarDischarge()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.StartGame();

        yield return new WaitForSeconds(0.5f);

        PlayerMovement playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        playerMovement.MovementType = MovementType.Dash;
        playerMovement.ChargeBar.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        
        float currentCharge = playerMovement.CurrentCharge;

        Press(_keyboard.spaceKey);
        yield return new WaitForSeconds(0.3f);

        Assert.That(currentCharge > playerMovement.CurrentCharge);
    }

    [UnityTest]
    public IEnumerator TestPlayerTakeDamageOnCollisionWithProjectile()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.StartGame();

        yield return new WaitForSeconds(0.2f);

        GameObject enemyProjectile = PoolManager.Instance.GetPooledObject(PoolObjectType.EnemyBullet);

        PlayerHealth playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        int currentHealth = playerHealth.CurrentHealth;

        enemyProjectile.transform.position = playerHealth.transform.position;
        enemyProjectile.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        Assert.That(currentHealth > playerHealth.CurrentHealth);
    }

    [UnityTest]
    public IEnumerator TestEnemyTakeDamageOnPlayerProjectileCollision()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.StartGame();

        yield return new WaitForSeconds(0.15f);

        GameObject playerProjectile = PoolManager.Instance.GetPooledObject(PoolObjectType.PlayerBullet);

        EnemyHealth enemyHealth = GameObject.FindObjectOfType<EnemyHealth>();
        float enemyCurrentHealth = enemyHealth.CurrentHealth;

        playerProjectile.transform.position = enemyHealth.transform.position;
        playerProjectile.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        Assert.That(enemyCurrentHealth > enemyHealth.CurrentHealth);
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionWithBulletWhileInvulnerabilityActive()
    {
        SceneManager.LoadScene("Scenes/Level01");

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.StartGame();

        yield return new WaitForSeconds(0.2f);

        GameObject enemyProjectile = PoolManager.Instance.GetPooledObject(PoolObjectType.EnemyBullet);

        PlayerHealth playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        int currentHealth = playerHealth.CurrentHealth;

        playerHealth.SetInvulnerabilityBubble(true);
        playerHealth.CanTakeDamage = false;

        enemyProjectile.transform.position = playerHealth.transform.position;
        enemyProjectile.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        Assert.That(currentHealth == playerHealth.CurrentHealth);
    }
    #endregion
}
