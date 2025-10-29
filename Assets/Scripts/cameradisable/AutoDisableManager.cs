using System.Collections.Generic;
using UnityEngine;

public class AutoDisableManager : MonoBehaviour
{
    private static List<AutoDisableByCamera> disabledObjects = new List<AutoDisableByCamera>();
    private static AutoDisableManager instance;
    private Camera mainCam;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        disabledObjects = new List<AutoDisableByCamera>();
        instance = null;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        mainCam = Camera.main;
    }

    public static void RegisterDisabledObject(AutoDisableByCamera obj)
    {
        if (!disabledObjects.Contains(obj))
            disabledObjects.Add(obj);
    }

    private void Update()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        for (int i = disabledObjects.Count - 1; i >= 0; i--)
        {
            var obj = disabledObjects[i];
            if (obj == null)
            {
                disabledObjects.RemoveAt(i);
                continue;
            }
            if (obj.GetComponent<HitCounter>() != null)
            {
                HitCounter hc = obj.GetComponent<HitCounter>();
                if (hc.debugDestroyInfo)
                {
                    Debug.Log("debug info=" + obj.name);
                }
            }

            obj.TryEnable(mainCam);

            if (obj.gameObject.activeSelf)
                disabledObjects.RemoveAt(i);
        }
    }
}
