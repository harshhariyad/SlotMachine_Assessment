# 🎰 Slot Machine Game - Technical Assessment

A clean, responsive, and modular 2D Slot Machine prototype built in Unity for an Engineering Internship Assessment. The project utilizes a decoupled architecture to separate game state, win evaluation logic, and UI display features.

🎮 **[Click Here to Play the Live WebGL Build](https://harshhariyad.github.io/SlotMachine_Assessment/)**

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
   ```
6. **Payout System:** The calculated payout is added to the wallet, and the UI is updated dynamically with smooth animations.

---

## 🎮 Instructions to Run WebGL Build

The project includes a ready-to-play WebGL build located in the `/Build/WebGL` folder.

**Option 1: Play Online**
Click the link at the top of this README to play the live version hosted on GitHub Pages.

**Option 2: Run Locally**
1. Clone this repository to your local machine.
2. Navigate to the `/Build/WebGL` folder.
3. Open `index.html` in your web browser. *(Note: Some browsers block local file execution for WebGL. If it doesn't load, use a local server like `python -m http.server` or a VS Code Live Server extension).*

---

## ✨ Bonus Features

- **Dynamic Bet Multipliers:** The amount won scales based on the specific bet size chosen ($10, $50, or $100).
- **Anti-Spam State Locking:** The game prevents players from clicking "Spin" multiple times or changing bets while the reels are spinning, ensuring the game state doesn't break.
- **Resolution Scaling:** Fully responsive Canvas layout that adapts gracefully to different screen sizes while maintaining the core 9:16 aspect ratio.

---

## 🧠 Thought Process & Approach

1. **Decoupling Logic & UI:** I started by separating the core mathematics (RNG, Win checking, Wallet balance) from the visual components (Reel animations, UI text updates). This makes the code much more modular and easier to debug.
2. **Deterministic Outcomes:** The outcome of the spin is determined *before* the visuals stop. This ensures that the game state is always 100% accurate and mathematically sound, and the animations are just a visual representation of that predetermined outcome.
3. **Smooth Coroutines:** I used Unity Coroutines for the reel spinning logic because they allow for precise timing and sequenced stopping (Reel 1, then Reel 2, then Reel 3), creating a more polished user experience.
4. **Project Structure:** I reorganized the project to keep `Scripts`, `Prefabs`, `Animations`, `UI`, and `Sounds` strictly separated in the `Assets/` directory, adhering to standard Unity best practices for maintainability.
