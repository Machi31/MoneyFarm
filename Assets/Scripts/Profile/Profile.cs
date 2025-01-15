using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private MethodsProfile _methodsProfile;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _profileWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;
    [SerializeField] private Image _bgAboutProfile;

    // [SerializeField] private Button[] _profileButton;

    private float _fadeDuaration = 0.5f;

    private void OnEnable(){
        MethodsFarm.OpenProfile += OpenProfile;
        MethodsFarm.OpenFullProfile += OpenFullProfile;
    }

    private void OnDisable(){
        MethodsFarm.OpenProfile -= OpenProfile;
        MethodsFarm.OpenFullProfile -= OpenFullProfile;
    }

    private void OpenProfile(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
    }

    private void OpenFullProfile(int id){
        _methodsProfile.BuyFarmMethod(id);
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
    }

    public void CloseProfile(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    public void OpenAboutProfile() => StartCoroutine(FadeAboutOpen());
    public void CloseAboutProfile() => StartCoroutine(FadeAboutClose());

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        Color bgFarmsColor = _bgFarms.color;
        float bgFarmsStartAlpha = bgFarmsColor.a;

        Color bgProfileColor = _bgProfile.color;
        float bgProfileStartAlpha = bgProfileColor.a;

        if (bgFarmsStartAlpha == 1){
            while (elapsedTime < _fadeDuaration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _fadeDuaration;

                bgFarmsColor.a = Mathf.Lerp(bgFarmsStartAlpha, 0f, t);
                _bgFarms.color = bgFarmsColor;

                bgProfileColor.a = Mathf.Lerp(bgProfileStartAlpha, 1f, t);
                _bgProfile.color = bgProfileColor;
                // for (int i = 0; i < _profileButton.Length; i++){
                //     _profileButton[i].gameObject.SetActive(true);
                //     _profileButton[i].interactable = true;
                // }

                yield return null;
            }
            bgFarmsColor.a = 0f;
            _bgFarms.color = bgFarmsColor;

            bgProfileColor.a = 1f;
            _bgProfile.color = bgProfileColor;
        }
        else{
            while (elapsedTime < _fadeDuaration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _fadeDuaration;

                bgFarmsColor.a = Mathf.Lerp(bgFarmsStartAlpha, 1f, t);
                _bgFarms.color = bgFarmsColor;

                bgProfileColor.a = Mathf.Lerp(bgProfileStartAlpha, 0f, t);
                _bgProfile.color = bgProfileColor;
                // for (int i = 0; i < _profileButton.Length; i++){
                //     _profileButton[i].gameObject.SetActive(false);
                //     _profileButton[i].interactable = false;
                // }

                yield return null;
            }
            bgFarmsColor.a = 1f;
            _bgFarms.color = bgFarmsColor;

            bgProfileColor.a = 0f;
            _bgProfile.color = bgProfileColor;
        }
    }

    private IEnumerator FadeAboutOpen(){
        float elapsedTime = 0f;

        Color bgAboutProfileColor = _bgAboutProfile.color;
        float bgboutProfileAlpha = bgAboutProfileColor.a;

        if (bgboutProfileAlpha == 0){
            while (elapsedTime < _fadeDuaration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _fadeDuaration;

                bgAboutProfileColor.a = Mathf.Lerp(bgboutProfileAlpha, 1f, t);
                _bgAboutProfile.color = bgAboutProfileColor;

                yield return null;
            }
            bgAboutProfileColor.a = 1f;
            _bgAboutProfile.color = bgAboutProfileColor;
        }
    }
    private IEnumerator FadeAboutClose(){
        float elapsedTime = 0f;

        Color bgAboutProfileColor = _bgAboutProfile.color;
        float bgboutProfileAlpha = bgAboutProfileColor.a;

        if (bgboutProfileAlpha == 1){
            while (elapsedTime < _fadeDuaration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _fadeDuaration;

                bgAboutProfileColor.a = Mathf.Lerp(bgboutProfileAlpha, 0f, t);
                _bgAboutProfile.color = bgAboutProfileColor;

                yield return null;
            }
            bgAboutProfileColor.a = 0f;
            _bgAboutProfile.color = bgAboutProfileColor;
        }
    }
}
