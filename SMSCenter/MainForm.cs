/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 05.05.2014
 * Time: 12:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ComponentModel;
using System.Text;

namespace SMSCenter
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		const int BUFFER_SIZE = 524288; // Размер буфера для получения входящего потока
		private SQLConnector SQL; // Подключение к базе MSSQL
		private SmsClient clientSMS; // Клиент для отправки сообщений
		private	Settings settings; // Настройки программы
		private BackgroundWorker BackgroundHTTPWorker; // Поток для прослушивания HTTP
		private BackgroundWorker BackgroundSMPPPWorker; // Поток для отправки сообщений 
		private TcpListener listenerHTTP;
		
		// Процедура загружает настройки программы из файла
		//
		private void LoadConfig()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                if (!File.Exists("settings.cfg"))
                {
                    using (TextWriter writer = new StreamWriter("settings.cfg"))
                    {
                    	serializer.Serialize(writer, new Settings("w8r2", "SMS", "Login1C", "662421", 2776, false, false));
                    }
                }
                using (FileStream fs = new FileStream("settings.cfg", FileMode.Open))
                {
                    settings = (Settings)serializer.Deserialize(fs);                    
                }
            }
            catch (Exception e)
            {
            	Notification.AddEventToLog("Не удалось загрузить настройки программы: " + e.Message, true);
            }

        }
					
		// Подключение к SQL
		//
		private bool ConnectToSQLServer()
		{						
			Notification.AddEventToLog("Подключение к SQL-серверу...");
			
			SQL = new SQLConnector(settings.SQLServer, settings.SQLDatebase, settings.SQLUsername, settings.SQLPassword);
			
			bool connected = SQL.IsOpen;
			
			if (!connected)			
				Notification.AddEventToLog("Не удалось подключиться: " + SQL.TextStatus, true);
			else			
				Notification.AddEventToLog("Подключение успешно выполнено");
			
			return connected;
						
		}
		
		// Получение источника для отображения неотправленных сообщений
		//
		private bool LoadUnsentMessages()
		{
			string countMessage = "";
					
			DataTable source = SQL.GetDataSourceForUnsentMessages();
						
			if (source != null)			
			{
				if (UnsentMessages.InvokeRequired)
					Log.BeginInvoke(new Action(delegate() {UnsentMessages.DataSource = source;}));
				else
					UnsentMessages.DataSource = source;
				
				countMessage = source.Rows.Count.ToString();
			}
			else
			{
				Notification.AddEventToLog("Не удалось получить список неотправленных сообщений: " + SQL.TextStatus, true);				
				countMessage = "-"; 
			}
			
			if (lbCountUnsentMessages.InvokeRequired)
					lbCountUnsentMessages.BeginInvoke(new Action(delegate() {lbCountUnsentMessages.Text = countMessage;}));
				else
					lbCountUnsentMessages.Text = countMessage;
			
			return (source != null);
		}
		
		// Получение источника для отображения всех сообщений
		//
		private bool LoadMessages(bool applyFilter)
		{
			
			DataTable source = null;
			
			if (applyFilter)			
			{				
				source = SQL.GetDataSourceForMessages(dtpDateBegin.Value, dtpDateEnd.Value, (cbStatus.SelectedIndex == 1), (cbStatus.SelectedIndex == 2));
			}
			else
			{
				source = SQL.GetDataSourceForMessages();
			}
						
			if (source != null)			
			{
				if (AllMessages.InvokeRequired)
					Log.BeginInvoke(new Action(delegate() {AllMessages.DataSource = source;}));
				else
					AllMessages.DataSource = source;
								
			}
			else
			{
				Notification.AddEventToLog("Не удалось получить список сообщений: " + SQL.TextStatus, true);								
			}
			
			
			return (source != null);
		}
		
		// Проверяет возможно ли отправлять сообщения, если нет, то сервис отправки останавливается
        //
        private void SMPPService(object sender, DoWorkEventArgs e)
        {
        	clientSMS = new SmsClient(settings.WriteSMPPLog);
        	clientSMS.SQL = SQL;
        	Notification.AddEventToLog("Запуск сервиса отправки сообщений...");
        	
        	int sendCount = 0;
        	int allSend = 0;
        	int unsendCount = 0;
        	
        	System.Threading.Thread.Sleep(ParametersSMS.SMPP_CONNECTION_TIMEOUT);
        	        	
        	if (clientSMS.CanSend())
        	{
        		Notification.AddEventToLog("Сервис отправки сообщений успешно запущен");
        		
        		while (!BackgroundSMPPPWorker.CancellationPending)
        		{        			
        			List<SMS> messages = SQL.GetUnsentMessages(ParametersSMS.MAX_SMS_IN_PORTION);
        			
        			if (messages.Count > 0)
        			{    
						sendCount = 0;
        				unsendCount = 0;        				
	        			        			
        				Notification.AddEventToLog("Сообщений для отправки " + messages.Count.ToString());
        				
        				DateTime beginSend = DateTime.Now;        				
	        			
	        			foreach (SMS currentSMS in messages)
	        			{   
	        				/*
	        				if (!SQL.SaveMessageUIN(currentSMS.id, currentSMS.id + 1))
	        				{
	        					Notification.AddEventToLog("Не удалось сохранить uin сообщения: " + SQL.TextStatus, true);
	        					unsendCount++;
	        				}
	        				else
	        				{
	        					if (!SQL.SaveMessageDeliveryDate(currentSMS.id + 1, 5, "Bla-bla-bla"))
	        					{
	        						Notification.AddEventToLog("Не удалось сохранить дату доставки сообщения: " + SQL.TextStatus, true);
	        						unsendCount++;
	        					}
	        					else
	        					{
	        						sendCount++;
	        					}
	        				}
	        				*/	        					        				
	        				
	        				if (clientSMS.SendSms(currentSMS.source, currentSMS.NumberTxt, currentSMS.text, currentSMS.id, true))
	        				{
	        					sendCount++;        					
	        				}
	        				else
	        				{
	        					unsendCount++;
	        				}
	        				
	        				
	        				if (sendCount % ParametersSMS.MAX_SMS_PER_SECOND == 0)	    
	        				{
	        					// Соблюдение интервала между отправками
	        					DateTime endSend = DateTime.Now;	        			
			        			TimeSpan sendTime = endSend - beginSend;
					        			
			        			int elapsedMillisecond = Convert.ToInt32(sendTime.TotalMilliseconds);
			        			if (elapsedMillisecond < 1000)
			        			{
			        				System.Threading.Thread.Sleep(1000 - elapsedMillisecond);
			        			}
	        				}
	        				
	        				if (sendCount % 1000 == 0)
		        			{
		        				LoadUnsentMessages();
		        			}
	        				
	        			}
	        			
	        			if (sendCount > 0)        			
	        				Notification.AddEventToLog(String.Format("Успешно отправлено {0} сообщений", sendCount));
	        			
	        			if (unsendCount > 0)
	        				Notification.AddEventToLog(String.Format("Не удалось отправить {0} сообщений", unsendCount), true);	        		
	        			
	        			BackgroundSMPPPWorker.CancelAsync();
	        			
        			}
        			else
        			{
        				LoadUnsentMessages();
        				messages = null;
    					GC.Collect();
            			GC.WaitForFullGCComplete();
            			System.Threading.Thread.Sleep(ParametersSMS.SENDING_INTERVAL);
        			}        			        		        	
        		}
        	}
        	else
        	{
        		Notification.AddEventToLog("Не удалось запустить сервис отправки сообщений", true);
        		BackgroundSMPPPWorker.CancelAsync();
				clientSMS.Disconnect();
				clientSMS = null;
				GC.Collect();
                GC.WaitForFullGCComplete();
        	}
        		                	
        }
		
		// Прослушивание порта в отдельном потоке для получения входящих сообщений
        //
        private void HTTPService(object sender, DoWorkEventArgs e)
        {
            Notification.AddEventToLog("Сервис получения сообщений успешно запущен");
            			                        
            while (!BackgroundHTTPWorker.CancellationPending)
            {
            	int result = ExchangeErrors.UnknownError;

                TcpClient Client = null;
                try
                {
                    // Запускаем прослушивание порта                    
                    Client = listenerHTTP.AcceptTcpClient();                    
                    Notification.AddEventToLog("Входящее соединение (" + Client.Client.RemoteEndPoint.ToString() + ")");
                }
                catch
                {
                    GC.Collect();
                    GC.WaitForFullGCComplete();
                    break;
                }

                // Входящее соединение. Получаем поток клиента
                NetworkStream ClientStream = Client.GetStream();
                
                string response = String.Empty;
				
                // Получаем запрос из потока                    
                try
                {
                	string request = "";
                	int ReadBytes = 0;
                	while (ClientStream.DataAvailable)
                	{
                		byte[] buffer = new byte[Client.Available];
                		ReadBytes += ClientStream.Read(buffer, 0, buffer.Length);
                		request += Encoding.UTF8.GetString(buffer);
                	}
                	                    
                    Notification.AddEventToLog("Получено байт: " + ReadBytes);                    
					
                    int startXML = request.IndexOf("<?xml");
                    if (startXML > 0)
                    {                    	
                    	request = request.Substring(startXML, request.Length - startXML);
                    	
                    	int endXML = request.IndexOf("</package>");
                    	
                    	if (endXML > 0)
                    	{	                    	
                    		request = request.Substring(0, endXML + 10);
	                    	
	                    	try
	                    	{	                    		
	                    		Notification.AddEventToLog("Разбор входящих сообщений...");
	                    		List<SMS> messages = XMLConverter.GetMessagesFromXML(request);	                    		
	                    		Notification.AddEventToLog("Получено сообщений: " + messages.Count.ToString());
	                    		
	                    		if (SQL.SaveNewMessages(messages))
	                    		{
	                    			LoadUnsentMessages();
	                    			result = messages.Count;
	                    		}
	                    		else
	                    		{
	                    			result = ExchangeErrors.SaveError;
	                    			Notification.AddEventToLog("Не удалось записать сообщения в базу данных: " + SQL.TextStatus, true);
	                    		}	                    		
	                    		
	                    	}
	                    	catch(Exception exc)
	                    	{
	                    		Notification.AddEventToLog("Не удалось разобрать входящий XML: " + exc.Message, true);	
	                    		result = ExchangeErrors.ErrorXML;
	                    	}
                    	}
                    	else
                    	{                    		                    		
                    		result = ExchangeErrors.CorruptedData;
                    	}
                    }
                    else
                    	result = ExchangeErrors.MissingXML;

                    string resultStr = result.ToString();
                    
                    while (resultStr.Length < 6)
                    {
                    	resultStr += " ";
                    }
                                        
                    // Возвращаем клиенту ответ 
                    Notification.AddEventToLog("Возвращен ответ \"" + resultStr + "\"");
                    byte[] responseBuffer = Encoding.UTF8.GetBytes(resultStr);                    
                    ClientStream.Write(responseBuffer, 0, responseBuffer.Length);
                    ClientStream.Close();
                                        
                    responseBuffer = null;

                    Client = null;
                    ClientStream.Dispose();                    
                }
                catch (Exception exc)
                {
                    Notification.AddEventToLog("Ошибка при получении входящих данных: " + exc.Message, true);
                    ClientStream.Close();
                }

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
                        
        }
		
		// Запуск сервиса получения сообщений
		//
		private bool StartHTTPService()
		{
			bool serviceStarted = true; 
			
			if (listenerHTTP == null)
                            {
				Notification.AddEventToLog("Запуск сервиса получения сообщений (" + settings.ListenPort.ToString() + ")...");
                listenerHTTP = new TcpListener(IPAddress.Any, settings.ListenPort);
                try
                {
	                listenerHTTP.Start();
                }
                catch(Exception e)
                {
                	Notification.AddEventToLog("Не удалось запустить сервис получения сообщений: " + e.Message, true);
                }
                BackgroundHTTPWorker.RunWorkerAsync();
            }
            else
                Notification.AddEventToLog("Сервер уже запущен");
            
            CreateTrayIconText();
			
			return serviceStarted;
		}
		
		// Остановка сервиса получения сообщений
		//
		private bool StopHTTPService(bool showUnrun = false)
		{
			bool serviceStoped = true; 
						
			if (listenerHTTP != null)
			{
                BackgroundHTTPWorker.CancelAsync();                
                listenerHTTP.Stop();                
                listenerHTTP = null;
                Notification.AddEventToLog("Сервис получения сообщений остановлен");
                GC.Collect();
                GC.WaitForFullGCComplete();
            }
            else
            {
            	if (showUnrun)
            		Notification.AddEventToLog("Сервис получения сообщений не запущен");
            }
            
            CreateTrayIconText();
			
			return serviceStoped;	
		}
		
		// Запуск сервиса отправки сообщений
		//
		private bool StartSMPPService()
		{
			bool serviceStarted = true;

			if (clientSMS == null)
			{				
				BackgroundSMPPPWorker.RunWorkerAsync();				
			}
			else
				Notification.AddEventToLog("Сервис отправки сообщений уже запущен");
			
			CreateTrayIconText();
			
			return serviceStarted;			
		}
		
		// Остановка сервиса отправки сообщений
		//
		private bool StopSMPPService(bool showUnrun = false)
		{			
			bool serviceStoped = true;
		
			if (clientSMS != null)
			{				
				BackgroundSMPPPWorker.CancelAsync();
				clientSMS.Disconnect();
				clientSMS = null;
				GC.Collect();
                GC.WaitForFullGCComplete();
				Notification.AddEventToLog("Сервис отправки сообщений остановлен");
			}
			else
			{
				if (showUnrun)
					Notification.AddEventToLog("Сервис отправки сообщений не запущен");
			}
			
			CreateTrayIconText();
			
			return serviceStoped;	
		}
		
		private void InitBackgroundWorkers()
		{
			BackgroundHTTPWorker = new BackgroundWorker();            
            BackgroundHTTPWorker.DoWork += new DoWorkEventHandler(HTTPService);
            BackgroundHTTPWorker.WorkerSupportsCancellation = true;
            
            BackgroundSMPPPWorker = new BackgroundWorker();            
            BackgroundSMPPPWorker.DoWork += new DoWorkEventHandler(SMPPService);            	
            BackgroundSMPPPWorker.WorkerSupportsCancellation = true;
		}
		
		
		
		public MainForm()
		{			
			InitializeComponent();
		
			// Инициализация класса для вывода сообщений на форму и в лог						
			Notification.Initialize(Log);
					
			// Инициализация фонового прослушивания порта
			InitBackgroundWorkers();
			
			// Загрузка настроек из файла
			LoadConfig();

			// Соединение с SQL
			if (ConnectToSQLServer())
			{				
				// Показ неотправленных сообщений
				if (LoadUnsentMessages())
				{								
					// Запуск службы приема сообщений
					if (settings.AutoStartHTTP)
						StartHTTPService();
				
					// Запуск службы отправки сообщений				
					if (settings.AutoStartSMPP)
						StartSMPPService();
					
					LoadMessages(false);
				}
				else
					Enabled = false;							
			}
			else
				Enabled = false;
			
			CreateTrayIconText();
			
			dtpDateBegin.Value = SQLSettings.minDateTime;
						
		}

		// Обработчик нажатия на кнопку "Запустить HTTP"
		//
		void BtStartHTTPServiceClick(object sender, EventArgs e)
		{
			StartHTTPService();
		}
		
		// Обработчик нажатия на кнопку "Остановить HTTP"
		//
		void BtStopHTTPServiceClick(object sender, EventArgs e)
		{
			StopHTTPService(true);
		}
		
		// Обработчик нажатия на кнопку "Запустить SMPP"
		//
		void BtStartSMPPServiceClick(object sender, EventArgs e)
		{
			StartSMPPService();
		}
		
		// Обработчик нажатия на кнопку "Остановить SMPP"
		//
		void BtStopSMPPServiceClick(object sender, EventArgs e)
		{
			StopSMPPService(true);
		}
		
		// При закрытии формы
		//
		void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{	
			Hide();			
		}
		
		// Перед закрытием формы
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{	
			/*			
			if (e.CloseReason == CloseReason.UserClosing)				
			{
				if (Visible)
				{
					e.Cancel = true;
					Hide();
				}
				else
				{
					if (MessageBox.Show("Вы действительно хотите завершить работу программы?", "SMS Center", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
					{
						e.Cancel = true;
					}
					else
					{
						OnFormClose();
					}
				}
			}
			*/
			OnFormClose();
		}
				
			
		// Обработчик нажатия на кнопку "Фильтровать"
		// 
		void BtFilterClick(object sender, EventArgs e)
		{
			LoadMessages(true);
		}
		
		// Обработчик нажатия на кнопку "Открыть" в трее
		//		
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			Show();
		}
		
		void CreateTrayIconText()
		{
			string trayText = "SMS Center\n";
			trayText += (listenerHTTP != null ? "Прием запущен" : "Прием остановлен") + "\n";
			trayText += clientSMS != null ? "Отправка запущена" : "Отправка остановлена";
			trayIcon.Text = trayText;
		}
		
		// Вызывается при закртии программы
		//
		void OnFormClose()
		{			
			StopHTTPService();
			StopSMPPService();
			SQL.CloseConnection();			
			Notification.Dispose();
		}
		
		// Обработчик нажатия на кнопку "Закрыть" в трее
		//		
		void CloseToolStripMenuItemClick(object sender, EventArgs e)
		{			
			Close();
		}
		
		// При выводе ячейки таблицы сообщений
		//
		void MessagesCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{						
			DataGridViewRow currentRow = ((DataGridView)sender).CurrentRow;
			DataGridViewColumn currentColumn = ((DataGridView)sender).Columns[e.ColumnIndex];			
			
			if (e.Value != null)
			{
				Type type = e.Value.GetType();
				if (type == typeof(DateTime))
				{
					if ((DateTime)e.Value == SQLSettings.minDateTime)
					{
						e.Value = "";
					}
				}
				else if ((type == typeof(long)) || (type == typeof(int)) || (type == typeof(byte)) || (type == typeof(decimal)))
				{
					long currentValue = Convert.ToInt64(e.Value);
					
					if (currentValue == 0)
					{
						e.Value = "";
					}
					else
					{						
						if (currentColumn.Name.ToLower() == "send_status")
						{
							e.Value = Notification.GetErrorDescription(Convert.ToInt32(currentValue));
						}						
					}
				}
			}
		}
		
		// При выводе ячейки таблицы всех сообщений
		//
		void AllMessagesCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{	
			MessagesCellFormatting(sender, e);
		}
		
		// При выводе ячейки таблицы неотправленных сообщений
		//
		void UnsentMessagesCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			MessagesCellFormatting(sender, e);
		}
		
		// Выводит значок с статуса в необходимую ячейку
		//
		void CellStatusPainting(object sender, DataGridViewCellPaintingEventArgs e, Bitmap statusImage, string statusText)
		{				
			using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor), gridBrush = new SolidBrush(((DataGridView)sender).GridColor))
			{											
				// Очищаем содержимое
				e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
				// Рисуем грани						
				using (Pen gridLinePen = new Pen(gridBrush))
				{	
					
					e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
		                    e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
		                    e.CellBounds.Bottom - 1);
		            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
		                    e.CellBounds.Top, e.CellBounds.Right - 1,
		                    e.CellBounds.Bottom);
					
					e.Graphics.DrawImageUnscaled(Image.FromHbitmap(statusImage.GetHbitmap()), e.CellBounds.X + 1, e.CellBounds.Y + 2);
					
					if (e.Value != null)
		            {								
						// Выводим текст
						e.Graphics.DrawString(String.IsNullOrEmpty(statusText) ? e.Value.ToString() : statusText, e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 16, e.CellBounds.Y + 3, StringFormat.GenericDefault);
		            }
												
					e.Handled = true;
				}
			}
		}
		
		// При прорисовке ячейки таблицы со всеми сообщениями
		//
		void AllMessagesCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{												
			if (e.RowIndex >= 0)
			{
				using (DataGridViewRow currentRow = AllMessages.Rows[e.RowIndex])
				{
					// Вывод признака сообщения "отправлено/нет"
					if (e.ColumnIndex == -1)
					{				
						if ((DateTime)currentRow.Cells["send_date"].Value != SQLSettings.minDateTime && Convert.ToInt32(currentRow.Cells["send_status"].Value) == 0)
						{
							CellStatusPainting(sender, e, Properties.Resources.statusOK, "");
						}
						else
						{
							CellStatusPainting(sender, e, Properties.Resources.statusFail, "");
						}
					}
					else
					{
						int columnDeliveryIdx = 8;
						if (e.ColumnIndex == columnDeliveryIdx && e.RowIndex >= 0)
						{
							if ((DateTime)currentRow.Cells["delivery_date"].Value != SQLSettings.minDateTime)
							{
								int deliveryStatus = Convert.ToInt32(currentRow.Cells["delivery_status"].Value);
								CellStatusPainting(sender, e, (deliveryStatus == DeliveryCodes.DELIVERED) ? Properties.Resources.statusOK : Properties.Resources.statusFail, Notification.GetDeliveryErrorDescription(deliveryStatus));
							}
						}
					}
				}
			}
		}
		
		// Обработчик нажатия на счетчик сообщений
		//
		void LbCountUnsentMessagesClick(object sender, EventArgs e)
		{
			LoadUnsentMessages();
		}
	}
}

