using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    private GameObject _spawnedGameObject;
    private uint ChestCount = 0;
    [SerializeField] private uint maxObjects = 0;
    private uint objectCount = 0;
    

    [SerializeField] 
    private GameObject placeableGameObject;

    [SerializeField] 
    private GameObject chestToSpawn;

    //List of raycast hits
    private static List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    
    //list of game objects to track
    private List<GameObject> _trackedObjects = new List<GameObject>();

    [SerializeField] private Animator anim;

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        anim = chestToSpawn.GetComponent<Animator>();
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

          
            //Spawn an object at this location & add it to the list of tracked objects if the number of objets are less than 3.
            if (objectCount < maxObjects)
            {
                /*
                _spawnedGameObject = Instantiate(placeableGameObject, hitpos.position, hitpos.rotation);
                _trackedObjects.Add(_spawnedGameObject);
                objectCount++;
                */
                
                SpawnPrefab(hitpos);
            }
            
            if (objectCount == maxObjects)
            {
                if (ChestCount < 1)
                {
                    _spawnedGameObject = Instantiate(chestToSpawn, hitpos.position, hitpos.rotation);
                    anim.Play("Chest_Lid_anim");
                    ChestCount++;
                }
                
            }
            
        }
    }
    

    //Function for checking if touched the screen. If the touchcount is higher than 0 we store the position of the first touch in the list
    bool TryGetTouchPosition(out Vector2 touchposition)
    {
        //Checks if we started touching the screen (so that we only ever place one object every touch)
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchposition = Input.GetTouch(0).position;
            return true;
        }

        touchposition = default;
        return false;
    }

    void SpawnPrefab(Pose hitpose)
    {
        _spawnedGameObject = Instantiate(placeableGameObject, hitpose.position, hitpose.rotation);
        _trackedObjects.Add(_spawnedGameObject);
        objectCount++;
    }
}
