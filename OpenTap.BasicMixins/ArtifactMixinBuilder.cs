namespace OpenTap.BasicMixins
{

    [Display("Artifact")]
    [MixinBuilder(typeof(ITestStep))]
    public class ArtifactMixinBuilder : IMixinBuilder
    {

        public string Name { get; set; } = "Artifact";
        public void Initialize(ITypeData targetType)
        {
            
        }

        public MixinMemberData ToDynamicMember(ITypeData targetType)
        {
            return new ArtifactMixinMemberData(this, Make)
            {
                Name = "Artifact:" + Name,
                TypeDescriptor = TypeData.FromType(typeof(string)),
                Attributes = new object[]{new DisplayAttribute(Name, Group:"Artifacts"), new FilePathAttribute()},
                DeclaringType = TypeData.FromType(typeof(ITestStep))
            };
        }
        
        object Make() => "";

        public IMixinBuilder Clone() => (IMixinBuilder) MemberwiseClone();
    }
}
