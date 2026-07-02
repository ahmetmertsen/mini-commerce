using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Application.Features.Users.Commands.Update.UpdatePassword
{
    public class UpdateUserPasswordCommand : IRequest<UpdateUserPasswordCommandResponse>
    {
        public string Email { get; set; } = null!;
        public string VerificationCode { get; set; } = null!;
        public string newPassword { get; set; } = null!;
        public string newPasswordConfirmed { get; set; } = null!;
    }
}
