using System;
using System.Linq;
using System.Threading;
namespace OpenTap.BasicMixins
{

    [Display("Artifact", "This mixin type adds a an artifact (file) to a test step.")]
    [MixinBuilder(typeof(ITestStep))]
    public class ArtifactMixinBuilder : ValidatingObject, IMixinBuilder
    {
        ITypeData type;
        
        public ArtifactMixinBuilder()
        {
            Rules.Add(() =>
            {
                var members = type?.GetMembers()
                    .Where(mem =>
                    {
                        if (mem is ArtifactMixinMemberData a && a.Source == this)
                        {
                            return false;
                        }
                        return ("Artifact:" + Name) == mem.Name;
                    });
                if (members == null) return true;
                return !members.Any();
            }, "An artifact of this name already exists.", nameof(Name));
        }
        
        public string Name { get; set; } = "Artifact";
        public void Initialize(ITypeData targetType)
        {
            type = targetType;
        }

        public MixinMemberData ToDynamicMember(ITypeData targetType)
        {
            type = targetType;
            if (string.IsNullOrEmpty(Error) == false)
                throw new Exception(Error);
            return new ArtifactMixinMemberData(this, Make)
            {
                Name = "Artifact:" + Name,
                TypeDescriptor = TypeData.FromType(typeof(string)),
                Attributes = new object[]{new DisplayAttribute(Name, Group:"Artifacts"), new FilePathAttribute()},
                DeclaringType = TypeData.FromType(typeof(ITestStep))
            };
        }
        
        object Make() => "";
    }
}
