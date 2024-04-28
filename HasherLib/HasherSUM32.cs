using System.IO;

namespace hasherLib 
{
    internal class HasherSUM32 : IHasher 
    {
        private readonly string _filePath;

        public HasherSUM32(string filePath) 
        {
            _filePath = filePath;
        }

        public ulong GetHash() 
        {
            const int bufferSize = 4;
            uint num = 0;
            ulong crc = 0;

            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read)) 
            {
                var buffer = new byte[bufferSize];
                int bytesRead;
            
                while ((bytesRead = fileStream.Read(buffer, 0, bufferSize)) > 4) 
                {
                    for (var i = 3; i >= 0; --i) 
                    {
                        num |= ((uint)buffer[i]) << (i * 8);
                    }
                    crc += num;
                }
            }

            return crc;
        }
    }
}