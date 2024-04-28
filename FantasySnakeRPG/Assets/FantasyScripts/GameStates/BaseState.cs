using UnityEngine;

public abstract class BaseState : MonoBehaviour,IState
{
    public abstract void Initialize();
    protected abstract void OnStartState();
    protected abstract void OnEndState();
    
    public void StartState()
    {
        OnStartState();
    }

    public void EndState()
    {
        OnEndState();
    }
}
