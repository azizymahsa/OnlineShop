using Shopping.Authentication.Models.QueryModel.Dto;

namespace Shopping.Authentication.Models.QueryModel
{
    public class UserRoleDto: IUserRoleDto
    {
        public int Value { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}