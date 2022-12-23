using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using System;

public class DVC_ShareAudio : MonoBehaviour
{

    const int SAMPLE_RATE = 44100;
    public AudioSource audioSource;
    public bool GrabSceneAudio = true;
    private IRtcEngine mRtcEngine;
    public int AudioChannels = 1;

    public void initSharing()
    {
        mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetExternalAudioSource(true, SAMPLE_RATE, AudioChannels);
    }

    private IEnumerator PlaySoundFromURL(string url)
    {
        Debug.Log(url);
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;
        //yield return www;

        Debug.Log("The file has been downloaded, playing it");

        audioSource.clip = (AudioClip)www.GetAudioClip();
        audioSource.volume = 1;
        audioSource.Play();

        StartCoroutine(CoAudioRender());

        Debug.Log("Audio played");
    }

    public void StartSendingNow()
    {
        //audioSource.Play();
        //StartCoroutine(CoAudioRender());
        StartCoroutine(PlaySoundFromURL("https://localhost/agora/audio.mp3"));
    }

    public void StopSendingNow()
    {
        audioSource.Stop();
        StopCoroutine(CoAudioRender());
        mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetExternalAudioSource(false, SAMPLE_RATE, AudioChannels);
    }

    /// OnAudioListenerRender
    virtual public void OnAudioFilterRead(float[] data, int channels)
    {
        if (GrabSceneAudio)
        {
            short[] intData = new short[data.Length];
            //converting in 2 steps : float[] to Int16[], //then Int16[] to Byte[]

            byte[] bytesData = new byte[data.Length * 2];
            //bytesData array is twice the size of
            //dataSource array because a float converted in Int16 is 2 bytes.

            var rescaleFactor = 32767; //to convert float to Int16

            for (int i = 0; i < data.Length; i++)
            {
                float sample = data[i];
                if (sample > 1) sample = 1;
                else if (sample < -1) sample = -1;

                intData[i] = (short)(sample * rescaleFactor);
                byte[] byteArr = new byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            PushExternalAudioFrame(bytesData, channels);
        }
    }

    /// FINALLY PUSH FRAME INTO STREAM
    // _externalAudioFrameBuffer.Length = samples * channels * bytesPerSample
    IEnumerator CoAudioRender()
    {
        int channels = audioSource.clip.channels;
        float[] samples = new float[audioSource.clip.samples * channels];
        audioSource.clip.GetData(samples, 0);
        GrabSceneAudio = true;
        int SourceDataIndex = channels * audioSource.timeSamples;
        //int SourceDataIndex = channels * (audioSource.clip.samples - 120000);
        Debug.LogWarning("CoAudioRender started. Found audio samples = " +
            samples.Length + " channels = " + audioSource.clip.channels);

        while (audioSource != null && audioSource.isActiveAndEnabled && audioSource.isPlaying)
        {
            int readSamples = (int)(SAMPLE_RATE * Time.deltaTime); // SamplesRate * elapsedTime => number of samples to read
            int delta = channels * readSamples;
            float[] copySample = new float[delta];
            if (readSamples + SourceDataIndex / channels <= audioSource.clip.samples)
            {
                Array.Copy(samples, SourceDataIndex, copySample, 0, delta);
            }
            else // wrap
            {
                int cur2EndCnt = samples.Length - SourceDataIndex;
                int wrap2HeadCnt = delta - cur2EndCnt;
                Array.Copy(samples, SourceDataIndex, copySample, 0, cur2EndCnt);
                Array.Copy(samples, 0, copySample, cur2EndCnt, wrap2HeadCnt);
            }
            SourceDataIndex = (SourceDataIndex + delta) % samples.Length;

            OnAudioFilterRead(copySample, channels);
            yield return new WaitForEndOfFrame();
        }
        GrabSceneAudio = false;
        Debug.LogWarning("Done Audio Render coroutine...");
    }

    //bool  flag  = false;
    virtual protected void PushExternalAudioFrame(byte[] _externalAudioFrameBuffer, int channels)
    {
        AudioFrame _externalAudioFrame = new AudioFrame();

        int bytesPerSample = 2;

        _externalAudioFrame.type = AUDIO_FRAME_TYPE.FRAME_TYPE_PCM16;
        _externalAudioFrame.samples = _externalAudioFrameBuffer.Length / (channels * bytesPerSample);
        _externalAudioFrame.bytesPerSample = bytesPerSample;
        _externalAudioFrame.samplesPerSec = SAMPLE_RATE;
        _externalAudioFrame.channels = channels;
        _externalAudioFrame.buffer = _externalAudioFrameBuffer;

        Debug.Log("byte length: " + _externalAudioFrameBuffer.Length);
        int i = 0;
        string str = "";
        while (i < 100)
        {
            str += "" + _externalAudioFrameBuffer[i];
            i++;
        }
        Debug.Log(str);
        Debug.Log("byte lendgh ended test it");

        if (mRtcEngine != null)
        {
            //if (flag == false)
            //{
            int a = mRtcEngine.PushAudioFrame(_externalAudioFrame);
            //flag = true;
            //}
        }
    }


}
