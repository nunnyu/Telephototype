## Overview

Tellephoto is a top-down 2D game that combines exploration, combat, and narrative elements. The core gameplay revolves around using a camera to interact with the world, capture spirits, and progress through story-driven levels. The game features a custom animation system, diverse enemy AI behaviors, and a dialogue system that drives the narrative forward.

### Technical

#### Input Management
- Unity Input System 1.11.2 for modern input handling
- Supports keyboard and gamepad input
- Configurable action maps for player movement, camera controls, combat, and interaction

#### Physics and Movement
- Physics2D system for collision detection and rigidbody interactions
- Custom collision detection using BoxCast for precise movement boundaries
- Rigidbody2D-based player movement with configurable speed and physics parameters
- Layer-based collision system for selective object interactions

#### Animation System
- Unity Animator Controllers for state machine-based animations
- Custom procedural animation system for dynamic limb positioning
- Sorting order management for sprite layering in 2D space
- Runtime sprite rotation and scaling for directional animations

#### Gameplay Systems
- **Camera/Photography Mechanic**: Directional camera poses with cooldown system and visual feedback
- **Combat System**: Enemy AI with attack patterns, health management, and spawn systems
- **Dialogue System**: Text-based dialogues with sprite portraits and audio support
- **Checkpoint System**: Save point management with world state reset functionality
- **Spirit Capture**: Game state tracking for captured spirits with persistence across scenes

#### Scene Management
- Multi-scene architecture with scene transition handling
- Fade-in/fade-out transitions between levels
- Persistent GameManager using DontDestroyOnLoad pattern
- Event-driven scene progression system

#### Audio System
- AudioSource management for sound effects and music
- Background music controller with crossfade capabilities
- Randomized audio clip selection for variety
- Spatial audio integration where applicable

### Code Architecture

#### Script Organization
- Modular script structure separated by functionality
- Character-specific controllers for unique behaviors
- Reusable components for common game mechanics
- Static state management for cross-scene communication

#### Key Design Patterns
- Singleton pattern for GameManager instance
- Event-driven architecture using UnityEvents
- Component-based design following Unity conventions
- State machine pattern for character AI behaviors

#### Performance Considerations
- Object pooling considerations for frequently instantiated objects
- Efficient collision detection using layer masks
- Optimized sprite rendering with sorting layers
- Resource management for audio and visual effects

### Dependencies
- Universal Render Pipeline 17.0.3
- TextMesh Pro for UI text rendering

## Design

### Visual Style

#### Art Direction
- Pixel art aesthetic with hand-crafted sprites
- Consistent color palette maintaining visual cohesion
- Detailed character animations with multi-part sprite composition
- Environmental tilemaps for level construction

#### Character Design
- **Main Character (Rinko)**: Player-controlled character with complex sprite layering system
  - Scarf physics and positioning system
  - Dynamic limb positioning for directional movement
  - Multiple animation states for idle, walking (up/down/side), and camera pose
  - Sorting order management for proper depth representation

- **Supporting Characters**: Unique designs with individual animation sets
  - Haneko: Custom attack behaviors and animations
  - Yonaki: Distinct attack patterns and visual effects
  - Enemy variants: Gobbler, Chuedevil, Lockling, and others with unique behaviors

#### Animation Design
- Frame-by-frame animation sequences for character actions
- Procedural animation system for scarf and accessory movement
- Smooth transitions between animation states
- Directional sprite flipping for efficient asset usage
- Layered sprite system allowing independent movement of body parts

#### Level Design
- Tilemap-based level construction for efficient level creation
- Environmental storytelling through visual design
- Interactive elements integrated into level geometry

### Gameplay Design

#### Core Mechanics
- **Photography System**: Central gameplay mechanic requiring players to take pictures to progress
  - Directional camera poses based on player facing direction
  - Cooldown system preventing spam usage
  - Visual and audio feedback for player actions
  - Integration with game progression systems

- **Exploration**: Top-down navigation through interconnected areas
  - Collision-based level boundaries
  - Trigger zones for scripted events
  - Checkpoint system for player progression

- **Combat**: Action-oriented encounters with various enemy types
  - Enemy AI with distinct attack patterns
  - Health management for player and enemies
  - Dynamic difficulty through enemy spawn management

- **Story Progression**: Narrative-driven gameplay with dialogue sequences
  - Dialogue system with character portraits
  - Voice acting integration through audio clips
  - Event triggers for story moments
  - Multi-part narrative structure

#### User Interface
- Minimalist UI design maintaining immersion
- Health indicator system
- Dialogue UI with text display and character sprites
- Camera ready indicator for gameplay feedback
- Menu system for scene transitions

#### Audio Design
- Background music system with scene-appropriate tracks
- Sound effects for player actions (walking, camera shutter, combat)
- Audio integration in dialogue system
- Randomized audio variation for repetitive actions

### Narrative Design

#### Story Structure
- Multi-scene narrative progression
- Character-driven storytelling through dialogue
- Spirit capture mechanics integrated with narrative themes
- Tutorial integration for player onboarding

#### Character Development
- Unique personalities for supporting characters
- Dialogue-driven character exposition
- Visual storytelling through character animations and expressions
- Narrative themes exploring personal growth and relationships

### User Experience

#### Controls
- Intuitive input mapping for accessibility
- Responsive movement system with configurable sensitivity
- Clear feedback for all player actions
- Smooth camera transitions and scene loading

#### Pacing
- Checkpoint system preventing excessive player frustration
- Gradual difficulty curve
- Balanced exploration and combat encounters
- Narrative beats integrated into gameplay flow

#### Accessibility Considerations
- Configurable input options through Unity Input System
- Clear visual feedback for all interactions
- Audio cues supplementing visual information
- Respawn system maintaining player progress appropriately

