using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ObjectonPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Plane Prefab")]
    GameObject mPlacedPrefab;
    UnityEvent placementUpdate;

    [SerializeField]
    GameObject selectObjectPrefab;

    public GameObject plasedPrefab
    {
        get
        {
            return mPlacedPrefab;
        }

        set
        {
            mPlacedPrefab = value;
        }
    }

    public GameObject spawnedObject
    {
        get;
        private set;
    }

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null)
        {
            placementUpdate = new UnityEvent();
            placementUpdate.AddListener(DisableVisual);
        }
    }

    bool getTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!getTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, selectHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = selectHits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(mPlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
            placementUpdate.Invoke();
        }
    }

    public void DisableVisual()
    {
        selectObjectPrefab.SetActive(false);
    }

    static List<ARRaycastHit> selectHits = new List<ARRaycastHit>();
    ARRaycastManager raycastManager;

}
