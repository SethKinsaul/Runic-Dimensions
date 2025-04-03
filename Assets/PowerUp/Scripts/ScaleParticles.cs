using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScaleParticles : MonoBehaviour {
	void Update () {
        var ps = GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        mainModule.startSize = transform.lossyScale.magnitude;
    }
}