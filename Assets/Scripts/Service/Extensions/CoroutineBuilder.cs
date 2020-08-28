using UnityEngine;
using System.Collections;
using System;
namespace GameServices.Extensions
{
    public static class CoroutineBuilder
    {
        public static IEnumerator InvokeInCoroutine(this Action callback)
        {
            callback?.Invoke();
            yield break;
        }

        public static IEnumerator PlayAndWait(this Animator animator, string clipName)
        {
            if (animator != null)
            {
                animator.Play(clipName);

                if (animator.updateMode == AnimatorUpdateMode.UnscaledTime)
                {
                    yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
                }
                else
                {
                    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                }
            }
        }

        public static IEnumerator LerpValue(float from, float to, float duration, Action<float> callback)
        {
            float step = 0.05F / duration;

            for (float alpha = 0; alpha < 1.0F + step; alpha += step)
            {
                callback?.Invoke(Mathf.Lerp(from, to, alpha));

                yield return new WaitForSecondsRealtime(step);
            }
        }
    }
}
