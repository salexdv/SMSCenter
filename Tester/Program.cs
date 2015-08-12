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
 * File Name: Program.cs
 * 
 * File Authors:
 * 		Balan Name, http://balan.name
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SMSCenter
{
    class Program
    {
        static void Main(string[] args)
        {

        	DateTime beginSend = DateTime.Now;
        	        	
        	System.Threading.Thread.Sleep(1000);
        	        	
        	DateTime endSend = DateTime.Now;
        	
        	TimeSpan sp = endSend - beginSend;
        	
        	Console.WriteLine("{0}", Convert.ToInt32(sp.Seconds));
        	
        	/*
        	SmsClient client = new SmsClient(false);                        
            client.Connect();            
            
            System.Threading.Thread.Sleep(2000);
            //Console.ReadLine();
            
            int n = 0;
            
            do 
            {            	
	            string from = "amital";
				string to = "79204606556";
				string text = "test " + n;
	            
	            if (client.SendSms(from, to, text, 5, false))
	            {
	                Console.WriteLine("Message sent {0}", client.MessageID);	            	
	            }
	            else
	                Console.WriteLine("Error");
	            
	            n++;
	            
            }while(n < 1);
            
            client.Disconnect();
            */
            
            Console.ReadLine();
        }
        
    }
}
