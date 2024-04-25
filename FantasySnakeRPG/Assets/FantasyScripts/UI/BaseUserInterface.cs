using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUserInterface : MonoBehaviour,IUserInterface
{
    [SerializeField] protected GameObject panel;
    public abstract void Initialize();
    protected abstract void OnTriggerShowPopup();
    protected abstract void OnTriggerHidePopup();
    public void OnShowPopup()
    {
        OnTriggerShowPopup();
    }

    public void OnHidePopup()
    {
        OnTriggerHidePopup();
    }
}
