using UnityEngine;
using System.Linq;

public class GradientToTextureEditor : MonoBehaviour
{
    [SerializeField] Gradient _gradient;
    [SerializeField] RenderTexture _gradientRenderTexture;
    [SerializeField] Material _gradientMaterial;

    Vector2Int _resolution;
    float[] _timeArray = new float[10];
    Color[] _colorArray = new Color[10];

    public Gradient FogGradient => _gradient;

    void Start()
    {
        // -------------
        
        if (_gradientMaterial == null || _gradientRenderTexture == null) return;

        _resolution = new Vector2Int(_gradientRenderTexture.width, _gradientRenderTexture.height);
        
        OnValidate();
        
        // -------------
    }

    void OnValidate()
    {
        // -------------
        
        if (_gradientMaterial == null || _gradientRenderTexture == null) return;

        _timeArray = new float[10];
        _colorArray = new Color[10];

        int length = _gradient.colorKeys.Length;

        float[] timeKeys = 
            (from item in _gradient.colorKeys
            select item.time).ToArray();

        for(int i=0; i < timeKeys.Length; i++)
        {
            _timeArray[i] = timeKeys[i];
        }
        
        Color[] colorKeys = 
            (from item in _gradient.colorKeys
            select item.color).ToArray();

        for(int i=0; i < colorKeys.Length; i++)
        {
            _colorArray[i] = colorKeys[i];
        }

        _gradientMaterial.SetInt("_GradinentArraySize", length);
        _gradientMaterial.SetFloatArray("_GradientTimeArray", _timeArray);
        _gradientMaterial.SetColorArray("_GradientColorArray", _colorArray);

        Graphics.Blit(null, _gradientRenderTexture, _gradientMaterial);
        
        // -------------
    }
}
