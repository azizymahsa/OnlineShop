namespace Shopping.Authentication.Models.QueryModel.Dto
{
    public interface IUserRoleDto
    {
        int Value { get; set; }
        string Title { get; set; }
        string Description { get; set; }
    }
}