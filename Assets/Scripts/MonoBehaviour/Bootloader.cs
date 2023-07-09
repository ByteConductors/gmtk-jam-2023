using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootloader : MonoBehaviour
{
    public string UIScene;
    public string bootScene;

    public GameObject gameMangerPrefab;
    public GameObject playerPrefab;
    public GameObject pointerPrefab;
    public GameObject LevelPrefab;

    private void Awake()
    {
        StartCoroutine(Boot());
    }

    IEnumerator Boot()
    {
        SceneManager.LoadSceneAsync(UIScene, LoadSceneMode.Additive);

        yield return null;
    }
}
