using System.Collections.Generic;
using System.Xml.Serialization;

namespace FrameDescriptorLib
{
    public class FrameDescriptor
    {
        public int[] Frame;
        public int Count;

        public FrameDescriptor()
        {
            Frame = new int[0];
            Count = 0;
        }

        public FrameDescriptor(int[] m, int c)
        {
            Frame = (int[])m.Clone();
            Count = c;
        }
    }

    public class FrameDescriptorContainer
    {
        [XmlArray(ElementName="Frames")]
        public FrameDescriptor[] Frames;

        public FrameDescriptorContainer()
        {
            Frames = new FrameDescriptor[0];
        }

        public FrameDescriptorContainer(List<FrameDescriptor> f)
        {
            Frames = f.ToArray();
        }
    }
}
