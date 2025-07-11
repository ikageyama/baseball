using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;
using R3.Triggers;
using System;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] List<Button> scenebutton;

    [SerializeField] AudioSource click_Source;
    [SerializeField] AudioClip click_clip;

    enum SceneTransitions
    {
        start,
        quit,
    }

    void Start()
    {
        //設定シーンへの遷移
        scenebutton[(int)SceneTransitions.start].OnClickAsObservable()
            .Subscribe(_ => {
                SceneManager.LoadScene("SettingScene");
                click_Source.PlayOneShot(click_clip);
            });

        //ゲームの終了
        scenebutton[(int)SceneTransitions.quit].OnClickAsObservable()
            .Subscribe(_ =>
            {
             #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
             #else
                  Application.Quit();
             #endif
            });
    }

}
