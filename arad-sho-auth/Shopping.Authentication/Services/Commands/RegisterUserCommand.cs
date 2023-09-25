namespace Shopping.Authentication.Services.Commands
{
    public class RegisterUserCommand
    {
        /// <summary>
        /// نام حساب کاربری
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// رمز عبور حساب کاربری
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// رمز عبور حساب کاربری تأییدی
        /// </summary>
        public string ConfirmedPassword { get; set; }
    }
}