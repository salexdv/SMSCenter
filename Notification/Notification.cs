/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 07.05.2014
 * Time: 12:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace SMSCenter
{
	/// <summary>
	/// Description of Notification.
	/// </summary>
	public static class Notification
	{
		public static RichTextBox Log;	
		private static List<LogEvent> Events;		
		private static BackgroundWorker logWriter;
		
		public static void Initialize(RichTextBox logElement)
		{		
			Log = logElement;
			
			if (!Directory.Exists("Logs"))
			{
				Directory.CreateDirectory("Logs");
			}		
			
			System.Windows.Forms.Timer sizeCheckTimer = new System.Windows.Forms.Timer();			
			sizeCheckTimer.Interval = 15 * 60 * 1000;			
			sizeCheckTimer.Tick += new EventHandler(sizeLogCheckTimer_Tick);
			sizeCheckTimer.Enabled = true;			
			
			Events = new List<LogEvent>();			
			
			logWriter = new BackgroundWorker();
			logWriter.DoWork += new DoWorkEventHandler(logWriter_DoWork);
			logWriter.ProgressChanged += new ProgressChangedEventHandler(logWriter_ProgressChanged);
			logWriter.WorkerReportsProgress = true;
			logWriter.WorkerSupportsCancellation = true;
			logWriter.RunWorkerAsync();
		}
		
		public static void Dispose()
		{		
			Log = null;
								
			Events.Clear();
			Events = null;
			
			logWriter.CancelAsync();
			logWriter.Dispose();
			logWriter = null;			
		}

		static void logWriter_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			LogEvent Event = (LogEvent)e.UserState;
			if (Event.IsError)
			{
				if (Log.InvokeRequired)
					Log.BeginInvoke(new Action(delegate() {Log.SelectionColor = Color.Red;}));
				else
					Log.SelectionColor = Color.Red;
			}
			
			if (Log.InvokeRequired)
			{
				Log.BeginInvoke(new Action(delegate() {Log.AppendText(Event.EventTime + " - " + Event.EventText + "\n");}));
				Log.BeginInvoke(new Action(delegate() {Log.ScrollToCaret();}));
			}
			else				
			{
				Log.AppendText(Event.EventTime + " - " + Event.EventText + "\n");
				Log.ScrollToCaret();
			}
		}

		static void logWriter_DoWork(object sender, DoWorkEventArgs e)
		{			
			int eventIndex = 0;
			int writingInterval = 100;
			LogEvent Event = null;
			
			while (!logWriter.CancellationPending)
			{							
				if (Events.Count > 0)
				{	
					eventIndex = 0;					
					Event = null;
					TextWriter fileWriter = new StreamWriter(@"Logs\events.log", true, Encoding.UTF8);								
					
					while(eventIndex < Events.Count)
					{
						Event = Events[eventIndex];
						
						if (Event.DisplayInForm)
						{			
							logWriter.ReportProgress(0, Event);							
						}			
									
						fileWriter.WriteLine(String.Format("{0}{1}", Event.IsError ? "!!! ": "", Event.EventTime + " - " + Event.EventText));						
												
						Events.Remove(Event);
						eventIndex++;
					}
					
					fileWriter.Close();			
					fileWriter.Dispose();
					fileWriter = null;
				}
				
				System.Threading.Thread.Sleep(writingInterval);
			}			
		}

		static void sizeLogCheckTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				FileInfo logInfo = new FileInfo(@"Logs\events.log");
				if (logInfo.Length > 5242880)				
				{					
					File.Move(@"Logs\events.log", String.Format("Logs\\events_{0}.log", DateTime.Now.ToString("yyyyMMddHHmmss")));
				}
			}
			catch
			{
				
			}
		}
		
		public static void AddEventToLog(string text, bool isError = false, bool displayInForm = true)
		{		
			Events.Add(new LogEvent(text, DateTime.Now, isError, displayInForm));				
		}
		
		public static string GetErrorDescription(int errorCode)
		{
			string description = "";
			
			switch (errorCode)
			{
				case StatusCodes.ESME_ROK:
					break;
				case StatusCodes.ESME_RINVMSGLEN:
					description = "Недопустимая длина сообщения";
					break;
				case StatusCodes.ESME_RINVCMDLEN:
					description = "Недопустимая длина команды";
					break;
				case StatusCodes.ESME_RINVCMDID:
					description = "Недопустимый command_id";
					break;
				case StatusCodes.ESME_RINVBNDSTS:
					description = "Неправильный BIND status для данной команды";
					break;
				case StatusCodes.ESME_RALYBND:
					description = "ESME уже в bound state";
					break;
				case StatusCodes.ESME_RINVPRTFLG:
					description = "Недопустимый флаг приоритета";
					break;
				case StatusCodes.ESME_RINVREGDLVFLG:
					description = "Недопустимый флаг зарегистрированной доставки ";
					break;
				case StatusCodes.ESME_RSYSERR:
					description = "Системная ошибка";
					break;				
				case StatusCodes.ESME_RINVSRCADR:
					description = "Недопустимый исходный адрес";
					break;
				case StatusCodes.ESME_RINVDSTADR:
					description = "Недопустимый номер абонента-получателя";
					break;
				case StatusCodes.ESME_RINVMSGID:
					description = "Недопустимый message_id";
					break;
				case StatusCodes.ESME_RBINDFAIL:
					description = "Неудача bind";
					break;
				case StatusCodes.ESME_RINVPASWD:
					description = "Недопустимый пароль";
					break;
				case StatusCodes.ESME_RINVSYSID:
					description = "Недопустимый system_id";
					break;
				case StatusCodes.ESME_RCANCELFAIL:
					description = "Неудача cancel_sm";
					break;
				case StatusCodes.ESME_RREPLACEFAIL:
					description = "Неудача replace_sm";
					break;
				case StatusCodes.ESME_RMSGQFUL:
					description = "Заполнена очередь сообщений";
					break;
				case StatusCodes.ESME_RINVSERTYP:
					description = "Недопустимый service тип";
					break;
				case StatusCodes.ESME_RINVNUMDESTS:
					description = "Недопустимое количество адресатов";
					break;
				case StatusCodes.ESME_RINVDLNAME:
					description = "Недопустимое имя distribution list";
					break;
				case StatusCodes.ESME_RINVDESTFLAG:
					description = "Недопустимый флажок адресата";
					break;
				case StatusCodes.ESME_RINVSUBREP:
					description = "Недопустимый запроса \"представление с заменой\"";
					break;
				case StatusCodes.ESME_RINVESMCLASS:
					description = "Недопустимые данные поля esm_class";
					break;
				case StatusCodes.ESME_RCNTSUBDL:
					description = "Нельзя представить в список распределения";
					break;
				case StatusCodes.ESME_RSUBMITFAIL:
					description = "Неудача submit_sm или submit_multi";
					break;
				case StatusCodes.ESME_RINVSRCTON:
					description = "Недопустимый ton исходного адреса";
					break;
				case StatusCodes.ESME_RINVSRCNPI:
					description = "Недопустимый npi исходного адреса";
					break;
				case StatusCodes.ESME_RINVDSTTON:
					description = "Недопустимый ton номера";
					break;
				case StatusCodes.ESME_RINVDSTNPI:
					description = "Недопустимый npi номера";
					break;
				case StatusCodes.ESME_RINVSYSTYP:
					description = "Недопустимое поле system_type";
					break;
				case StatusCodes.ESME_RINVREPFLAG:
					description = "Недопустимый флажок replace_if_present";
					break;
				case StatusCodes.ESME_RINVNUMMSGS:
					description = "Недопустимое число сообщений";
					break;
				case StatusCodes.ESME_RTHROTTLED:
					description = "Ошибка дросселирования (ESME превысил разрешенные лимиты сообщения)";
					break;
				case StatusCodes.ESME_RINVSCHED:
					description = "Недопустимое назначенное время доставки";
					break;
				case StatusCodes.ESME_RINVEXPIRY:
					description = "Недопустимый период достоверности сообщения";
					break;
				case StatusCodes.ESME_RINVDFTMSGID:
					description = "Недопустимое предопределенное сообщение или не найдно";
					break;
				case StatusCodes.ESME_RX_T_APPN:
					description = "Код ошибки временного приложения приемника ESME";
					break;
				case StatusCodes.ESME_RX_P_APPN:
					description = "Код ошибки постоянного приложения приемника ESME";
					break;
				case StatusCodes.ESME_RX_R_APPN:
					description = "Код ошибки отклонения сообщения приемника ESME";
					break;
				case StatusCodes.ESME_RQUERYFAIL:
					description = "Неудача запроса query_sm";
					break;
				case StatusCodes.ESME_RINVOPTPARSTREAM:
					description = "Ошибка в опционной части PDU Body";
					break;
				case StatusCodes.ESME_ROPTPARNOTALLWD:
					description = "Не разрешен optional parameter";
					break;
				case StatusCodes.ESME_RINVPARLEN:
					description = "Недопустимая длина параметра";
					break;
				case StatusCodes.ESME_RMISSINGOPTPARAM:
					description = "Отсутствует ожидаемый optional parameter";
					break;
				case StatusCodes.ESME_RINVOPTPARAMVAL:
					description = "Недопустимое значение опционного параметра";
					break;
				case StatusCodes.ESME_RDELIVERYFAILURE:
					description = "Неудача доставки";
					break;
				case StatusCodes.ESME_RUNKNOWNERR:
					description = "Неизвестная ошибка";
					break;
				default:
					description = "Неизвестная ошибка " + errorCode.ToString("X8");
					break;
			}
							       
	    	return description;
		}
		
		public static string GetDeliveryErrorDescription(int errorCode)
		{
			string description = "";
			
			switch (errorCode)
			{
				case DeliveryCodes.ENROUTE:
					description = "Сообщение находится в состоянии в пути";
					break;
				case DeliveryCodes.DELIVERED:
					description = "Доставлено адресату";
					break;
				case DeliveryCodes.EXPIRED:
					description = "Истек период допустимости сообщения";
					break;
				case DeliveryCodes.DELETED:
					description = "Сообщение было удалено";
					break;
				case DeliveryCodes.UNDELIVERABLE:
					description = "Сообщение не доставлено";
					break;
				case DeliveryCodes.ACCEPTED:
					description = "Сообщение  находится  в  принятом  состоянии";
					break;
				case DeliveryCodes.UNKNOWN:
					description = "Сообщение находится в недопустимом состоянии";
					break;
				case DeliveryCodes.REJECTED:
					description = "Сообщение находится в отклоненном состоянии";
					break;
				default:
					description = "Неизвестный статус доставки " + errorCode.ToString("X8");
					break;
			}
							       
	    	return description;
		}
	}
}
