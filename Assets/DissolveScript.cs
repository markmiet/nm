using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    [SerializeField] private float _dissolveTime =0.75f;
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private int _dissolveAmout = Shader.PropertyToID("_DissolveAmount");
    private int _vrerticalSissolveAmout = Shader.PropertyToID("_VerticalDissolve");
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderers = GetComponentsInParent<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;

        }

    }




    private IEnumerator Vanish(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;
        while (elapsedTime<_dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / _dissolveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / _dissolveTime));
            for (int i=0;i<_materials.Length;i++)
            {
                if (useDissolve)
                    _materials[i].SetFloat(_dissolveAmout, lerpedDissolve);
                if (useVertical)
                    _materials[i].SetFloat(_vrerticalSissolveAmout, lerpedVerticalDissolve);

            }
            yield return null;
        }

    }

    private IEnumerator Appear(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));
            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDissolve)
                    _materials[i].SetFloat(_dissolveAmout, lerpedDissolve);
                if (useVertical)
                    _materials[i].SetFloat(_vrerticalSissolveAmout, lerpedVerticalDissolve);

            }
            yield return null;
        }

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Vanish(true, false));

        }
        if (Input.GetKey(KeyCode.B))
        {
            StartCoroutine(Appear(true, false));

        }
    }
}
