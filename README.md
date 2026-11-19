# BackupFolderFinder

A modern WPF desktop application (.NET 8) designed to scan, analyze, and clean redundant daily project backup folders while ensuring that earliest and protected copies are permanently safeguarded.

---

## 🛠 Features

- **Automated Directory Scanning**:
  - Traverses hierarchical backup directories conforming to the structure:
    ```
    {Root} / {ProjectName} / {yyyyMM} / {dd} / {HHMM or time} / {ProjectName}_bak / {ProjectName}_bak.db1
    ```
- **Intelligent Retention Rules**:
  - **Earliest Backup (Keep)**: Automatically identifies and retains the first backup created each day for every project.
  - **Protected Backups (Protected)**: Preserves folders containing safety phrases (such as `dont delete`, `do not delete`, `DontDelete`, `do-not-delete`) regardless of creation time.
  - **Redundant Candidates (Delete)**: Identifies subsequent duplicate backups that are safe for deletion.
- **Modern WPF Experience**:
  - Built with modern **WPF on .NET 8 (LTS)** using the **MVVM** pattern.
  - Custom multi-resolution application icon (`app.ico`, 16x16 to 256x256) embedded for executable and window chrome.
  - Build number tracking and subtle `M13` developer signature in both the header card and status bar.
  - Native Windows modern folder picker via `Microsoft.Win32.OpenFolderDialog`.
  - Non-blocking asynchronous scanning and deletion (`Task.Run` with cancellation support).
  - High-DPI support, responsive master-detail layout with an interactive `GridSplitter`.
  - Visual status chips / badges (Green for Keep, Blue for Protected, Red for Delete).
  - Summary cards displaying days requiring cleanup, redundant backup count, and protected backup count.
- **Storage Metrics & Space Calculation**:
  - Automatically calculates directory sizes for each backup folder.
  - Displays individual backup sizes (KB, MB, GB) and total reclaimable space per day.
  - Dedicated storage metric card in UI header showing total reclaimable disk space across all scanned backups.
- **Enhanced Deletion Safety (Windows Recycle Bin)**:
  - Default deletion uses Windows Shell (`SHFileOperation`) to move backups to the **Windows Recycle Bin**, allowing full recovery in case of accidental deletions.
  - UI toggle enables switching between **Recycle Bin (Safe)** and **Permanent Deletion**.
  - Multi-layer protection check: Deleter verifies that folders flagged as protected are never deleted or recycled.
  - Confirmation dialogs report exact disk space to be freed and deletion destination.
  - Direct Windows File Explorer integration (Open Folder button, double-click, and right-click context menu).

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10 / 11

### Building the Solution

```powershell
dotnet build BackupFolderFinder.slnx
```

### Running the Application

```powershell
dotnet run --project BackupFolderFinder/BackupFolderFinder.csproj
```

### Running Unit & Integration Tests

```powershell
dotnet test
```

---

## 📁 Project Structure

```
BackupFolderFinder/
├── BackupFolderFinder.slnx                # Modern Visual Studio solution
├── BackupFolderFinder/                    # Main WPF Application (.NET 8)
│   ├── App.xaml / App.xaml.cs             # Application resources and entry
│   ├── MainWindow.xaml / .cs              # Modern responsive XAML UI
│   ├── Common/                            # MVVM helpers (RelayCommand, ViewModelBase)
│   ├── Converters/                        # Value converters for status pills and UI states
│   ├── Models/                            # Domain models (BackupInfo, BackupRetentionGroup)
│   ├── Services/                          # Core business logic
│   │   ├── BackupScanner.cs               # Scans and parses backup folder structure
│   │   ├── BackupRules.cs                 # Identifies protected phrases
│   │   ├── BackupRetentionAnalyzer.cs     # Groups and evaluates retention
│   │   ├── BackupDeleter.cs               # Safe folder deletion
│   │   └── DuplicateBackupFinder.cs       # Duplicate backup detection utility
│   └── ViewModels/
│       └── MainViewModel.cs               # Master-detail ViewModel with async operations
└── BackupFolderFinder.Tests/              # xUnit Test Suite
    ├── BackupRulesTests.cs                # Rule phrase matching tests
    ├── BackupRetentionAnalyzerTests.cs    # Retention logic tests
    ├── BackupScannerTests.cs              # Directory discovery tests
    ├── BackupDeleterTests.cs              # Safe deletion tests
    └── MainViewModelTests.cs              # ViewModel behavior and state tests
```
