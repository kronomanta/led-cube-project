namespace LedCubeClient.CommunicationSystem
{
    class FrameToSerial
    {
        public static int[] SerializeFrame(int[,,] f)
        {
            var output = new int[64]; // 8 layers 
            for (int y = 0; y < 8;y++ )
            {
                for(int z=0;z<8;z++)
                {
                    for(int x=0;x<8;x++)
                    {
                        output[8*y+z] |= f[x, y, z] << x;
                    }
                }
            }
            return output;
        }
    }
}
