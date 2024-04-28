namespace HasherLib 
{
   public static class HasherFactory 
   {
       public static IHasher GetHasher(string filePath, HasherType type) 
       {
           return type switch
           {
               HasherType.CRC32 => new HasherCRC32(filePath),
               HasherType.SUM32 => new HasherSUM32(filePath),
               _ => new HasherSUM32(filePath)
           };
       }
   }
}