using System;
using System.Text;
using Unity.Collections;
using Verse;

namespace Prioritize
{
    public static class PrioritizeDataExposeUtility
    {
        private const int NewlineInterval = 100;

        public static void LookByteArray(ref byte[] arr, string label)
        {
            if (Scribe.mode == LoadSaveMode.Saving && arr != null)
            {
                byte[] array = CompressUtility.Compress(arr);
                if (array.Length < arr.Length)
                {
                    string value = Convert.ToBase64String(array).AddLineBreaksToLongString();
                    Scribe_Values.Look(ref value, label + "Deflate");
                }
                else
                {
                    string value2 = Convert.ToBase64String(arr).AddLineBreaksToLongString();
                    Scribe_Values.Look(ref value2, label);
                }
            }
            if (Scribe.mode != LoadSaveMode.LoadingVars)
            {
                return;
            }
            string value3 = null;
            Scribe_Values.Look(ref value3, label + "Deflate");
            if (value3 != null)
            {
                arr = CompressUtility.Decompress(Convert.FromBase64String(value3.RemoveLineBreaks()));
                return;
            }
            Scribe_Values.Look(ref value3, label);
            if (value3 != null)
            {
                arr = Convert.FromBase64String(value3.RemoveLineBreaks());
            }
            else
            {
                arr = null;
            }
        }

        public static string AddLineBreaksToLongString(this string str)
        {
            StringBuilder stringBuilder = new StringBuilder(str.Length + (str.Length / 100 + 3) * 2 + 1);
            stringBuilder.AppendLine();
            for (int i = 0; i < str.Length; i++)
            {
                if (i % 100 == 0 && i != 0)
                {
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append(str[i]);
            }
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }

        public static string RemoveLineBreaks(this string str)
        {
            return new StringBuilder(str).Replace("\n", "").Replace("\r", "").ToString();
        }
    }
}
