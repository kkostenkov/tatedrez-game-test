
using Bonsai.Core;
using UnityEngine;

public class BehaviourTreeHost : MonoBehaviour
{
    public BehaviourTree tree;

    private void Start()
    {
        this.tree.Start();      
        tree.BeginTraversal();
    }

    // Update is called once per frame
    private void Update()
    {
        this.tree.Update();
    }
}
