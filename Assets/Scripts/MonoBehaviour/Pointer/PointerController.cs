using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    Queue<PointerAction> actionQueue = new Queue<PointerAction>();
    public PointerAction currentAction = null;

    private void Awake()
    {
        var action = new PointerActionMove(null, Random.insideUnitCircle * 4);
        var action2 = new PointerActionMove(null, Random.insideUnitCircle * 4);
        var action3 = new PointerActionMove(null, Random.insideUnitCircle * 4);
        var action4 = new PointerActionMove(null, Random.insideUnitCircle * 4);

        actionQueue.Enqueue(action);
        actionQueue.Enqueue(action2);
        actionQueue.Enqueue(action3);
        actionQueue.Enqueue(action4);
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

    private void Action_OnFinish()
    {
        currentAction = null;
    }
}
