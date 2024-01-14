using MediatR;
using ShopFront.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFront.Application.User.Queries
{
    public class DeleteUserRefreshTokenQuery : IRequest
    {
        public Guid Id { get; set; }
    }
}
