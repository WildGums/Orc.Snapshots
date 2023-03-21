namespace Orc.Snapshots.Watchers;

using System;
using System.Linq;
using Notifications;

internal class ShowNotificationOnSnapshotEventsWatcher
{
    private readonly ISnapshotManager _snapshotManager;
    private readonly INotificationService _notificationService;

    public ShowNotificationOnSnapshotEventsWatcher(ISnapshotManager snapshotManager, INotificationService notificationService)
    {
        ArgumentNullException.ThrowIfNull(snapshotManager);
        ArgumentNullException.ThrowIfNull(notificationService);

        _snapshotManager = snapshotManager;
        _notificationService = notificationService;

        _snapshotManager.Loaded += OnSnapshotManagerLoaded;
        _snapshotManager.SnapshotCreated += OnSnapshotManagerSnapshotCreated;
        _snapshotManager.SnapshotRestored += OnSnapshotManagerSnapshotRestored;
    }

    private void OnSnapshotManagerLoaded(object sender, EventArgs e)
    {
        ShowNotification("Loaded snapshots", $"Loaded '{_snapshotManager.Snapshots.Count()}' snapshots");
    }

    private void OnSnapshotManagerSnapshotCreated(object sender, SnapshotEventArgs e)
    {
        ShowNotification("Created snapshot", $"Snapshot '{e.Snapshot.Title}' has been created");
    }

    private void OnSnapshotManagerSnapshotRestored(object sender, SnapshotEventArgs e)
    {
        ShowNotification("Restored snapshot", $"Snapshot '{e.Snapshot.Title}' has been restored");
    }

    private void ShowNotification(string title, string description)
    {
        var notification = new Notification
        {
            Title = title,
            Message = description
        };

        _notificationService.ShowNotification(notification);
    }
}
