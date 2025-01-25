using GamePush;
using UnityEngine;

public class BgChoseMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _bg1;
    [SerializeField] private GameObject _bg2;

    private void Start() {
        bool isPortrait = GP_Device.IsPortrait();
        if (isPortrait){
            _bg1.SetActive(true);
            _bg2.SetActive(false);
        }
        else{
            _bg1.SetActive(false);
            _bg2.SetActive(true);
        }
    }

    // private void Update() {
    //     if (GP_Device.IsPortrait()){
    //         _bg1.SetActive(true);
    //         _bg1.SetActive(false);
    //     }
    //     else{
    //         _bg1.SetActive(false);
    //         _bg1.SetActive(true);
    //     }
    // }
}
