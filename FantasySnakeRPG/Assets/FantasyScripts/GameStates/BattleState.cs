using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : BaseState
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Initialize()
    {
    }

    protected override void OnStartState()
    {
        GameManager.Instance.UI.Battle.OnShowPopup();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.Battle.OnHidePopup();
    }
}
