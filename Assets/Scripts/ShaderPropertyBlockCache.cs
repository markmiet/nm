using UnityEngine;
using System.Collections.Generic;

public class ShaderPropertyBlockCache
{
    private readonly Renderer _renderer;
    private readonly MaterialPropertyBlock _block;

    // Caches
    private readonly Dictionary<int, float> _floats = new();
    private readonly Dictionary<int, Color> _colors = new();
    private readonly Dictionary<int, Texture> _textures = new();

    public ShaderPropertyBlockCache(Renderer renderer)
    {
        _renderer = renderer;
        _block = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_block); // initialize with current values
    }

    public void SetFloat(int propertyID, float value)
    {
        if (!_floats.TryGetValue(propertyID, out var current) || !Mathf.Approximately(current, value))
        {
            _block.SetFloat(propertyID, value);
            _floats[propertyID] = value;
            Apply();
        }
    }

    public void SetColor(int propertyID, Color value)
    {
        if (!_colors.TryGetValue(propertyID, out var current) || current != value)
        {
            _block.SetColor(propertyID, value);
            _colors[propertyID] = value;
            Apply();
        }
    }

    public void SetTexture(int propertyID, Texture value)
    {
        if (!_textures.TryGetValue(propertyID, out var current) || current != value)
        {
            _block.SetTexture(propertyID, value);
            _textures[propertyID] = value;
            Apply();
        }
    }

    private void Apply()
    {
        _renderer.SetPropertyBlock(_block);
    }
}
