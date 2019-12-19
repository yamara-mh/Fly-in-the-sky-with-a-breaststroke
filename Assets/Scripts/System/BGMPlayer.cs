using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

using UniRx;
using System;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private AudioMixerGroup mixer;

    [SerializeField]
    private AudioClip BGMslowClip, BGMslowActClip, BGMfastClip, BGMfastActClip;

    [SerializeField]
    private AudioClip introClip, resultClip;

    private AudioSource BGMslow, BGMfast, BGMslowAct, BGMfastAct;

    private const float
        VOLUME = 0.15f,
        INTRO_TIME = 19.2f,
        PITCH_MIN = 1,
        TEMPO_RATE = 0.001f,    //0.005f,
        CHANGE_START_SPEED = 25,
        CHANGE_END_SPEED = 75,
        CHANGE_RATE = 1 / (CHANGE_END_SPEED - CHANGE_START_SPEED),
        CGANGE_ACT_TIME = 4,
        MUSICAL_SCALE_RATE = 100 / 12;

    public const float
        CHANGE_ACT_START_DISTANCE = 500 * 500;

    private static float
        rbMagnitudeTempoRated,
        switchingBPMfastVolume,
        switchingActVolume,
        beforePitchShift = 1,
        beforeCutoff = 0,
        pitchShift,
        cutoff,
        pitchShiftOut;

    private static bool isIntro;

    void Awake()
    {
        BGMslow = gameObject.AddComponent<AudioSource>();
        //BGMfast = gameObject.AddComponent<AudioSource>();
        BGMslowAct = gameObject.AddComponent<AudioSource>();
        //BGMfastAct = gameObject.AddComponent<AudioSource>();

        BGMslow.clip = introClip;
        //BGMfast.clip = BGMfastClip;
        BGMslowAct.clip = BGMslowActClip;
        //BGMfastAct.clip = BGMfastActClip;

        BGMslow.outputAudioMixerGroup = mixer;
        //BGMfast.outputAudioMixerGroup = mixer;
        BGMslowAct.outputAudioMixerGroup = mixer;
        //BGMfastAct.outputAudioMixerGroup = mixer;

        isIntro = true;

        BGMslow.playOnAwake = false;
        //BGMfast.playOnAwake = false;
        BGMslowAct.playOnAwake = false;
        //BGMfastAct.playOnAwake = false;
        BGMslow.loop = true;
        //BGMfast.loop = true;
        BGMslowAct.loop = true;
        //BGMfastAct.loop = true;
        BGMslow.volume = VOLUME;
        //BGMfast.volume = 0;
        BGMslowAct.volume = 0;
        //BGMfastAct.volume = 0;
    }

    void Update()
    {
        if (Manager.gaming)
        {
            rbMagnitudeTempoRated = player.rbMagnitude * TEMPO_RATE;

            //BPM
            //pitchShift = 1 / (PITCH_MIN + rbMagnitudeTempoRated);
            //mixer.audioMixer.SetFloat("pitch", PITCH_MIN + rbMagnitudeTempoRated);
            //mixer.audioMixer.SetFloat("pitchShift", beforePitchShift);  //Mathf.Lerp(beforePitchShift, beforePitchShift, 0.01f)
            //mixer.audioMixer.GetFloat("pitchShift", out pitchShiftOut);
            //mixer.audioMixer.SetFloat("pitch", 1 / pitchShiftOut);
            //beforePitchShift = pitchShift;
            //ハイパス
            cutoff = InputManager.HMDTransform.position.y < 150 ? 1 : 0;
            mixer.audioMixer.SetFloat("cutoff", 22000 - 21000 * Mathf.Lerp(beforeCutoff, cutoff, 0.01f));
            beforeCutoff = cutoff;

            if (Manager.gameTime * Manager.GAME_TIME <= 5)
            {   //フェードアウト
                BGMslow.volume = Manager.gameTime * Manager.GAME_TIME * 0.2f * (1 - switchingActVolume) * VOLUME;
                BGMslowAct.volume = Manager.gameTime * Manager.GAME_TIME * 0.2f * switchingActVolume * VOLUME;
            }
            else
            {   //音色切替
                BGMslow.volume = Mathf.Lerp(BGMslow.volume, (1 - switchingActVolume) * VOLUME, 0.1f);
                BGMslowAct.volume = Mathf.Lerp(BGMslowAct.volume, switchingActVolume * VOLUME, 0.1f);
            }
        }

        if (Manager.result && BGMslow.clip != resultClip)
        {
            //リザルトBGM再生
            mixer.audioMixer.SetFloat("pitchShift", 1);
            mixer.audioMixer.SetFloat("pitch", 1);
            BGMslow.clip = resultClip;
            BGMslow.time = 0;
            BGMslow.volume = VOLUME;
            BGMslow.Play();
        }

        if (isIntro)
        {
            if (!BGMslow.isPlaying && player.transform.position.z > -2062f)
            {
                BGMslow.Play();
                //BGMfast.Play();
                //BGMfastAct.Play();
            }

            if (BGMslow.time >= INTRO_TIME)
            {
                isIntro = false;
                BGMslow.clip = BGMslowClip;
                float time = BGMslow.time - INTRO_TIME;
                BGMslow.time = time;
                BGMslowAct.time = time;
                //BGMfast.time = time;
                //BGMfastAct.time = time;

                BGMslow.Play();
                BGMslowAct.Play();
            }

            return;
        }

        /*
        //音色切替
        switchingBPMfastVolume =
            player.rbMagnitude <= CHANGE_START_SPEED ?
                0 :
            player.rbMagnitude <= CHANGE_END_SPEED ?
                rbMagnitudeTempoRated * CHANGE_RATE :
                1;

        //音量
        BGMslow.volume = (1 - switchingBPMfastVolume) * (1 - switchingActVolume) * VOLUME;
        BGMfast.volume = switchingBPMfastVolume * (1 - switchingActVolume) * VOLUME;
        BGMslowAct.volume = (1 - switchingBPMfastVolume) * switchingActVolume * VOLUME;
        BGMfastAct.volume = switchingBPMfastVolume * switchingActVolume * VOLUME;
        // */
    }

    public static void BattleStart()
    {
        DOTween.To(() => switchingActVolume, (n) => switchingActVolume = n, 1, CGANGE_ACT_TIME);
    }

    public static void BattleEnd()
    {
        DOTween.To(() => switchingActVolume, (n) => switchingActVolume = n, 0, CGANGE_ACT_TIME);
    }
}
