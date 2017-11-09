using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject startPanel;
    public GameObject scanningPanel;
    public GameObject menuPanel;

    public Image scanningProgress;

    //singleton
    static UIManager _ui;
    public static UIManager instance
    {
        get
        {
            if (!_ui)
                _ui = GameObject.FindWithTag("UI").GetComponent<UIManager>();

            return _ui;
        }
    }
    
    // Use this for initialization
    void Start () {
        OpenStartScreen();
	}


    public void SetScannimgProgress(float progress)
    {
        scanningProgress.fillAmount = progress;
    }

    public void HideAll()
    {
        startPanel.SetActive(false);
        scanningPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void OpenStartScreen()
    {
        HideAll();
        startPanel.SetActive(true);        
    }

    public void OpenScanningScreen()
    {
        startPanel.SetActive(false);
        scanningPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OpenMenuScreen()
    {
        startPanel.SetActive(false);
        scanningPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
