using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TLJ_LoginService;

class NetRespond_Login
{
    public static string doAskCilentReq_Login(IntPtr connId, string reqData)
    {
        JObject respondJO = new JObject();

        try
        {
            JObject jo = JObject.Parse(reqData);
            string tag = jo.GetValue("tag").ToString();
            respondJO.Add("tag", tag);

            int platform = Convert.ToInt32(jo.GetValue("platform"));
            switch (platform)
            {
                // 官方包
                case (int)TLJCommon.Consts.Platform.Platform_Official:
                    {
                        string account = jo.GetValue("account").ToString();
                        string password = jo.GetValue("password").ToString();
                        int passwordtype = (int)jo.GetValue("passwordtype");

                        // 传给数据库服务器
                        {
                            JObject temp = new JObject();
                            temp.Add("tag", "Login");
                            temp.Add("connId", connId.ToInt32());

                            temp.Add("account", account);
                            temp.Add("password", password);
                            temp.Add("passwordtype", passwordtype);

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
                    break;

                // 未知渠道包
                default:
                    {

                    }
                    break;
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
                    int passwordtype = (int)jo.GetValue("passwordtype");
                    // 官方账号密码登录
                    if (passwordtype == 1)
                    {
                        Request_CheckRepeatLogin.doRequest(uid);

                        // 提交任务
                        Request_ProgressTask.doRequest(uid,208);
                    }
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
