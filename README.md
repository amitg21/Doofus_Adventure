# Doofus Adventure 

**Hitwicket Game Developer Challenge 2026**

## 📖 Overview
Meet Doofus, a cube that loves exploring green platforms called Pulpits! The catch? Pulpits don't last long and disappear within seconds. The ultimate challenge is to guide Doofus to walk on at least 50 Pulpits.

## 🎮 Gameplay Mechanics
- **The Goal:** Guide Doofus to walk on as many Pulpits as possible.
- **The Threat:** Each Pulpit has a limited timer. When the timer runs out, the platform is destroyed. If Doofus is still on it, he falls and the game ends!
- **Spawning Rules:** 
  - Only two Pulpits can exist simultaneously.
  - A new Pulpit appears adjacent to the previous one (never in the exact same position).
- **Dynamic Configuration:** Game parameters such as Doofus's movement speed and the Pulpit timers are dynamically loaded from `doofus_diary.json`.

## ⚙️ Controls
- **W, A, S, D** or **Arrow Keys** to move Doofus (Left, Right, Forward, Backward).

## 🚀 Implementation Stages
This project was built iteratively to ensure modular and robust code:
- **Level 1:** Implemented core character movement and platform placements, accurately reading values from the provided JSON file.
- **Level 2:** Developed the scoring system, updating the score after every successful move to a different pulpit.
- **Level 3:** Integrated the UI, including a "Start" screen and a "Game Over" screen.

## 📝 Developer Notes & Assumptions
- **JSON Interpretation:** The `doofus_diary.json` specifies `pulpit_spawn_time` as a fixed value (x seconds) and the pulpit destruction time as a range between y and z seconds. Thus, each pulpit's lifetime is randomized between the y and z bounds, and a new spawn is triggered after exactly x seconds.
- **Edge Cases:** The spawning logic strictly adheres to the "only two at once" rule. If a spawn is triggered but two platforms are already active, the system waits for one to be destroyed.

## 🛠️ Tech Stack
- **Engine:** Unity (Version 6+)
- **Environment:** 3D
- **Language:** C#
