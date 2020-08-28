using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices.CoroutineSequencing
{
    public class CoroutineSequence
    {
        private IEnumerator[] sequencedCoroutines = new IEnumerator[0];

        public CoroutineSequence(params IEnumerator[] coroutines)
        {
            sequencedCoroutines = coroutines;
        }

        public void Run(MonoBehaviour controller, Action callback = null)
        {
            controller.StartCoroutine(SemaphoreCoroutine(callback));
        }

        public static CoroutineSequence operator + (CoroutineSequence a, CoroutineSequence b)
        {
            if (a == null && b == null)
            {
                return null;
            }
            else if (a == null)
            {
                return b;
            }
            else if (b == null)
            {
                return a;
            }
            else
            {
                return new CoroutineSequence(a.sequencedCoroutines.GetEnumerator(), b.sequencedCoroutines.GetEnumerator());
            }
        }

        private IEnumerator SemaphoreCoroutine(Action endCallback)
        {
            foreach (IEnumerator routine in sequencedCoroutines)
            {
                yield return routine;
            }

            endCallback?.Invoke();
        }
    }
}
