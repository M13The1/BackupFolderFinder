using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BackupFolderFinder.Common;
using BackupFolderFinder.Models;
using BackupFolderFinder.Services;
using Microsoft.Win32;

namespace BackupFolderFinder.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BackupScanner _scanner;
        private readonly BackupRetentionAnalyzer _analyzer;
        private readonly BackupDeleter _deleter;

        private string _rootFolder;
        private string _statusMessage = "Ready. Select a backup root directory and click Scan.";
        private bool _isBusy;
        private BackupRetentionGroup _selectedGroup;
        private BackupInfo _selectedBackup;
        private CancellationTokenSource _cts;

        private int _totalCandidateDeletes;
        private int _totalProtectedBackups;
        private int _daysRequiringCleanup;

        public MainViewModel()
        {
            _scanner = new BackupScanner();
            _analyzer = new BackupRetentionAnalyzer();
            _deleter = new BackupDeleter();

            RetentionGroups = new ObservableCollection<BackupRetentionGroup>();
            DisplayBackups = new ObservableCollection<BackupInfo>();

            BrowseFolderCommand = new RelayCommand(BrowseFolder, () => !IsBusy);
            ScanCommand = new RelayCommand(async () => await ScanAsync(), () => !IsBusy && !string.IsNullOrWhiteSpace(RootFolder));
            CancelCommand = new RelayCommand(CancelScan, () => IsBusy);
            DeleteSelectedCommand = new RelayCommand(async () => await DeleteSelectedGroupAsync(), () => !IsBusy && SelectedGroup != null && SelectedGroup.DeleteCount > 0);
            DeleteAllCommand = new RelayCommand(async () => await DeleteAllGroupsAsync(), () => !IsBusy && RetentionGroups.Count > 0 && TotalCandidateDeletes > 0);
            OpenFolderCommand = new RelayCommand(OpenFolder, () => SelectedBackup != null || SelectedGroup != null);
            CopyPathCommand = new RelayCommand(CopyPath, () => SelectedBackup != null || SelectedGroup != null);
        }

        public string RootFolder
        {
            get => _rootFolder;
            set
            {
                if (SetProperty(ref _rootFolder, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ObservableCollection<BackupRetentionGroup> RetentionGroups { get; }

        public ObservableCollection<BackupInfo> DisplayBackups { get; }

        public BackupRetentionGroup SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (SetProperty(ref _selectedGroup, value))
                {
                    UpdateDisplayBackups();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public BackupInfo SelectedBackup
        {
            get => _selectedBackup;
            set
            {
                if (SetProperty(ref _selectedBackup, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int TotalCandidateDeletes
        {
            get => _totalCandidateDeletes;
            set => SetProperty(ref _totalCandidateDeletes, value);
        }

        public int TotalProtectedBackups
        {
            get => _totalProtectedBackups;
            set => SetProperty(ref _totalProtectedBackups, value);
        }

        public int DaysRequiringCleanup
        {
            get => _daysRequiringCleanup;
            set => SetProperty(ref _daysRequiringCleanup, value);
        }

        public ICommand BrowseFolderCommand { get; }
        public ICommand ScanCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
        public ICommand DeleteAllCommand { get; }
        public ICommand OpenFolderCommand { get; }
        public ICommand CopyPathCommand { get; }

        private void BrowseFolder()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Backup Root Folder",
                Multiselect = false
            };

            if (!string.IsNullOrWhiteSpace(RootFolder) && Directory.Exists(RootFolder))
            {
                dialog.InitialDirectory = RootFolder;
            }

            if (dialog.ShowDialog() == true)
            {
                RootFolder = dialog.FolderName;
                StatusMessage = $"Selected folder: {RootFolder}";
            }
        }

        public async Task ScanAsync()
        {
            if (string.IsNullOrWhiteSpace(RootFolder))
            {
                MessageBox.Show("Please select a Backup folder first.", "Folder Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Directory.Exists(RootFolder))
            {
                MessageBox.Show($"The directory does not exist:{Environment.NewLine}{RootFolder}", "Folder Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsBusy = true;
            _cts = new CancellationTokenSource();
            StatusMessage = "Scanning for backup folders...";

            try
            {
                var progress = new Progress<string>(msg => StatusMessage = msg);
                var backups = await _scanner.ScanAsync(RootFolder, progress, _cts.Token);

                StatusMessage = "Analyzing backup retention rules...";
                var retentionGroups = await Task.Run(() => _analyzer.Analyze(backups));

                RetentionGroups.Clear();
                foreach (var g in retentionGroups)
                {
                    RetentionGroups.Add(g);
                }

                DaysRequiringCleanup = RetentionGroups.Count;
                TotalCandidateDeletes = RetentionGroups.Sum(g => g.DeleteCount);
                TotalProtectedBackups = RetentionGroups.Sum(g => g.ProtectedCount);

                if (RetentionGroups.Count > 0)
                {
                    SelectedGroup = RetentionGroups[0];
                    StatusMessage = $"Scan complete. {DaysRequiringCleanup} day(s) require cleanup ({TotalCandidateDeletes} redundant backup(s), {TotalProtectedBackups} protected).";
                }
                else
                {
                    SelectedGroup = null;
                    DisplayBackups.Clear();
                    StatusMessage = backups.Count > 0
                        ? $"Scan complete. Found {backups.Count} backup(s); all follow retention rules (no cleanup needed)."
                        : "Scan complete. No matching backup folders found.";
                }
            }
            catch (OperationCanceledException)
            {
                StatusMessage = "Scan cancelled by user.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Scan error: {ex.Message}";
                MessageBox.Show($"An error occurred during scan:{Environment.NewLine}{ex.Message}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void CancelScan()
        {
            _cts?.Cancel();
        }

        private void UpdateDisplayBackups()
        {
            DisplayBackups.Clear();
            if (SelectedGroup?.DisplayBackups != null)
            {
                foreach (var item in SelectedGroup.DisplayBackups)
                {
                    DisplayBackups.Add(item);
                }

                SelectedBackup = DisplayBackups.FirstOrDefault();
            }
            else
            {
                SelectedBackup = null;
            }
        }

        private async Task DeleteSelectedGroupAsync()
        {
            var group = SelectedGroup;
            if (group == null || group.DeleteCount == 0)
                return;

            string confirmMessage =
                $"Delete {group.DeleteCount} duplicate backup(s) for project '{group.ProjectName}' on {group.BackupDateFormatted}?\n\n" +
                $"Retained earliest backup: {group.KeepTime}\n" +
                (group.ProtectedCount > 0 ? $"Protected backup(s) retained: {group.ProtectedTimes} ({group.ProtectedCount})\n\n" : "\n") +
                $"Backup(s) to be permanently deleted:\n{group.DeleteTimes}";

            var result = MessageBox.Show(confirmMessage, "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;

            IsBusy = true;
            StatusMessage = $"Deleting {group.DeleteCount} redundant backup(s)...";

            try
            {
                var delResult = await _deleter.DeleteGroupAsync(group);

                if (delResult.HasErrors)
                {
                    MessageBox.Show(
                        string.Join(Environment.NewLine + Environment.NewLine, delResult.Errors),
                        "Some folders could not be deleted",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }

                MessageBox.Show(
                    $"{delResult.DeletedCount} backup(s) deleted successfully.",
                    "Cleanup Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                await ScanAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Deletion failed:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteAllGroupsAsync()
        {
            if (RetentionGroups.Count == 0 || TotalCandidateDeletes == 0)
                return;

            string confirmMessage =
                $"Are you sure you want to delete ALL candidate duplicate backups?\n\n" +
                $"• Total days to clean: {DaysRequiringCleanup}\n" +
                $"• Total folders to be deleted: {TotalCandidateDeletes}\n" +
                $"• Protected backups that will remain safe: {TotalProtectedBackups}\n\n" +
                $"Earliest daily backups will be preserved for each project.\nThis action permanently deletes the redundant folders.";

            var result = MessageBox.Show(confirmMessage, "Confirm Batch Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;

            IsBusy = true;
            _cts = new CancellationTokenSource();
            StatusMessage = "Batch deletion in progress...";

            try
            {
                var progress = new Progress<string>(msg => StatusMessage = msg);
                var delResult = await _deleter.DeleteGroupsAsync(RetentionGroups.ToList(), progress, _cts.Token);

                if (delResult.HasErrors)
                {
                    MessageBox.Show(
                        string.Join(Environment.NewLine + Environment.NewLine, delResult.Errors),
                        "Some folders could not be deleted",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }

                MessageBox.Show(
                    $"{delResult.DeletedCount} backup folder(s) deleted successfully across {DaysRequiringCleanup} day(s).",
                    "Batch Cleanup Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                await ScanAsync();
            }
            catch (OperationCanceledException)
            {
                StatusMessage = "Batch deletion cancelled.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Batch deletion error:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void OpenFolder()
        {
            string targetFolder = SelectedBackup?.TimeFolder;
            if (string.IsNullOrWhiteSpace(targetFolder) && SelectedGroup?.KeepBackup != null)
            {
                targetFolder = SelectedGroup.KeepBackup.TimeFolder;
            }

            if (string.IsNullOrWhiteSpace(targetFolder))
            {
                MessageBox.Show("No folder selected.", "Open Folder", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!Directory.Exists(targetFolder))
            {
                MessageBox.Show($"The folder does not exist:{Environment.NewLine}{targetFolder}", "Folder Missing", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetFolder,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open folder:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyPath()
        {
            string targetFolder = SelectedBackup?.TimeFolder;
            if (string.IsNullOrWhiteSpace(targetFolder) && SelectedGroup?.KeepBackup != null)
            {
                targetFolder = SelectedGroup.KeepBackup.TimeFolder;
            }

            if (!string.IsNullOrWhiteSpace(targetFolder))
            {
                Clipboard.SetText(targetFolder);
                StatusMessage = $"Copied path to clipboard: {targetFolder}";
            }
        }
    }
}
