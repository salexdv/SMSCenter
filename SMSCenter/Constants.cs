/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 07.05.2014
 * Time: 17:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SMSCenter
{
	public class ParametersSMS
	{
		public const int MAX_SMS_IN_PORTION = 1; // Максимальное количество сообщений, получаемых из БД за раз (0 - без ограничений)
		public const int MAX_SMS_PER_SECOND = 100; // Максимальное количество отправляемых в секунду сообщений
		public const int SENDING_INTERVAL = 5000; // Интервал между проверками наличия новых сообщений для отправки в миллисекундах
		public const int SMPP_CONNECTION_TIMEOUT = 5000; // Время ожидания соединения с SMPP сервером в миллисекундах
	}
}
