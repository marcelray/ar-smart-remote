using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class AppController : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    ARSessionOrigin _sessionOrigin;
    ARPlaneManager _planeManager;
    ARPointCloudManager _pointCloudManager;
    ARRaycastManager _raycastManager;

    EventSystem _eventSystem;

    Vector3 _touchStartPosition;
    ObjectHotspot _selectedHotspot;

    void Awake()
    {
        _sessionOrigin = FindObjectOfType<ARSessionOrigin>();
        _planeManager = FindObjectOfType<ARPlaneManager>();
        _pointCloudManager = FindObjectOfType<ARPointCloudManager>();
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    void Start()
    {
    }

    public void Update()
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            // TODO: Check for collision with existing hot spot and select it
            // Otherwise, place a new one
            _selectedHotspot = PlaceNewHotspot();
        }
        else if ( Input.GetMouseButton(0) && _selectedHotspot != null )
        {
            ResizeHotspot( _selectedHotspot );
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _selectedHotspot = null;
        }

    }

    private ObjectHotspot PlaceNewHotspot()
    {
        // Get placement point in front of camera based on placepoint visual size
        Ray ray = new Ray( Camera.main.transform.position , Camera.main.transform.forward );
		ObjectHotspot hotspot = Instantiate( prefab , Vector3.zero , Quaternion.identity ).GetComponent<ObjectHotspot>();
        Vector3 placePoint = ray.GetPoint( GameObjectUtils.GetBounds( hotspot.gameObject ).size.z );
        hotspot.transform.position = placePoint;

        // Save touch start position for scaling
        _touchStartPosition = Input.mousePosition;

        return hotspot;
    }

    private void ResizeHotspot( ObjectHotspot hotspot )
    {
        float _change = _touchStartPosition.y - Input.mousePosition.y;
        
        hotspot.SetVisualScale( _change / 100 );
    }

    private void EditHotspot(ObjectHotspot hotspot)
    {
        
    }

    private void RemoveHotspot()
    {
        
    }

    private void OnEnable() 
    {
        SetEvents();
    }

    private void OnDisable()
    {
        ClearEvents();
    }

    private void SetEvents()
    {
    }

    private void ClearEvents()
    {
    }
}
