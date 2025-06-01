using UnityEngine;

public class beaverMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveDirection = Vector2.zero;
    private Camera mainCamera;
    public LayerMask obstacleMask = ~0;

    [Header("Sprite Change")]
    public Sprite normalSprite;
    public Sprite specialSprite;
    public string specialLayerName = "SpecialZone"; // Set this to your ignored layer name

    private SpriteRenderer spriteRenderer;
    private int specialLayer;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        specialLayer = LayerMask.NameToLayer(specialLayerName);
    }

    void Update()
    {
        if (moveDirection != Vector2.zero)
        {
            Vector2 targetPosition = (Vector2)transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime;

            // Raycast to check for obstacles, ignoring self
            RaycastHit2D[] hits = Physics2D.RaycastAll(
                transform.position,
                moveDirection.normalized,
                moveSpeed * Time.deltaTime,
                obstacleMask
            );
            bool blocked = false;
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != this.gameObject)
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
            {
                transform.position = targetPosition;
                // Rotate sprite to face movement direction, correcting for upside-down sprite
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(10, 10, angle + 90); // +90 if your sprite faces down, +270 if it faces up
            }
        }

        // Sprite change logic: check for overlap with special layer
        Collider2D myCollider = GetComponent<Collider2D>();
        bool inSpecial = false;
        if (myCollider != null)
        {
            if (myCollider != null)
            {
                Collider2D[] results = new Collider2D[10];
                int count = Physics2D.OverlapCollider(myCollider, new ContactFilter2D { layerMask = 1 << specialLayer, useLayerMask = true }, results);
                inSpecial = count > 0 && results[0] != null;
            }
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = inSpecial ? specialSprite : normalSprite;
        }

        // Clamp position to camera view
        Vector3 clampedPosition = ClampToCameraView(transform.position);
        if ((Vector2)transform.position != (Vector2)clampedPosition)
        {
            transform.position = clampedPosition;
        }
    }

    // Call this from other scripts to set movement direction
    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    // Keeps the object within the camera's view
    Vector3 ClampToCameraView(Vector3 position)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(position);
        viewportPos.x = Mathf.Clamp01(viewportPos.x);
        viewportPos.y = Mathf.Clamp01(viewportPos.y);
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0; // For 2D
        return worldPos;
    }
}