using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJ_LoginService;

class NetRespond_QuickRegister
{
    public static string doAskCilentReq_QuickRegister(IntPtr connId,string reqData)
    {
        JObject respondJO = new JObject();
        
        try
        {
            JObject jo = JObject.Parse(reqData);
            string tag = jo.GetValue("tag").ToString();
            respondJO.Add("tag", tag);

            string account = jo.GetValue("account").ToString();
            string password = jo.GetValue("password").ToString();

            // 传给数据库服务器
            {
                JObject temp = new JObject();
                temp.Add("tag", "QuickRegister");
                temp.Add("connId", connId.ToInt32());

                temp.Add("account", account);
                temp.Add("password", password);

                //for (int k = 0; k < 1000; k++)
                {
                    if (!LoginService.m_mySqlServerUtil.sendMseeage(temp.ToString()))
                    {
                        // 连接不上数据库服务器，通知客户端
                        {
                            respondJO.Add("code", Convert.ToInt32(TLJCommon.Consts.Code.Code_MySqlError));

                            // 发送给客户端
                            LoginService.m_serverUtil.sendMessage(connId, respondJO.ToString());
                        }
                    }
                }
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

    public static void onMySqlRespond(int connId, string respondData)
    {
        try
        {
            // 上报给逻辑服务器和游戏服务器
            {
                JObject jo = JObject.Parse(respondData);
                int code = (int)jo.GetValue("code");

                if (code == (int)TLJCommon.Consts.Code.Code_OK)
                {
                    string uid = jo.GetValue("uid").ToString();
                    Request_CheckRepeatLogin.doRequest(uid);
                }
            }

            // 发送给客户端
            {
                LoginService.m_serverUtil.sendMessage((IntPtr)connId, respondData);
            }
        }
        catch (Exception ex)
        {
            LogUtil.getInstance().addDebugLog(ex.Message);

            // 客户端参数错误
            //respondJO.Add("code", Convert.ToInt32(TLJCommon.Consts.Code.Code_ParamError));

            // 发送给客户端
            //LogicService.m_serverUtil.sendMessage(connId, respondJO.ToString());
        }
    }
}
