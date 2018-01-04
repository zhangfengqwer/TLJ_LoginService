using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJ_LoginService;

class NetRespond_ThirdLogin
{
    public static string doAskCilentReq_ThirdLogin(IntPtr connId, string reqData)
    {
        JObject respondJO = new JObject();

        try
        {
            JObject jo = JObject.Parse(reqData);
            //string tag = jo.GetValue("tag").ToString();
            jo.Add("connId", connId.ToInt32());


           // respondJO.Add("tag", tag);

            // 传给数据库服务器
            {
                /**
                JObject temp = new JObject();
                temp.Add("tag", tag);
                temp.Add("connId", connId.ToInt32());

                temp.Add("platform", jo.GetValue("platform"));
                temp.Add("third_id", jo.GetValue("third_id"));
                temp.Add("nickname", jo.GetValue("nickname"));
                */
                if (!LoginService.m_mySqlServerUtil.sendMseeage(jo.ToString()))
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

                    // 提交任务
                    Request_ProgressTask.doRequest(uid, 208);
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