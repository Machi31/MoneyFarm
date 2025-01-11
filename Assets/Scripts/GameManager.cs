using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamePush;
using System.Collections.Generic;

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

        StartCoroutine(SaveTime());
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
        if (_isFirst)
            StartCoroutine(SaveDataCor());
    }

    private void SaveData()
    {
        _isFirst = false;
        PlayerPrefsX.SetBool("IsFirst", _isFirst);
    }

    private IEnumerator SaveDataCor(){
        yield return new WaitForSeconds(5);
        SaveData();
    }

    private IEnumerator SaveTime(){
        yield return new WaitForSeconds(1);
        DateTime exitTime = DateTime.Now;
        string exitTimeString = exitTime.ToString();
        PlayerPrefs.SetString("ExitTime", exitTimeString);
    }
}
