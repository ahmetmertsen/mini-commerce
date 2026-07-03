using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Domain.Enums
{
    public enum VerificationPurpose
    {
        EmailVerification = 1,
        PasswordReset = 2,
        EmailChangeOld = 3,
        EmailChangeNew = 4
    }
}
