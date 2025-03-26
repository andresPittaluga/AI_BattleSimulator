using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticles : MonoBehaviour
{
    [Header("<color=#AA129F>Options")]
    [SerializeField] private Sprite _nervous;
    [SerializeField] private Sprite _health;
    [SerializeField] private Sprite _dmg;
    [Header("<color=#1111CF>References")]
    [SerializeField] private Material _mat;
    [SerializeField] private ParticleSystem _pSystem;
    
    public void PlayNervousParticles()
    {
        _mat.SetColor("_TintColor", Color.white);
        _mat.mainTexture = _nervous.texture;
        _pSystem.Play();
    }

    public void PlayHealthParticles()
    {
        _mat.SetColor("_TintColor", Color.green);
        _mat.mainTexture = _health.texture;
        _pSystem.Play();
    }

    public void PlayDmgParticles()
    {
        _mat.SetColor("_TintColor", Color.red);
        _mat.mainTexture = _dmg.texture;
        _pSystem.Play();
    }
}
