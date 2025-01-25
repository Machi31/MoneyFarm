using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamePush;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int _secondsFromExit;

    public bool _isFirst = true;

    [SerializeField] private GameObject _playBtn;
    [SerializeField] private GameObject _loadingText;
    [SerializeField] private Slider _loadingSlider;

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
        _playBtn.SetActive(false);
        _loadingText.SetActive(true);
        _loadingSlider.gameObject.SetActive(true);

        StartCoroutine(LoadAsync());

        if (_isFirst)
            StartCoroutine(SaveDataCor());
    }

    private void SaveData() {
        _isFirst = false;
        PlayerPrefsX.SetBool("IsFirst", _isFirst);
    }

    private IEnumerator SaveDataCor(){
        yield return new WaitForSeconds(15);
        SaveData();
    }

    private IEnumerator SaveTime(){
        while (true){
            yield return new WaitForSeconds(1);
            DateTime exitTime = DateTime.Now;
            string exitTimeString = exitTime.ToString();
            PlayerPrefs.SetString("ExitTime", exitTimeString);
        }
    }

    private IEnumerator LoadAsync(){
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(1);
        loadAsync.allowSceneActivation = false;

        while (!loadAsync.isDone){
            _loadingSlider.value = loadAsync.progress;

            if (loadAsync.progress >= 0.9f && !loadAsync.allowSceneActivation){
                yield return new WaitForSeconds(0.5f);
                loadAsync.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
