using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HNC
{
    public class MachineTool
    {
        /// <summary>
        /// Redis数据库的连接字符串
        /// </summary>
        public static string ConnectionString { get; private set; }

        private static IConnectionMultiplexer Redis;
        private const int databases = 16;
        private readonly IDatabase db;
        private readonly ISubscriber sub;

        public MachineTool(string ip, string connectionString = null)
        {
#if OFFLINE
            connectionString = "localhost:6379,allowAdmin=true";
#endif
            if (Redis == null || !Redis.IsConnected)
            {
                ConnectionString = connectionString ?? ConfigurationManager.ConnectionStrings["Redis"].ConnectionString;
                Redis = ConnectionMultiplexer.Connect(ConnectionString);
            }
            bool found = false;
            for (int i = 0; i < databases; i++)
            {
                if (Redis.GetDatabase(i).StringGet("IP") == ip)
                {
                    db = Redis.GetDatabase(i);
                    sub = Redis.GetSubscriber();
                    found = true;
                    break;
                }
            }
#if !DEBUG
            if (!found)
            {
                throw new ArgumentException(string.Format("未找到IP为{0}的机床！", ip));
            }
#endif
        }

        internal MachineTool(IDatabase database, ISubscriber subscriber)
        {
            db = database;
            sub = subscriber;
        }

        private const int CONNECT_TIMEOUT = 3;

        public bool IsNCConnectToDatabase()
        {
            var result = false;
            long timestamp;
            if (db.StringGet("TimeStamp").TryParse(out timestamp))
            {
                var span = DateTime.Now - DateTime.FromBinary(timestamp);
                if (span.TotalSeconds < CONNECT_TIMEOUT)
                {
                    result = true;
                }
            }
            return result;
        }

        private void WriteList(string key, IList<string> list)
        {
            IBatch bacth = db.CreateBatch();
            if (db.KeyExists(key))
            {
                db.KeyDelete(key);
            }
            for (int i = 0; i < list.Count; i++)
            {
                bacth.ListRightPushAsync(key, list[i]);
            }
            bacth.Execute();
        }

        public string Machine { get { return db.StringGet("Machine"); } }

        private bool Publish(RedisValue message)
        {
            var channel = Machine + ":SetValue";
            sub.Publish(channel, message);
            return true;
        }

        private bool Publish(HncRegType type, int index, int value, int NCMessageFunction)
        {
            var message = new NCMessage
            {
                Type = "Register",
                Index = NCMessageFunction,
                Value = JsonConvert.SerializeObject(
                    new
                    {
                        regType = Enum.GetName(typeof(HncRegType), type).Replace("REG_TYPE_", ""),
                        index,
                        value,
                    }),
            };
            return Publish(message.ToString());
        }

        public IPEndPoint GetIPEndPoint()
        {
            var address = IPAddress.Parse(db.StringGet("IP"));
            var port = (int)db.StringGet("Port");
            return new IPEndPoint(address, port);
        }

        public string HNC_SystemGetValue(HncSystem type)
        {
            return db.HashGet("System", Enum.GetName(typeof(HncSystem), type)).ToString();
        }

        public bool HNC_RegGetValue(HncRegType type, int index, out int value)
        {
            var key = "Register:" + Enum.GetName(typeof(HncRegType), type).Replace("REG_TYPE_", "");
            return db.HashGet(key, index.ToString("D4")).TryParse(out value);
        }

        public bool HNC_RegSetValue(HncRegType type, int index, int value)
        {
            return Publish(type, index, value, NCMessageFunction.PARAMAN_SET);
        }

        public bool HNC_RegSetValue(HncRegType type, int index, short value)
        {
            return HNC_RegSetValue(type, index, Convert.ToInt32(value));
        }

        public bool HNC_RegSetValue(HncRegType type, int index, byte value)
        {
            return HNC_RegSetValue(type, index, Convert.ToInt32(value));
        }

        public bool HNC_RegSetBit(HncRegType type, int index, int bit)
        {
            return Publish(type, index, bit, NCMessageFunction.REG_SET_BIT);
        }

        public bool HNC_RegClrBit(HncRegType type, int index, int bit)
        {
            return Publish(type, index, bit, NCMessageFunction.REG_CLR);
        }

        /// <summary>
        /// 查询信号是否存在
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="bit">Bit</param>
        /// <param name="type">寄存器类型</param>
        /// <returns>信号值为1则返回TRUE</returns>
        public bool Exist(int index, int bit, HncRegType type = HncRegType.REG_TYPE_B)
        {
            try
            {
                int value = 0;
                if (HNC_RegGetValue(type, index, out value))
                {
                    return ((value >> bit) & 1) == 1;
                }
            }
            catch (Exception)
            {
                //TODO
                //throw;
            }
            return false;
        }

        public bool Set(int index, int bit, HncRegType type = HncRegType.REG_TYPE_B)
        {
            try
            {
                return HNC_RegSetBit(type, index, bit);
            }
            catch (Exception)
            {
                //TODO
                //throw;
            }
            return false;
        }

        public bool Clear(int index, int bit, HncRegType type = HncRegType.REG_TYPE_B)
        {
            try
            {
                return HNC_RegClrBit(type, index, bit);
            }
            catch (Exception)
            {
                //TODO
                //throw;
            }
            return false;
        }

        public bool HNC_NetFileSend(string localPath, string filename)
        {
            if (!File.Exists(localPath))
            {
                throw new FileNotFoundException("G代码文件未找到！", localPath);
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("文件名不能为空！");
            }
            var list = new List<string>();
            using (var sr = new StreamReader(localPath, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        list.Add(line);
                    }
                }
            }
            WriteList("GCodeFileSent:" + @"h/lnc8/prog/" + filename, list);
            var message = new NCMessage
            {
                Type = "File",
                SubType = "h/lnc8/prog",
                Index = 0,
                Value = filename,
            };
            return Publish(message.ToString());
        }

        public bool HNC_NetFileRemove(string destinationPath)
        {
            var message = new NCMessage
            {
                Type = "File",
                Index = 1,
                Value = destinationPath
            };
            return Publish(message.ToString());
        }

        public bool HNC_NetFileCheck(string localPath, string destinationPath)
        {
            var message = new NCMessage
            {
                Type = "File",
                SubType = destinationPath,
                Index = 2,
                Value = localPath.Split('\\').LastOrDefault(),
            };
            return Publish(message.ToString());
        }

        /// <summary>
        /// 从下位机加载G代码程序
        /// </summary>
        /// <param name="destinationPath">程序路径</param>
        /// <returns></returns>
        public bool HNC_SysCtrlSelectProg(string destinationPath)
        {
            string key = "GCodeFile:" + destinationPath;
            if (!db.KeyExists(key))
            {
                return false;
            }
            var fileName = destinationPath.Split('/').LastOrDefault();
            var GCodeArray = db.ListRange(key, 0, db.ListLength(key));
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Machine, fileName);
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            using (var sw = new StreamWriter(filePath, false, Encoding.Default))
            {
                for (int i = 0; i < GCodeArray.Length; i++)
                {
                    sw.WriteLine(GCodeArray[i].ToString());
                }
            }
            return true;
        }

        /// <summary>
        /// 设置刀补
        /// </summary>
        /// <param name="toolVal">补偿值（标准值-测量值）</param>
        /// <param name="toolNO">刀具编号（从1开始）</param>
        /// <param name="type">（1:X向,2:Z向)??(1:长度,2:长度磨损,3:半径,4:半径磨损）</param>
        /// <returns>是否成功</returns>
        public bool HNC_ToolSetValue(double toolVal, int toolNO, int type)
        {
            var message = new NCMessage
            {
                Type = "ToolSet",
                Value = JsonConvert.SerializeObject(new { toolVal, toolNO, type }),
            };
            return Publish(message.ToString());
        }

        /// <summary>
        /// 测试宏变量
        /// </summary>
        /// <returns></returns>
        public string HNC_GetUserVar()
        {
            string key = "MacroVariables:USER";
            if (!db.KeyExists(key))
            {
                return string.Empty;
            }
            return db.HashGet(key, 50040).ToString();
        }

    }
}
