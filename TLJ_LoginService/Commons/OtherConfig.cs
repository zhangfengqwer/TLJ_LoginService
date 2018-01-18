using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class OtherConfig
{
    // 日志路径
    public static string s_logPath;

    // apk版本号
    public static string s_apkVersion;

    // 代码版本
    public static int s_codeVersion;

    // 资源版本
    public static int s_resVersion;

    // 是否开放充值
    public static bool s_canRecharge;

    // 是否可以调试
    public static bool s_canDebug;

    // 服务器是否在维护
    public static bool s_isStopServer;

    // 版号
    public static string s_banhao;

    public static bool init()
    {
        try
        {
            // 读取文件
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "OtherConfig.json");
                string str = sr.ReadToEnd().ToString();
                sr.Close();

                JObject jo = JObject.Parse(str);

                // 日志路径
                s_logPath = jo.GetValue("LogPath").ToString();

                // apk版本号
                s_apkVersion = jo.GetValue("ApkVersion").ToString();

                // 代码版本
                s_codeVersion = (int)jo.GetValue("CodeVersion");

                // 资源版本
                s_resVersion = (int)jo.GetValue("ResVersion");

                // 是否开放充值
                s_canRecharge = (bool)jo.GetValue("canRecharge");

                // 是否可以调试
                s_canDebug = (bool)jo.GetValue("canDebug");

                // 服务器是否在维护
                s_isStopServer = (bool)jo.GetValue("isStopServer");

                // 版号
                s_banhao = jo.GetValue("banhao").ToString();
            }

            return true;
        }
        catch(Exception ex)
        {
            LogUtil.getInstance().writeLogToLocalNow("读取OtherConfig文件出错：" + ex.Message);

            return false;
        }
    }
}
