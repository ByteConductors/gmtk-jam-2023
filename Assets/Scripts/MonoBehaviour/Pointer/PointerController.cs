using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PointerController : MonoBehaviour
{

    static PointerController instance;
    public static PointerController Instance { get =>  instance; }

    Queue<PointerAction> actionQueue = new Queue<PointerAction>();
    public PointerAction currentAction = null;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (currentAction == null && actionQueue.TryPeek(out PointerAction action)) ExecuteAction(action);
        if (currentAction != null) currentAction.Update();
    }

    public void ExecuteAction(PointerAction action)
    {
        Debug.Log("Changing Action");
        if (currentAction != null) return;
        action.Execute(this);
        action.OnFinish += Action_OnFinish;
        currentAction = actionQueue.Dequeue();
    }

    public void QueueActions(List<PointerAction> actions)
    {
        if (actions == null) return;
        foreach (var action in actions)
        {
            actionQueue.Enqueue(action);
        }
    }

    private void Action_OnFinish()
    {
        currentAction = null;
    }
}
