using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class uiManager : MonoBehaviour
{
    // 1. Inventory wood counter
    public TMP_Text woodCounter;

    // 2-5. Beaver profession counters
    public TMP_Text idleCounter;
    public TMP_Text damWorkerCounter;
    public TMP_Text lumberjackCounter;
    public TMP_Text builderCounter;

    // 6-11. Buttons for profession management
    public Button decreaseDamWorkerBtn;
    public Button increaseDamWorkerBtn;
    public Button decreaseLumberjackBtn;
    public Button increaseLumberjackBtn;
    public Button decreaseBuilderBtn;
    public Button increaseBuilderBtn;

    // Reference to beaverManager and inventory
    public beaverManager manager;
    public int woodInventory = 0;

    void Start()
    {
        // Wire up button events
        decreaseDamWorkerBtn.onClick.AddListener(DecreaseDamWorker);
        increaseDamWorkerBtn.onClick.AddListener(IncreaseDamWorker);
        decreaseLumberjackBtn.onClick.AddListener(DecreaseLumberjack);
        increaseLumberjackBtn.onClick.AddListener(IncreaseLumberjack);
        decreaseBuilderBtn.onClick.AddListener(DecreaseBuilder);
        increaseBuilderBtn.onClick.AddListener(IncreaseBuilder);
    }

    void Update()
    {
        // Update counters
        woodCounter.text = woodInventory.ToString();

        if (manager != null && manager.allBeavers != null)
        {
            idleCounter.text = manager.allBeavers.Count(b => b.profession == BeaverProfession.Idle).ToString();
            damWorkerCounter.text = manager.allBeavers.Count(b => b.profession == BeaverProfession.DamWorker).ToString();
            lumberjackCounter.text = manager.allBeavers.Count(b => b.profession == BeaverProfession.Lumberjack).ToString();
            builderCounter.text = manager.allBeavers.Count(b => b.profession == BeaverProfession.Builder).ToString();
        }
        else
        {
            idleCounter.text = "0";
            damWorkerCounter.text = "0";
            lumberjackCounter.text = "0";
            builderCounter.text = "0";
        }
    }

    // 6. Decrease dam workers, return excess to idle
    void DecreaseDamWorker()
    {
        var damWorkers = GameObject.FindObjectsByType<beaverAI>(FindObjectsSortMode.None).Where(b => b.profession == BeaverProfession.DamWorker).ToList();
        if (damWorkers.Count > 0)
        {
            var toIdle = damWorkers[Random.Range(0, damWorkers.Count)];
            toIdle.profession = BeaverProfession.Idle;
            toIdle.targetObject = null;
        }
    }

    // 7. Increase dam workers from idle
    void IncreaseDamWorker()
    {
        var idles = GameObject.FindObjectsByType<beaverAI>(FindObjectsSortMode.None).Where(b => b.profession == BeaverProfession.Idle).ToList();
        if (idles.Count > 0)
        {
            var toDam = idles[Random.Range(0, idles.Count)];
            toDam.profession = BeaverProfession.DamWorker;
            toDam.targetObject = null; // Will auto-assign in Start/Update
        }
    }

    // 8. Decrease lumberjacks, return excess to idle
    void DecreaseLumberjack()
    {
        var lumberjacks = GameObject.FindObjectsByType<beaverAI>(FindObjectsSortMode.None).Where(b => b.profession == BeaverProfession.Lumberjack).ToList();
        if (lumberjacks.Count > 0)
        {
            var toIdle = lumberjacks[Random.Range(0, lumberjacks.Count)];
            toIdle.profession = BeaverProfession.Idle;
            toIdle.targetObject = null;
        }
    }

    // 9. Increase lumberjacks from idle
    void IncreaseLumberjack()
    {
        var idles = GameObject.FindObjectsByType<beaverAI>(FindObjectsSortMode.None).Where(b => b.profession == BeaverProfession.Idle).ToList();
        if (idles.Count > 0)
        {
            var toLumber = idles[Random.Range(0, idles.Count)];
            toLumber.profession = BeaverProfession.Lumberjack;
            toLumber.targetObject = null;
        }
    }

    // 10. Decrease builders, return excess to idle
    void DecreaseBuilder()
    {
        var builders = GameObject.FindObjectsByType<beaverAI>(FindObjectsSortMode.None).Where(b => b.profession == BeaverProfession.Builder).ToList();
        if (builders.Count > 0)
        {
            var toIdle = builders[Random.Range(0, builders.Count)];
            toIdle.profession = BeaverProfession.Idle;
            toIdle.targetObject = null;
        }
    }

    // 11. Increase builders from idle
    void IncreaseBuilder()
    {
        var idles = GameObject.FindObjectsByType<beaverAI>(FindObjectsSortMode.None).Where(b => b.profession == BeaverProfession.Idle).ToList();
        if (idles.Count > 0)
        {
            var toBuilder = idles[Random.Range(0, idles.Count)];
            toBuilder.profession = BeaverProfession.Builder;
            toBuilder.targetObject = null;
        }
    }
}