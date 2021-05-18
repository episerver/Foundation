using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Test
{
    [ServiceConfiguration(typeof(IContentRepositoryDescriptor))]
    public class CommentsPaneDescriptor : ContentRepositoryDescriptorBase
    {
        public static string RepositoryKey { get { return "commets"; } }

        public override string Key { get { return RepositoryKey; } }

        public override string Name { get { return "Comments"; } }

        public override IEnumerable<Type> ContainedTypes
        {
            get
            {
                return new[]
                {
                    typeof(ContentFolder),
                    typeof(Comment)
                };
            }
        }

        public override IEnumerable<Type> CreatableTypes
        {
            get
            {
                return new[] { typeof(Comment) };
            }
        }

        public override IEnumerable<ContentReference> Roots
        {
            get
            {
                return Enumerable.Empty<ContentReference>();
            }
        }

        public override IEnumerable<Type> MainNavigationTypes
        {
            get
            {
                return new[]
                {
                    typeof(ContentFolder)
                };
            }
        }
    }
}