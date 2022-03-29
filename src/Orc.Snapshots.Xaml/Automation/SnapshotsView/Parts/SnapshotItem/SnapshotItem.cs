namespace Orc.Snapshots.Automation
{
    using Catel;
    using Orc.Automation;
    using Views;

    public class SnapshotItem
    {
        private readonly SnapshotItemMap _map;

        public SnapshotItem(SnapshotItemMap map)
        {
            Argument.IsNotNull(() => map);
            
            _map = map;
        }

        public string Title => _map.TitleText?.Value;

        public void Restore()
        {
            _map.RestoreButton?.Click();
        }

        public bool CanEdit()
        {
            return _map.EditButton.IsVisible();
        }

        public SnapshotWindow Edit()
        {
            if (!CanEdit())
            {
                return null;
            }

            _map.EditButton.Click();

            Wait.UntilResponsive();

            var hostWindow = _map.EditButton.Element.GetHostWindow();
            var snapshotWindow = hostWindow.Find<SnapshotWindow>();

            return snapshotWindow;
        }

        public bool CanRemove()
        {
            return _map.RemoveButton.IsVisible();
        }

        public void Remove()
        {
            if (!CanRemove())
            {
                return;
            }

            var removeButton = _map.RemoveButton;

            removeButton.Click();

            var hostWindow = removeButton.Element.GetHostWindow();

            hostWindow.SetFocus();

            Wait.UntilResponsive();

            var messageBox = hostWindow.Find<MessageBox>();
            messageBox?.Yes();

            Wait.UntilResponsive();

            hostWindow.SetFocus();

        }
    }
}
