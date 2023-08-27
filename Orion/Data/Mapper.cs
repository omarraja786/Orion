using UserData = Orion.Data.UserData;

namespace Orion.Data
{
    public static class Mapper
    {
        public static UserData ToData(this Models.UserData userModel)
        {
            var userData = new UserData
            {
                Id = userModel.Id,
                Email = userModel.Email,
                Password = userModel.Password,
                Organization = userModel.Organization,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                IsAdmin = userModel.IsAdmin
            };
            return userData;
        }

        public static Models.UserData ToModel(this UserData userDataEntity)
        {
            var userData = new Models.UserData
            {
                Id = userDataEntity.Id,
                Email = userDataEntity.Email,
                Password = userDataEntity.Password,
                Organization = userDataEntity.Organization,
                FirstName = userDataEntity.FirstName,
                LastName = userDataEntity.LastName,
                IsAdmin = userDataEntity.IsAdmin
            };
            return userData;
        }
        public static AdminUserData ToData(this Models.AdminUserData adminUserModel)
        {
            var adminUserData = new AdminUserData
            {
                Id = adminUserModel.Id,
                Email = adminUserModel.Email,
                Password = adminUserModel.Password,

            };
            return adminUserData;
        }

        public static Models.AdminUserData ToModel(this AdminUserData adminUserDataEntity)
        {
            var adminUserData = new Models.AdminUserData()
            {
                Id = adminUserDataEntity.Id,
                Email = adminUserDataEntity.Email,
                Password = adminUserDataEntity.Password,

            };
            return adminUserData;
        }
    }
}
