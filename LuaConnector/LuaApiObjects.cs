using System;

using GrandTheftMultiplayer.Shared.Math;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector
{
	class Vector3Proxy
	{
		public double x => Target.X;
		public double y => Target.Y;
		public double z => Target.Z;

		[Lua.MoonSharpHidden]
		public Vector3 Target;

		[Lua.MoonSharpHidden]
		public Vector3Proxy(Vector3 v)
		{
			this.Target = v;
		}

		public static Vector3 New(double x = 0.0, double y = 0.0, double z = 0.0)
		{
			return new Vector3(x, y, z);
		}

		public double Dot(Vector3 r)
		{
			return (Target.X * r.X + Target.Y * r.Y + Target.Z * r.Z);
		}

		public Vector3 Cross(Vector3 r)
		{
			Vector3 result = new Vector3();

			result.X = Target.Y * r.Z - Target.Z * r.Y;
			result.Y = Target.Z * r.X - Target.X * r.Z;
			result.Z = Target.X * r.Y - Target.Y * r.X;

			return result;
		}

		public double Length()
		{
			return Target.Length();
		}

		public Vector3 Normal()
		{
			return Target.Normalized;
		}

		public Vector3 Normalize()
		{
			Target.Normalize();
			return Target;
		}
		
		public double DistanceTo(Vector3 v)
		{
			return Target.DistanceTo(v);
		}

		#region operators

		[Lua.MoonSharpUserDataMetamethod("__add")]
		public static Vector3 Addition(Vector3 l, Vector3 r)
		{
			return new Vector3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
		}

		[Lua.MoonSharpUserDataMetamethod("__sub")]
		public static Vector3 Subtraction(Vector3 l, Vector3 r)
		{
			return new Vector3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
		}

		[Lua.MoonSharpUserDataMetamethod("__mul")]
		public static Vector3 MultiplicationByNumber(Vector3 l, double r)
		{
			return new Vector3(l.X * r, l.Y * r, l.Z * r);
		}

		[Lua.MoonSharpUserDataMetamethod("__mul")]
		public static Vector3 MultiplicationByNumber2(double l, Vector3 r)
		{
			return new Vector3(l * r.X, l * r.Y, l * r.Z);
		}

		[Lua.MoonSharpUserDataMetamethod("__div")]
		public static Vector3 DivisionByNumber(Vector3 l, double r)
		{
			return new Vector3(l.X / r, l.Y / r, l.Z / r);
		}

		[Lua.MoonSharpUserDataMetamethod("__unm")]
		public static Vector3 Inversion(Vector3 v)
		{
			return new Vector3(-v.X, -v.Y, -v.Z);
		}

		[Lua.MoonSharpUserDataMetamethod("__eq")]
		public static bool Equality(Vector3 l, Vector3 r)
		{
			return (l == r);
		}

		[Lua.MoonSharpUserDataMetamethod("__lt")]
		public static bool LessThan(Vector3 l, Vector3 r)
		{
			return (l.X < r.X && l.Y < r.Y && l.Z < r.Z);
		}

		[Lua.MoonSharpUserDataMetamethod("__le")]
		public static bool LessThanOrEqual(Vector3 l, Vector3 r)
		{
			return (LessThan(l, r) || Equality(l, r));
		}

		#endregion

	}

	/*class ClientProxy
	{
		public string Address => Client.address;
		public int Ping => Client.ping;

		public Vector3 AimingPoint => Client.aimingPoint;
		public int Armor { get { return Client.armor; } set { Client.armor = value; } }
		public bool Collisionless { get { return Client.collisionless; } set { Client.collisionless = value; } }
		public WeaponHash CurrentWeapon => Client.currentWeapon;
		public WeaponHash[] Weapons => Client.weapons;
		public int Dimension { get { return Client.dimension; } set { Client.dimension = value; } }
		public bool Exists => Client.exists;
		public NetHandle Handle => Client.handle;
		public int Health { get { return Client.health; } set { Client.health = value; } }

		public bool IsDead => Client.dead;
		public bool IsInFreefall => Client.inFreefall;
		public bool IsInvictible { get { return Client.invincible; } set { Client.invincible = value; } }
		public bool IsAiming => Client.isAiming;
		public bool IsCefEnabled => Client.isCEFenabled;
		public bool IsInCover => Client.isInCover;
		public bool IsInVehicle => Client.isInVehicle;
		public bool IsMediaStreamEnabled => Client.isMediaStreamEnabled;
		public bool IsNull => Client.IsNull;
		public bool IsOnLadder => Client.isOnLadder;
		public bool IsParachuting => Client.isParachuting;
		public bool IsReloading => Client.isReloading;
		public bool IsShooting => Client.isShooting;
		public bool IsOnFire => Client.onFire;
		public bool IsSpectating => Client.spectating;

		public int Model => Client.model;
		public string Name { get { return Client.name; } set { Client.name = value; } }
		public string NameTag { get { return Client.nametag; } set { Client.nametag = value; } }
		public Color NameTagColor { get { return Client.nametagColor; } set { Client.nametagColor = value; } }
		public bool IsNameTagVisible { get { return Client.nametagVisible; } set { Client.nametagVisible = value; } }
		public bool Seatbelt { get { return Client.seatbelt; } set { Client.seatbelt = value; } }
		public string SocialClubName => Client.socialClubName;
		public int Team { get { return Client.team; } set { Client.team = value; } }
		public EntityType Type => Client.type;
		public Vehicle Vehicle => Client.vehicle;
		public int VehicleSeat => Client.vehicleSeat;
		public Vector3 Velocity { get { return Client.velocity; } set { Client.velocity = value; } }
		public int WantedLevel { get { return Client.wantedLevel; } set { Client.wantedLevel = value; } }
		

		[Lua.MoonSharpHidden]
		public Client Client;

		[Lua.MoonSharpHidden]
		public ClientProxy(Client c)
		{
			this.Client = c;
		}
	}*/
}
