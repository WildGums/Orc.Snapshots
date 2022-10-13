namespace Orc.Snapshots.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class SnapshotWindow : Window
    {
        public SnapshotWindow(AutomationElement element) 
            : base(element)
        {
        }

        private SnapshotWindowMap Map => Map<SnapshotWindowMap>();

        public string Title
        {
            get => Map.TitleEdit?.Text ?? string.Empty;
            set
            {
                var titleEdit = Map.TitleEdit;
                if (titleEdit is not null)
                {
                    titleEdit.Text = value;
                }
            }
        }
        public void Accept() => Map.OkButton?.Click();
        public void Decline() => Map.CancelButton?.Click();
    }
}
