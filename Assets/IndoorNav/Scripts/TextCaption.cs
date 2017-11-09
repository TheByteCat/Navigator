using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ECaptionType { pos, rot };

public class TextCaption : MonoBehaviour {

    public GameObject cam;
    Text caption;

    public ECaptionType caption_type = ECaptionType.pos; 



	// Use this for initialization
    void Start () {
        caption = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (caption && cam){
            switch (caption_type)
            {
                case ECaptionType.pos: 
                    caption.text = string.Format("{0} pos: {1}", cam.gameObject.name, cam.transform.position.ToString());
                    break;

                case ECaptionType.rot: 
                    caption.text = string.Format("{0} rot: {1}", cam.gameObject.name, cam.transform.rotation.eulerAngles.ToString());
                    break;
            }
        }
            
            
	}
}
