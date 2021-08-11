using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Cursor : MonoBehaviour
{
    public GameObject cursorChildObject;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;

    public bool useCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    // Update is called once per frame
    void Update()
    {
        if(useCursor)
        {
            UpdateCursor();
        }
        //check if the user has touched/ is it just the user holding down the touch
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(useCursor)//cursor is true and placed on the surface
            {
                GameObject.Instantiate(objectToPlace, transform.position, transform.rotation);
                //just replace the cursor with object passing the posn and rotn of the cursor
            }
            else
            {
                //cursor is off and want to place the object where we touch.
                //make another raycast at the touch position
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                if(hits.Count > 0)
                {
                    GameObject.Instantiate(objectToPlace, hits[0].pose.position, hits[0].pose.rotation);
                }
            }
        }
    }

    void UpdateCursor()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if(hits.Count>0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }
}
