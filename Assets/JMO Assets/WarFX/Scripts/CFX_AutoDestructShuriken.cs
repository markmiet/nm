using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;
	
	void OnEnable()
	{
		OnlyDeactivate = true;
	//	StartCoroutine("CheckIfAlive");
	}
	/*
	IEnumerator CheckIfAlive ()
	{
		//MJM muutti tämän vain on
		while(true)
		{
			yield return new WaitForSeconds(2f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if(OnlyDeactivate)
				{
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
						this.gameObject.SetActive(false);
					#endif
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
	*/
}
