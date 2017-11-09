using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPath : MonoBehaviour {

    public GameObject shape;
    public int ShapeCount = 5;
    public Transform StartPosition;
    public Transform EndPosition;

    public float AnimScale = 1.5f;
    public float ChangeScaleTime = 0.3f;

    private GameObject[] shapes;
    private int currentAnimShape;

	// Use this for initialization
	void Start () {
        shapes = new GameObject[ShapeCount];
        Vector3 dir = (EndPosition.position - StartPosition.position);
        float delta = dir.magnitude / ShapeCount;
        Vector3 normalizedDir = dir.normalized;
        for (int i = 0; i < ShapeCount; i++) {
            shapes[i] = Instantiate(shape, StartPosition.position + normalizedDir * delta * i, Quaternion.identity, transform); 
        }
        StartCoroutine(Animate());
	}
	
    IEnumerator Animate()
    {
        currentAnimShape = 0;
        shapes[0].transform.localScale *= AnimScale;
        Vector3 normalScale = shapes[0].transform.localScale;
        Vector3 targetScale = normalScale * AnimScale;
        Vector3 deltaScale = targetScale - normalScale;
        float deltaTime = 0;
        while (true) {           
            for (int i = 0; i < ShapeCount; i++) {
                deltaTime = 0;
                while(deltaTime < ChangeScaleTime) {
                    deltaTime += Time.deltaTime;
                    shapes[i].transform.localScale = targetScale -  deltaScale *  deltaTime / ChangeScaleTime;
                    shapes[(i + 1) % ShapeCount].transform.localScale = normalScale + deltaScale * deltaTime / ChangeScaleTime;
                    yield return null;
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
