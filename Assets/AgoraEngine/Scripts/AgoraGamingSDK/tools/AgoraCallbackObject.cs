using System;
using UnityEngine;

namespace agora_gaming_rtc
{
    public sealed class AgoraCallbackObject {
        public GameObject _CallbackGameObject 
        {
            get; 
            set;
        }

        public AgoraCallbackQueue _CallbackQueue {
            set;
            get;
        } 

        public string _GameObjectName {
            set;
            get;
        }  

        public AgoraCallbackObject(string gameObjectName)
        {
            InitGameObject(gameObjectName);
        }

        public void Release()
        {
            if (!ReferenceEquals(_CallbackGameObject, null))
            {
                if (!ReferenceEquals(_CallbackQueue, null))
                {
                    _CallbackQueue.ClearQueue();
                }
                GameObject.Destroy(_CallbackGameObject);
                _CallbackGameObject = null;
                _CallbackQueue = null;
            }
        }

        private void InitGameObject(string gameObjectName)
        {
            DeInitGameObject(gameObjectName);
            _CallbackGameObject = new GameObject(gameObjectName);
            _CallbackQueue = _CallbackGameObject.AddComponent<AgoraCallbackQueue>();
            GameObject.DontDestroyOnLoad(_CallbackGameObject);
            _CallbackGameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private void DeInitGameObject(string gameObjectName)
        {
            GameObject gameObject = GameObject.Find(gameObjectName);
            if (!ReferenceEquals(gameObject, null))
            {
                AgoraCallbackQueue callbackQueue = gameObject.GetComponent<AgoraCallbackQueue>();
                if (!ReferenceEquals(callbackQueue, null))
                {
                    callbackQueue.ClearQueue();
                }
                GameObject.Destroy(gameObject);
                gameObject = null;
                callbackQueue = null;
            }
        }
    } 
}