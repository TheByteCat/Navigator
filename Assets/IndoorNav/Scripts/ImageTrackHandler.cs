using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ImageTrackHandler : MonoBehaviour, ITrackableEventHandler
{

    public bool UseAvgTracking = true;



    int avg_counter = 100;
    int avg_amount = 0;
    bool tracking = false;
    bool marckTracking = false;
    Vector3 realPos;
    Quaternion realRot;


    protected TrackableBehaviour mTrackableBehaviour;


    protected virtual void Start()
    {
        realPos = transform.position;
        realRot = transform.rotation;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }


    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        } else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                   newStatus == TrackableBehaviour.Status.NOT_FOUND) {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        } else {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }


    protected virtual void OnTrackingFound()
    {
        //reset traction params
        NavController.instance.avg_vcam_pos = Vector3.zero;
        NavController.instance.avg_vcam_rot = Quaternion.identity;
        avg_counter = 100;
        avg_amount = 0;

        //run tracking loop
        tracking = true;

    }


    protected virtual void OnTrackingLost()
    {
        tracking = false;
        marckTracking = false;
    }

    public void EnableMarckTrack()
    {
        if (tracking)
            marckTracking = true;
    }


    private void Update()
    {
        if (tracking && marckTracking) {
            avg_counter--;
            UIManager.instance.SetScannimgProgress(1 - (avg_counter / 100));

            if (avg_counter > 0) {
                NavController.status = "tracking: " + avg_counter;

                if (avg_counter < 55 && avg_counter > 45) {

                    avg_amount++;

                    if (UseAvgTracking) {
                        NavController.instance.avg_vcam_pos = Vector3.Lerp(NavController.instance.avg_vcam_pos, NavController.instance.vcam.transform.position, 1f / avg_amount);
                        NavController.instance.avg_vcam_rot = Quaternion.Lerp(NavController.instance.avg_vcam_rot, NavController.instance.vcam.transform.rotation, 1f / avg_amount);
                    } else {
                        NavController.instance.avg_vcam_pos = NavController.instance.vcam.transform.position;
                        NavController.instance.avg_vcam_rot = NavController.instance.vcam.transform.rotation;
                    }

                }


            } else {
                NavController.status = "Running switch...";

                NavController.instance.ProcessImageStep();
                UIManager.instance.OpenMenuScreen();
            }

        } else {
            NavController.status = "not tracking";
        }
    }
}
