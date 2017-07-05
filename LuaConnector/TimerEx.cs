using System;
using System.Timers;

namespace LuaConnector
{
	class TimerEx : IDisposable
	{
		public bool Finished { get; private set; }

		public bool Repeat
		{
			get { return _timer.AutoReset; }
			set { _timer.AutoReset = value; }
		}

		public double Interval
		{
			get { return _timer.Interval; }
			set
			{
				_timer.Interval = value;
				if (_isPaused || _wasPaused)
					_oldInterval = value;
			}
		}

		public double Remaining
		{
			get { return (_startTime - DateTime.UtcNow).Milliseconds; }
		}

		public Action Elapsed { get; set; }

		private Timer _timer;

		private DateTime _startTime;
		private DateTime _pauseTime;

		private bool _wasPaused;
		private bool _isPaused;
		private double _oldInterval;

		/// <summary>
		/// Creates a new timer
		/// </summary>
		/// <param name="interval">Time between ticks in milliseconds</param>
		/// <param name="repeat"></param>
		public TimerEx(double interval, bool repeat = false)
		{
			_timer = new Timer(interval);
			_timer.AutoReset = repeat;
			_timer.Elapsed += OnTick;
		}

		public void Start()
		{
			if (_wasPaused)
				_timer.Interval = (_startTime - _pauseTime).Milliseconds;

			Finished = false;
			_startTime = DateTime.UtcNow;
			_isPaused  = false;
			_timer.Start();
		}

		public void Pause()
		{
			if (_isPaused || !_timer.Enabled)
				return;

			_timer.Stop();
			_pauseTime = DateTime.UtcNow;
			_isPaused  = true;
			_wasPaused = true;
			_oldInterval = _timer.Interval;
		}

		public void Stop()
		{
			_timer.Stop();
			
			_isPaused = false;
			
			if (_wasPaused)
			{
				_wasPaused = false;
				_timer.Interval = _oldInterval;
			}
		}

		private void OnTick(object sender, ElapsedEventArgs e)
		{
			if (!_timer.AutoReset)
				Finished = true;

			Elapsed?.Invoke();

			if (_wasPaused)
			{
				_wasPaused = false;
				_isPaused = false;
				_timer.Interval = _oldInterval;
			}

			_startTime = e.SignalTime;
		}

		public void Dispose()
		{
			Stop();
			_timer.Dispose();
		}
	}
}
