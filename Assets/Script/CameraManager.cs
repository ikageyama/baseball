using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera telephotoCamera1;
    [SerializeField] Camera telephotoCamera2;
    [SerializeField] Camera batCamera;
    [SerializeField] Camera ballCamera;

    [SerializeField] GameObject dec;

    [SerializeField]List<bool> camerachange = new List<bool>();

    //カメラスピード
    [SerializeField] float xspeed;
    [SerializeField] float yspeed;

    [SerializeField] GManager gm;

    enum CameraBool
    {
        tc1,//カメラ1    0
        tc2,//カメラ2    1
        bt1,//バッターカメラ    2
    }

    void Start()
    {
        StartCoroutine(TelephotoCamera());

        gm = FindAnyObjectByType<GManager>();

        //カメラ切り替え
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (camerachange[(int)CameraBool.tc1])
                {
                    TelephotoCameraChange1();
                }

                if (camerachange[(int)CameraBool.tc2])
                {
                    TelephotoCameraChange2();
                }

                if (camerachange[(int)CameraBool.bt1])
                {
                    BatCameraChange();
                }

            })
            .AddTo(this);

        //バッター視点へのカメラの切り替え
        this.UpdateAsObservable()
            .Where(_ => !camerachange[(int)CameraBool.bt1] && !gm.cameraswitch.Value && gm._round.Value < 9)
            .Delay(TimeSpan.FromSeconds(10))
            .Subscribe(_ => camerachange[(int)CameraBool.bt1] = true);

        gm.cameraswitch
            .Where(camera => !camera)
            .Subscribe(_ => gm.balldeath.Value = true);

        //バットにボールが当たった場合
        dec.OnCollisionEnterAsObservable()
            .Where(collision => collision.gameObject.tag == "ball")
            .Delay(TimeSpan.FromSeconds(2))
            .Subscribe(_ => 
            {
                BallCameraChange();
                camerachange[(int)CameraBool.bt1] = false;
            });
    }
    //打った際のボールへのカメラ切り替え
    void BallCameraChange()
    {
        ballCamera.gameObject.SetActive(true);
        batCamera.gameObject.SetActive(false);
        telephotoCamera1.gameObject.SetActive(false);
        telephotoCamera2.gameObject.SetActive(false);
    }

    //バッター視点のカメラ
    void BatCameraChange()
    {
        batCamera.gameObject.SetActive(true);
        telephotoCamera1.gameObject.SetActive(false);
        telephotoCamera2.gameObject.SetActive(false);
        ballCamera.gameObject.SetActive(false);
    }

    //望遠カメラ1
    void TelephotoCameraChange1()
    {
        telephotoCamera1.gameObject.SetActive(true);
        telephotoCamera2.gameObject.SetActive(false);
        batCamera.gameObject.SetActive(false);
        ballCamera.gameObject.SetActive(false);

        telephotoCamera1.gameObject.transform.position += new Vector3(xspeed, yspeed, -0.006f);
    }

    //望遠カメラ2
    void TelephotoCameraChange2()
    {
        telephotoCamera2.gameObject.SetActive(true);
        telephotoCamera1.gameObject.SetActive(false);
        batCamera.gameObject.SetActive(false);
        ballCamera.gameObject.SetActive(false);

        telephotoCamera2.gameObject.transform.position += new Vector3(xspeed, yspeed, 0.008f);
    }

    //カメラ切り替え
    IEnumerator TelephotoCamera()
    {
        camerachange[(int)CameraBool.tc1] = true;
        yield return new WaitForSeconds(5.0f);
        camerachange[(int)CameraBool.tc1] = false;
        camerachange[(int)CameraBool.tc2] = true;
        yield return new WaitForSeconds(5.0f);
        camerachange[(int)CameraBool.tc2] = false;
        camerachange[(int)CameraBool.bt1] = true;
        gm.cameraswitch.Value = false;

        yield return new WaitUntil(() => gm.cameraswitch.Value);

        camerachange[(int)CameraBool.tc1] = true;
        camerachange[(int)CameraBool.bt1] = false;
        yield return new WaitForSeconds(8.0f);
        camerachange[(int)CameraBool.tc1] = false;
        camerachange[(int)CameraBool.tc2] = true;
        yield return new WaitForSeconds(8.0f);
        camerachange[(int)CameraBool.tc2] = false;
        gm.game_end.Value = true;
        SceneManager.LoadScene("TitleScene");
    }
}
