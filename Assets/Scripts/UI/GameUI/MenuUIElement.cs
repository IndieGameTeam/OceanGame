using GameServices.Extensions;
using GameServices.UI;
using UnityEngine;

public class MenuUIElement : UIElementBase
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
