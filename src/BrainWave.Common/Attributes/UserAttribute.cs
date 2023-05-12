using HotChocolate;
using BrainWave.Common.Middlewares;

namespace BrainWave.Common.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class UserAttribute : GlobalStateAttribute
{
    //method([User] User user){ string id = user.id }
    public UserAttribute() : base(UserMiddleware.USER_CONTEXT_KEY)
    {
        
    }
}
