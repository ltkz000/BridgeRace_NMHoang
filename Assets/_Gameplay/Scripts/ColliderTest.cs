using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    [SerializeField] private BoxCollider wallColli;
    public void TurnColliOn()
    {
        wallColli.enabled = true;
    }
}
