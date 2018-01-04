using HPSocketCS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace TLJ_LoginService
{
    public partial class LoginService : ServiceBase
    {
        public static HPServerUtil m_serverUtil;
        public static MySqlServerUtil m_mySqlServerUtil;
        public static LogicServerUtil m_logicServerUtil;
        //public static PlayServerUtil m_playServerUtil;

        public LoginService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (!OtherConfig.init())
            {
                return;
            }

            LogUtil.getInstance().start(OtherConfig.s_logPath + "TLJ_LoginServiceLog");

            if (!NetConfig.init())
            {
                return;
            }

            m_serverUtil = new HPServerUtil();
            m_mySqlServerUtil = new MySqlServerUtil();
            m_logicServerUtil = new LogicServerUtil();
            //m_playServerUtil = new PlayServerUtil();

            LogUtil.getInstance().addDebugLog("服务开启");

            // 开启TCP服务组件与客户端通信
            m_serverUtil.startTCPService();

            // 开启TCP客户端组件与数据库服务器通信
            m_mySqlServerUtil.start();

            // 开启TCP客户端组件与逻辑服务器通信
            m_logicServerUtil.start();

            //// 开启TCP客户端组件与游戏服务器通信
            //m_playServerUtil.start();
        }

        protected override void OnStop()
        {
            m_serverUtil.stopTCPService();
            m_mySqlServerUtil.stop();

            LogUtil.getInstance().writeLogToLocalNow("服务关闭");
        }
    }
}
