using GameServices.UI;
using System.Collections.Generic;

public class GameUIManager : UIManager
{
    private class UIElementCache
    {
        public bool isShow;
        public UIElementBase element;
    }

    private List<UIElementCache> cachedUIElements = new List<UIElementCache>();

    private void Awake()
    {
        foreach (UIElementBase element in GetComponentsInChildren<UIElementBase>(true))
        {
            UIElementCache elementCache = new UIElementCache();

            elementCache.isShow = element.gameObject.activeSelf;
            elementCache.element = element;

            cachedUIElements.Add(elementCache);
        }
    }

    public override T GetUIElement<T>(UIElementOption option = UIElementOption.None)
    {
        var cachedElement = cachedUIElements.Find(x => x.element is T && IsValid(x, option));

        if (cachedElement != null)
        {
            return (T)cachedElement.element;
        }

        return null;
    }

    private bool IsValid(UIElementCache cache, UIElementOption option)
    {
        return (option == UIElementOption.None || cache.isShow == (option == UIElementOption.OnlyShowed));
    }
}
