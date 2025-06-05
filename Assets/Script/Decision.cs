using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decision : MonoBehaviour
{
    //UI‚ÌÔ˜g‚ÌŒ^
    [SerializeField] RectTransform decisionI;
    [SerializeField] float UIspeed;

    //ŽÀÛ‚Ì“–‚½‚è”»’è‚ÌŒ^
    [SerializeField] Transform decisionG;
    [SerializeField] float Ispeed;

    Vector3 speed = Vector3.zero;

    [SerializeField] bool key;
    [SerializeField] bool range;

    float decisionIndex;

    [SerializeField] PlayerStatusManager ps;
    [SerializeField] Score score;

    private CompositeDisposable disposables = new();

    //ƒ~[ƒg”ÍˆÍ‚ÌÝ’è
    public void MeatSwitchPlayer(int meatIndex)
    {
        disposables.Clear();

        ps._meat[meatIndex]
           .Subscribe(meat =>
           {
                decisionI.localScale = new Vector3(6, 5, 1) * ps._meat[meatIndex].Value;
                decisionG.localScale = new Vector3(0.2f, 0.17f, 0.14f) * ps._meat[meatIndex].Value;
                decisionI.anchoredPosition = new Vector2(4, -6.5f) * ((ps._meat[meatIndex].Value - 1) * 10) + new Vector2(40, -225);
                decisionG.position = new Vector3(-13.674f, 1.03f, 13.736f);
               decisionIndex = meat;
           })
           .AddTo(disposables);
    }

    void Start()
    {
        ps = FindAnyObjectByType<PlayerStatusManager>();

        score.MeatSwitchPlayer
            .Subscribe(MeatSwitchPlayer)
            .AddTo(this);

        MeatSwitchPlayer(0);

        this.UpdateAsObservable()
        .Subscribe(_ =>
        {
            speed = Vector3.zero;
            key = true;
        })
        .AddTo(this);

        //ã•ûŒü‚Ö‚ÌˆÚ“®
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.W) && key && decisionI.position.y < 495 - (decisionIndex - 1) * 70)
            .Subscribe(_ =>
            {
                decisionI.position += new Vector3(0, UIspeed, 0);
                speed.z = -Ispeed;
                key = false;
            });

        //¶•ûŒü‚Ö‚ÌˆÚ“®
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.A) && key && decisionI.position.x > 880 + (decisionIndex - 1) * 40)
            .Subscribe(_ =>
            {
                decisionI.position -= new Vector3(UIspeed, 0, 0);
                speed.y = Ispeed;
                key = false;
            });

        //‰º•ûŒü‚Ö‚ÌˆÚ“®
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.S) && key && decisionI.position.y > 155 - (decisionIndex - 1) * 70)
            .Subscribe(_ =>
            {
                decisionI.position -= new Vector3(0, UIspeed, 0);
                speed.z = Ispeed;
                key = false;
            });

        //‰E•ûŒü‚Ö‚ÌˆÚ“®
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.D) && key && decisionI.position.x < 1120 + (decisionIndex - 1) * 40)
            .Subscribe(_ =>
            {
                decisionI.position += new Vector3(UIspeed, 0, 0);
                speed.y = -Ispeed;
                key = false;
            });

        //“–‚½‚è”»’è‘¤‚ÌˆÚ“®
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                decisionG.Translate(speed);
            })
            .AddTo(this);
    }
}
