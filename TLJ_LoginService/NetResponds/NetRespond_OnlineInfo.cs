using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJ_LoginService;

class NetRespond_OnlineInfo
{
    public static string doAskCilentReq_OnlineInfo(IntPtr connId, string reqData)
    {
        JObject respondJO = new JObject();

        try
        {
            JObject jo = JObject.Parse(reqData);
            string tag = jo.GetValue("tag").ToString();
            respondJO.Add("tag", tag);

            string key = jo.GetValue("key").ToString();

            if (key.CompareTo("jinyou123") == 0)
            {
                respondJO.Add("code", Convert.ToInt32(TLJCommon.Consts.Code.Code_OK));
                respondJO.Add("onlineCount", LoginService.m_serverUtil.getOnlineCount());

                // 发送给客户端
                LoginService.m_serverUtil.sendMessage(connId, respondJO.ToString());
            }
            else
            {
                // 客户端参数错误
                respondJO.Add("code", Convert.ToInt32(TLJCommon.Consts.Code.Code_ParamError));

                // 发送给客户端
                LoginService.m_serverUtil.sendMessage(connId, respondJO.ToString());
            }
        }
        catch (Exception ex)
        {
            // 客户端参数错误
            respondJO.Add("code", Convert.ToInt32(TLJCommon.Consts.Code.Code_ParamError));

            // 发送给客户端
            LoginService.m_serverUtil.sendMessage(connId, respondJO.ToString());
        }

        //return respondJO.ToString();
        return "";
    }
}
