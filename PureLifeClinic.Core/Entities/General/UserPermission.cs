namespace PureLifeClinic.Core.Entities.General
{
    public class UserPermission: Base<int> 
    {
        private User? _user;
        private Permission? _permission;

        public int UserId { get; private set; }

        public User User
        {
            set => _user = value;
            get => _user
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(User));
        }

        public int PermissionId { get; private set; }

        public Permission Permission
        {
            set => _permission = value;
            get => _permission
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Permission));
        }
        public UserPermission(int id, int userId, int permissionId)
        {
            Id = id;
            UserId = userId;
            PermissionId = permissionId;
        }
    }
}
