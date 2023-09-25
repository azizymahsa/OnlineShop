using System;
using Shopping.Authentication.Models.QueryModel.Dto;

namespace Shopping.Authentication.Models.QueryModel
{
    public class RefreshTokenDto: IRefreshTokenDto
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}