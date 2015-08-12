/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 06.05.2014
 * Time: 15:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml;
using System.Collections.Generic;

namespace SMSCenter
{
	/// <summary>
	/// Description of XMLConverter.
	/// </summary>
	public static class XMLConverter
	{
		public static List<SMS> GetMessagesFromXML(string text)
		{
			List<SMS> messages = new List<SMS>();
						
			XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text);			

			foreach (XmlNode rootNode in xmlDoc.ChildNodes)
            {
            	if (rootNode.Name == "package")
            	{
            		foreach (XmlNode childNode in rootNode.ChildNodes)
            		{
            			if (childNode.Name == "send")
            			{
            				foreach (XmlNode messageNode in childNode.ChildNodes)
            				{
            					if (messageNode.Name == "message")
            					{		
            						SMS newSMS = new SMS();            						
            						newSMS.id = Convert.ToInt32(messageNode.Attributes["id"].Value);            						            						
            						newSMS.number = Convert.ToInt64(messageNode.Attributes["receiver"].Value);            						
            						newSMS.source = messageNode.Attributes["sender"].Value;
            						newSMS.text = messageNode.InnerText;
            						
            						messages.Add(newSMS);
            					}
            				}
            			}
            			else
            				throw new System.SystemException("Неверная структура XML");
            		}
            	}            	
            }
                        
            xmlDoc = null;
            
			return messages;            
		}
	}
}
