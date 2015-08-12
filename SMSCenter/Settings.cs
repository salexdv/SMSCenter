/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 06.05.2014
 * Time: 11:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SMSCenter
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public class Settings
	{
		// Параметры соединения с SQL
		private string sqlServer;
        private string sqlDatebase;
        private string sqlUsername;
        private string sqlPassword;
        
        // Автозапуск служб приема и отправки сообщений
        private bool autoStartHTTP;
        private bool autoStartSMPP;
                
        // Прослушиваемый порт 
        private int listenPort;
        
        // Параметры логгирования
        private bool writeSMPPLog;
            
        public Settings()
		{
			
		}
        
		public Settings(string sqlServer, string sqlDatebase, string sqlUsername, string sqlPassword, int listenPort, bool autoStartHTTP, bool autoStartSMPP)
		{
			this.sqlServer = sqlServer;
			this.sqlDatebase = sqlDatebase;
			this.sqlUsername = sqlUsername;
			this.sqlPassword = sqlPassword;
			this.autoStartHTTP = autoStartHTTP;
			this.autoStartSMPP = autoStartSMPP;			
			this.listenPort = listenPort;
			this.WriteSMPPLog = true;
		}
		
		public bool WriteSMPPLog
        {
            get
            {
                return writeSMPPLog;
            }
            set
            {
                writeSMPPLog = value;
            }
        }
		
		public string SQLServer
        {
            get
            {
                return sqlServer;
            }
            set
            {
                sqlServer = value;
            }
        }
		
		public string SQLDatebase
        {
            get
            {
                return sqlDatebase;
            }
            set
            {
                sqlDatebase = value;
            }
        }
		
		public string SQLUsername
        {
            get
            {
                return sqlUsername;
            }
            set
            {
                sqlUsername = value;
            }
        }
		
		public string SQLPassword
        {
            get
            {
                return sqlPassword;
            }
            set
            {
                sqlPassword = value;
            }
        }
		
		public int ListenPort
        {
            get
            {
                return listenPort;
            }
            set
            {
                listenPort = value;
            }
        }
		
		public bool AutoStartHTTP
        {
            get
            {
                return autoStartHTTP;
            }
            set
            {
                autoStartHTTP = value;
            }
        }
		
		public bool AutoStartSMPP
        {
            get
            {
                return autoStartSMPP;
            }
            set
            {
                autoStartSMPP = value;
            }
        }
	}
}
