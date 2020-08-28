using GameServices.UI;
using System;
using UnityEngine;

public enum UIElementOption
{
    None,
    OnlyShowed,
    OnlyHided
}

public abstract class UIManager : MonoBehaviour
{
    public abstract T GetUIElement<T>(UIElementOption option = UIElementOption.None) where T : UIElementBase;
}
