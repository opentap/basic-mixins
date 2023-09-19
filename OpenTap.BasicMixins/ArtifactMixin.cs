using System;
using System.Linq;
namespace OpenTap.BasicMixins
{
    class ArtifactAttribute : Attribute
    {
        
    }
    
    
   

    public class ArtifactPublishMixin: ITestStepPostRunMixin
    {

        public void OnPostRun(TestStepPostRunEventArgs eventArgs)
        {
            foreach (var member in TypeData.GetTypeData(eventArgs.TestStep).GetMembers()
                .Where(x => x.HasAttribute<ArtifactAttribute>()))
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
