using UnityEngine;
using GameServices.CoroutineSequencing;
using System;

namespace GameServices.UI
{
    public abstract class UIElementBase : MonoBehaviour
    {
        public void BeginShow(Action endCallback = null)
        {
            gameObject.SetActive(true);

            var sequencer = OnShowBegin();

            if (sequencer == null)
            {
                endCallback?.Invoke();
            }
            else
            {
                sequencer.Run(this, endCallback);
            }
        }

        public void BeginHide(Action endCallback = null)
        {
            var sequencer = OnHideBegin();

            if (sequencer == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                sequencer.Run(this, () =>
                {
                    endCallback?.Invoke();
                    gameObject.SetActive(false);
                });
            }
        }

        protected virtual CoroutineSequence OnShowBegin()
        {
            return null;
        }

        protected virtual CoroutineSequence OnHideBegin()
        {
            return null;
        }
    }
}
