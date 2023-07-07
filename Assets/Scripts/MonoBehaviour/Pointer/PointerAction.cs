using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public abstract class PointerAction
{
    private event Action onFinish;
    public event Action OnFinish { 
        add => onFinish += value; 
        remove => onFinish -= value;
    }

    public PointerAction (Action onFinish)
    {

    } 

    public abstract void Execute(PointerController controller);
    public abstract void Update();
    public void Finish() => onFinish?.Invoke();

}
