using System;

namespace Shopping.Authentication.Models.QueryModel.Dto
{
    public interface IRefreshTokenDto
    {
         string Id { get; set; }
         string Subject { get; set; }
         string ClientId { get; set; }
         DateTime IssuedUtc { get; set; }
         DateTime ExpiresUtc { get; set; }
         string ProtectedTicket { get; set; }
    }
}