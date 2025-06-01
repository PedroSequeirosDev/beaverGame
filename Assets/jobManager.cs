using UnityEngine;

public class jobManager : MonoBehaviour
{
    // this script changes the sprite in the sprite renderer according to the job the beaver has

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite houseSprite;
    [SerializeField] private Sprite damSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite lumberSprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void GetJob(string job)
    {
        switch (job)
        {
            case "house":
                spriteRenderer.sprite = houseSprite;
                break;
            case "dam":
                spriteRenderer.sprite = damSprite;
                break;
            case "idle":
                spriteRenderer.sprite = idleSprite;
                break;
            case "lumber":
                spriteRenderer.sprite = lumberSprite;
                break;
        }
    }
    
    
}
