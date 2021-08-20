using UnityEngine;
using System;
using System.Collections.Generic;

namespace agora_gaming_rtc {
        public sealed class AgoraCallbackQueue : MonoBehaviour
        {
            private Queue<Action> queue = new Queue<Action>();

            public void ClearQueue()
            {
                lock (queue)
                {
                    queue.Clear();
                }
            }

            public void EnQueue(Action action)
            {
                lock (queue)
                {
                    if (queue.Count >= 250)
                    {
                        queue.Dequeue();
                    }
                    queue.Enqueue(action);
                }
            }

            private Action DeQueue()
            {
                Action action = null;
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        action = queue.Dequeue();
                    }
                }
                return action;
            }

            void Awake()
            {

            }
            // Update is called once per frame
            void Update()
            {
                var action = DeQueue();

                if (action != null)
                {
                    action();
                }
                action = null;
            }

            void OnDestroy()
            {
                ClearQueue();
            }
        }
}