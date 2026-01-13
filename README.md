# Sprite Grid

Implements a customizable grid system with interactive sprite-based cells. Each cell consists of multiple layers (background, sprite, border) that can be individually customized and interacted with.

## Overview

Sprite Grid is a Unity-based grid system that creates and manages a dynamic grid of cells. Each cell is composed of three distinct layers that can be independently controlled, allowing for flexible visual customization and interactive gameplay mechanics.

## Features

- **Multi-layered Cell System**: Each cell consists of three layers:
  - Background layer (base cell appearance)
  - Sprite layer (numbered sprites 1-9)
  - Border layer (cell outline)
  
- **Dynamic Grid Creation**: Configure grid dimensions, cell size, and spacing through Unity Inspector
- **Interactive Controls**: 
  - Click and drag to interact with cells
  - Mouse-based cell selection
  - Input system integration via Unity's new Input System
  
- **Flexible Customization**:
  - Adjustable cell colors
  - Configurable cell spacing and sizing
  - Automatic camera adjustment to fit grid
  
- **Event-Driven Architecture**: Event system for grid creation and cell interactions
- **Undo/Redo System**: Built-in support for action reversal
- **Unit Testing**: NUnit test suite included

## Project Structure

```
Assets/
├── Input/                  # Input system configuration
│   └── PlayerInputActions  # Input action mappings
├── Prefabs/               # Reusable game objects
├── Resources/             # Sprite assets and resources
├── Scenes/                # Unity scenes
│   └── DemoScene.unity    # Main demo scene
└── Scripts/
    ├── Controllers/       # Game flow controllers
    │   └── Actions/       # Action system
    ├── Game/
    │   ├── Grid/
    │   │   ├── Cell/      # Cell implementation
    │   │   │   └── SpriteGridCell.cs
    │   │   ├── Layers/    # Layer system
    │   │   │   └── SpriteGridCellLayer.cs
    │   │   └── SpriteGrid.cs
    │   └── UI/            # UI components
    └── Managers/          # Core managers
        ├── CameraManager.cs
        └── GameManager.cs
```

## Key Components

### SpriteGrid
The main grid controller that:
- Creates and manages the grid of cells
- Handles mouse input and drag operations
- Broadcasts grid creation events
- Manages cell interactions

### SpriteGridCell
Represents an individual cell with:
- Three independent layers (background, sprite, border)
- Cell position and identification
- Color customization
- Collider for mouse interaction

### SpriteGridCellLayer
Base class for all cell layers that:
- Manages sprite rendering
- Handles layer scaling and positioning
- Supports dynamic color updates
- Implements layer-specific modifications

### GameManager
Orchestrates the overall game flow:
- Responds to grid creation
- Handles cell click events
- Manages undo/redo operations

### CameraManager
Automatically adjusts the camera to frame the entire grid based on grid dimensions and cell sizes.

## Getting Started

### Prerequisites
- Unity 2021.3 or later
- TextMesh Pro package
- Unity Input System package

### Setup
1. Clone or download this repository
2. Open the project in Unity
3. Open the DemoScene located in `Assets/Scenes/`
4. Press Play to see the grid system in action

### Configuration
Select the SpriteGrid GameObject in the hierarchy to adjust:
- **Width**: Number of cells horizontally
- **Height**: Number of cells vertically
- **Cell Size**: Size of each cell
- **Cell Spacing**: Gap between cells
- **Cell Color**: Default background color for cells

## Usage Example

```
// Create a new grid programmatically
var gridObject = new GameObject("SpriteGrid");
var grid = gridObject.AddComponent<SpriteGrid>();

// Subscribe to grid events
grid.GridCreated += OnGridCreated;
grid.GridCellMouseDown += OnCellClicked;

// Access individual cells
void OnCellClicked(SpriteGridCellInfo cellInfo)
{
    // Change cell color
    cellInfo.Cell.SetColor(1f, 0f, 0f); // Set to red
    
    // Access cell properties
    Debug.Log($"Cell clicked at: {cellInfo.X}, {cellInfo.Y}");
}
```

## Testing

Unit tests are located in the `SpriteGridTest/` directory. Run tests through Unity's Test Runner window (Window → General → Test Runner).

Current test coverage:
- Cell instantiation
- Color modification
- Layer creation and management

## Build

The project includes build artifacts in the `Build/` directory. To create a new build:
1. File → Build Settings
2. Select target platform
3. Click Build

## Technical Details

- **Layer System**: Uses Unity's sorting order for proper layer rendering
- **Input System**: Implements Unity's new Input System for cross-platform compatibility
- **Sprite Rendering**: Uses sliced sprite draw mode for scalable visuals
- **Resource Loading**: Sprites loaded from Resources folder at runtime
 
