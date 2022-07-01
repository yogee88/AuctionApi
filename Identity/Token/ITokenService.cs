using Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Token
{
    public interface ITokenService
    {
        string Generate(User user);
    }
}
