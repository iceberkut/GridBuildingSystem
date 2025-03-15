using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    

    public GameObject selectedObject;

    private BuildingGridPlacer buildingGridPlacer;

    public GameObject UIDeleteButton;
    public BuildingPlacer buildingPlacer;


    void Start()
    {
        buildingGridPlacer = GameObject.Find("BuildingPlacerManager").GetComponent<BuildingGridPlacer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI() && !HasActiveBuilding())
        {
            HandleSelection();
        }

        if (Input.GetMouseButtonDown(1) && selectedObject != null)
        {
            Deselect();
        }
    }

    private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    private bool HasActiveBuilding() => buildingPlacer._toBuild != null;
    private void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000) && hit.collider.CompareTag("Building"))
        {
            Select(hit.collider.gameObject);
        }
        else
        {
            Deselect();
        }
    }

    public void Select(GameObject Object)
    {
        if (Object == selectedObject) return;
        if (selectedObject != null) Deselect();

        Outline outline = Object.GetComponent<Outline>();
        if (outline == null)
        {
            outline = Object.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 3f;
        }
        outline.enabled = true;

        UIDeleteButton.SetActive(true);
        selectedObject = Object;
    }

    public void Deselect()
    {
        UIDeleteButton.SetActive(false);
        if (selectedObject != null)
        {
            Outline outline = selectedObject.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
        }
        selectedObject = null;
    }

    public void Delete()
    {
        if (selectedObject != null)
        {
            GameObject objectToDestroy = selectedObject;
            Deselect();
            Destroy(objectToDestroy);
        }
    }

}
