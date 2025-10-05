using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopilotTextControl : BaseController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /**
     * Weapons online: “Weapon systems online. All armaments operational.”

Weapons offline: “Weapon systems offline. Conserving power.”

Weapons ineffective: “Current weapons are ineffective. Consider evasive maneuvers.”

Weapons overheat: “Weapon systems overheating. Reducing output to prevent damage.”

Weapons charged: “Weapons fully charged and ready.”

    Hyperdrive engaged: “Hyperdrive engaged. Accelerating to target velocity.”

Hyperdrive disengaged: “Hyperdrive disengaged. Systems stable.”

Hyperdrive charging: “Hyperdrive charging. ETA to full power: [time].”

Hyperdrive unavailable: “Hyperdrive offline. Insufficient energy or damage detected.”

    Collecting bonus: “Bonus collected. Inventory updated.”

Mission complete: “Objective complete. Standing by for next orders.”

Level transition: “Entering next sector. Prepare for new encounter.”

Evasive maneuvers: “Initiating evasive maneuvers. Course adjusted.”
    */

    [TextArea]
    public string message = "";

    public float delaybetweenlines = 1.0f;

    public float messagedestroytime = 15.0f;
    private bool asetettu = false;
    // Update is called once per frame
    void Update()
    {
        if (!asetettu && IsMoreThanHalfOnLeft() && IsGameObjectVisible())
        {
            GameManager.Instance.checkPoint.GetComponent<CheckPointController>().x = transform.position.x;

            if (message != null)
            {

                string[] rows = message.Split('\n');
                float delay = 0.0f;
                foreach (var row in rows)
                {
                    GameManager.Instance.LisaaTekstiViiveella(row, delay, messagedestroytime);
                    delay += delaybetweenlines;
                }
            }
            asetettu = true;
        }
    }
}
