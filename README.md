# Yash Game System

A robust, decoupled, and modular Manager System for Unity.

## 🌟 Features

-   **Hub & Spoke Architecture:** Single entry point (`MainManager`) for all game systems.
    
-   **Zero-Dependency Managers:** Managers communicate via a Type-Safe Event Bus.
    
-   **Auto-Bootstrapping:** The system loads itself automatically; no need to drag prefabs into every scene.
    
-   **State Machine:** Integrated `GameStateManager` for flow control (Menu -> Game -> Pause).
    
-   **Scene Loading:** Integrated wrapper for async scene transitions and loading screens.
    

----------

## 🚀 Installation

1.  Open Unity Package Manager.
    
2.  Click `+` -> **Add package from disk...**
    
3.  Select the `package.json` file from this folder.
    

----------

## 🛠️ Setup Guide

### 1. Create the Bridge (`Game.cs`)

Since this package is read-only, you must create the access bridge in your project. Create a script named `Game.cs` in your `Assets/Scripts` folder:

C#

```
using Yash.GameSystem.Core;
using Yash.GameSystem.Services;

public static class Game
{
    // Access to Core Package Managers
    public static GameStateManager State => MainManager.Instance.Get<GameStateManager>();
    public static SceneLoaderManager Scene => MainManager.Instance.Get<SceneLoaderManager>();
    
    // ADD YOUR CUSTOM MANAGERS BELOW:
    // public static AudioManager Audio => MainManager.Instance.Get<AudioManager>();
}

```

### 2. Setup the Prefab

1.  Create an empty GameObject named **`GameSystem`**.
    
2.  Add component **`MainManager`**.
    
3.  Create child objects for `GameStateManager` and `SceneLoaderManager` and attach their scripts.
    
4.  **Crucial:** Make it a Prefab and move it to `Assets/Resources/GameSystem.prefab`.
    
5.  Delete it from the scene.
    

----------

## 📖 How to Add a New Manager

### Step 1: Write the Script

Create a new script (e.g., `AudioManager.cs`) inheriting from `BaseManager`.

C#

```
using UnityEngine;
using Yash.GameSystem.Core;

public class AudioManager : BaseManager
{
    protected override void OnInitialize()
    {
        Debug.Log("Audio Initialized");
    }

    public void PlayMusic(string track) { /* Logic */ }
}

```

### Step 2: Register it

1.  Open your `GameSystem` prefab.
    
2.  Add a child object.
    
3.  Attach `AudioManager`.
    

### Step 3: Expose it

Open your local `Game.cs` file and add the property:

C#

```
public static AudioManager Audio => MainManager.Instance.Get<AudioManager>();

```

### Step 4: Use it

C#

```
public class Player : MonoBehaviour
{
    void Start()
    {
        Game.Audio.PlayMusic("BattleTheme");
    }
}

```

----------

## 📡 Event System (Communication)

Do not reference managers directly if possible. Use the Event Bus.

**1. Define Event**

C#

```
public struct PlayerDiedEvent { public int GoldLost; }

```

**2. Subscribe (Inside a Manager)**

C#

```
protected override void OnInitialize()
{
    EventBus.Subscribe<PlayerDiedEvent>(OnDeath);
}

private void OnDeath(PlayerDiedEvent e)
{
    Debug.Log($"Lost {e.GoldLost} gold.");
}

```

**3. Trigger (From Gameplay)**

C#

```
EventBus.Raise(new PlayerDiedEvent { GoldLost = 50 });

```

----------

## 🧠 State Management

Switch game states easily.

C#

```
// Switch to Gameplay
Game.State.SwitchState(new GameplayState(Game.State));

// Define a new state
public class GameplayState : BaseGameState
{
    public GameplayState(GameStateManager ctx) : base(ctx) { }
    
    public override void EnterState() { Debug.Log("Game Start"); }
    public override void UpdateState() { /* Run Logic */ }
    public override void ExitState() { Debug.Log("Game End"); }
}
```
