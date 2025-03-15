using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingGridPlacer : BuildingPlacer
{
    public float cellSize;
    public Vector2 gridOffset;


    protected void Update()
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
                _toBuild.transform.position = _ClampToNearest(_hit.point, cellSize);


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
            else if (_toBuild.activeSelf) _toBuild.SetActive(false);
        }
    }
    private Vector3 _ClampToNearest(Vector3 pos, float threshold)
    {
        float t = 1f / threshold;
        Vector3 v = ((Vector3)Vector3Int.FloorToInt(pos * t)) / t;

        float s = threshold * 0.5f;
        v.x += s + gridOffset.x;
        v.z += s + gridOffset.y;

        return v;
    }
}
