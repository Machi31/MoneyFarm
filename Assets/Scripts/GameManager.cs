using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int _secondsFromExit;

    public bool _isFirst = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        _isFirst = PlayerPrefsX.GetBool("IsFirst", true);

        if (!_isFirst)
        {
            string exitTimeString = PlayerPrefs.GetString("ExitTime");
            DateTime exitTime = DateTime.Parse(exitTimeString);
            TimeSpan timeSinceExit = DateTime.Now - exitTime;
            _secondsFromExit = (int)timeSinceExit.TotalSeconds;
            Debug.Log("Игрок был не в игре: " + _secondsFromExit);
        }
    }

    public void StartGame() => SceneManager.LoadScene(1);

    private void OnApplicationPause(bool pauseStatus)
    {
        _isFirst = false;
        DateTime exitTime = DateTime.Now;
        string exitTimeString = exitTime.ToString();
        PlayerPrefs.SetString("ExitTime", exitTimeString);
        PlayerPrefsX.SetBool("IsFirst", _isFirst);
    }
    private void OnApplicationQuit()
    {
        _isFirst = false;
        DateTime exitTime = DateTime.Now;
        string exitTimeString = exitTime.ToString();
        PlayerPrefs.SetString("ExitTime", exitTimeString);
        PlayerPrefsX.SetBool("IsFirst", _isFirst);
    }
}
