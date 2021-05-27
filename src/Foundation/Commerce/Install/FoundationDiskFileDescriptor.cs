using Mediachase.BusinessFoundation;
using System;

namespace Foundation.Commerce.Install
{
    public class FoundationDiskFileDescriptor : FileDescriptor
    {
        public FoundationDiskFileDescriptor(FoundationDiskFileProvider provider)
            : base(provider)
        {
        }

        public int ModuleOrder { get; set; }

        public long FileCreationTimeTicks { get; set; }

        internal static int Compare(FoundationDiskFileDescriptor x, FoundationDiskFileDescriptor y)
        {
            var result = 0;

            if (x == null)
            {
                if (y != null)
                {
                    result = -1;
                }
            }
            else
            {
                if (y == null)
                {
                    result = 1;
                }
                else
                {
                    result = x.ModuleOrder.CompareTo(y.ModuleOrder);
                    if (result == 0)
                    {
                        result = string.Compare(x.FileNameWithoutExtension, y.FileNameWithoutExtension, StringComparison.OrdinalIgnoreCase);
                        if (result == 0)
                        {
                            result = x.FileCreationTimeTicks.CompareTo(y.FileCreationTimeTicks);
                        }
                    }
                }
            }
            return result;
        }
    }
}
