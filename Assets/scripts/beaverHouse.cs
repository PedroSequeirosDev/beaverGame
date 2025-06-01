using UnityEngine;

public class beaverHouse : MonoBehaviour
{
    public float buildPoints = 30; // Set initial build points in Inspector
    public float maxBuildPoints = 30; // Set max build points in Inspector
    public bool isPlaced = true; // Default to true for already-placed houses

    private SpriteRenderer spriteRenderer;
    public bool IsBuilt => buildPoints <= 0;

    [SerializeField] private ParticleSystem buildFinishEffect;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOpacity();
    }

    void Update()
    {
        // Clamp buildPoints to never be less than 0
        if (buildPoints < 0)
            buildPoints = 0;

        UpdateOpacity();
    }

    // Call this method to reduce build points (e.g., by a beaver)
    public void Build(float amount)
    {
        buildPoints -= amount;
        if (buildPoints < 0) buildPoints = 0;

        if (buildPoints == 0)
        {
            var emissor = buildFinishEffect.emission;
            emissor.enabled = true;
            buildFinishEffect.Play();
        }

        UpdateOpacity();
    }

    private void UpdateOpacity()
    {
        if (spriteRenderer == null) return;

        float alpha = Mathf.Lerp(0.5f, 1f, 1f - Mathf.Clamp01((float)buildPoints / maxBuildPoints));
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}