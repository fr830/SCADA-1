using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    class ConvertHelper
    {
        public static string BytesToHexString(byte[] data, int length = -1, int start = 0)
        {
            if (length < 0)
            {
                length = data.Length;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < start + length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
