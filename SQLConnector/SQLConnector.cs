/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 08.05.2014
 * Time: 10:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SMSCenter
{
	/// <summary>
	/// Description of UserControl1.
	/// </summary>
	public class SQLConnector
	{
		private SqlConnection dbConn;
		
		private string serverName;
		private string dbName;
		private string login;
		private string password;
		private bool isBusy;
		
		private bool isOpen;		
		private string textStatus;
		
		
		public bool IsOpen
        {
            get { return isOpen; }            
        }
		
		public string TextStatus
        {
            get { return textStatus; }            
        }
		
		private void UpdateState()
		{
			isOpen = (dbConn.State == ConnectionState.Open);			
		}
		
		private bool CreateConnection()
		{
			CloseConnection();
			
			this.textStatus = "";
			
			// Формируем строку подключеяни
			string connectionString  = String.Format("Server={0};Database={1};User Id={2};Password={3};", serverName, dbName, login, password);
			
			// Создаем соединение и открываем его
			dbConn = new SqlConnection(connectionString);
			try	
			{
				Notification.AddEventToLog(String.Format("Подключение к базе данных MSSQL ({0})", connectionString), false, false);
				dbConn.Open();
				Notification.AddEventToLog("Подключение выполнено", false, false);
			}
			catch(Exception e)
			{
				this.textStatus	= e.Message;
				Notification.AddEventToLog(String.Format("Не удалось подключиться к базе: ({0})", e.Message), true, false);
			}
			UpdateState();
			
			return this.IsOpen;
		}
		
		private bool ConnectionIsOpen()
		{		
			bool result = false;
			
			if (dbConn.State == ConnectionState.Open)
			{
				DataTable tempTable = Query("SELECT TOP 1 * FROM Messages");
				
				result = (tempTable != null);				
				
				if (result)
					tempTable.Dispose();
				
				this.isBusy = false;
			}
			
			return result;
		}
		
		public void CloseConnection()
		{
			if (dbConn != null)
			{
				dbConn.Close();
				dbConn.Dispose();
			}
		}
				
		public SQLConnector(string serverName, string dbName, string login, string password)
		{		
			this.serverName = serverName;
			this.dbName = dbName;
			this.login = login;
			this.password = password;
			this.isBusy = false;
			
			CreateConnection();			
		}
		
		~SQLConnector()
		{
			
		}
					
		// Выполняет произвольный запрос и возвращает данные в виде DataTable
		//		
		private DataTable Query(string queryText)
		{						
			
			while (this.isBusy)
			{
				System.Threading.Thread.Sleep(SQLSettings.waitingInterval);
			}

			this.isBusy = true;			
			
			DataTable queryResult = new DataTable();
			SqlDataAdapter dbAdapter = new SqlDataAdapter(queryText, dbConn);
			
			try
			{
				dbAdapter.Fill(queryResult);				
				dbAdapter.Dispose();
				dbAdapter = null;
			}
			catch(Exception e)
			{
				this.textStatus = e.Message;
				queryResult = null;
				Notification.AddEventToLog(String.Format("Ошибка выполнения запроса \"{0}\": ({1})", queryText, e.Message), true, false);
			}

			return queryResult;			
		}
		
		// Возвращает таблицу с данными для отображения в форме
		//
		public DataTable GetDataSourceForUnsentMessages()
		{		
			DataTable result;
			
			if (ConnectionIsOpen() || CreateConnection())
			{
				result = Query("SELECT id, sender, number, text, create_date, send_status FROM Messages WHERE uin=0");				
				this.isBusy = false;
				
			}
			else
			{
				result = null;	
			}
			
			return result;
		}
		
		// Возвращает таблицу с данными для отображения в форме
		//
		public DataTable GetDataSourceForMessages(DateTime dateBegin, DateTime dateEnd, bool onlyDelivery, bool onlyNotDelivery)
		{		
			DataTable result;
			
			if (ConnectionIsOpen() || CreateConnection())
			{
				string queryText = "SELECT id, sender, number, text, create_date, send_date, send_status, delivery_date, delivery_status, delivery_text, uin FROM Messages WHERE";
				
				if (onlyDelivery)
					queryText += " uin != 0 AND";
				
				if (onlyNotDelivery)
					queryText += " uin = 0 AND";
				
				if (dateBegin != DateTime.MinValue)
				{
					dateBegin = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day, 0, 0, 0);
					queryText += String.Format(" create_date >= '{0}' AND", dateBegin.ToString());
				}
				
				if (dateEnd != DateTime.MinValue)
				{
					dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59);
					queryText += String.Format(" create_date <= '{0}' AND", dateEnd.ToString());
				}
				
				if (queryText.EndsWith("AND"))
					queryText = queryText.Substring(0, queryText.Length - 4);
				else
					queryText = queryText.Substring(0, queryText.Length - 6);
				
				result = Query(queryText);
				this.isBusy = false;
			}
			else
			{
				result = null;	
			}
			
			return result;
		}
		
		// Возвращает таблицу с данными для отображения в форме
		//
		public DataTable GetDataSourceForMessages()
		{		
			return GetDataSourceForMessages(DateTime.MinValue, DateTime.MinValue, false, false);
		}
		
		// Возвращает коллекцию неотправленных сообщений, размер которой ограничен maxCount 
		//
		public List<SMS> GetUnsentMessages(int maxCount)
		{			
			List<SMS> messages = new List<SMS>();
		
			if (ConnectionIsOpen() || CreateConnection())
			{			
				string queryText = String.Format("SELECT {0} * FROM Messages WHERE uin=0", maxCount == 0 ? "" : String.Format("TOP {0}", maxCount));
				
				DataTable table = Query(queryText);
				
				if (table != null)
				{
					foreach (DataRow row in table.Rows)
					{
						SMS message = new SMS();
							
						message.source 			= Convert.ToString(row["sender"]);
						message.create_date 	= Convert.ToDateTime(row["create_date"]);						
						message.id 				= Convert.ToInt32(row["id"]);
						message.number 			= Convert.ToInt64(row["number"]);
						message.send_date 		= Convert.ToDateTime(row["send_date"]);
						message.send_status 	= Convert.ToInt16(row["send_status"]);
						message.delivery_date	= Convert.ToDateTime(row["delivery_date"]);
						message.delivery_status = Convert.ToInt16(row["delivery_status"]);
						message.delivery_text	= Convert.ToString(row["delivery_text"]); 
						message.text			= Convert.ToString(row["text"]);
						message.uin				= Convert.ToInt64(row["uin"]);
						
						messages.Add(message);
						
					}
					
					table.Dispose();
					table = null;
				}
				
				this.isBusy = false;
								
			}
			
			return messages;
		}
		
		// Записывает переданные сообщения в базу данных
		//
		public bool SaveNewMessages(List<SMS> messages)
		{
			bool result = true;
			
			if (ConnectionIsOpen() || CreateConnection())
			{
				
				while (this.isBusy)
				{
					System.Threading.Thread.Sleep(SQLSettings.waitingInterval);
				}
				
				this.isBusy = true;
				
				DataSet dbDataSet = new DataSet();
				SqlDataAdapter dbAdapter = new SqlDataAdapter("SELECT * FROM Messages", dbConn);
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dbAdapter);
				dbAdapter.Fill(dbDataSet, "Messages");	
				
				if (messages.Count > 0)
				{
					foreach(SMS currentSMS in messages)
					{			
						DataRow newRow 				= dbDataSet.Tables["Messages"].NewRow();
						newRow["id"] 				= currentSMS.id;
						newRow["number"] 			= currentSMS.number;
						newRow["text"] 				= currentSMS.text;
						newRow["sender"] 			= currentSMS.source;
						newRow["create_date"] 		= currentSMS.create_date;
						newRow["delivery_status"] 	= currentSMS.delivery_status;
						newRow["delivery_date"] 	= currentSMS.delivery_date;
						newRow["delivery_text"]		= currentSMS.delivery_text;
						newRow["send_date"]			= currentSMS.send_date;
						newRow["send_status"] 		= currentSMS.send_status;					
						newRow["uin"]				= currentSMS.uin;
						
						dbDataSet.Tables["Messages"].Rows.Add(newRow);
					}
					
					try
					{			
						dbAdapter.Update(dbDataSet, "Messages");
					}
					catch(Exception e)
					{				
						result = false;
						this.textStatus = e.Message;
					}
				}
				else
				{
					result = false;
					this.textStatus = "Не передано ни одного сообщения для сохранения";
				}

				commandBuilder.Dispose();
				commandBuilder = null;
				
				dbAdapter.Dispose();
				dbAdapter = null;
				
				dbDataSet.Dispose();
				dbDataSet = null;
				
				this.isBusy = false;
						
			}
			else
				result = false;
				
			
			return result;			
		}
		
		// Сохраняет полученный uin для только что переданного сообщения 
		//
		public bool SaveMessageDeliveryDate(long messageUIN, byte deliveryState, string deliveryText)
		{
			bool result = true;
			
			if (ConnectionIsOpen() || CreateConnection())
			{
				while (this.isBusy)
				{
					System.Threading.Thread.Sleep(SQLSettings.waitingInterval);
				}
				
				this.isBusy = true;
				
				DataSet dbDataSet = new DataSet();						
				SqlDataAdapter dbAdapter = new SqlDataAdapter(String.Format("SELECT * FROM Messages WHERE uin={0}", messageUIN), dbConn);
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dbAdapter);
				dbAdapter.Fill(dbDataSet, "Messages");	
				
				if (dbDataSet.Tables["Messages"].Rows.Count > 0)
				{
					foreach(DataRow row in dbDataSet.Tables["Messages"].Rows)
					{						
						row["delivery_date"] = DateTime.Now;
						row["delivery_status"] = deliveryState;
						row["delivery_text"] = deliveryText;
					}
					
					try
					{			
						dbAdapter.Update(dbDataSet, "Messages");
					}
					catch(Exception e)
					{				
						result = false;
						this.textStatus = e.Message;
					}
				}
				else
				{
					result = false;
					this.textStatus = String.Format("Не найдено сообщение с uin {0} ({1})", messageUIN, deliveryText);
				}

				commandBuilder.Dispose();
				commandBuilder = null;
				
				dbAdapter.Dispose();
				dbAdapter = null;
				
				dbDataSet.Dispose();
				dbDataSet = null;	

				this.isBusy = false;
			}
			else
				result = false;
				
			
			return result;			
		}
		
		// Сохраняет полученный uin для только что переданного сообщения 
		//
		public bool SaveMessageUIN(long messageID, long messageUIN)
		{
			bool result = true;
			
			if (ConnectionIsOpen() || CreateConnection())
			{
				while (this.isBusy)
				{
					System.Threading.Thread.Sleep(SQLSettings.waitingInterval);
				}
				
				this.isBusy = true;
				
				DataSet dbDataSet = new DataSet();						
				SqlDataAdapter dbAdapter = new SqlDataAdapter(String.Format("SELECT id, uin, send_date, send_status FROM Messages WHERE id={0}", messageID), dbConn);				
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dbAdapter);
				dbAdapter.Fill(dbDataSet, "Messages");	
				
				if (dbDataSet.Tables["Messages"].Rows.Count > 0)
				{
					foreach(DataRow row in dbDataSet.Tables["Messages"].Rows)
					{	
						row["uin"] = messageUIN;														
						row["send_date"] = DateTime.Now;
						row["send_status"] = 0;					
					}
					
					try
					{			
						dbAdapter.Update(dbDataSet, "Messages");
					}
					catch(Exception e)
					{				
						result = false;
						this.textStatus = e.Message;
					}
				}
				else
				{
					result = false;
					this.textStatus = String.Format("Не найдено сообщение с id {0}", messageID);
				}								
				
				commandBuilder.Dispose();
				commandBuilder = null;
				
				dbAdapter.Dispose();
				dbAdapter = null;
				
				dbDataSet.Dispose();
				dbDataSet = null;
				
				this.isBusy = false;
			}
			else
				result = false;
				
			
			return result;			
		}
		
		// Сохраняет полученный uin для только что переданного сообщения 
		//
		public bool SaveMessageErrorStatus(long messageID, int status)
		{
			bool result = true;
			
			if (ConnectionIsOpen() || CreateConnection())
			{
				while (this.isBusy)
				{
					System.Threading.Thread.Sleep(SQLSettings.waitingInterval);
				}
				
				this.isBusy = true;
				
				DataSet dbDataSet = new DataSet();						
				SqlDataAdapter dbAdapter = new SqlDataAdapter(String.Format("SELECT id, send_status, send_date FROM Messages WHERE id={0}", messageID), dbConn);
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dbAdapter);
				dbAdapter.Fill(dbDataSet, "Messages");	
				
				if (dbDataSet.Tables["Messages"].Rows.Count > 0)
				{
					foreach(DataRow row in dbDataSet.Tables["Messages"].Rows)
					{	
						row["send_status"] = status;														
						row["send_date"] = DateTime.Now;					
					}
					
					try
					{			
						dbAdapter.Update(dbDataSet, "Messages");
					}
					catch(Exception e)
					{				
						result = false;
						this.textStatus = e.Message;
					}
				}
				else
				{
					result = false;
					this.textStatus = String.Format("Не найдено сообщение с id {0}", messageID);
				}	
				
				commandBuilder.Dispose();
				commandBuilder = null;
				
				dbAdapter.Dispose();
				dbAdapter = null;
				
				dbDataSet.Dispose();
				dbDataSet = null;
				
				this.isBusy = false;
				
			}
			else
				result = false;
				
			
			return result;			
		}
	}	
}