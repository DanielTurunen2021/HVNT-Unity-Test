using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    private GameObject _spawnedGameObject;

    [SerializeField] 
    private GameObject placeableGameObject;

    //List of raycast hits
    private static List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    
    //list of game objects to track
    private List<GameObject> _trackedObjects = new List<GameObject>();

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If we are not touching the screen, return and do nothing
        if (!TryGetTouchPosition(out Vector2 touchposition))
        {
            return;
        }

        if (_raycastManager.Raycast(touchposition, _raycastHits, TrackableType.PlaneWithinPolygon))
        {
            //if we touched the screen. store the first position.
            var hitpos = _raycastHits[0].pose;

            //If we don't already have an object at this location.
            //Spawn an object at this location & add it to the list of tracked objects if the number of objets are less than 3.
            if (_spawnedGameObject == null && _trackedObjects.Count < 3)
            {
                _spawnedGameObject = Instantiate(placeableGameObject, hitpos.position, hitpos.rotation);
                _trackedObjects.Add(_spawnedGameObject);
            }
            else
            {
                _spawnedGameObject.transform.position = hitpos.position;
                _spawnedGameObject.transform.rotation = hitpos.rotation;
            }
        }
    }

    //Function for checking if touched the screen. If the touchcount is higher than 0 we store the position of the first touch in the list
    bool TryGetTouchPosition(out Vector2 touchposition)
    {
        if (Input.touchCount > 0)
        {
            touchposition = Input.GetTouch(0).position;
            return true;
        }

        touchposition = default;
        return false;
    }
}
