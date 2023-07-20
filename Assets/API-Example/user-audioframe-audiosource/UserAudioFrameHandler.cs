using System;
using UnityEngine;
using agora_gaming_rtc;
using RingBuffer;

public class UserAudioFrameHandler : MonoBehaviour
{

    AudioSource _audioSource;
    IUserAudioFrameDelegate _userAudioFrameDelegate;

    private uint UID { get; set; }

    private int CHANNEL = 2;
    private int PULL_FREQ_PER_SEC = 100;
    private int SAMPLE_RATE = 48000; // this should = CLIP_SAMPLES x PULL_FREQ_PER_SEC
    private int CLIP_SAMPLES = 480;

    private int _count;

    private int _writeCount;
    private int _readCount;

    private RingBuffer<float> _audioBuffer;
    private AudioClip _audioClip = null;


    // Use this for initialization (runs after Init)
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _userAudioFrameDelegate.HandleAudioFrameForUser += HandleAudioFrame;
        SetupAudio(_audioSource, "clip_for_" + UID);
    }

    private void OnDisable()
    {
        _userAudioFrameDelegate.HandleAudioFrameForUser -= HandleAudioFrame;
        ResetHandler();
    }

    public void Init(uint uid, IUserAudioFrameDelegate userAudioFrameDelegate, AudioFrame audioFrame)
    {
        Debug.Log("INIT:" + uid + " audioFrame:" + audioFrame);
        _userAudioFrameDelegate = userAudioFrameDelegate;
        UID = uid;
        CLIP_SAMPLES = audioFrame.samples;
        SAMPLE_RATE = audioFrame.samplesPerSec;
        CHANNEL = audioFrame.channels;
    }

    void SetupAudio(AudioSource aud, string clipName)
    {
        if (_audioClip != null) return;

        //The larger the buffer, the higher the delay
        var bufferLength = SAMPLE_RATE / PULL_FREQ_PER_SEC * CHANNEL * 100; // 1-sec-length buffer
        _audioBuffer = new RingBuffer<float>(bufferLength, true);
        // Debug.Log($"{UID} Created clip for SAMPLE_RATE:" + SAMPLE_RATE + " CLIP_SAMPLES:" + CLIP_SAMPLES + " channel:" + CHANNEL + " => bufferLength = " + bufferLength);
        _audioClip = AudioClip.Create(clipName,
            CLIP_SAMPLES,
            CHANNEL, SAMPLE_RATE, true,
            OnAudioRead);
        aud.clip = _audioClip;
        aud.loop = true;
        aud.Play();
    }

    void ResetHandler()
    {
        if (_audioBuffer != null)
        {
            _audioBuffer.Clear();
        }
        _count = 0;
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.LogWarning("Pausing application");
        if (pause)
        {
            ResetHandler();
        }
        else
        {
            Debug.Log("Application resumed.");
        }
    }

    void HandleAudioFrame(uint uid, AudioFrame audioFrame)
    {
        if (UID != uid || _audioBuffer == null) return;

        var floatArray = ConvertByteToFloat16(audioFrame.buffer);
        lock (_audioBuffer)
        {
            _audioBuffer.Put(floatArray);
            _writeCount += floatArray.Length;
            _count++;
        }
    }

    private void OnAudioRead(float[] data)
    {

        for (var i = 0; i < data.Length; i++)
        {
            lock (_audioBuffer)
            {
                if (_audioBuffer.Count > 0)
                {
                    data[i] = _audioBuffer.Get();
                    _readCount += 1;
                }
            }
        }
        // Debug.Log("buffer length remains: {0}", writeCount - readCount);
    }

    private static float[] ConvertByteToFloat16(byte[] byteArray)
    {
        var floatArray = new float[byteArray.Length / 2];
        for (var i = 0; i < floatArray.Length; i++)
        {
            floatArray[i] = BitConverter.ToInt16(byteArray, i * 2) / 32768f; // -Int16.MinValue
        }

        return floatArray;
    }
}