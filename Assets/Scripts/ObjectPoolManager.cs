using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<int, Queue<GameObject>> poolDict = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
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
            // Attach tracking component
            var po = obj.GetComponent<PooledObject>();
            if (po == null) po = obj.AddComponent<PooledObject>();
            po.prefab = prefab;
        }

        if (obj == null)
        {

            obj = Instantiate(prefab);
            // Attach tracking component
            var po = obj.GetComponent<PooledObject>();
            if (po == null) po = obj.AddComponent<PooledObject>();
            po.prefab = prefab;

        }
        else
        {
          //  Debug.Log("saatiin sielta jotain sentaan"+prefab.name);
        }

        obj.transform.SetPositionAndRotation(position, rotation);

        BaseController bc = obj.GetComponent<BaseController>();
        if (bc != null)
        {
            bc.isGoingToBeDestroyed = false;
            bc.hengissaoloaika = 0.0f;

            bc.ResetState();
        }

        obj.SetActive(true);

        return obj;
    }
    /*
    public void ReturnToPool(GameObject prefapjokaturha,GameObject obj)
    {
        //@tod poista
        
        ReturnToPool(obj);

    }
    */

        public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;

        var po = obj.GetComponent<PooledObject>();
        if (po == null || po.prefab == null)
        {
            Debug.LogWarning($"[ObjectPoolManager] Tried to return {obj.name} but it has no PooledObject prefab reference!");
            Destroy(obj); // safer than enqueuing incorrectly
            return;
        }

        BaseController bc = obj.GetComponent<BaseController>();
        if (bc != null)
        {
            bc.isGoingToBeDestroyed = true;

            bc.OnDestroyPoolinlaittaessa();
        }

        obj.SetActive(false);



        int prefabId = po.prefab.GetInstanceID();
        if (!poolDict.ContainsKey(prefabId))
        {
            poolDict[prefabId] = new Queue<GameObject>();
        }

        poolDict[prefabId].Enqueue(obj);
    }

    


    /*
    public void ReturnToPool(GameObject prefappijokasiisturha,GameObject obj, float secondsAfterReturnToPool)
    {
        //@todoo poista
        ReturnToPool(obj, secondsAfterReturnToPool);
    }
    */
    public void ReturnToPool(GameObject obj, float secondsAfterReturnToPool)
    {
        if (obj == null) return;
        StartCoroutine(ReturnAfterDelay(obj, secondsAfterReturnToPool));
    }

    private IEnumerator ReturnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null) ReturnToPool(obj);
    }
}
