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

    Vector3 _lastTouchPosition;
    SpatialTarget _selectedTarget;

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
            // Check if user tapped on an existing target
            RaycastHit raycastHit;
            Ray ray = _sessionOrigin.camera.ScreenPointToRay( Input.mousePosition );
            LayerMask layerMask = LayerMask.GetMask( "Targets" );
            if ( Physics.Raycast(ray, out raycastHit, 20 , layerMask ) )
            {
                _selectedTarget = raycastHit.collider.gameObject.GetComponentInParent<SpatialTarget>();
            }
            // Otherwise, place a new one
            else
            {
                _selectedTarget = PlaceNewHotspot();
            }
            _selectedTarget.selected = true;
        }
        else if ( Input.GetMouseButton(0) && _selectedTarget != null )
        {
            ResizeHotspot( _selectedTarget );
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _selectedTarget.selected = false;
            _selectedTarget = null;
        }

    }

    private SpatialTarget PlaceNewHotspot()
    {
        // Get placement point in front of camera based on placepoint visual size
        Ray ray = new Ray( Camera.main.transform.position , Camera.main.transform.forward );
		SpatialTarget hotspot = Instantiate( prefab , Vector3.zero , Quaternion.identity ).GetComponent<SpatialTarget>();
        Vector3 placePoint = ray.GetPoint( GameObjectUtils.GetBounds( hotspot.gameObject ).size.z );
        hotspot.transform.position = placePoint;

        _lastTouchPosition = Input.mousePosition;

        return hotspot;
    }

    private void ResizeHotspot( SpatialTarget hotspot )
    {
        float _change = _lastTouchPosition.y - Input.mousePosition.y;
        
        hotspot.SetRelativeVisualScale( _change / 1000 );

        _lastTouchPosition = Input.mousePosition;
    }

    private void EditHotspot(SpatialTarget hotspot)
    {
        
    }

    private void RemoveHotspot()
    {
        
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log( "OnPointerDown() >> count: " + data.clickCount );
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
