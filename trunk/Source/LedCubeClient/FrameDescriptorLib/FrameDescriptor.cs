using System.Collections.Generic;
using System.Xml.Serialization;

namespace FrameDescriptorLib
{
    public class FrameDescriptor
    {
        public byte[] Frame;
        public int Count;

        public FrameDescriptor()
        {
            Frame = null;
            Count = 0;
        }

        public FrameDescriptor(byte[] m, int c)
        {
            Frame = (byte[])m.Clone();
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
