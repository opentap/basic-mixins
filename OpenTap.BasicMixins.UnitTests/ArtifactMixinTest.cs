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

}
