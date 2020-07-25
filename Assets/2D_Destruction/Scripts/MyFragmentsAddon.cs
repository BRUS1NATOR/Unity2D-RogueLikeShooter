using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFragmentsAddon : ExplodableAddon
{
    public Vector2 velocity = Vector2.zero;
    override public void OnFragmentsGenerated(List<GameObject> fragments)
    {
        GameManager.instance.fragmentsManager.GetFragments(fragments, velocity);
    }
}
