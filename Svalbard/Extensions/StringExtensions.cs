using System;
using Svalbard.Utils;

namespace Svalbard
{
  public static class StringExtensions
  {
    public static Int64 ToMurmurHash(this string text)
    {
      var hash = MurmurHash2.ComputeHash(text);
      return Convert.ToInt64(hash);
    }
  }
}