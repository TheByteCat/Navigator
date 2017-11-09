using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisible : MonoBehaviour {

    public Material material;
    public GameObject Root;

	// Use this for initialization
	void Start () {
        Renderer[] renders = GetComponentsInChildren<Renderer>();
        foreach (var r in renders) {
            r.material = material;
        }
	}
}
