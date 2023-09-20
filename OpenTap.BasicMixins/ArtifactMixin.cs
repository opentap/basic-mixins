using System;
using System.Linq;
namespace OpenTap.BasicMixins
{
    public class ArtifactPublishMixin: ITestStepPostRunMixin
    {
        public void OnPostRun(TestStepPostRunEventArgs eventArgs)
        {
            foreach (var member in TypeData.GetTypeData(eventArgs.TestStep).GetMembers()
                .OfType<ArtifactMixinMemberData>())
            {
                var artifact = (string)member.GetValue(eventArgs.TestStep);
                if (string.IsNullOrEmpty(artifact))
                {
                    // error?
                    continue;
                }
                eventArgs.TestStep.StepRun.PublishArtifact(artifact);    
            }       
        }
    }
}
