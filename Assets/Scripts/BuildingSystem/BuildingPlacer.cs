using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer Instance;

    public LayerMask groundLayerMask;

    protected GameObject _buildingPrefab;
    public GameObject _toBuild;

    protected Camera _mainCamera;

    protected Ray _ray;
    protected RaycastHit _hit;

    [SerializeField] private EventSystem _eventSystem;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Update()
    {
        if (_buildingPrefab != null)
        {

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(_toBuild);
                _toBuild = null;
                _buildingPrefab = null;
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (_toBuild.activeSelf) _toBuild.SetActive(false);
            }
            else if (!_toBuild.activeSelf) _toBuild.SetActive(true);
          

            if (Input.GetKeyDown(KeyCode.R))
            {
                _toBuild.transform.Rotate(Vector3.up, 90);
                _toBuild.GetComponent<BuildingManager>().SetPlacementMode(
                    _toBuild.GetComponent<BuildingManager>().hasValidPlacement ?
                    PlacementMode.Valid : PlacementMode.Invalid
                );
            }

            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
            {
                if (!_toBuild.activeSelf) _toBuild.SetActive(true);
                _toBuild.transform.position = _hit.point;


                if (Input.GetMouseButtonDown(0))
                {
                    BuildingManager m = _toBuild.GetComponent<BuildingManager>();
                    if (m.hasValidPlacement)
                    {
                        m.SetPlacementMode(PlacementMode.Fixed);
                        _buildingPrefab = null;
                        _toBuild = null;

                    }
                }
            }
            else
            {
                if (_toBuild.activeSelf) _toBuild.SetActive(false);
            }
        }
    }
    public void SetBuildingPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        _PrepareBuilding();
    }

    protected virtual void _PrepareBuilding()
    {
        if (_toBuild) Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab);
        _toBuild.SetActive(false);

        BuildingManager m = _toBuild.GetComponent<BuildingManager>();
        m.isFixed = false;
        m.SetPlacementMode(PlacementMode.Valid);
    }

    
}
