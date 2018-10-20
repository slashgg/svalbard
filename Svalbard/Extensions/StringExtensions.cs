using System;
using Svalbard.Utils;

namespace Svalbard
{
  public static class StringExtensions
  {
    public static int ToMurmurHash(this string text)
    {
      var hash = MurmurHash2.ComputeHash(text);
      return Convert.ToInt32(hash);
    }
  }
}