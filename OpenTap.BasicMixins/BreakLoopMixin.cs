using System;
using System.Collections.Generic;
using System.Linq;
using OpenTap.Plugins.BasicSteps;
namespace OpenTap.BasicMixins
{
    
    public class BreakLoopMixin : ITestStepPostRunMixin
    {
        [Display("Break Loop?", Description: "Break the loop when running the test step?",  Order: 19999)]
        public bool BreakLoop { get; set; }
        public void OnPostRun(TestStepPostRunEventArgs eventArgs)
        {
            if (BreakLoop)
            {
                var loopStep = eventArgs.TestStep.GetParent<LoopTestStep>();
                loopStep?.BreakLoop();
            }
        }
    }
    
    [Display("Break Loop", "This mixin makes it possible for a test step to break a loop.")]
    [MixinBuilder(typeof(ITestStep))]
    public class BreakLoopMixinBuilder : ValidatingObject, IMixinBuilder
    {

        public void Initialize(ITypeData targetType)
        {
            
        }
        
        IEnumerable<Attribute> GetAttributes()
        {
            yield return new EmbedPropertiesAttribute(){PrefixPropertyName = false};
        }
        
        public MixinMemberData ToDynamicMember(ITypeData targetType)
        {
            return new MixinMemberData(this, () => new BreakLoopMixin())
            {
                TypeDescriptor = TypeData.FromType(typeof(BreakLoopMixin)),
                Attributes = GetAttributes().ToArray(),
                Writable = true,
                Readable = true,
                DeclaringType = targetType,
                Name = "Mixin.BreakLoop"
            };
        }
    }
}
