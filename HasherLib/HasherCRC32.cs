using System.IO;

namespace HasherLib
{
    internal class HasherCRC32 : IHasher
    {
        private readonly string _filePath;
        
        public HasherCRC32(string filePath)
        {
            _filePath = filePath;
        }

        public ulong GetHash()
        {
            var crcTable = GenerateCRCTable();
            ulong crc = 0;
            
            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[1024];
                int bytesRead;
                
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        crc = crcTable[(crc ^ buffer[i]) & 0xFF] ^ (crc >> 8);
                    }
                }
            }
            
            return crc ^ 0xFFFFFFFFUL;
        }
        
        private ulong[] GenerateCRCTable()
        {
            var crcTable = new ulong[256];
            
            for (ulong i = 0; i < crcTable.Length; i++)
            {
                var crc = i;
                for (int j = 0; j < 8; j++)
                {
                    crc = ((crc & 1) == 1) ? (crc >> 1) ^ 0xEDB88320UL : crc >> 1;
                }
                crcTable[i] = crc;
            }
            
            return crcTable;
        }
    }
}