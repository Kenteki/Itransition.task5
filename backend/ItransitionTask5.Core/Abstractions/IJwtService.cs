using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItransitionTask5.Core.Abstractions
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
    }
}
