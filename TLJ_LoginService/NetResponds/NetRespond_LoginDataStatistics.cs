using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJ_LoginService;

class NetRespond_LoginDataStatistics
{
    public static string doAskCilentReq_LoginDataStatistics(IntPtr connId, string reqData)
    {
        try
        {
            JObject jo = JObject.Parse(reqData);

            // 获取客户端ip和端口
            string ip = string.Empty;
            ushort port = 0;
            if (LoginService.m_serverUtil.m_tcpServer.GetRemoteAddress(connId, ref ip, ref port))
            {
                jo.Add("ip", ip.ToString());
            }
            else
            {
                jo.Add("ip", "");
            }

            // 传给数据库服务器
            LoginService.m_mySqlServerUtil.sendMseeage(jo.ToString());
        }
        catch (Exception ex)
        {
        }
        
        return "";
    }
}