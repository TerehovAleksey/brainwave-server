namespace BrainWave.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class UseUserAttribute : ObjectFieldDescriptorAttribute
{
    public UseUserAttribute([CallerLineNumber] int order = 0)
    //[CallerLineNumber] int order = 0 задаёт порядок использования атрибутов
    //При использовании сначала должен быть атрибут [Authorize], а потом [UseUser]
    //и очень важно в каком порядке они отработают
    {
        Order = order;
    }

    protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use<UserMiddleware>();
    }
}
