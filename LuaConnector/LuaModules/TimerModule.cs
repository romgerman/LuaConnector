using System;
using System.Collections.Generic;
using System.Linq;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	internal class LuaTimer
	{
		[Lua.MoonSharpHidden]
		public string Name;

		[Lua.MoonSharpHidden]
		public Lua.Script Owner;

		private double _interval;
		private bool _repeat;
		private bool _isPaused;
		private bool _isStopped;

		private DateTime _startTime;

		private Lua.Closure _callback;

		internal LuaTimer(double interval, Lua.Closure callback, bool repeat = false)
		{
			this._interval = interval;
			this._repeat = repeat;
			this._callback = callback;
			this._isPaused = false;
			this._isStopped = true;
		}

		public void start()
		{
			if (_isStopped)
			{
				_startTime = DateTime.Now;
				_isStopped = false;
			}

			if (_isPaused)
				_isPaused = false;
		}

		public void pause()
		{
			_isPaused = true;
		}

		public void stop()
		{
			_isStopped = true;
			_isPaused = false;
		}

		public void destroy()
		{
			TimerModule.DestroyTimerForScript(Owner, this);
		}

		internal void Tick()
		{
			if (_isPaused || _isStopped)
				return;

			if ((DateTime.Now - _startTime).TotalMilliseconds >= _interval)
			{
				_callback.Call();

				if (!_repeat)
					stop();

				_startTime = DateTime.Now;
			}
		}
	}

	[Lua.MoonSharpModule(Namespace = "timer")]
	internal class TimerModule
	{
		private static Dictionary<Lua.Script, HashSet<LuaTimer>> _timers = new Dictionary<Lua.Script, HashSet<LuaTimer>>();
		private static TimerEx _instance = new TimerEx(10.0, true) { Elapsed = ProcessTimers };
		public static bool IsHighResolutionSet = false;

		public static void MoonSharpInit(Lua.Table globalTable, Lua.Table namespaceTable)
		{
			Lua.UserData.RegisterType<LuaTimer>();
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue setHighResolution(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				IsHighResolutionSet = true;
				context.GetScript().Options.CheckThreadAccess = false;
				_instance.Start();

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		/// <summary>
		/// Creates a new timer -> timer.create("name", 100, true, function() print("tick") end)
		/// <para>Name can be null</para>
		/// </summary>
		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue create(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "create", Lua.DataType.String, true);
				var delay = args.AsType(1, "create", Lua.DataType.Number);
				var repeat = args.AsType(2, "create", Lua.DataType.Boolean);
				var callback = args.AsType(3, "create", Lua.DataType.Function);

				if (!_timers.ContainsKey(context.GetScript()))
					_timers.Add(context.GetScript(), new HashSet<LuaTimer>());

				if (!name.IsNil() && _timers[context.GetScript()].Where(x => x.Name.Equals(name.String)).Count() > 0)
					throw new Lua.ScriptRuntimeException("There is already a timer with that name");

				var luaTimer = new LuaTimer(delay.Number, callback.Function, repeat.Boolean)
				{
					Name = name.String,
					Owner = context.GetScript()
				};

				_timers[context.GetScript()].Add(luaTimer);

				return Lua.DynValue.FromObject(context.GetScript(), luaTimer);
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		/// <summary>
		/// Get timer by name
		/// </summary>
		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue get(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "get", Lua.DataType.String);

				if (!_timers.ContainsKey(context.GetScript()))
					return Lua.DynValue.Nil;

				var timer = _timers[context.GetScript()].FirstOrDefault(x => x.Name.Equals(name.String));

				if (timer == null)
					return Lua.DynValue.Nil;

				return Lua.DynValue.FromObject(context.GetScript(), timer);
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue start(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "start", Lua.DataType.String);

				if (!_timers.ContainsKey(context.GetScript()))
					return Lua.DynValue.Nil;

				var timer = _timers[context.GetScript()].FirstOrDefault(x => x.Name.Equals(name.String));

				if (timer == null)
					return Lua.DynValue.Nil;

				timer.start();

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue pause(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "pause", Lua.DataType.String);

				if (!_timers.ContainsKey(context.GetScript()))
					return Lua.DynValue.Nil;

				var timer = _timers[context.GetScript()].FirstOrDefault(x => x.Name.Equals(name.String));

				if (timer == null)
					return Lua.DynValue.Nil;

				timer.pause();

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue stop(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "stop", Lua.DataType.String);

				if (!_timers.ContainsKey(context.GetScript()))
					return Lua.DynValue.Nil;

				var timer = _timers[context.GetScript()].FirstOrDefault(x => x.Name.Equals(name.String));

				if (timer == null)
					return Lua.DynValue.Nil;

				timer.stop();

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue destroy(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "destroy", Lua.DataType.String);

				if (!_timers.ContainsKey(context.GetScript()))
					return Lua.DynValue.Nil;

				var timer = _timers[context.GetScript()].FirstOrDefault(x => x.Name.Equals(name.String));

				if (timer == null)
					return Lua.DynValue.Nil;

				timer.destroy();

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		public static void ProcessTimers()
		{
			foreach (var script in _timers)
				foreach(var timer in script.Value)
					timer.Tick();
		}

		// Global methods

		public static void DestroyTimerForScript(Lua.Script script, string name)
		{
			if (!_timers.ContainsKey(script))
				return;

			_timers[script].RemoveWhere(x => x.Name.Equals(name));
		}

		internal static void DestroyTimerForScript(Lua.Script script, LuaTimer timer)
		{
			if (!_timers.ContainsKey(script))
				return;

			_timers[script].Remove(timer);
		}

		public static void DestroyAllTimerForScript(Lua.Script script)
		{
			if (!_timers.ContainsKey(script))
				return;

			_timers[script].Clear();
		}

		public static void DestroyAllTimers()
		{
			foreach(var kvp in _timers)
			{
				kvp.Value.Clear();
			}

			_timers.Clear();
		}
	}
}
