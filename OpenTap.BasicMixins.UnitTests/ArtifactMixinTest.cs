using System;
using System.Linq;
using OpenTap.UnitTest;

namespace OpenTap.BasicMixins.UnitTests
{
    
    public class ArtifactMixinTest : ITestFixture 
    {
        [Test]
        public void TestArtifactMixin()
        {
            
            var builder = new ArtifactMixinBuilder();
            var step = new TestTestStep();
            var member = MixinTestUtils.LoadMixin(step, builder);
            member.SetValue(step, "OpenTap.dll");
            var plan = new TestPlan();
            plan.ChildTestSteps.Add(step);
            var run = plan.Execute();
            Assert.IsTrue(run.Artifacts.Contains("OpenTap.dll"));
        }
    }

    public class TestTestStep : TestStep
    {

        public override void Run()
        {
            
        }
    }

    public class MixinTestUtils
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
