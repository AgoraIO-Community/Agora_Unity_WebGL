using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

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
                    if (action != null)
                    {
                        queue.Enqueue(action);
                    }
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
                lock (queue)
                {
                    while (queue.Count > 0)
                    {
                        queue.Dequeue().Invoke();
                    }
                }
            }

            void OnDestroy()
            {
                ClearQueue();
            }
        }
}