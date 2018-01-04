using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TLJ_LoginService;

class Request_CheckRepeatLogin
{
    public static void doRequest(string uid)
    {
        try
        {
            JObject respondJO = new JObject();

            respondJO.Add("tag", TLJCommon.Consts.Tag_CheckRepeatLogin);
            respondJO.Add("uid", uid);

            // 传给逻辑服务器
            LoginService.m_logicServerUtil.sendMseeage(respondJO.ToString());

            //Thread.Sleep(200);

            //// 传给游戏服务器
            //LoginService.m_playServerUtil.sendMseeage(respondJO.ToString());
        }
        catch (Exception ex)
        {
            // 客户端参数错误
            LogUtil.getInstance().addErrorLog("Request_CheckRepeatLogin----" + ex.Message);
        }
    }
}