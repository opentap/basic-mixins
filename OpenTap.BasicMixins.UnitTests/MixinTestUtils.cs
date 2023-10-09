namespace OpenTap.BasicMixins.UnitTests
{
    public static class MixinTestUtils
    {
        public static MixinMemberData LoadMixin(object target, IMixinBuilder mixin)
        {
            MixinMemberData dynamicMember = mixin.ToDynamicMember(TypeData.GetTypeData(target));
            DynamicMember.AddDynamicMember(target, dynamicMember);
            if(dynamicMember.TypeDescriptor.CanCreateInstance)
                dynamicMember.SetValue(target, dynamicMember.TypeDescriptor.CreateInstance());
            return dynamicMember;
        }
    }
}
