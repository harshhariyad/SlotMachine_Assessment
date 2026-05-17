using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class SlotMachine : MonoBehaviour
{
    [Header("UI Elements")]
    public Button bet10Button;
    public Button bet50Button;
    public Button bet100Button;
    public Text resultText;
    [Tooltip("Optional: Assign a Text component to display credits.")]
    public Text creditsText; 

    [Header("Timeline Animation")]
    [Tooltip("The PlayableDirector that controls the handle animation Timeline.")]
    public PlayableDirector handleTimeline;

    [Header("Reels Setup")]
    // Drag your 3 Reel GameObjects here in the Inspector
    public GameObject[] reels; 
    
    [Header("Available Symbols")]
    // Drag your different symbol Sprites here in the Inspector
    public Sprite[] symbolSprites; 

    private bool isSpinning = false;
    private int credits = 1000;
    private int betAmount = 50;

    void Start()
    {
        // Link the button clicks to our Spin function with respective bet amounts
        if (bet10Button != null) bet10Button.onClick.AddListener(() => StartSpin(10));
        if (bet50Button != null) bet50Button.onClick.AddListener(() => StartSpin(50));
        if (bet100Button != null) bet100Button.onClick.AddListener(() => StartSpin(100));
        UpdateUI("Press a Bet button to Play!");
    }

    void StartSpin(int amount)
    {
        if (!isSpinning && credits >= amount)
        {
            betAmount = amount;
            credits -= betAmount;

            if (handleTimeline != null)
            {
                handleTimeline.time = 0; // Rewind to start
                handleTimeline.Play();
            }

            StartCoroutine(SpinRoutine());
        }
        else if (!isSpinning && credits < amount)
        {
            UpdateUI("Not enough credits!");
        }
    }

    // A Coroutine allows us to pause code execution over time (perfect for animations)
    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        SetButtonsInteractable(false); // Disable buttons so player can't spam them
        UpdateUI("Spinning...");

        int numReels = reels.Length;
        int[] finalIDs = new int[numReels];
        
        // 4. Randomized Outcomes: Determine final outcomes using proper RNG
        for (int i = 0; i < numReels; i++)
        {
            finalIDs[i] = Random.Range(0, symbolSprites.Length);
        }

        // 2. Smooth Reel Animations: Staggered stopping
        float[] spinDurations = { 1.0f, 2f, 3.0f }; // Reels stop one after another
        bool[] stopped = new bool[numReels];
        float elapsedTime = 0f;
        float spinInterval = 0.05f; // Control spinning speed for a cleaner look instead of tying to framerate

        while (elapsedTime < spinDurations[Mathf.Min(numReels - 1, spinDurations.Length - 1)])
        {
            for (int i = 0; i < numReels; i++)
            {
                if (i < spinDurations.Length && elapsedTime >= spinDurations[i] && !stopped[i])
                {
                    // Stop this reel
                    stopped[i] = true;
                    SetReelSymbols(reels[i], finalIDs[i]);
                }
                else if (!stopped[i])
                {
                    // Continue spinning this reel
                    SetReelSymbols(reels[i], Random.Range(0, symbolSprites.Length));
                }
            }

            elapsedTime += spinInterval;
            yield return new WaitForSeconds(spinInterval); // Ensures smooth animation across all devices
        }

        // Ensure all reels are set to their final state
        for (int i = 0; i < numReels; i++)
        {
            SetReelSymbols(reels[i], finalIDs[i]);
        }

        // 5 & 6. Winning Combinations & Bonus Features
        EvaluateWin(finalIDs);

        isSpinning = false;
        SetButtonsInteractable(true); // Re-enable the buttons
    }

    void SetButtonsInteractable(bool state)
    {
        if (bet10Button != null) bet10Button.interactable = state;
        if (bet50Button != null) bet50Button.interactable = state;
        if (bet100Button != null) bet100Button.interactable = state;
    }

    // 3. Clean Symbol Display: Keep symbols aligned properly on the "belt"
    void SetReelSymbols(GameObject reel, int middleSymbolID)
    {
        Image[] images = reel.GetComponentsInChildren<Image>();
        if (images.Length >= 3)
        {
            // Top symbol (ID - 1)
            int topID = (middleSymbolID + symbolSprites.Length - 1) % symbolSprites.Length;
            images[0].sprite = symbolSprites[topID];

            // Middle symbol
            images[1].sprite = symbolSprites[middleSymbolID];

            // Bottom symbol (ID + 1)
            int bottomID = (middleSymbolID + 1) % symbolSprites.Length;
            images[2].sprite = symbolSprites[bottomID];
        }
        else if (images.Length > 0)
        {
            // Fallback if the layout isn't standard top/middle/bottom
            foreach (Image img in images)
            {
                img.sprite = symbolSprites[middleSymbolID];
            }
        }
    }

    void EvaluateWin(int[] finalIDs)
    {
        // Provide escalating multipliers for higher bets
        int jackpotMultiplier = 10;
        int winMultiplier = 5;
        int smallWinMultiplier = 2;

        if (betAmount >= 50 && betAmount < 100)
        {
            jackpotMultiplier = 30;
            winMultiplier = 10;
            smallWinMultiplier = 3;
        }
        else if (betAmount >= 100)
        {
            jackpotMultiplier = 50;
            winMultiplier = 15;
            smallWinMultiplier = 5;
        }

        // 1. Winning Logic: Check if all slots have the same symbol
        if (finalIDs.Length >= 3 && finalIDs[0] == finalIDs[1] && finalIDs[1] == finalIDs[2])
        {
            int winID = finalIDs[0];
            
            // 6. Bonus Features: Special payout for the first symbol (e.g., Jackpot symbol)
            if (winID == 0) 
            {
                int payout = betAmount * jackpotMultiplier;
                credits += payout;
                UpdateUI($"MEGA JACKPOT! You won {payout} credits!");
            }
            else
            {
                int payout = betAmount * winMultiplier;
                credits += payout;
                UpdateUI($"WINNER! You won {payout} credits!");
            }
        }
        // Small win for 2 matching symbols
        else if (finalIDs.Length >= 3 && (finalIDs[0] == finalIDs[1] || finalIDs[1] == finalIDs[2] || finalIDs[0] == finalIDs[2]))
        {
            int payout = betAmount * smallWinMultiplier;
            credits += payout;
            UpdateUI($"Small Win! You won {payout} credits!");
        }
        else
        {
            UpdateUI("Try Again!");
        }
    }

    void UpdateUI(string message)
    {
        if (resultText != null) resultText.text = message;
        if (creditsText != null) creditsText.text = $"Credits: {credits}";
    }
}