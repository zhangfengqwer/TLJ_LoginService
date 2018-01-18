using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TLJ_LoginService;

class NetRespond_CheckVerisionCode
{
    public static string doAskCilentReq_CheckVerisionCode(IntPtr connId, string reqData)
    {
        JObject respondJO = new JObject();

        try
        {
            JObject jo = JObject.Parse(reqData);

            string tag = jo.GetValue("tag").ToString();
            respondJO.Add("tag", tag);
            respondJO.Add("code", (int)TLJCommon.Consts.Code.Code_OK);
            respondJO.Add("apkVersion", OtherConfig.s_apkVersion);
            respondJO.Add("codeVersion", OtherConfig.s_codeVersion);
            respondJO.Add("canRecharge", OtherConfig.s_canRecharge);
            respondJO.Add("canDebug", OtherConfig.s_canDebug);
            respondJO.Add("isStopServer", OtherConfig.s_isStopServer);
            respondJO.Add("banhao", OtherConfig.s_banhao);



            // 发送给客户端
            LoginService.m_serverUtil.sendMessage(connId, respondJO.ToString());
        }
        catch (Exception ex)
        {
            LogUtil.getInstance().addDebugLog("NetRespond_CheckVerisionCode.doAskCilentReq_CheckVerisionCode异常----" + ex.Message);

            // 客户端参数错误
            respondJO.Add("code", Convert.ToInt32(TLJCommon.Consts.Code.Code_ParamError));

            // 发送给客户端
            LoginService.m_serverUtil.sendMessage(connId, respondJO.ToString());
        }

        //return respondJO.ToString();
        return "";
    }
}
