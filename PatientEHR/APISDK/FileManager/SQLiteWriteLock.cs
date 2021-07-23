using System;
using System.Collections.Generic;
using System.Threading;

namespace PatientEHR.APISDK.FileManager
{
	/// <summary>
	/// 用于在多线程访问sqlite时防止同步写导致锁文件
	/// 
	/// 使用方法：
	/// using (SQLiteWriteLock sqliteLock = new SQLiteWriteLock(SQLite链接字符串))
	/// {
	///     //sqlite 写操作代码
	/// }
	/// 
	/// 可以通过在配置文件appSettings节中添加设置 SQLiteWriteLockTimeout 的value值控制锁等待的超时时间，该值必须为正整数数字，单位为毫秒，
	/// 默认的超时时间是1000ms
	/// </summary>
	public sealed class SQLiteWriteLock : IDisposable
	{
		#region 静态字段和属性
		const short WAIT_TIME = 5;
		static readonly object locker = new object ();
		static System.Collections.Concurrent.ConcurrentDictionary<string, int> _dbThreadIdDict = new System.Collections.Concurrent.ConcurrentDictionary<string, int> ();
		private bool _isLocking = false;
		/// <summary>
		/// 获得写操作的超时时间，单位为毫秒，可以通过配置文件appSettings节中添加设置 SQLiteWriteLockTimeout 的value值控制锁等待的超时时间，该值必须为正整数数字，单位为毫秒
		/// 默认的超时时间是1000ms
		/// </summary>
		public static int SQLiteWriteLockTimeout {
			get {
				return 10000;
			}
		}
		#endregion
		private readonly string _connString;
		private string dbName => System.IO.Path.GetFileName(_connString);
		//隐藏无参构造函数
		private SQLiteWriteLock ()
		{
		}

		public SQLiteWriteLock (string connString)
		{
			_connString = connString;
			AcquireWriteLock ();
		}
		#region 私有方法
		private void AcquireWriteLock()
		{
			int threadId = Thread.CurrentThread.ManagedThreadId;

			int waitTimes = 0;
			while (_dbThreadIdDict.ContainsKey(_connString))  // && _dbThreadIdDict[_connString] != threadId)
			{
				Thread.Sleep(WAIT_TIME);
				waitTimes += WAIT_TIME;
				//Console.WriteLine (_connString + " wait for " + waitTimes + " ms");
				if (waitTimes > SQLiteWriteLockTimeout)
				{
					throw new TimeoutException($"SQLite Timeout for {System.IO.Path.GetFileName(_connString)}");
				}
			}

			//lock (locker)
			//{
			if (!_dbThreadIdDict.ContainsKey(_connString))
			{
				_isLocking = _dbThreadIdDict.TryAdd(_connString, threadId);
				if (!dbName.Equals("localfile.db3"))
				{
					Console.WriteLine($"lock -> { dbName} = {_isLocking}");
				}
			}
			//}
			if (!_isLocking)
			{
				throw new TimeoutException($"SQLite lock failed for {dbName}");
			}
		}

		private void ReleaseWriteLock ()
		{
			if (_isLocking)
			{
				if (_dbThreadIdDict.ContainsKey(_connString))
				{
					_dbThreadIdDict.TryRemove(_connString, out _);
					if (!dbName.Equals("localfile.db3"))
					{
						Console.WriteLine($"relase -> { dbName}");
					}
				}
			}
		}
		#endregion
		#region IDisposable 成员
		public void Dispose ()
		{
			ReleaseWriteLock ();
		}
		#endregion
	}
}

