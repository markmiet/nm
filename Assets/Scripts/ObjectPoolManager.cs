using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDict = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDict[prefab].Count > 0)
        {
            obj = poolDict[prefab].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle.Play();
        }
        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Queue<GameObject>();
        }
        poolDict[prefab].Enqueue(obj);
    }


    public void ReturnToPool(GameObject prefab, GameObject obj, float secondsAfterReturnToPool)
    {
        StartCoroutine(ReturnAfterDelay(prefab, obj, secondsAfterReturnToPool));
    }

    private IEnumerator ReturnAfterDelay(GameObject prefab, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(prefab, obj);
    }
}
