# ChessGame
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET 8](https://img.shields.io/badge/.NET%208.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-0C54C2?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
[![MVVM](https://img.shields.io/badge/Architecture-MVVM-0078D4?style=for-the-badge)](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
[![LAN Multiplayer](https://img.shields.io/badge/Multiplayer-LAN-2EA043?style=for-the-badge&logo=cisco&logoColor=white)](#)

A desktop multiplayer chess game that allows two players to play against each other over a Local Area Network (LAN). Built with C# and WPF, featuring professional architecture with SOLID principles, design patterns, and clean code practices.

## Table of Contents
- [Features](#features)
- [How to Play](#how-to-play)
- [Programming Principles](#programming-principles)
- [Design Patterns](#design-patterns)
- [Refactoring Techniques](#refactoring-techniques)

---
## Features

### Core Gameplay
- ✅ Full chess rule implementation (piece movement, capture, check, checkmate, stalemate)
- ✅ Support for special moves: En Passant, Pawn Promotion
- ✅ Multiplayer over LAN via TCP/IP networking

### User Interface
- ✅ Interactive chess board
- ✅ Highlight available moves for selected piece
- ✅ Custom cursors for each player side
- ✅ Settings autoload from json file
- ✅ Game end detection with result display

### Advanced Features
- ✅ Network synchronization for multiplayer games
- ✅ End game rule detection pipeline (Checkmate, Stalemate, Repetition, Insufficient Material)
- ✅ Efficient board position hashing for game history tracking

---
## How to play

### Setup
1. **Player 1:** Clicks "Create Game"
2. **Player 2:** Enters Player 1's local IP address and clicks "Connect"

### Gameplay
- Click on a piece to see available moves (highlighted on the board)
- Click on a highlighted square to move the piece
- Game automatically detects check, checkmate, and stalemate situations
- Use the menu to access game settings and history


---

## Programming Principles

### **S - Single Responsibility Principle**
Each class has a single reason to change, promoting maintainability:
- `GameService` - manages game state and player turns
- `ChessValidator` - validates chess rules and legal moves
- `TcpNetworkService` - manages network communication
- `NavigationService` - controls UI navigationRules
NavigationServiceIMove

**Reference:**
- [GameService.cs](ChessApplication/Services/Game/GameService.cs)
- [TcpNetworkService.cs](ChessInfrastructure/Network/TcpNetworkService.cs)
- [ChessValidator.cs](ChessLibrary/Rules/Validation/ChessValidator.cs)
- [NavigationService.cs](ChessGame/Utils/NavigationService.cs)

### **O - Open/Closed Principle**
Code is open for extension but closed for modification:
- New move strategies can be added without modifying existing pieces
- New end-game rules can be registered without changing the pipeline
- New message handlers can be added without touching existing ones

**Reference:** 
- [Moves/Strategies](ChessLibrary/Moves/Strategies)
- [Rules](ChessLibrary/Rules)
- [Handlers](ChessInfrastructure/DTO/Handlers)

### **L - Liskov Substitution Principle**
All piece subclasses can be substituted for the base Piece class:
- `Bishop`, `Knight`, `Rook`, `Queen`, `King`, `Pawn` inherit from `Piece`
- Each piece can be used interchangeably through the base interfaceHand
- No special casting or type checking required Piece

**Reference:** [Piece.cs](ChessLibrary/Pieces/Piece.cs)

#### **I - Interface Segregation Principle**
Clients depend only on interfaces they need:
- `ISubPieceFactory` - minimal interface for individual piece creation
- `IPieceFactory` - interface for factory composition
- `IMoveStrategy` - specific interface for movement algorithms
- `IEndGameRule` - specific interface for end-game conditions

**Reference:** 
- [ISubPieceFactory.cs](ChessLibrary/Factories/PieceFactories/ISubPieceFactory.cs)
- [IPieceFactory.cs](ChessLibrary/Factories/IPieceFactory.cs)

**Usage:**
- [PieceFactory.cs](ChessLibrary/Factories/PieceFactory.cs)

### **D - Dependency Inversion Principle**
High-level modules depend on abstractions, not concrete implementations:
- Dependency Injection through Microsoft.Extensions.DependencyInjection
- All dependencies passed through constructors
- Configuration centralized in App.xaml.cs

**Reference:** [App.xaml.cs](ChessGame/App.xaml.cs)

**Usage:**
- [PieceFactory.cs](ChessLibrary/Factories/PieceFactory.cs)

### **DRY (Don't Repeat Yourself)**
- Reusable NotifyPropertyChanged() in `BaseViewModel.cs`
- Common factory infrastructure shared across all piece factories
- Shared validation and rule checking utilities

**Reference:** [BaseViewModel.cs](ChessGame/ViewModel/Base/BaseViewModel.cs)

## Design Patterns

### **1. Factory Pattern**
**Purpose:** Encapsulate object creation and manage complex dependencies
- `IPieceFactory` - creates pieces with appropriate move strategies
- `IBoardFactory` - creates initialized game boards
- `IGameStateFactory` - creates game state instances
- `ISubPieceFactory` - individual piece factory interface for each piece type

**Reference:** 
- [IPieceFactory.cs](ChessLibrary/Factories/IPieceFactory.cs)
- [PieceFactory.cs](ChessLibrary/Factories/PieceFactory.cs)

### **2. Chain of Responsibility Pattern**
**Purpose:** Pass requests along a chain of handlers until one handles it
- `EndGameEvaluator` - chains multiple end-game rules
- `EndGameRuleHandler` - abstract base handler
- `CheckmateRule`, `StalemateRule`, `RepetitionRule`, `InsufficientMaterial` - handlers
- Each rule checks condition; if satisfied, returns result; otherwise passes to next

**Reference:** 
- [EndGameEvaluator.cs](ChessLibrary/Rules/EndGameEvaluator.cs)
- [EndGameRuleHandler.cs](ChessLibrary/Rules/GameEnd/EndGameRuleHandler.cs)

### **3. Memento Pattern**
**Purpose:** Capture and externalize object state without violating encapsulation
- `GameStateMemento` - immutable snapshot of game state
- `GameState` - originator that creates and restores mementos
- `GameHistoryService` - caretaker that manages memento collection
- Enables undo/redo functionality without exposing internal details

**Reference:** 
- [GameStateMemento.cs](ChessLibrary/Game/GameStateMemento.cs)
- [GameState.cs](ChessLibrary/Game/GameState.cs)
- [GameHistoryService.cs](ChessApplication/Services/Game/GameHistoryService.cs)

---

## Refactoring Techniques

### **1. Extract Service**
**Purpose:** Extract business logic into dedicated services
- Networking abstraction into `INetworkService`
- Settings management into `ISettingsService`
- Navigation logic into `INavigationService`

**Benefits:**
- Single Responsibility Principle
- Easier to mock in tests
- Reusable across application

**Reference:** 
- [INetworkService.cs](ChessApplication/Interfaces/Network/INetworkService.cs)
- [ISettingsService.cs](ChessApplication/Interfaces/Utils/ISettingsService.cs)
- [INavigationService.cs](ChessGame/Utils/INavigationService.cs)

### **2. Replace Magic Numbers with Constants**
**Purpose:** Use meaningful names instead of cryptic numbers
- `Board.Size = 8` instead of hardcoded 8's throughout
- `Direction` instead of magic coordinate offsets
- Named constants in movement calculations

**Example:**
```csharp
// Before
if (pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8)

// After
public const int Size = 8;
public bool IsInside(Position pos)
{
    return pos.Row >= 0 && pos.Row < Size && pos.Column >= 0 && pos.Column < Size;
}
```

**Reference:** 
- [Board.cs](ChessLibrary/Board/Board.cs#L10)
- [Direction.cs](ChessLibrary/ValueObjects/Direction.cs)

### **3. Replace Conditional with Polymorphism**
**Purpose:** Replace type-checking conditionals with polymorphic behavior
- `IEndGameRule` implementations replace branching end-game logic
- `ISubPieceFactory` implementations replace branching of piece creation

**Example:**
```csharp
// Before
public GameResult? CheckEndGame(IBoard board, Player player)
{
    if (IsCheckmate(board, player))
        return GameResult.Checkmate;

    if (IsStalemate(board, player))
        return GameResult.Stalemate;

    return null;
}

// After
public interface IEndGameRule
{
    GameResult? Check(IBoard board, Player player);
}

// usage
foreach (var rule in _rules)
{
    var result = rule.Check(board, player);
    if (result != null)
        return result;
}
```

**Reference:** 
- [IEndGameRule.cs](ChessLibrary/Rules/IEndGameRule.cs)
- [ISubPieceFactory.cs](ChessLibrary/Factories/PieceFactories/ISubPieceFactory.cs)

### **4. Replace Type Code with State/Strategy**
**Purpose:** Replace type flags / enums controlling behavior with dedicated classes
- `INetworkState` replaces branching logic based on connection status
- Each state encapsulates its own behavior (`ConnectedState`, `DisconnectedState`, etc.)
- Eliminates `if (state == ...)` / `switch(state)` logic

**Benefits:**
- Open/Closed Principle (easy to add new states)
- Removes conditional complexity
- Makes runtime behavior explicit and extensible

**Example:**
```csharp
// Before
if (_networkState == NetworkState.Connected)
{
    Send(message);
}
else if (_networkState == NetworkState.Disconnected)
{
    return;
}

// After
_networkState.SendAsync(type, message);
```

```csharp
// Before
switch (_state)
{
    case State.Connected:
        Send();
        break;

    case State.Disconnected:
        return;
}

// After
public interface INetworkState
{
    Task SendAsync(DtoType type, IDtoMessage message);
}

// Example implementation
public class ConnectedState : INetworkState
{
    public Task SendAsync(DtoType type, IDtoMessage message)
        => _ctx.SendInternal(type, message);
}
```

**Reference:** 
- [INetworkState.cs](ChessApplication/Interfaces/Network/INetworkState.cs)
- [NetworkStates/States](ChessInfrastructure/NetworkStates/States)

### **5. Replace Nested Conditional with Guard Clauses**
**Purpose:** Replace nested conditions with early returns
- Used in `DisconnectInternal`, `SendInternal`, `InitializeAsync`
- Simplifies control flow
- Reduces nesting complexity

**Benefits:**
- Flat control flow
- Easier readability
- Fewer branching paths

**Example:**
```csharp
// Before
public async Task SendInternal(...)
{
    if (_state != ConnectedState)
    {
        if (_state != ReconnectingState)
        {
            return;
        }
    }

    await _writer.WriteLineAsync(data);
}

// After
public async Task SendInternal(...)
{
    if (_state is DisconnectedState)
        return;

    if (_writer is null)
        return;

    await _writer.WriteLineAsync(data);
}

```

**Reference:** 
- [TcpNetworkService.cs](ChessInfrastructure/Network/TcpNetworkService.cs)
---
