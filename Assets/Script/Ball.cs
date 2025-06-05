using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //�{�[���̌��݈ʒu
    [SerializeField] GameObject balltrans;

    [SerializeField] Rigidbody rb;
    //�{�[���ɑ΂��Ă̏d�͂̐ݒ�
    [SerializeField] Vector3 localGravity;

    [SerializeField] bool gravity;
    
    [SerializeField] float time;

    [SerializeField] PlayerStatusManager ps;
    [SerializeField] Pitcher pit;
    [SerializeField] Batter bt;
    [SerializeField] GManager gm;

    // Start is called before the first frame update
    void Start()
    {
        balltrans = GameObject.Find("balltrans");
        ps = FindAnyObjectByType<PlayerStatusManager>();
        pit = FindAnyObjectByType<Pitcher>();
        bt = FindAnyObjectByType<Batter>();
        gm = FindAnyObjectByType<GManager>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                Speed();
            })
            .AddTo(this);

        //�{�[���̏���
        this.UpdateAsObservable()
            .Delay(TimeSpan.FromSeconds(10))
            .Subscribe(_ =>
            {
                gm.scorerenewal.Value = true;
                //pit.pitting_start.Value = false;
                Destroy(gameObject);
            })
            .AddTo(this);

        //�^�C�~���O�ɂ���Ċp�x��ݒ�
        this.UpdateAsObservable()
            .Where(_ => bt.angle_finish.Value)
            .Subscribe(_ =>
            {
                time += Time.deltaTime;
                bt.hit_angle.Value = 240 * time;
            });

        //�o�b�g�ɓ���������
        this.OnCollisionEnterAsObservable()
            .Where(collision => collision.gameObject.tag == "BallDec")
            .Subscribe(_ =>
            {
                this.transform.eulerAngles = new Vector3(1.65f, -90 + bt.hit_angle.Value, -1.4f);
                rb.AddForce(-transform.forward * ps._power[ps.player_id.Value].Value);
                gravity = true;
                bt.angle_finish.Value = false;
                bt.audiosource.PlayOneShot(bt.hitSE);
            });

        //�o�b�g�ɓ�����Ȃ��ꍇ
        this.OnCollisionEnterAsObservable()
             .Where(collision => collision.gameObject.tag == "Catcher")
             .Subscribe(_ =>
             {
                 gm.outCount.Value++;
                 bt.catcher_audio.PlayOneShot(bt.catchSE);
               //  pit.pitting_start.Value = false;
                 Destroy(gameObject);
             });
        //�o�b�g�ɓ���������̏d�͒ǉ�
        this.UpdateAsObservable()
            .Where(_ => gravity)
            .Subscribe(_ => rb.AddForce(localGravity));
    }
    //�o�b�g�ɂԂ���܂ł̏���
    void Speed()
    {
        var speed = Vector3.zero;
        speed.z = pit.sp.Value;

        balltrans.transform.position = this.transform.position;
        if (!gravity)
        {
            this.transform.Translate(speed);
        }
    }
    void OnDestroy()
    {
        gm.balldeath.Value = true;
    }
}
