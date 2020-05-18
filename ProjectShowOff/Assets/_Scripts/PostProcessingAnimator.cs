using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingAnimator : MonoBehaviour
{
    [SerializeField] private Volume _postProcessingProfile = null;

    private ColorAdjustments _colorAdjustment;
    [SerializeField] private float _postExposureStart = 0;
    [SerializeField] private float _postExposureFadeDuration = 0;
    private float orginalPostExposure = 0;


    void Awake()
    {
        foreach (VolumeComponent comp in _postProcessingProfile.profile.components)
        {
            if (comp is ColorAdjustments) _colorAdjustment = comp as ColorAdjustments;
        }

        StartCoroutine(fadeExposure(_postExposureFadeDuration));

    }

    //private void Update()
    //{
    //    Debug.Log("Exposure: " );
    //}

    private IEnumerator fadeExposure(float pDuration)
    {
        float time = 0;
        float delta = _postExposureStart - orginalPostExposure;

        while (time < _postExposureFadeDuration)
        {
            time += Time.deltaTime;

            float t = 1 - time / _postExposureFadeDuration;
            _colorAdjustment.postExposure.value = (orginalPostExposure + delta * t);        
            yield return null;
        }
    }
}
