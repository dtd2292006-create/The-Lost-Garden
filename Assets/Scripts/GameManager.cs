using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int totalSeeds = 8;

    private int collectedSeeds;
    private bool gateUnlocked;

    public int CollectedSeeds
    {
        get { return collectedSeeds; }
    }

    public int TotalSeeds
    {
        get { return totalSeeds; }
    }

    public bool GateUnlocked
    {
        get { return gateUnlocked; }
    }

    public event Action<int, int> OnSeedCountChanged;
    public event Action OnGateUnlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        collectedSeeds = 0;
        gateUnlocked = false;

        OnSeedCountChanged?.Invoke(
            collectedSeeds,
            totalSeeds
        );
    }

    public void CollectSeed(int amount)
    {
        if (gateUnlocked)
            return;

        collectedSeeds += amount;
        collectedSeeds = Mathf.Min(
            collectedSeeds,
            totalSeeds
        );

        OnSeedCountChanged?.Invoke(
            collectedSeeds,
            totalSeeds
        );

        if (collectedSeeds >= totalSeeds)
        {
            gateUnlocked = true;
            OnGateUnlocked?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}