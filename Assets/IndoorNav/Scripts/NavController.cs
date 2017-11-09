using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.XR.iOS;


public class NavController : MonoBehaviour
{

    [Header("Vuforia")]
    public GameObject vcam;
    public GameObject vimage;

    [Header("ARKit")]
    public GameObject acam;
    public GameObject acam_mananger;
    public GameObject apoint_cloud;

    [Header("Navigation")]
    public bool FastStart = true;
    public GameObject world_holder;
    public GameObject nav_mesh;


    [HideInInspector]
    public Vector3 avg_vcam_pos;
    [HideInInspector]
    public Quaternion avg_vcam_rot;

    //[HideInInspector]
    //public Vector3 real_target_pos;
    //[HideInInspector]
    //public Quaternion real_target_rot;

    [Header("Other settings")]
    public GameObject text_object;



    //private & static
    Text status_text;
    public static string status;

    Quaternion temp_acam_rot;

    bool arkit_enable = false;
    bool path_showing = false;


    //singleton


    static NavController _nav;
    public static NavController instance
    {
        get
        {
            if (!_nav)
                _nav = GameObject.FindWithTag("nav_controller").GetComponent<NavController>();

            return _nav;
        }
    }


    IEnumerator ARSwitch()
    {
        NavController.status = "turn off vuforia";
        //turn off Vuforia
        vcam.SetActive(false);
        vimage.SetActive(false);

        //VuforiaRuntime.Instance.Deinit();

        //wait for frame to init
        yield return 0;

        //turn on ARKit
        NavController.status = "turn on ARKit";

        acam.SetActive(true);
        acam_mananger.SetActive(true);
        apoint_cloud.SetActive(true);

        //wait for frame
        yield return new WaitForSeconds(0.5f);

        //save current arkit cam rotation
        temp_acam_rot = acam.transform.rotation;

        if (FastStart)
            ShowPath();

        //set flags
        arkit_enable = true;

    }


    public void ProcessImageStep()
    {
        StartCoroutine(ARSwitch());
    }


    void ShowPath()
    {
        NavController.status = "turn on nav mesh";
        

        //setup nav meshes
        world_holder.transform.position = avg_vcam_pos * -1f;
        world_holder.transform.rotation = Quaternion.Inverse(avg_vcam_rot * Quaternion.Inverse(temp_acam_rot));

        //ajust rotation and pos change after ARKit started
        //nav_mesh.transform.position -= acam.transform.position;
        //nav_mesh.transform.rotation *= Quaternion.Inverse(temp_acam_rot); 

        //setup flags
        world_holder.SetActive(true);
        path_showing = true;
    }


    void TryShowPath()
    {
        if ((path_showing || !arkit_enable) && !FastStart)
            return;



        var screenPosition = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width / 2, Screen.height / 2));

        ARPoint point = new ARPoint {
            x = screenPosition.x,
            y = screenPosition.y
        };

        // prioritize reults types
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, ARHitTestResultType.ARHitTestResultTypeFeaturePoint);

        if (hitResults.Count > 0) {
            ShowPath();
        }
    }


    // Use this for initialization
    void Start()
    {
        status_text = text_object.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        status_text.text = NavController.status;

        TryShowPath();
    }
}
