using System.ComponentModel;

namespace PureLifeClinic.Core.Enums
{
    [Flags]
    public enum PermissionAction
    {
        [Description("Views")]
        View = 1,

        [Description("Create/Delete")]
        CreateDelete = 2,

        [Description("Update")]
        Update = 4,

        [Description("Active/Deactive")]
        ActiveDeactive = 8,

        [Description("Approve")]
        Approve = 16,

        [Description("Send")]
        Send = 32,

        [Description("Import/Export")]
        ImportExport = 64,

        [Description("Lock/Unlock")]
        LockUnlock = 128,
    }
}
