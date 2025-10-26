using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public DirectionSpriteSwitcher directionSpriteSwitcher;
    public void ChangeState(DirectionSpriteSwitcher.State state)
    {
        directionSpriteSwitcher.ChangeState(state);
    }
    public void LukitseXy()
    {
        directionSpriteSwitcher.LukitseXy();
    }
}
