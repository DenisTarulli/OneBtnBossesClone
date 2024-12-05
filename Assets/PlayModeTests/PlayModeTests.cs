using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeTests
{
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
}
