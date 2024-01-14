using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFront.Application.User.Queries
{
    public class DeleteUserRefreshTokenQueryHandler : IRequestHandler<DeleteUserRefreshTokenQuery>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserRefreshTokenQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            await _userRepository.DeleteUserRefreshToken(request.Id);
        }
    }
}
