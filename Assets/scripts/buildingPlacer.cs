using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    public LayerMask placementMask;
    private GameObject previewInstance;
    private SpriteRenderer previewRenderer;
    private bool canPlace = false;
    private float blinkTimer = 0f;
    private bool isPlacing = false;

    public void StartPlacing()
    {
        if (previewInstance != null) Destroy(previewInstance);
        previewInstance = Instantiate(buildingPrefab);
        previewRenderer = previewInstance.GetComponent<SpriteRenderer>();
        SetPreviewColor(Color.red);
        isPlacing = true;
    }

void Update()
{
    if (!isPlacing || previewInstance == null) return;

    // Move preview to mouse
    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mouseWorld.z = 0;
    previewInstance.transform.position = mouseWorld;

    // Get the preview's own collider
    Collider2D previewCollider = previewInstance.GetComponent<Collider2D>();

    // Check for overlaps (excluding the preview itself)
    Collider2D[] overlaps = Physics2D.OverlapBoxAll(
        previewInstance.transform.position,
        previewRenderer.bounds.size,
        0f,
        placementMask
    );
    canPlace = true;
    foreach (var col in overlaps)
    {
        if (col != previewCollider)
        {
            canPlace = false;
            break;
        }
    }

    // Color logic
    if (canPlace)
    {
        blinkTimer += Time.deltaTime;
        float t = Mathf.Abs(Mathf.Sin(blinkTimer * 2f));
        SetPreviewColor(Color.Lerp(Color.green, Color.white, t));
    }
    else
    {
        SetPreviewColor(Color.red);
    }

    // Left click to place
    if (Input.GetMouseButtonDown(0) && canPlace && !EventSystem.current.IsPointerOverGameObject())
    {
        Instantiate(buildingPrefab, previewInstance.transform.position, Quaternion.identity);
        uiManager.Instance.woodInventory -= 50;
        Destroy(previewInstance);
        isPlacing = false;
    }
    // Right click to cancel
    else if (Input.GetMouseButtonDown(1))
    {
        Destroy(previewInstance);
        isPlacing = false;
    }
}

    private void SetPreviewColor(Color color)
    {
        if (previewRenderer != null)
            previewRenderer.color = color;
    }
}