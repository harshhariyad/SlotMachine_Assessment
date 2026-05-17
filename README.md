# 🎰 Slot Machine Game - Technical Assessment

A clean, responsive, and modular 2D Slot Machine prototype built in Unity for an Engineering Internship Assessment. The project utilizes a decoupled architecture to separate game state, win evaluation logic, and UI display features.

🎮 **[Click Here to Play the Live WebGL Build](https://harshhariyad.github.io/SlotMachine-Assessment/)**

---

## 🚀 Features & Gameplay

* **Multi-Bet System:** Players can choose their risk level using three dedicated bet buttons: **$10**, **$50**, or **$100**. Clicking a bet button instantly triggers a spin using that specific allocation.
* **Tiered Reward System:**
  *  **No Match:** No payout.
  *  **Partial Match (2/3 Reels):** Awards a **Small Win** (payout scales dynamically based on the current bet amount).
  *  **Three-of-a-Kind Match:** Awards a Standard Win.
  *  **7-7-7 Triple Match:** Triggers the ultimate **Jackpot** multiplier.
* **Anti-Spam Input Protection:** The entire betting interface and UI buttons are instantly locked down during execution cycles to guarantee data integrity and prevent state duplication.
* **Responsive Layout:** The UI scales cleanly using a Canvas Scaler matching a target resolution of `1080x1920 (9:16 Portrait)`.

---

## 🛠️ Project Architecture & Logic Flow

The engine is engineered using fundamental Game Programming Design Patterns to keep data evaluation fully isolated from visual components:

1. **Input Stage:** The user selects a bet variant ($10, $50, $100).
2. **State & Deduction:** The system verifies the wallet balance, subtracts the selected wager, locks the inputs, and flags the machine state to `Spinning`.
3. **Pseudo-RNG Evaluation:** An isolated backend array pre-determines the stopping frames mathematically using `UnityEngine.Random` weight matrices before the symbols visually stop moving.
4. **Visual Loop (The Illusion):** The UI Reels sample from a sprite inventory rapidly while executing a timed routine.
5. **Win Analyzer:** The system matches indices across rows:
   ```text
   If (Reel1 == Reel2 == Reel3) 
       If (Symbol == '7') -> JACKPOT MUTLIPIER
       Else -> STANDARD MULTIPLIER
   Else If (Reel1 == Reel2 || Reel2 == Reel3 || Reel1 == Reel3)
       -> SMALL WIN MULTIPLIER
