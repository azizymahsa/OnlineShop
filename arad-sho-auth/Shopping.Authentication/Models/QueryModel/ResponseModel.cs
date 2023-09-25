
namespace Shopping.Authentication.Models.QueryModel
{
    public class ResponseModel
    {
        /// <summary>
        /// متن پیغام
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// آیا عملیات با موفقیت انجام شده است؟
        /// </summary>
        public bool Success { get; set; }
    }
}