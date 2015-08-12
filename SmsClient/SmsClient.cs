/*
 * EasySMPP - SMPP protocol library for fast and easy
 * SMSC(Short Message Service Centre) client development
 * even for non-telecom guys.
 * 
 * Easy to use classes covers all needed functionality
 * for SMS applications developers and Content Providers.
 * 
 * Written for .NET 2.0 in C#
 * 
 * Copyright (C) 2006 Balan Andrei, http://balan.name
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://easysmpp.sf.net/
 * 
 * 
 * "Support Open Source software. What about a donation today?"
 *
 * 
 * File Name: SmsClient.cs
 * 
 * File Authors:
 * 		Balan Name, http://balan.name
 */
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace SMSCenter
{
    public class SmsClient
    {
        private SMPPClient smppClient;
        private int waitForResponse = 30000;
		private string messageID;
		private bool writeLog;
		private	SQLConnector sqlConn;		
        private SortedList<int, AutoResetEvent> events = new SortedList<int,AutoResetEvent>();
        private SortedList<int, int> statusCodes = new SortedList<int, int>();
        #region Properties
        public int WaitForResponse
        {
            get { return waitForResponse; }
            set { waitForResponse = value; }
        }
        
        public SQLConnector SQL
        {
            get { return sqlConn; }
            set { sqlConn = value; }
        }
        
        public string MessageID
        {
            get { return messageID; }            
        }
        #endregion Properties

        #region Public functions
        public SmsClient(bool writeLog)
        {
        	this.writeLog = writeLog;
        	
            smppClient = new SMPPClient();
            smppClient.WriteLog = writeLog;
            smppClient.OnDeliverSm += new DeliverSmEventHandler(onDeliverSm);
            smppClient.OnSubmitSmResp += new SubmitSmRespEventHandler(onSubmitSmResp);

            smppClient.OnLog += new LogEventHandler(onLog);
            smppClient.LogLevel = LogLevels.LogErrors;

            LoadConfig();

            smppClient.Connect();

        }
        public void Connect()
        {
            smppClient.Connect();
        }
        public void Disconnect()
        {
            smppClient.Disconnect();
        }
             
        public bool CanSend()
        {
        	return smppClient.CanSend;
        }
        
        public bool SendSms(string from, string to, string text, int msgID, bool isLongMessage)
        {
            bool result = false;
            if (smppClient.CanSend)
            {                
            	AutoResetEvent sentEvent;
                int sequence;
                lock (events)
                {
                	sequence = isLongMessage ? smppClient.SendLongSms(from, to, text, msgID) : smppClient.SendSms(from, to, text, msgID);
                    sentEvent = new AutoResetEvent(false);
                    events[sequence] = sentEvent;
                }
                if (sentEvent.WaitOne(waitForResponse, true))
                {
                    lock (events)
                    {
                        events.Remove(sequence);
                    }
                    int statusCode;
                    bool exist;
                    lock (statusCodes)
                    {
                        exist = statusCodes.TryGetValue(sequence, out statusCode);
                    }
                    if (exist)
                    {
                        lock (statusCodes)
                        {
                            statusCodes.Remove(sequence);
                        }
                        if (statusCode == StatusCodes.ESME_ROK)
                            result = true;
                    }
                }
            }
            return result;
        }

        #endregion Public functions

        #region Events

        public event NewSmsEventHandler OnNewSms;

        public event LogEventHandler OnLog;

        #endregion Events

        #region Private functions
        private void onDeliverSm(DeliverSmEventArgs args)
        {                   	
        	if (!SQL.SaveMessageDeliveryDate(Convert.ToInt64(args.ReceiptedMessageID), args.MessageState, args.TextString))
    		{
        		Notification.AddEventToLog("�� ������� ��������� ���� �������� ���������: " + SQL.TextStatus, true);
        		smppClient.sendDeliverSmResp(args.SequenceNumber, StatusCodes.ESME_RUNKNOWNERR);
    		}
        	else
        	{
        		smppClient.sendDeliverSmResp(args.SequenceNumber, StatusCodes.ESME_ROK);
        	}
        	    
        	//smppClient.sendDeliverSmResp(args.SequenceNumber, StatusCodes.ESME_ROK);
        	
            if (OnNewSms != null)
                OnNewSms(new NewSmsEventArgs(args.From, args.To, args.TextString));
        }
        private void onSubmitSmResp(SubmitSmRespEventArgs args)
        {
        	// �������� ������������� ����, ��� ������������ ���������
        	// �������� SMPP, �������� ��� uin
        	messageID = args.MessageID;
        	if (SQL != null)
        	{        		
        		if (args.Status == StatusCodes.ESME_ROK)
        		{
        			if (!SQL.SaveMessageUIN(Convert.ToInt64(args.Sequence), Convert.ToInt64(messageID)))
	        		{
		        		Notification.AddEventToLog("�� ������� ��������� uin ���������: " + SQL.TextStatus, true);
	        		}
        		}
        		else
        		{
        			if (!SQL.SaveMessageErrorStatus(Convert.ToInt64(args.Sequence), args.Status))
	        		{
		        		Notification.AddEventToLog("�� ������� ��������� ������ ���������: " + SQL.TextStatus, true);        		
	        		}
        		}
        	}
        	
            AutoResetEvent sentEvent;
            bool exist;
            lock (events)
            {
                exist = events.TryGetValue(args.Sequence, out sentEvent);
            }
            if (exist)
            {
                lock (statusCodes)
                {
                    statusCodes[args.Sequence] = args.Status;
                }
                sentEvent.Set();
            }
        }
        private void onLog(LogEventArgs args)
        {        	
            if (OnLog != null)
                OnLog(args);
        }
        private void LoadConfig()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SMSC));

                if (!File.Exists("smsc.cfg"))
                {
                    using (TextWriter writer = new StreamWriter("smsc.cfg"))
                    {
                        serializer.Serialize(writer, new SMSC("amital", "192.168.0.1", 2776, "test", "test", "test", 0, 0, "", 0));
                    }
                    onLog(new LogEventArgs("����������, ��������� ��������� ����������� � ����� smsc.cfg"));
                }
                using (FileStream fs = new FileStream("smsc.cfg", FileMode.Open))
                {
                    SMSC smsc = (SMSC)serializer.Deserialize(fs);
                    smppClient.AddSMSC(smsc);
                }
            }
            catch (Exception ex)
            {
                onLog(new LogEventArgs("Error on loading smsc.cfg : " + ex.Message));
            }

        }

        #endregion


    }
}
