using System;
using System.Collections.Generic;
using System.Linq;
namespace OpenTap.BasicMixins
{
    [Display("Repeat")]
    [MixinBuilder(typeof(ITestStep))]
    public class RepeatMixinBuilder : IMixinBuilder
    {
        IEnumerable<Attribute> GetAttributes()
        {
            yield return new EmbedPropertiesAttribute();
            yield return new DisplayAttribute("Repeat", Order: 19999);
        }

        public void Initialize(ITypeData targetType)
        {
            
        }
        public MixinMemberData ToDynamicMember(ITypeData targetType)
        {
            return new MixinMemberData(this)
            {
                TypeDescriptor = TypeData.FromType(typeof(RepeatMixin)),
                Attributes = GetAttributes().ToArray(),
                Writable = true,
                Readable = true,
                DeclaringType = targetType,
                Name = "RepeatMixin"
            };
        }
    }
}
