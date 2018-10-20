using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace Svalbard.Utils {
  public static class MurmurHash2
  {
    /// <summary>
    /// The seed used for hashing. Do not change this.
    /// </summary>
    public const int Seed = 1;

    /// <summary>
    /// Buffer size used during hashing.
    /// </summary>
    public const int BufferSize = 65536;

    /// <summary>
    /// Hashes <paramref name="input"/>, optionally normalizing whitespace.
    /// </summary>
    /// <param name="input">The string to hash. It is encoded as UTF8 before hashing.</param>
    /// <returns>The Murmur2 hash.</returns>
    public static uint ComputeHash(string input)
    {
      return ComputeHash(Encoding.UTF8.GetBytes(input));
    }

    /// <summary>
    /// Hashes <paramref name="input"/>, optionally normalizing whitespace.
    /// </summary>
    /// <param name="input">The byte array to hash.</param>
    /// <returns>The Murmur2 hash.</returns>
    public static uint ComputeHash(byte[] input)
    {
      return ComputeHash(new MemoryStream(input));
    }

    /// <summary>
    /// Hashes <paramref name="input"/>, optionally normalizing whitespace.
    /// </summary>
    /// <param name="input">The steam to hash.</param>
    /// <param name="precomputedLength">The length of the stream if precomputed.</param>
    /// <returns>The Murmur2 hash.</returns>
    public static uint ComputeHash(Stream input, long precomputedLength = 0)
    {
      var length = precomputedLength != 0 ? precomputedLength : input.Length;
      var pool = ArrayPool<byte>.Shared;
      var buffer = pool.Rent(BufferSize);
      try
      {
        if (length == 0)
        {
            throw new Exception("Stream must contain at least 1 byte.");
        }

        // 'm' and 'r' are mixing constants generated offline. They're not really 'magic', they just happen to work well.
        const uint m = 0x5bd1e995;
        const int r = 24;

        uint h = (uint)(Seed ^ length);

        uint k = 0;
        int shift = 0;

        while (true)
        {
          var bufferLength = input.Read(buffer, 0, buffer.Length);

          // End of stream reached.
          if (bufferLength == 0)
          {
              break;
          }

          for (int i = 0; i < bufferLength; ++i)
          {
            var b = buffer[i];

            // Add the byte to the uint.
            k |= ((uint)b) << shift;
            shift += 8;

            if (shift == 32)
            {
              // Hash the uint.
              k *= m;
              k ^= k >> r;
              k *= m;
              h *= m;
              h ^= k;

              // Reset uint and shift
              k = 0;
              shift = 0;
            }
          }
        }

        // Handle the last few bytes of the input stream.
        if (shift > 0)
        {
            h ^= k;
            h *= m;
        }

        // Do a few final mixes of the hash to ensure the last few bytes are well-incorporated.
        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return h;
      }
      finally
      {
        pool.Return(buffer, true);
      }
    }
  }
}