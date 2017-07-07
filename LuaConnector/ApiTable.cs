using System;
using System.Collections.Generic;
using System.Linq;

using LuaConnector.Utilities;

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector
{
	class ApiTable
	{
		//public static Lua.Table table = new Lua.Table(new Lua.Script());
		public static Dictionary<string, Lua.DynValue> Table = new Dictionary<string, Lua.DynValue>();

		public ApiTable(Script context)
		{
			Table["attachEntityToEntity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.attachEntityToEntity(Utils.TryGetHandleFromEntity(a[0]),
													Utils.TryGetHandleFromEntity(a[1]),
													a[2].ToString(),
													a[3].ToVector3(),
													a[4].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["banPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				if (a.Count > 1)
					context.API.banPlayer(a[0].ToClient(), a[1].CastToString());
				else
					context.API.banPlayer(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["breakVehicleDoor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.breakVehicleDoor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].CastToNumber().Value, a[2].CastToBool());

				return Lua.DynValue.Nil;
			});

			Table["breakVehicleWindow"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.breakVehicleWindow(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].CastToNumber().Value, a[2].CastToBool());

				return Lua.DynValue.Nil;
			});

			Table["clearPlayerAccessory"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.clearPlayerAccessory(a[0].ToClient(), (int)a[1].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["clearPlayerTasks"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.clearPlayerTasks(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["call"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.call(a[0].String, a[1].String, a.GetArray().Skip(2).Select(arg => arg.ToObject()));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#region CREATE__

			Table["create2DColShape"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.create2DColShape((float)a[0].CastToNumber().Value,
														(float)a[1].CastToNumber().Value,
														(float)a[2].CastToNumber().Value,
														(float)a[3].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["create3DColShape"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.create3DColShape(a[0].ToVector3(), a[1].ToVector3());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createBlip"] = Lua.DynValue.NewCallback((c, a) =>
			{
				Blip obj = null;

				try
				{
					var handle = Utils.TryGetHandleFromEntity(a[0]);

					obj = context.API.createBlip(handle);
				}
				catch
				{
					var vector = a[0].ToVector3();

					if (a.Count > 2)
						obj = context.API.createBlip(vector, (float)a[1].CastToNumber().Value, (int)a[2].CastToNumber().Value);
					else
						obj = context.API.createBlip(vector, (int)a[1].CastToNumber().Value);
				}

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createCylinderColShape"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createCylinderColShape(a[0].ToVector3(), (float)a[1].CastToNumber().Value, (float)a[2].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createExplosion"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.createExplosion(a[0].ToObject<ExplosionType>(), a[1].ToVector3(), (float)a[2].CastToNumber().Value, (int)a[3].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["createLoopedParticleEffectOnEntity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createLoopedParticleEffectOnEntity(a[0].CastToString(),
																			a[1].CastToString(),
																			Utils.TryGetHandleFromEntity(a[2]),
																			a[3].ToVector3(),
																			a[4].ToVector3(),
																			(float)a[5].CastToNumber().Value,
																			(int)a[6].CastToNumber().Value,
																			(int)a[7].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createLoopedParticleEffectOnPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createLoopedParticleEffectOnPosition(a[0].CastToString(),
																			a[1].CastToString(),
																			a[2].ToVector3(),
																			a[3].ToVector3(),
																			(float)a[4].CastToNumber().Value,
																			(int)a[5].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createMarker"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createMarker((int)a[0].CastToNumber().Value,
													a[1].ToVector3(),
													a[2].ToVector3(),
													a[3].ToVector3(),
													a[4].ToVector3(),
													(int)a[5].CastToNumber().Value,
													(int)a[6].CastToNumber().Value,
													(int)a[7].CastToNumber().Value,
													(int)a[8].CastToNumber().Value,
													(int)a[9].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createObject"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createObject((int)a[0].CastToNumber().Value,
													a[1].ToVector3(),
													a[2].ToVector3(),
													(int)a[3].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createOwnedExplosion"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.createOwnedExplosion(a[0].ToClient(), a[1].ToObject<ExplosionType>(),
													a[2].ToVector3(), (float)a[3].CastToNumber().Value,
													(int)a[4].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["createOwnedProjectile"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.createOwnedProjectile(a[0].ToClient(),
													a[1].ToObject<WeaponHash>(),
													a[2].ToVector3(),
													a[3].ToVector3(),
													(int)a[4].CastToNumber().Value,
													(float)a[5].CastToNumber().Value,
													(int)a[6].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["createParticleEffectOnEntity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.createParticleEffectOnEntity(a[0].CastToString(),
														a[1].CastToString(),
														Utils.TryGetHandleFromEntity(a[2]),
														a[3].ToVector3(),
														a[4].ToVector3(),
														(float)a[5].CastToNumber().Value,
														(int)a[6].CastToNumber().Value,
														(int)a[7].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["createParticleEffectOnPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.createParticleEffectOnPosition(a[0].CastToString(),
															a[1].CastToString(),
															a[2].ToVector3(),
															a[3].ToVector3(),
															(float)a[4].CastToNumber().Value,
															(int)a[5].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["createPed"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createPed(a[0].ToObject<PedHash>(),
												a[1].ToVector3(),
												(float)a[2].CastToNumber().Value,
												(int)a[3].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createPickup"] = Lua.DynValue.NewCallback((c, a) =>
			{
				Pickup pickup = null;

				try
				{
					pickup = context.API.createPickup((int)a[0].CastToNumber().Value,
														a[1].ToVector3(),
														a[2].ToVector3(),
														a[3].CastToBool(),
														(uint)a[4].CastToNumber().Value,
														(int)a[5].CastToNumber().Value);
				}
				catch
				{
					pickup = context.API.createPickup(a[0].ToObject<PickupHash>(),
														a[1].ToVector3(),
														a[2].ToVector3(),
														(int)a[3].CastToNumber().Value,
														(uint)a[4].CastToNumber().Value,
														(int)a[5].CastToNumber().Value);
				}

				return Lua.DynValue.FromObject(c.GetScript(), pickup);
			});

			Table["createProjectile"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.createProjectile(a[0].ToObject<WeaponHash>(),
											a[1].ToVector3(),
											a[2].ToVector3(),
											(int)a[3].CastToNumber().Value,
											(float)a[4].CastToNumber().Value,
											(int)a[5].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["createSphereColShape"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createSphereColShape(a[0].ToVector3(), (float)a[1].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createTextLabel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createTextLabel(a[0].CastToString(),
														a[1].ToVector3(),
														(float)a[2].CastToNumber().Value,
														(float)a[3].CastToNumber().Value,
														a[4].CastToBool(),
														(int)a[5].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["createVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.createVehicle(a[0].ToObject<VehicleHash>(),
													a[1].ToVector3(),
													a[2].ToVector3(),
													(int)a[3].CastToNumber().Value,
													(int)a[4].CastToNumber().Value,
													a[5].IsNil() ? 0 : (int)a[5].CastToNumber().Value);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#endregion

			/*Table["delay"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.delay((int)a[0].CastToNumber().Value, a[1].CastToBool(), (Action)a[2].Function.);

				return Lua.DynValue.Nil;
			});*/

			Table["deleteColShape"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.deleteColShape(a[0].ToObject<ColShape>());

				return Lua.DynValue.Nil;
			});

			Table["deleteEntity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.deleteEntity(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.Nil;
			});

			Table["deleteObject"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.deleteObject(a[0].ToClient(), a[1].ToVector3(), (int)a[2].CastToNumber().Value);

				return Lua.DynValue.Nil;
			});

			Table["detachEntity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.detachEntity(Utils.TryGetHandleFromEntity(a[0]), a[1].CastToBool());

				return Lua.DynValue.Nil;
			});

			Table["detonatePlayerStickies"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.detonatePlayerStickies(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["doesConfigExist"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.doesConfigExist(a[0].String);

				return Lua.DynValue.Nil;
			});

			Table["doesEntityExist"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.doesEntityExist(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["doesEntityExistForPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.doesEntityExistForPlayer(a[0].ToClient(), Utils.TryGetHandleFromEntity(a[1]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["doesPlayerHaveAccessToCommand"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.doesPlayerHaveAccessToCommand(a[0].ToClient(), a[1].CastToString());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["doesResourceExist"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.doesResourceExist(a[0].CastToString());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["downloadData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.downloadData(a[0].ToClient(), a[1].CastToString());

				return Lua.DynValue.Nil;
			});

			Table["explodeVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.explodeVehicle(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.Nil;
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//context.API.fetchNativeFromPlayer()

				return Lua.DynValue.Nil;
			});*/

			Table["freezePlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.freezePlayer(a[0].ToClient(), a[1].CastToBool());

				return Lua.DynValue.Nil;
			});

			Table["freezePlayerTime"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.freezePlayerTime(a[0].ToClient(), a[1].CastToBool());

				return Lua.DynValue.Nil;
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//context.API.fromJson

				return Lua.DynValue.Nil;
			});*/

			#region GET__

			#region ALL__

			Table["getAllBlips"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllBlips();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllEntityData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllEntityData(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllEntitySyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllEntitySyncedData(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllLabels"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllLabels();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllMarkers"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllMarkers();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllObjects"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllObjects();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllPeds"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllPeds();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllPickups"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllPickups();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllPlayers"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllPlayers();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllResources"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllResources();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllVehicles"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllVehicles();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllWeaponComponents"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllWeaponComponents(a[0].ToObject<WeaponHash>());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllWorldData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllWorldData();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getAllWorldSyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getAllWorldSyncedData();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#endregion

			#region BLIP__

			Table["getBlipColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getBlipName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipName(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewString(obj);
			});

			Table["getBlipPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipPosition(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getBlipRouteColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipRouteColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getBlipRouteVisible"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipRouteVisible(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getBlipScale"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipScale(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getBlipShortRange"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipShortRange(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getBlipSprite"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipSprite(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getBlipTransparency"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getBlipTransparency(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			#endregion

			Table["getCurrentGamemode"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getCurrentGamemode();

				return Lua.DynValue.NewString(obj);
			});

			#region ENTITY__

			Table["getEntityCollisionless"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityCollisionless(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityData(Utils.TryGetHandleFromEntity(a[0]), a[1].CastToString());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityDimension"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityDimension(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getEntityFromHandle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var handle = a[0].ToNetHandle();
				var type = a[1].ToObject<EntityType>();
				object obj = null;

				switch (type)
				{
					case EntityType.Blip:
						obj = context.API.getEntityFromHandle<Blip>(handle);
						break;
					case EntityType.Marker:
						obj = context.API.getEntityFromHandle<Marker>(handle);
						break;
					case EntityType.Particle:
						obj = context.API.getEntityFromHandle<ParticleEffect>(handle);
						break;
					case EntityType.Ped:
						obj = context.API.getEntityFromHandle<Ped>(handle);
						break;
					case EntityType.Pickup:
						obj = context.API.getEntityFromHandle<Pickup>(handle);
						break;
					case EntityType.Player:
						obj = context.API.getEntityFromHandle<Client>(handle);
						break;
					case EntityType.TextLabel:
						obj = context.API.getEntityFromHandle<TextLabel>(handle);
						break;
					case EntityType.Vehicle:
						obj = context.API.getEntityFromHandle<Vehicle>(handle);
						break;
					case EntityType.Prop:
						break;
					case EntityType.World:
						break;
				}

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityInvincible"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityInvincible(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityModel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityModel(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getEntityPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityPosition(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityRotation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityRotation(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntitySyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntitySyncedData(Utils.TryGetHandleFromEntity(a[0]), a[0].CastToString());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityTransparency"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityTransparency(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityType"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityType(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getEntityVelocity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getEntityVelocity(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#endregion

			Table["getHashKey"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getHashKey(a[0].CastToString());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getHashSHA256"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getHashSHA256(a[0].CastToString());

				return Lua.DynValue.NewString(obj);
			});

			Table["getMapGamemodes"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMapGamemodes(a[0].CastToString());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getMapsForGamemode"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMapsForGamemode(a[0].CastToString());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#region MARKER__

			Table["getMarkerColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMarkerColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getMarkerDirection"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMarkerDirection(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getMarkerScale"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMarkerScale(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getMarkerType"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMarkerType(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			#endregion

			Table["getMaxPlayers"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getMaxPlayers();

				return Lua.DynValue.NewNumber(obj);
			});

			#region PICKUP__

			Table["getPickupAmount"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPickupAmount(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPickupCustomModel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPickupCustomModel(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPickupPickedUp"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPickupPickedUp(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			#endregion

			#region PLAYER__

			Table["getPlayerAccessoryDrawable"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerAccessoryDrawable(a[0].ToClient(), (int)a[1].CastToNumber().Value);

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerAccessoryTexture"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerAccessoryTexture(a[0].ToClient(), (int)a[1].CastToNumber().Value);

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerAclGroup"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerAclGroup(a[0].ToClient());

				return Lua.DynValue.NewString(obj);
			});

			Table["getPlayerAddress"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerAddress(a[0].ToClient());

				return Lua.DynValue.NewString(obj);
			});

			Table["getPlayerAimingPoint"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerAimingPoint(a[0].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerArmor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerArmor(a[0].ToClient());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerClothesDrawable"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerClothesDrawable(a[0].ToClient(), (int)a[1].CastToNumber().Value);

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerClothesTexture"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerClothesTexture(a[0].ToClient(), (int)a[1].CastToNumber().Value);

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerCurrentWeapon"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerCurrentWeapon(a[0].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerFromHandle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerFromHandle(a[0].ToNetHandle());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerFromName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerFromName(a[0].CastToString());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerHealth"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerHealth(a[0].ToClient());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerName(a[0].ToClient());

				return Lua.DynValue.NewString(obj);
			});

			Table["getPlayerNametag"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerNametag(a[0].ToClient());

				return Lua.DynValue.NewString(obj);
			});

			Table["getPlayerNametagColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerNametagColor(a[0].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerNametagVisible"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerNametagVisible(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getPlayerPing"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerPing(a[0].ToClient());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerSeatbelt"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerSeatbelt(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getPlayersInRadiusOfPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayersInRadiusOfPlayer((float)a[0].CastToNumber().Value, a[1].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayersInRadiusOfPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayersInRadiusOfPosition((float)a[0].CastToNumber().Value, a[1].ToVector3());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerTeam"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerTeam(a[0].ToClient());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerVehicle(a[0].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerVehicleSeat"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerVehicleSeat(a[0].ToClient());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerVelocity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerVelocity(a[0].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerWantedLevel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerWantedLevel(a[0].ToClient());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerWeaponAmmo"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerWeaponAmmo(a[0].ToClient(), a[1].ToObject<WeaponHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getPlayerWeaponComponents"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerWeaponComponents(a[0].ToClient(), a[1].ToObject<WeaponHash>());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerWeapons"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerWeapons(a[0].ToClient());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getPlayerWeaponTint"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getPlayerWeaponTint(a[0].ToClient(), a[1].ToObject<WeaponHash>());

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#endregion

			#region RESOURCE__

			Table["getResourceAuthor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceAuthor(a[0].String);

				return Lua.DynValue.NewString(obj);
			});

			Table["getResourceCommandInfo"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceCommandInfo(a[0].String, a[1].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getResourceCommandInfos"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceCommandInfos(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getResourceCommands"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceCommands(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getResourceDescription"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceDescription(a[0].String);

				return Lua.DynValue.NewString(obj);
			});

			Table["getResourceFolder"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceFolder();

				return Lua.DynValue.NewString(obj);
			});

			Table["getResourceName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceName(a[0].String);

				return Lua.DynValue.NewString(obj);
			});

			Table["getResourceSetting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceSetting<object>(a[0].String, a[1].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getResourceSettingDescription"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceSettingDescription(a[0].String, a[1].String);

				return Lua.DynValue.NewString(obj);
			});

			Table["getResourceSettings"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceSettings(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getResourceType"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceType(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getResourceVersion"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getResourceVersion(a[0].String);

				return Lua.DynValue.NewString(obj);
			});

			Table["getRunningResources"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getRunningResources();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#endregion

			Table["getServerName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getServerName();

				return Lua.DynValue.NewString(obj);
			});

			Table["getServerPort"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getServerPort();

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getSetting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getSetting<object>(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#region TEXTLABEL__

			Table["getTextLabelColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getTextLabelColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});			

			Table["getTextLabelRange"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getTextLabelRange(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getTextLabelSeethrough"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getTextLabelSeethrough(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getTextLabelText"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getTextLabelText(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewString(obj);
			});

			#endregion

			Table["getThisResource"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getThisResource();

				return Lua.DynValue.NewString(obj);
			});

			Table["getTime"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getTime();

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#region VEHICLE__

			Table["getVehicleBulletproofTyres"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleBulletproofTyres(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleClass"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleClass(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleClassName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleClassName((int)a[0].Number);

				return Lua.DynValue.NewString(obj);
			});

			Table["getVehicleCustomPrimaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleCustomPrimaryColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleCustomSecondaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleCustomSecondaryColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleDashboardColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleDashboardColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleDisplayName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleDisplayName(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewString(obj);
			});

			Table["getVehicleDoorState"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleDoorState(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleEnginePowerMultiplier"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleEnginePowerMultiplier(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleEngineStatus"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleEngineStatus(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleEngineTorqueMultiplier"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleEngineTorqueMultiplier(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleExtra"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleExtra(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleHealth"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleHealth(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleLivery"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleLivery(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleLocked"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleLocked(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleMaxAcceleration"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleMaxAcceleration(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleMaxBraking"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleMaxBraking(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleMaxOccupants"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleMaxOccupants(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleMaxSpeed"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleMaxSpeed(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleMaxTraction"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleMaxTraction(a[0].ToObject<VehicleHash>());

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleMod"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleMod(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleModColor1"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleModColor1(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleModColor2"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleModColor2(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleNeonColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleNeonColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleNeonState"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleNeonState(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleNumberPlate"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleNumberPlate(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewString(obj);
			});

			Table["getVehicleNumberPlateStyle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleNumberPlateStyle(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleOccupants"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleOccupants(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});
			
			Table["getVehiclePearlescentColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehiclePearlescentColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehiclePrimaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehiclePrimaryColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleSecondaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleSecondaryColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleSirenState"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleSirenState(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleSpecialLightStatus"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleSpecialLightStatus(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["getVehicleTrailer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleTrailer(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleTraileredBy"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleTraileredBy(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleTrimColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleTrimColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleTyreSmokeColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleTyreSmokeColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["getVehicleWheelColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleWheelColor(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleWheelType"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleWheelType(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			Table["getVehicleWindowTint"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getVehicleWindowTint(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewNumber(obj);
			});

			#endregion

			Table["getWeather"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.getWeather();

				return Lua.DynValue.NewNumber(obj);
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//var obj = context.API.getWorldData

				return Lua.DynValue.Nil;
			});*/

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//var obj = context.API.getWorldSyncedData();

				return Lua.DynValue.Nil;
			});*/

			#endregion

			Table["givePlayerWeapon"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.givePlayerWeapon(a[0].ToClient(),
												a[1].ToObject<WeaponHash>(),
												(int)a[2].Number,
												a[3].Boolean,
												a[4].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["givePlayerWeaponComponent"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.givePlayerWeaponComponent(a[0].ToClient(),
														a[1].ToObject<WeaponHash>(),
														a[2].ToObject<WeaponComponent>());

				return Lua.DynValue.Nil;
			});

			#region HAS__

			Table["hasEntityData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasEntityData(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["hasEntitySyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasEntitySyncedData(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["hasPlayerGotWeaponComponent"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasPlayerGotWeaponComponent(a[0].ToClient(),
																	a[1].ToObject<WeaponHash>(),
																	a[2].ToObject<WeaponComponent>());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["hasResourceSetting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasResourceSetting(a[0].String, a[1].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["hasSetting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasSetting(a[0].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["hasWorldData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasWorldData(a[0].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["hasWorldSyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.hasWorldSyncedData(a[0].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			#endregion

			#region IS__

			Table["isAclEnabled"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isAclEnabled();

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isEntityAttachedToAnything"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isEntityAttachedToAnything(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isEntityAttachedToEntity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isEntityAttachedToEntity(Utils.TryGetHandleFromEntity(a[0]), Utils.TryGetHandleFromEntity(a[1]));

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPasswordProtected"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPasswordProtected();

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerAiming"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerAiming(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerConnected"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerConnected(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerDead"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerDead(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerInAnyVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerInAnyVehicle(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerInCover"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerInCover(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerInFreefall"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerInFreefall(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerLoggedIn"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerLoggedIn(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerOnFire"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerOnFire(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerOnLadder"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerOnLadder(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerParachuting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerParachuting(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerReloading"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerReloading(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerRespawning"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerRespawning(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerShooting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerShooting(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isPlayerSpectating"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isPlayerSpectating(a[0].ToClient());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isResourceRunning"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isResourceRunning(a[0].String);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isVehicleDoorBroken"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isVehicleDoorBroken(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isVehicleTyrePopped"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isVehicleTyrePopped(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["isVehicleWindowBroken"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.isVehicleWindowBroken(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.NewBoolean(obj);
			});

			#endregion

			Table["kickPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				if (a.Count > 1)
					context.API.kickPlayer(a[0].ToClient(), a[1].String);
				else
					context.API.kickPlayer(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.loadConfig();

				return Lua.DynValue.NewBoolean(obj);
			});*/

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.loadXml();

				return Lua.DynValue.NewBoolean(obj);
			});*/

			Table["loginPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.loginPlayer(a[0].ToClient(), a[1].String);

				return Lua.DynValue.NewNumber(obj);
			});

			Table["logoutPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.logoutPlayer(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["moveEntityPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.moveEntityPosition(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3(), (int)a[2].Number);

				return Lua.DynValue.Nil;
			});

			Table["moveEntityRotation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.moveEntityRotation(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3(), (int)a[2].Number);

				return Lua.DynValue.Nil;
			});

			Table["pedNameToModel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.pedNameToModel(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["pickupNameToModel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.pickupNameToModel(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			#region PLAY__

			Table["playPedAnimation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.playPedAnimation(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean, a[2].String, a[3].String);

				return Lua.DynValue.Nil;
			});

			Table["playPedScenario"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.playPedScenario(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["playPlayerAnimation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.playPlayerAnimation(a[0].ToClient(), (int)a[1].Number, a[2].String, a[3].String);

				return Lua.DynValue.Nil;
			});

			Table["playPlayerScenario"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.playPlayerScenario(a[0].ToClient(), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["playSoundFrontEnd"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.playSoundFrontEnd(a[0].ToClient(), a[1].String, a[2].String);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["popVehicleTyre"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.popVehicleTyre(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number, a[2].Boolean);

				return Lua.DynValue.Nil;
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//context.API.random

				return Lua.DynValue.Nil;
			});*/

			Table["registerCustomColShape"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.registerCustomColShape(a[0].ToObject<ColShape>());

				return Lua.DynValue.Nil;
			});

			#region REMOVE__

			Table["removeAllPlayerWeapons"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.removeAllPlayerWeapons(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["removeIpl"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.removeIpl(a[0].String);

				return Lua.DynValue.Nil;
			});

			Table["removePlayerWeapon"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.removePlayerWeapon(a[0].ToClient(), a[1].ToObject<WeaponHash>());

				return Lua.DynValue.Nil;
			});

			Table["removePlayerWeaponComponent"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.removePlayerWeaponComponent(a[0].ToClient(),
														a[1].ToObject<WeaponHash>(),
														a[2].ToObject<WeaponComponent>());

				return Lua.DynValue.Nil;
			});

			Table["removeVehicleMod"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.removeVehicleMod(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["repairVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.repairVehicle(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.Nil;
			});

			Table["requestIpl"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.requestIpl(a[0].String);

				return Lua.DynValue.Nil;
			});

			#region RESET__

			Table["resetEntityData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetEntityData(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["resetEntitySyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetEntitySyncedData(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.Nil;
			});
			 
			Table["resetIplList"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetIplList();

				return Lua.DynValue.Nil;
			});

			Table["resetPlayerNametag"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetPlayerNametag(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["resetPlayerNametagColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetPlayerNametagColor(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["resetWorldData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetWorldData(a[0].String);

				return Lua.DynValue.Nil;
			});

			Table["resetWorldSyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.resetWorldSyncedData(a[0].String);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["respawnPickup"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.respawnPickup(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.Nil;
			});

			#region SEND__

			Table["sendChatMessageToAll"] = Lua.DynValue.NewCallback((c, a) =>
			{
				if (a.Count > 1)
					context.API.sendChatMessageToAll(a[0].String, a[0].ToString());
				else
					context.API.sendChatMessageToAll(a[0].ToString());

				return Lua.DynValue.Nil;
			});

			Table["sendChatMessageToPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				if (a.Count > 2)
					context.API.sendChatMessageToPlayer(a[0].ToClient(), a[1].CastToString(), a[2].ToString());
				else
					context.API.sendChatMessageToPlayer(a[0].ToClient(), a[1].ToString());

				return Lua.DynValue.Nil;
			});

			Table["sendNativeToAllPlayers"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var arg0 = a[0];

				if (arg0.UserData.Object.GetType() == typeof(Hash))
				{
					context.API.sendNativeToAllPlayers(arg0.ToObject<Hash>(), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}
				else
				{
					context.API.sendNativeToAllPlayers(ulong.Parse(arg0.String), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}

				return Lua.DynValue.Nil;
			});

			Table["sendNativeToPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var arg1 = a[1];

				if (arg1.UserData.Object.GetType() == typeof(Hash))
				{
					context.API.sendNativeToPlayer(a[0].ToClient(), arg1.ToObject<Hash>(), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}
				else
				{
					context.API.sendNativeToPlayer(a[0].ToClient(), ulong.Parse(arg1.String), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}

				return Lua.DynValue.Nil;
			});

			Table["sendNativeToPlayersInDimension"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var arg1 = a[1];

				if (arg1.UserData.Object.GetType() == typeof(Hash))
				{
					context.API.sendNativeToPlayersInDimension((int)a[0].Number, arg1.ToObject<Hash>(), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}
				else
				{
					context.API.sendNativeToPlayersInDimension((int)a[0].Number, ulong.Parse(arg1.String), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}

				return Lua.DynValue.Nil;
			});

			Table["sendNativeToPlayersInRange"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var arg2 = a[2];

				if (arg2.UserData.Object.GetType() == typeof(Hash))
				{
					context.API.sendNativeToPlayersInRange(a[0].ToVector3(), (float)a[1].Number, arg2.ToObject<Hash>(), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}
				else
				{
					context.API.sendNativeToPlayersInRange(a[0].ToVector3(), (float)a[1].Number, ulong.Parse(arg2.String), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}

				return Lua.DynValue.Nil;
			});

			Table["sendNativeToPlayersInRangeInDimension"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var arg3 = a[3];

				if (arg3.UserData.Object.GetType() == typeof(Hash))
				{
					context.API.sendNativeToPlayersInRangeInDimension(a[0].ToVector3(), (float)a[1].Number, (int)a[2].Number, arg3.ToObject<Hash>(), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}
				else
				{
					context.API.sendNativeToPlayersInRangeInDimension(a[0].ToVector3(), (float)a[1].Number, (int)a[2].Number, ulong.Parse(arg3.String), a.GetArray().Skip(1).Select((arg) => { return arg.ToObject(); }));
				}

				return Lua.DynValue.Nil;
			});

			Table["sendNotificationToAll"] = Lua.DynValue.NewCallback((c, a) =>
			{
				if (a.Count > 1)
					context.API.sendNotificationToAll(a[0].CastToString(), a[1].Boolean);
				else
					context.API.sendNotificationToAll(a[0].CastToString());

				return Lua.DynValue.Nil;
			});

			Table["sendNotificationToPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				if (a.Count > 1)
					context.API.sendNotificationToPlayer(a[0].ToClient(), a[1].CastToString(), a[2].Boolean);
				else
					context.API.sendNotificationToPlayer(a[0].ToClient(), a[1].CastToString());

				return Lua.DynValue.Nil;
			});

			Table["sendPictureNotificationToAll"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.sendPictureNotificationToAll(a[0].CastToString(),
														a[1].String,
														(int)a[2].Number,
														(int)a[3].Number,
														a[4].String,
														a[5].String);

				return Lua.DynValue.Nil;
			});

			Table["sendPictureNotificationToPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.sendPictureNotificationToPlayer(a[0].ToClient(),
															a[1].CastToString(),
															a[2].String,
															(int)a[3].Number,
															(int)a[4].Number,
															a[5].String,
															a[6].String);

				return Lua.DynValue.Nil;
			});

			#endregion

			#region SET__

			#region BLIP__

			Table["setBlipColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setBlipName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipName(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["setBlipPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipPosition(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["setBlipRouteColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipRouteColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setBlipRouteVisible"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipRouteVisible(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setBlipScale"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipScale(Utils.TryGetHandleFromEntity(a[0]), (float)a[0].Number);

				return Lua.DynValue.Nil;
			});

			Table["setBlipShortRange"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipShortRange(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setBlipSprite"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipSprite(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setBlipTransparency"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setBlipTransparency(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["setCommandErrorMessage"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setCommandErrorMessage(a[0].String);

				return Lua.DynValue.Nil;
			});

			#region ENTITY_

			Table["setEntityCollisionless"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityCollisionless(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setEntityData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityData(Utils.TryGetHandleFromEntity(a[0]), a[1].String, a[2].ToObject());

				return Lua.DynValue.Nil;
			});

			Table["setEntityDimension"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityDimension(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setEntityInvincible"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityInvincible(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setEntityPosition"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityPosition(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["setEntityPositionFrozen"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var arg0 = a[0];

				if (arg0.UserData.Object.GetType() == typeof(NetHandle) || arg0.UserData.Object.GetType() == typeof(Entity))
					context.API.setEntityPositionFrozen(Utils.TryGetHandleFromEntity(arg0), a[1].Boolean);
				else
					context.API.setEntityPositionFrozen(arg0.ToClient(), Utils.TryGetHandleFromEntity(a[1]), a[2].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setEntityRotation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityRotation(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["setEntitySyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntitySyncedData(Utils.TryGetHandleFromEntity(a[0]), a[1].String, a[2].ToObject());

				return Lua.DynValue.Nil;
			});

			Table["setEntityTransparency"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setEntityTransparency(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["setGamemodeName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setGamemodeName(a[0].String);

				return Lua.DynValue.Nil;
			});

			#region MARKER__

			Table["setMarkerColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setMarkerColor(Utils.TryGetHandleFromEntity(a[0]),
											(int)a[1].Number,
											(int)a[2].Number,
											(int)a[3].Number,
											(int)a[4].Number);

				return Lua.DynValue.Nil;
			});

			Table["setMarkerDirection"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setMarkerDirection(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["setMarkerScale"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setMarkerScale(Utils.TryGetHandleFromEntity(a[0]), a[1].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["setMarkerType"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setMarkerType(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			#endregion

			#region PLAYER__

			Table["setPlayerAccessory"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerAccessory(a[0].ToClient(),
												(int)a[1].Number,
												(int)a[2].Number,
												(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerArmor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerArmor(a[0].ToClient(), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerClothes"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerClothes(a[0].ToClient(),
											(int)a[1].Number,
											(int)a[2].Number,
											(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerDefaultClothes"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerDefaultClothes(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["setPlayerHealth"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerHealth(a[0].ToClient(), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerIntoVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerIntoVehicle(a[0].ToClient(), Utils.TryGetHandleFromEntity(a[1]), (int)a[2].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerName(a[0].ToClient(), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerNametag"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerNametag(a[0].ToClient(), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerNametagColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerNametagColor(a[0].ToClient(),
													(byte)a[1].Number,
													(byte)a[2].Number,
													(byte)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerNametagVisible"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerNametagVisible(a[0].ToClient(), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerSeatbelt"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerSeatbelt(a[0].ToClient(), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerSkin"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerSkin(a[0].ToClient(), a[1].ToObject<PedHash>());

				return Lua.DynValue.Nil;
			});

			Table["setPlayerTeam"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerTeam(a[0].ToClient(), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerToSpectatePlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerToSpectatePlayer(a[0].ToClient(), a[1].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["setPlayerToSpectator"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerToSpectator(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["setPlayerVelocity"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerVelocity(a[0].ToClient(), a[1].ToVector3());

				return Lua.DynValue.Nil;
			});

			Table["setPlayerWantedLevel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerWantedLevel(a[0].ToClient(), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerWeaponAmmo"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerWeaponAmmo(a[0].ToClient(), a[1].ToObject<WeaponHash>(), (int)a[2].Number);

				return Lua.DynValue.Nil;
			});

			Table["setPlayerWeaponTint"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setPlayerWeaponTint(a[0].ToClient(), a[1].ToObject<WeaponHash>(), a[2].ToObject<WeaponTint>());

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["setResourceSetting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setResourceSetting(a[0].String, a[1].String, a[2].ToObject());

				return Lua.DynValue.Nil;
			});

			Table["setServerName"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setServerName(a[0].String);

				return Lua.DynValue.Nil;
			});

			Table["setServerPassword"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setServerPassword(a[0].String);

				return Lua.DynValue.Nil;
			});

			Table["setSetting"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setSetting(a[0].String, a[1].ToObject());

				return Lua.DynValue.Nil;
			});

			#region TEXTLABEL__

			Table["setTextLabelColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setTextLabelColor(Utils.TryGetHandleFromEntity(a[0]),
												(int)a[1].Number,
												(int)a[2].Number,
												(int)a[3].Number,
												(int)a[4].Number);

				return Lua.DynValue.Nil;
			});

			Table["setTextLabelRange"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setTextLabelRange(Utils.TryGetHandleFromEntity(a[0]), (float)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setTextLabelSeethrough"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setTextLabelSeethrough(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setTextLabelText"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setTextLabelText(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["setTime"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setTime((int)a[0].Number, (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			#region VEHICLE__

			Table["setVehicleBulletproofTyres"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleBulletproofTyres(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleCustomPrimaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleCustomPrimaryColor(Utils.TryGetHandleFromEntity(a[0]),
														(int)a[1].Number,
														(int)a[2].Number,
														(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleCustomSecondaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleCustomSecondaryColor(Utils.TryGetHandleFromEntity(a[0]),
															(int)a[1].Number,
															(int)a[2].Number,
															(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleDashboardColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleDashboardColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleDoorState"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleDoorState(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number, a[2].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleEnginePowerMultiplier"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleEnginePowerMultiplier(Utils.TryGetHandleFromEntity(a[0]), (float)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleEngineStatus"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleEngineStatus(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleEngineTorqueMultiplier"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleEngineTorqueMultiplier(Utils.TryGetHandleFromEntity(a[0]), (float)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleExtra"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleExtra(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number, a[2].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleHealth"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleHealth(Utils.TryGetHandleFromEntity(a[0]), (float)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleLivery"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleLivery(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleLocked"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleLocked(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleMod"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleMod(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number, (int)a[2].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleModColor1"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleModColor1(Utils.TryGetHandleFromEntity(a[0]),
												(int)a[1].Number,
												(int)a[2].Number,
												(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleModColor2"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleModColor2(Utils.TryGetHandleFromEntity(a[0]),
												(int)a[1].Number,
												(int)a[2].Number,
												(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleNeonColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleNeonColor(Utils.TryGetHandleFromEntity(a[0]),
												(int)a[1].Number,
												(int)a[2].Number,
												(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleNeonState"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleNeonState(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number, a[2].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleNumberPlate"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleNumberPlate(Utils.TryGetHandleFromEntity(a[0]), a[1].String);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleNumberPlateStyle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleNumberPlateStyle(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehiclePearlescentColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehiclePearlescentColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehiclePrimaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehiclePrimaryColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleSecondaryColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleSecondaryColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleSpecialLightStatus"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleSpecialLightStatus(Utils.TryGetHandleFromEntity(a[0]), a[1].Boolean);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleTrimColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleTrimColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleTyreSmokeColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleTyreSmokeColor(Utils.TryGetHandleFromEntity(a[0]),
													(int)a[1].Number,
													(int)a[2].Number,
													(int)a[3].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleWheelColor"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleWheelColor(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleWheelType"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleWheelType(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			Table["setVehicleWindowTint"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setVehicleWindowTint(Utils.TryGetHandleFromEntity(a[0]), (int)a[1].Number);

				return Lua.DynValue.Nil;
			});

			#endregion

			Table["setWeather"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.setWeather((int)a[0].Number);

				return Lua.DynValue.Nil;
			});

			Table["setWorldData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.setWorldData(a[0].String, a[1].ToObject());

				return Lua.DynValue.NewBoolean(obj);
			});

			Table["setWorldSyncedData"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.setWorldSyncedData(a[0].String, a[1].ToObject());

				return Lua.DynValue.NewBoolean(obj);
			});

			#endregion

			Table["sleep"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.sleep((int)a[0].Number);

				return Lua.DynValue.Nil;
			});

			Table["startResource"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.startResource(a[0].String);

				return Lua.DynValue.Nil;
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//context.API.startThread

				return Lua.DynValue.Nil;
			});*/

			Table["stopPedAnimation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.stopPedAnimation(Utils.TryGetHandleFromEntity(a[0]));

				return Lua.DynValue.Nil;
			});

			Table["stopPlayerAnimation"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.stopPlayerAnimation(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["stopResource"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.stopResource(a[0].String);

				return Lua.DynValue.Nil;
			});

			/*Table[""] = Lua.DynValue.NewCallback((c, a) =>
			{
				//context.API.toJson

				return Lua.DynValue.Nil;
			});*/

			Table["triggerClientEvent"] = Lua.DynValue.NewCallback((c, a) =>
			{
				Console.WriteLine(a.GetArray(2).Select(arg => arg.ToObject()).Count());

				context.API.triggerClientEvent(a[0].ToClient(), a[1].String, a.GetArray(2).Select(arg => arg.ToObject()).ToArray());

				return Lua.DynValue.Nil;
			});

			Table["triggerClientEventForAll"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.triggerClientEventForAll(a[0].String, a.GetArray(1).Select(arg => arg.ToObject()).ToArray());

				return Lua.DynValue.Nil;
			});

			Table["unbanPlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.unbanPlayer(a[0].String);

				return Lua.DynValue.Nil;
			});

			Table["unspectatePlayer"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.unspectatePlayer(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["vehicleNameToModel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.vehicleNameToModel(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});

			Table["warpPlayerOutOfVehicle"] = Lua.DynValue.NewCallback((c, a) =>
			{
				context.API.warpPlayerOutOfVehicle(a[0].ToClient());

				return Lua.DynValue.Nil;
			});

			Table["weaponNameToModel"] = Lua.DynValue.NewCallback((c, a) =>
			{
				var obj = context.API.weaponNameToModel(a[0].String);

				return Lua.DynValue.FromObject(c.GetScript(), obj);
			});
			
		}
	}
}
