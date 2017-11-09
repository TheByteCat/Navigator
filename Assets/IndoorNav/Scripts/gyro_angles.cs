using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gyro_angles : MonoBehaviour {

    Text caption;

	// Use this for initialization
	void Start () {
        caption = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (caption)
            caption.text = string.Format("gyro: {0}", Input.gyro.attitude.eulerAngles.ToString());
	}
}
