/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 06.05.2014
 * Time: 16:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SMSCenter
{
	public class SMS
	{
		public int id;
		public long number;
		public long uin;
		private string sender;
		private string message;	
		public string delivery_text;		
		public DateTime create_date;
		public DateTime delivery_date;
		public DateTime send_date;
		public int send_status;
		public int delivery_status;
		
		public SMS()
		{
			this.id = 0;	
			this.number = 0;
			this.uin = 0;
			this.sender = "";
			this.message = "";
			this.delivery_text = "";
			this.create_date = DateTime.Now;
			this.delivery_date = SQLSettings.minDateTime;
			this.send_date = SQLSettings.minDateTime;
			this.send_status = 0;
			this.send_status = 0;
			
		}
		
		public string source
		{
			get
            {
				return sender.Trim().ToLower();
            }
            set
            {
            	sender = value.Trim();
            }
		}
		
		public string text
		{
			get
            {
				return message.Trim();
            }
            set
            {
            	message = value.Trim();
            }
		}
		
		public string NumberTxt
		{
			get
            {
				return Convert.ToString(number);
            }
            set
            {
            	number = Convert.ToInt64(value);
            }
		}
		
		public string IdTxt
		{
			get
            {
				return Convert.ToString(id);
            }
            set
            {
            	id = Convert.ToInt32(value);
            }
		}
		
		public string UinTxt
		{
			get
            {
				return Convert.ToString(uin);
            }
            set
            {
            	uin = Convert.ToInt64(value);
            }
		}
		
		public string SendStatusTxt
		{
			get
            {
				return Convert.ToString(send_status);
            }
            set
            {
            	send_status = Convert.ToInt32(value);
            }
		}
	}
	
	public class ExchangeErrors
	{
		public const int MissingXML = -1; // В полученных данных не найден xml
		public const int CorruptedData = -2; // Не найдено окончание xml-файла (битые данные)
		public const int ErrorXML = -3; // Ошибка разбора xml
		public const int SaveError = -4; // Ошибка при сохранении сообщений в базу данных
		public const int UnknownError = -10; // Неизвестная ошибка
	}
	
	public class LogEvent
	{
		private DateTime eventTime;
		private bool isError;
		private bool displayInForm;
		private string eventText;
		
		public DateTime EventTime
		{
			get
            {
				return eventTime;
            }
            set
            {
            	eventTime = value;
            }
		}
		
		public bool IsError
		{
			get
            {
				return isError;
            }
            set
            {
            	isError = value;
            }
		}
		
		public bool DisplayInForm
		{
			get
            {
				return displayInForm;
            }
            set
            {
            	displayInForm = value;
            }
		}
		
		public string EventText
		{
			get
            {
				return eventText;
            }
            set
            {
            	eventText = value;
            }
		}
		
		public LogEvent(string eventText, DateTime eventTime):this(eventText, eventTime, false)
		{			 
		}
		
		public LogEvent(string eventText, DateTime eventTime, bool isError):this(eventText, eventTime, false, true)
		{			
		}
		
		public LogEvent(string eventText, DateTime eventTime, bool isError, bool displayInForm)
		{
			this.eventText = eventText;
			this.eventTime = eventTime;
			this.isError = isError;
			this.displayInForm = displayInForm;
		}
	}
}
