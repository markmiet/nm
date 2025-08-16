using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<int, Queue<GameObject>> poolDict = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int prefabId = prefab.GetInstanceID();

        if (!poolDict.ContainsKey(prefabId))
        {
            poolDict[prefabId] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDict[prefabId].Count > 0)
        {
            obj = poolDict[prefabId].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        if (obj == null) return Instantiate(prefab);

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        ParticleSystem particle = obj.GetComponentInChildren<ParticleSystem>(true);
        if (particle != null && particle.gameObject != null)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle.Play();
        }

        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);

        int prefabId = prefab.GetInstanceID();
        if (!poolDict.ContainsKey(prefabId))
        {
            poolDict[prefabId] = new Queue<GameObject>();
        }

        poolDict[prefabId].Enqueue(obj);
    }

    public void ReturnToPool(GameObject prefab, GameObject obj, float secondsAfterReturnToPool)
    {
        if (obj == null) return;
        StartCoroutine(ReturnAfterDelay(prefab, obj, secondsAfterReturnToPool));
    }

    private IEnumerator ReturnAfterDelay(GameObject prefab, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            ReturnToPool(prefab, obj);
        }
    }
}
