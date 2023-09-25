using System.ComponentModel;

namespace Shopping.Authentication.SeedWorks.Enums
{
    public enum UserRole
    {
        [Description("کاربر ارشد")]
        Admin,
        [Description("کاربر پشتیبان")]
        Support,
        [Description("مشتری")]
        Customer,
        [Description("فروشگاه")]
        Shop
    }
}