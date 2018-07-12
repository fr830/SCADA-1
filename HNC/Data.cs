using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNC
{
    public class CHANNEL_STATUS
    {
        public const int CH_STATE_CYCLING = 32;
    }

    [Serializable]
    public class HncMessage<T>
    {
        public string Header { get; set; }

        public T Body { get; set; }
    }

    [Serializable]
    public class NCMessage
    {
        public string Type { get; set; }

        public string SubType { get; set; }

        public int Index { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class NCMessageFunction
    {
        public static int REG_SET = 1;

        public static int REG_CLR = 0;

        public static int REG_SET_BIT = 2;

        public static int PARAMAN_SET = 0;

        public static int PARAMAN_SAVE = 1;
    }

    [Serializable]
    public class Parameter<T>
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public T PropValue { get; set; }

        public T DefaultValue { get; set; }

        public int EffectWay { get; set; }

        public T MaxValue { get; set; }

        public T MinValue { get; set; }

        public int StoreType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
