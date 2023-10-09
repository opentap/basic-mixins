using System.Collections.Generic;
using System.Linq;
using OpenTap.UnitTest;
namespace OpenTap.BasicMixins.UnitTests
{
    public class RepeatMixinTest : ITestFixture
    {
        class LogStep : TestStep
        {
            public string Message { get; set; } = string.Empty;

            public override void Run()
            {
                Log.Debug($"{Message}");
            }
        }

        class LogResults : ResultListener
        {
            public List<TestStepRun> stepRuns = new List<TestStepRun>();

            public IEnumerable<TestStepRun> StepRuns => stepRuns.Select(x => x);
            
            public override void OnTestStepRunCompleted(TestStepRun stepRun)
            {
                stepRuns.Add(stepRun);
                base.OnTestStepRunCompleted(stepRun);
            }
        }
        
        [TestCase(3)]
        [TestCase(5)]
        public void TestFixedCountRepeat(int repeatCount)
        {
            var plan = new TestPlan();
            var step = new LogStep();
            plan.ChildTestSteps.Add(step);
            var mixinBuilder = new RepeatMixinBuilder();
            var member = MixinTestUtils.LoadMixin(step, mixinBuilder);

            var count = TypeData.GetTypeData(step).GetMember("RepeatMixin.Count");
            var behavior = TypeData.GetTypeData(step).GetMember("RepeatMixin.Behavior");
            count.SetValue(step, repeatCount);
            var rl = new LogResults();
            plan.Execute(new[]
            {
                rl
            });
            
            Assert.AreEqual(repeatCount, rl.stepRuns.Count);
        }
        
    }
}
