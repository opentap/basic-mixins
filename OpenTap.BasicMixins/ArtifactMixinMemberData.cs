using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
namespace OpenTap.BasicMixins
{
    // The Artifact Mixin Member data is needed because
    // we also need a mixin that takes care of publishing artifacts
    // in addition to the actual artifact member.
    // So in the ArtifactMixinMemberData setter we add a 'publish mixin' which publishes all ArtifactMixin members.
    public class ArtifactMixinMemberData : MixinMemberData
    {
        class PublishMixin : ITestStepPostRunMixin
        {
            public void OnPostRun(TestStepPostRunEventArgs eventArgs)
            {
                foreach (var artifactMixin in TypeData.GetTypeData(eventArgs.TestStep).GetMembers().OfType<ArtifactMixinMemberData>())
                {
                    var path = (string) artifactMixin.GetValue(eventArgs.TestStep);
                    if(string.IsNullOrWhiteSpace(path) == false && System.IO.File.Exists(path))
                        eventArgs.TestStep.StepRun.PublishArtifact(path);
                }
            }
        }

        static readonly DynamicMember PublishMixinMember = new DynamicMember
        {
            Name = "ArtifactMixin.Publish",
            TypeDescriptor = TypeData.FromType(typeof(PublishMixin)),
            DeclaringType = TypeData.FromType(typeof(ITestStep)),
            Readable = true,
            Writable = true,
            Attributes = new object[]{new BrowsableAttribute(false), new XmlIgnoreAttribute(), new EmbedPropertiesAttribute()}
        };

        public ArtifactMixinMemberData(IMixinBuilder source, Func<object> make = null) : base(source, make)
        {
        }

        public override void SetValue(object owner, object value)
        {
            if (string.IsNullOrWhiteSpace((string)value) == false && PublishMixinMember.GetValue(owner) == null)
            {
                DynamicMember.AddDynamicMember(owner, PublishMixinMember);
                PublishMixinMember.SetValue(owner, new PublishMixin());
            }
            base.SetValue(owner, value);
        }
    }
}
