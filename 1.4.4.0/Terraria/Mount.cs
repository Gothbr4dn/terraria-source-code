using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria
{
	public class Mount
	{
		private class DrillBeam
		{
			public Point16 curTileTarget;

			public int cooldown;

			public int lastPurpose;

			public DrillBeam()
			{
				curTileTarget = Point16.NegativeOne;
				cooldown = 0;
				lastPurpose = 0;
			}
		}

		private class DrillMountData
		{
			public float diodeRotationTarget;

			public float diodeRotation;

			public float outerRingRotation;

			public DrillBeam[] beams;

			public int beamCooldown;

			public Vector2 crosshairPosition;

			public DrillMountData()
			{
				beams = new DrillBeam[8];
				for (int i = 0; i < beams.Length; i++)
				{
					beams[i] = new DrillBeam();
				}
			}
		}

		private class BooleanMountData
		{
			public bool boolean;

			public BooleanMountData()
			{
				boolean = false;
			}
		}

		private class ExtraFrameMountData
		{
			public int frame;

			public float frameCounter;

			public ExtraFrameMountData()
			{
				frame = 0;
				frameCounter = 0f;
			}
		}

		public class MountDelegatesData
		{
			public delegate bool OverridePositionMethod(Player player, out Vector2? position);

			public Action<Vector2> MinecartDust;

			public Action<Player, Vector2, int, int> MinecartJumpingSound;

			public Action<Player, Vector2, int, int> MinecartLandingSound;

			public Action<Player, Vector2, int, int> MinecartBumperSound;

			public OverridePositionMethod MouthPosition;

			public OverridePositionMethod HandPosition;

			public MountDelegatesData()
			{
				MinecartDust = DelegateMethods.Minecart.Sparks;
				MinecartJumpingSound = DelegateMethods.Minecart.JumpingSound;
				MinecartLandingSound = DelegateMethods.Minecart.LandingSound;
				MinecartBumperSound = DelegateMethods.Minecart.BumperSound;
			}
		}

		private class MountData
		{
			public Asset<Texture2D> backTexture = Asset<Texture2D>.Empty;

			public Asset<Texture2D> backTextureGlow = Asset<Texture2D>.Empty;

			public Asset<Texture2D> backTextureExtra = Asset<Texture2D>.Empty;

			public Asset<Texture2D> backTextureExtraGlow = Asset<Texture2D>.Empty;

			public Asset<Texture2D> frontTexture = Asset<Texture2D>.Empty;

			public Asset<Texture2D> frontTextureGlow = Asset<Texture2D>.Empty;

			public Asset<Texture2D> frontTextureExtra = Asset<Texture2D>.Empty;

			public Asset<Texture2D> frontTextureExtraGlow = Asset<Texture2D>.Empty;

			public int textureWidth;

			public int textureHeight;

			public int xOffset;

			public int yOffset;

			public int[] playerYOffsets;

			public int bodyFrame;

			public int playerHeadOffset;

			public int heightBoost;

			public int buff;

			public int extraBuff;

			public int flightTimeMax;

			public bool usesHover;

			public float runSpeed;

			public float dashSpeed;

			public float swimSpeed;

			public float acceleration;

			public float jumpSpeed;

			public int jumpHeight;

			public float fallDamage;

			public int fatigueMax;

			public bool constantJump;

			public bool blockExtraJumps;

			public int abilityChargeMax;

			public int abilityDuration;

			public int abilityCooldown;

			public int spawnDust;

			public bool spawnDustNoGravity;

			public int totalFrames;

			public int standingFrameStart;

			public int standingFrameCount;

			public int standingFrameDelay;

			public int runningFrameStart;

			public int runningFrameCount;

			public int runningFrameDelay;

			public int flyingFrameStart;

			public int flyingFrameCount;

			public int flyingFrameDelay;

			public int inAirFrameStart;

			public int inAirFrameCount;

			public int inAirFrameDelay;

			public int idleFrameStart;

			public int idleFrameCount;

			public int idleFrameDelay;

			public bool idleFrameLoop;

			public int swimFrameStart;

			public int swimFrameCount;

			public int swimFrameDelay;

			public int dashingFrameStart;

			public int dashingFrameCount;

			public int dashingFrameDelay;

			public bool Minecart;

			public bool MinecartDirectional;

			public Vector3 lightColor = Vector3.One;

			public bool emitsLight;

			public MountDelegatesData delegations = new MountDelegatesData();

			public int playerXOffset;
		}

		public static int currentShader = 0;

		public const int FrameStanding = 0;

		public const int FrameRunning = 1;

		public const int FrameInAir = 2;

		public const int FrameFlying = 3;

		public const int FrameSwimming = 4;

		public const int FrameDashing = 5;

		public const int DrawBack = 0;

		public const int DrawBackExtra = 1;

		public const int DrawFront = 2;

		public const int DrawFrontExtra = 3;

		private static MountData[] mounts;

		private static Vector2[] scutlixEyePositions;

		private static Vector2 scutlixTextureSize;

		public const int scutlixBaseDamage = 50;

		public static Vector2 drillDiodePoint1 = new Vector2(36f, -6f);

		public static Vector2 drillDiodePoint2 = new Vector2(36f, 8f);

		public static Vector2 drillTextureSize;

		public const int drillTextureWidth = 80;

		public const float drillRotationChange = MathF.PI / 60f;

		public static int drillPickPower = 210;

		public static int drillPickTime = 1;

		public static int amountOfBeamsAtOnce = 2;

		public const float maxDrillLength = 48f;

		private static Vector2 santankTextureSize;

		private MountData _data;

		private int _type;

		private bool _flipDraw;

		private int _frame;

		private float _frameCounter;

		private int _frameExtra;

		private float _frameExtraCounter;

		private int _frameState;

		private int _flyTime;

		private int _idleTime;

		private int _idleTimeNext;

		private float _fatigue;

		private float _fatigueMax;

		private bool _abilityCharging;

		private int _abilityCharge;

		private int _abilityCooldown;

		private int _abilityDuration;

		private bool _abilityActive;

		private bool _aiming;

		private bool _shouldSuperCart;

		public List<DrillDebugDraw> _debugDraw;

		private object _mountSpecificData;

		private bool _active;

		public static float SuperCartRunSpeed = 20f;

		public static float SuperCartDashSpeed = 20f;

		public static float SuperCartAcceleration = 0.1f;

		public static int SuperCartJumpHeight = 15;

		public static float SuperCartJumpSpeed = 5.15f;

		private MountDelegatesData _defaultDelegatesData = new MountDelegatesData();

		public bool Active => _active;

		public int Type => _type;

		public int FlyTime => _flyTime;

		public int BuffType => _data.buff;

		public int BodyFrame => _data.bodyFrame;

		public int XOffset => _data.xOffset;

		public int YOffset => _data.yOffset;

		public int PlayerXOFfset => _data.playerXOffset;

		public int PlayerOffset
		{
			get
			{
				if (!_active)
				{
					return 0;
				}
				if (_frame >= _data.totalFrames)
				{
					return 0;
				}
				return _data.playerYOffsets[_frame];
			}
		}

		public int PlayerOffsetHitbox
		{
			get
			{
				if (!_active)
				{
					return 0;
				}
				return -PlayerOffset + _data.heightBoost;
			}
		}

		public int PlayerHeadOffset
		{
			get
			{
				if (!_active)
				{
					return 0;
				}
				return _data.playerHeadOffset;
			}
		}

		public int HeightBoost => _data.heightBoost;

		public float RunSpeed
		{
			get
			{
				if (_type == 4 && _frameState == 4)
				{
					return _data.swimSpeed;
				}
				if ((_type == 12 || _type == 44 || _type == 49) && _frameState == 4)
				{
					return _data.swimSpeed;
				}
				if (_type == 12 && _frameState == 2)
				{
					return _data.runSpeed + 13.5f;
				}
				if (_type == 44 && _frameState == 2)
				{
					return _data.runSpeed + 4f;
				}
				if (_type == 5 && _frameState == 2)
				{
					float num = _fatigue / _fatigueMax;
					return _data.runSpeed + 4f * (1f - num);
				}
				if (_type == 50 && _frameState == 2)
				{
					return _data.runSpeed + 2f;
				}
				if (_shouldSuperCart)
				{
					return SuperCartRunSpeed;
				}
				return _data.runSpeed;
			}
		}

		public float DashSpeed
		{
			get
			{
				if (_shouldSuperCart)
				{
					return SuperCartDashSpeed;
				}
				return _data.dashSpeed;
			}
		}

		public float Acceleration
		{
			get
			{
				if (_shouldSuperCart)
				{
					return SuperCartAcceleration;
				}
				return _data.acceleration;
			}
		}

		public float FallDamage => _data.fallDamage;

		public bool AutoJump => _data.constantJump;

		public bool BlockExtraJumps => _data.blockExtraJumps;

		public bool IsConsideredASlimeMount
		{
			get
			{
				if (_type != 3)
				{
					return _type == 50;
				}
				return true;
			}
		}

		public bool Cart
		{
			get
			{
				if (_data == null || !_active)
				{
					return false;
				}
				return _data.Minecart;
			}
		}

		public bool Directional
		{
			get
			{
				if (_data == null)
				{
					return true;
				}
				return _data.MinecartDirectional;
			}
		}

		public MountDelegatesData Delegations
		{
			get
			{
				if (_data == null)
				{
					return _defaultDelegatesData;
				}
				return _data.delegations;
			}
		}

		public Vector2 Origin => new Vector2((float)_data.textureWidth / 2f, (float)_data.textureHeight / (2f * (float)_data.totalFrames));

		public bool AbilityCharging => _abilityCharging;

		public bool AbilityActive => _abilityActive;

		public float AbilityCharge => (float)_abilityCharge / (float)_data.abilityChargeMax;

		public bool AllowDirectionChange
		{
			get
			{
				int type = _type;
				if (type == 9)
				{
					return _abilityCooldown < _data.abilityCooldown / 2;
				}
				return true;
			}
		}

		private static void MeowcartLandingSound(Player Player, Vector2 Position, int Width, int Height)
		{
			SoundEngine.PlaySound(37, (int)Position.X + Width / 2, (int)Position.Y + Height / 2, 5);
		}

		private static void MeowcartBumperSound(Player Player, Vector2 Position, int Width, int Height)
		{
			SoundEngine.PlaySound(37, (int)Position.X + Width / 2, (int)Position.Y + Height / 2, 3);
		}

		public Mount()
		{
			_debugDraw = new List<DrillDebugDraw>();
			Reset();
		}

		public void Reset()
		{
			_active = false;
			_type = -1;
			_flipDraw = false;
			_frame = 0;
			_frameCounter = 0f;
			_frameExtra = 0;
			_frameExtraCounter = 0f;
			_frameState = 0;
			_flyTime = 0;
			_idleTime = 0;
			_idleTimeNext = -1;
			_fatigueMax = 0f;
			_abilityCharging = false;
			_abilityCharge = 0;
			_aiming = false;
			_shouldSuperCart = false;
		}

		public static void Initialize()
		{
			mounts = new MountData[MountID.Count];
			MountData mountData = new MountData();
			mounts[0] = mountData;
			mountData.spawnDust = 57;
			mountData.spawnDustNoGravity = false;
			mountData.buff = 90;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 160;
			mountData.runSpeed = 5.5f;
			mountData.dashSpeed = 12f;
			mountData.acceleration = 0.09f;
			mountData.jumpHeight = 17;
			mountData.jumpSpeed = 5.31f;
			mountData.totalFrames = 12;
			int[] array = new int[mountData.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 30;
			}
			array[1] += 2;
			array[11] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 13;
			mountData.bodyFrame = 3;
			mountData.yOffset = -7;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 6;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 6;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 30;
			mountData.idleFrameStart = 2;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.RudolphMount[0];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.RudolphMount[1];
				mountData.frontTextureExtra = TextureAssets.RudolphMount[2];
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[2] = mountData;
			mountData.spawnDust = 58;
			mountData.buff = 129;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 160;
			mountData.runSpeed = 5f;
			mountData.dashSpeed = 9f;
			mountData.acceleration = 0.08f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 6.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = 22;
			}
			array[12] += 2;
			array[13] += 4;
			array[14] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 8;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 7;
			mountData.runningFrameCount = 5;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 11;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 3;
			mountData.idleFrameDelay = 30;
			mountData.idleFrameStart = 8;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.PigronMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[1] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 128;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.8f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 7.8f;
			mountData.acceleration = 0.13f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.01f;
			mountData.totalFrames = 7;
			array = new int[mountData.totalFrames];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = 14;
			}
			array[2] += 2;
			array[3] += 4;
			array[4] += 8;
			array[5] += 8;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 5;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.BunnyMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[3] = mountData;
			mountData.spawnDust = 56;
			mountData.buff = 130;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.5f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 4f;
			mountData.acceleration = 0.18f;
			mountData.jumpHeight = 12;
			mountData.jumpSpeed = 8.25f;
			mountData.constantJump = true;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 20;
			}
			array[1] += 2;
			array[3] -= 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 11;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.SlimeMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[6] = mountData;
			mountData.Minecart = true;
			mountData.MinecartDirectional = true;
			mountData.delegations = new MountDelegatesData();
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
			mountData.spawnDust = 213;
			mountData.buff = 118;
			mountData.extraBuff = 138;
			mountData.heightBoost = 10;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int m = 0; m < array.Length; m++)
			{
				array[m] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.MinecartMount;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[15] = mountData;
			SetAsMinecart(mountData, 209, 208, TextureAssets.DesertMinecartMount);
			mountData = new MountData();
			mounts[18] = mountData;
			SetAsMinecart(mountData, 221, 220, TextureAssets.Extra[108]);
			mountData = new MountData();
			mounts[19] = mountData;
			SetAsMinecart(mountData, 223, 222, TextureAssets.Extra[109]);
			mountData = new MountData();
			mounts[20] = mountData;
			SetAsMinecart(mountData, 225, 224, TextureAssets.Extra[110]);
			mountData = new MountData();
			mounts[21] = mountData;
			SetAsMinecart(mountData, 227, 226, TextureAssets.Extra[111]);
			mountData = new MountData();
			mounts[22] = mountData;
			SetAsMinecart(mountData, 229, 228, TextureAssets.Extra[112]);
			mountData = new MountData();
			mounts[24] = mountData;
			SetAsMinecart(mountData, 232, 231, TextureAssets.Extra[115]);
			mountData.frontTextureGlow = TextureAssets.Extra[116];
			mountData = new MountData();
			mounts[25] = mountData;
			SetAsMinecart(mountData, 234, 233, TextureAssets.Extra[117]);
			mountData = new MountData();
			mounts[26] = mountData;
			SetAsMinecart(mountData, 236, 235, TextureAssets.Extra[118]);
			mountData = new MountData();
			mounts[27] = mountData;
			SetAsMinecart(mountData, 238, 237, TextureAssets.Extra[119]);
			mountData = new MountData();
			mounts[28] = mountData;
			SetAsMinecart(mountData, 240, 239, TextureAssets.Extra[120]);
			mountData = new MountData();
			mounts[29] = mountData;
			SetAsMinecart(mountData, 242, 241, TextureAssets.Extra[121]);
			mountData = new MountData();
			mounts[30] = mountData;
			SetAsMinecart(mountData, 244, 243, TextureAssets.Extra[122]);
			mountData = new MountData();
			mounts[31] = mountData;
			SetAsMinecart(mountData, 246, 245, TextureAssets.Extra[123]);
			mountData = new MountData();
			mounts[32] = mountData;
			SetAsMinecart(mountData, 248, 247, TextureAssets.Extra[124]);
			mountData = new MountData();
			mounts[33] = mountData;
			SetAsMinecart(mountData, 250, 249, TextureAssets.Extra[125]);
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksMeow;
			mountData.delegations.MinecartLandingSound = MeowcartLandingSound;
			mountData.delegations.MinecartBumperSound = MeowcartBumperSound;
			mountData = new MountData();
			mounts[34] = mountData;
			SetAsMinecart(mountData, 252, 251, TextureAssets.Extra[126]);
			mountData = new MountData();
			mounts[35] = mountData;
			SetAsMinecart(mountData, 254, 253, TextureAssets.Extra[127]);
			mountData = new MountData();
			mounts[36] = mountData;
			SetAsMinecart(mountData, 256, 255, TextureAssets.Extra[128]);
			mountData = new MountData();
			mounts[38] = mountData;
			SetAsMinecart(mountData, 270, 269, TextureAssets.Extra[150]);
			if (Main.netMode != 2)
			{
				mountData.backTexture = mountData.frontTexture;
			}
			mountData = new MountData();
			mounts[39] = mountData;
			SetAsMinecart(mountData, 273, 272, TextureAssets.Extra[155]);
			mountData.yOffset -= 2;
			if (Main.netMode != 2)
			{
				mountData.frontTextureExtra = TextureAssets.Extra[165];
			}
			mountData.runSpeed = 6f;
			mountData.dashSpeed = 6f;
			mountData.acceleration = 0.02f;
			mountData = new MountData();
			mounts[16] = mountData;
			mountData.Minecart = true;
			mountData.delegations = new MountDelegatesData();
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
			mountData.spawnDust = 213;
			mountData.buff = 211;
			mountData.extraBuff = 210;
			mountData.heightBoost = 10;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int n = 0; n < array.Length; n++)
			{
				array[n] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.FishMinecartMount;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[51] = mountData;
			SetAsMinecart(mountData, 339, 338, TextureAssets.Extra[246], -10, -8);
			mountData.spawnDust = 211;
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksFart;
			mountData.delegations.MinecartBumperSound = DelegateMethods.Minecart.BumperSoundFart;
			mountData.delegations.MinecartLandingSound = DelegateMethods.Minecart.LandingSoundFart;
			mountData.delegations.MinecartJumpingSound = DelegateMethods.Minecart.JumpingSoundFart;
			mountData = new MountData();
			mounts[53] = mountData;
			SetAsMinecart(mountData, 347, 346, TextureAssets.Extra[251], -10, -8);
			mountData.spawnDust = 211;
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksTerraFart;
			mountData.delegations.MinecartBumperSound = DelegateMethods.Minecart.BumperSoundFart;
			mountData.delegations.MinecartLandingSound = DelegateMethods.Minecart.LandingSoundFart;
			mountData.delegations.MinecartJumpingSound = DelegateMethods.Minecart.JumpingSoundFart;
			mountData = new MountData();
			mounts[4] = mountData;
			mountData.spawnDust = 56;
			mountData.buff = 131;
			mountData.heightBoost = 26;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 5f;
			mountData.swimSpeed = 10f;
			mountData.acceleration = 0.08f;
			mountData.jumpHeight = 12;
			mountData.jumpSpeed = 3.7f;
			mountData.totalFrames = 12;
			array = new int[mountData.totalFrames];
			for (int num = 0; num < array.Length; num++)
			{
				array[num] = 26;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 28;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 3;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 6;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 6;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.TurtleMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[5] = mountData;
			mountData.spawnDust = 152;
			mountData.buff = 132;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 2f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 12;
			array = new int[mountData.totalFrames];
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				array[num2] = 16;
			}
			array[8] = 18;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 5;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 3;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 5;
			mountData.inAirFrameCount = 3;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 5;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 8;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.BeeMount[0];
				mountData.backTextureExtra = TextureAssets.BeeMount[1];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[7] = mountData;
			mountData.spawnDust = 226;
			mountData.spawnDustNoGravity = true;
			mountData.buff = 141;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num3 = 0; num3 < array.Length; num3++)
			{
				array[num3] = 16;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 8;
			mountData.standingFrameDelay = 4;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 8;
			mountData.runningFrameDelay = 4;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 8;
			mountData.flyingFrameDelay = 4;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 8;
			mountData.inAirFrameDelay = 4;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.UfoMount[0];
				mountData.frontTextureExtra = TextureAssets.UfoMount[1];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[8] = mountData;
			mountData.spawnDust = 226;
			mountData.buff = 142;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 1f;
			mountData.usesHover = true;
			mountData.swimSpeed = 4f;
			mountData.runSpeed = 6f;
			mountData.dashSpeed = 4f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.emitsLight = true;
			mountData.lightColor = new Vector3(0.3f, 0.3f, 0.4f);
			mountData.totalFrames = 1;
			array = new int[mountData.totalFrames];
			for (int num4 = 0; num4 < array.Length; num4++)
			{
				array[num4] = 4;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 8;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.DrillMount[0];
				mountData.backTextureGlow = TextureAssets.DrillMount[3];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.backTextureExtraGlow = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.DrillMount[1];
				mountData.frontTextureGlow = TextureAssets.DrillMount[4];
				mountData.frontTextureExtra = TextureAssets.DrillMount[2];
				mountData.frontTextureExtraGlow = TextureAssets.DrillMount[5];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			drillTextureSize = new Vector2(80f, 80f);
			Vector2 vector = new Vector2(mountData.textureWidth, mountData.textureHeight / mountData.totalFrames);
			if (drillTextureSize != vector)
			{
				throw new Exception("Be sure to update the Drill texture origin to match the actual texture size of " + mountData.textureWidth + ", " + mountData.textureHeight + ".");
			}
			mountData = new MountData();
			mounts[9] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 143;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.fallDamage = 0f;
			mountData.abilityChargeMax = 40;
			mountData.abilityCooldown = 20;
			mountData.abilityDuration = 0;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.4f;
			mountData.jumpHeight = 22;
			mountData.jumpSpeed = 10.01f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 12;
			array = new int[mountData.totalFrames];
			for (int num5 = 0; num5 < array.Length; num5++)
			{
				array[num5] = 16;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 6;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 6;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 6;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 6;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.ScutlixMount[0];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.ScutlixMount[1];
				mountData.frontTextureExtra = TextureAssets.ScutlixMount[2];
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			scutlixEyePositions = new Vector2[10];
			scutlixEyePositions[0] = new Vector2(60f, 2f);
			scutlixEyePositions[1] = new Vector2(70f, 6f);
			scutlixEyePositions[2] = new Vector2(68f, 6f);
			scutlixEyePositions[3] = new Vector2(76f, 12f);
			scutlixEyePositions[4] = new Vector2(80f, 10f);
			scutlixEyePositions[5] = new Vector2(84f, 18f);
			scutlixEyePositions[6] = new Vector2(74f, 20f);
			scutlixEyePositions[7] = new Vector2(76f, 24f);
			scutlixEyePositions[8] = new Vector2(70f, 34f);
			scutlixEyePositions[9] = new Vector2(76f, 34f);
			scutlixTextureSize = new Vector2(45f, 54f);
			Vector2 vector2 = new Vector2(mountData.textureWidth / 2, mountData.textureHeight / mountData.totalFrames);
			if (scutlixTextureSize != vector2)
			{
				throw new Exception("Be sure to update the Scutlix texture origin to match the actual texture size of " + mountData.textureWidth + ", " + mountData.textureHeight + ".");
			}
			for (int num6 = 0; num6 < scutlixEyePositions.Length; num6++)
			{
				scutlixEyePositions[num6] -= scutlixTextureSize;
			}
			mountData = new MountData();
			mounts[10] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 162;
			mountData.heightBoost = 34;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 12f;
			mountData.acceleration = 0.3f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int num7 = 0; num7 < array.Length; num7++)
			{
				array[num7] = 28;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 34;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 15;
			mountData.runningFrameStart = 1;
			mountData.dashingFrameCount = 6;
			mountData.dashingFrameDelay = 40;
			mountData.dashingFrameStart = 9;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 15;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.UnicornMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[11] = mountData;
			mountData.Minecart = true;
			mountData.delegations = new MountDelegatesData();
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksMech;
			mountData.spawnDust = 213;
			mountData.buff = 167;
			mountData.extraBuff = 166;
			mountData.heightBoost = 12;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int num8 = 0; num8 < array.Length; num8++)
			{
				array[num8] = 9;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = -1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 11;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.MinecartMechMount[0];
				mountData.frontTextureGlow = TextureAssets.MinecartMechMount[1];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[12] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 168;
			mountData.heightBoost = 14;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 1f;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 4;
			mountData.jumpSpeed = 3f;
			mountData.swimSpeed = 16f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 23;
			array = new int[mountData.totalFrames];
			for (int num9 = 0; num9 < array.Length; num9++)
			{
				array[num9] = 12;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 2;
			mountData.bodyFrame = 3;
			mountData.yOffset = 16;
			mountData.playerHeadOffset = 16;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 8;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 14;
			mountData.runningFrameStart = 8;
			mountData.flyingFrameCount = 8;
			mountData.flyingFrameDelay = 16;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 8;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 8;
			mountData.swimFrameDelay = 4;
			mountData.swimFrameStart = 15;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.CuteFishronMount[0];
				mountData.backTextureGlow = TextureAssets.CuteFishronMount[1];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[13] = mountData;
			mountData.Minecart = true;
			mountData.MinecartDirectional = true;
			mountData.delegations = new MountDelegatesData();
			mountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
			mountData.spawnDust = 213;
			mountData.buff = 184;
			mountData.extraBuff = 185;
			mountData.heightBoost = 10;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 10f;
			mountData.dashSpeed = 10f;
			mountData.acceleration = 0.03f;
			mountData.jumpHeight = 12;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int num10 = 0; num10 < array.Length; num10++)
			{
				array[num10] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.MinecartWoodMount;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[14] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 193;
			mountData.heightBoost = 8;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 8f;
			mountData.acceleration = 0.25f;
			mountData.jumpHeight = 20;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num11 = 0; num11 < array.Length; num11++)
			{
				array[num11] = 8;
			}
			array[1] += 2;
			array[3] += 2;
			array[6] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 4;
			mountData.bodyFrame = 3;
			mountData.yOffset = 9;
			mountData.playerHeadOffset = 10;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 30;
			mountData.runningFrameStart = 2;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.BasiliskMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[17] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 212;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 8f;
			mountData.acceleration = 0.25f;
			mountData.jumpHeight = 20;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int num12 = 0; num12 < array.Length; num12++)
			{
				array[num12] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 2;
			mountData.bodyFrame = 3;
			mountData.yOffset = 17 - mountData.heightBoost;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[97];
				mountData.backTextureExtra = TextureAssets.Extra[96];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTextureExtra.Width();
				mountData.textureHeight = mountData.backTextureExtra.Height();
			}
			mountData = new MountData();
			mounts[23] = mountData;
			mountData.spawnDust = 43;
			mountData.spawnDustNoGravity = true;
			mountData.buff = 230;
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 9f;
			mountData.dashSpeed = 9f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 6;
			array = new int[mountData.totalFrames];
			for (int num13 = 0; num13 < array.Length; num13++)
			{
				array[num13] = 6;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = -2;
			mountData.bodyFrame = 0;
			mountData.yOffset = 8;
			mountData.playerHeadOffset = 0;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 0;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 0;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 6;
			mountData.inAirFrameDelay = 8;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 0;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[113];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[37] = mountData;
			mountData.spawnDust = 282;
			mountData.buff = 265;
			mountData.heightBoost = 12;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 6f;
			mountData.acceleration = 0.15f;
			mountData.jumpHeight = 14;
			mountData.jumpSpeed = 6.01f;
			mountData.totalFrames = 10;
			array = new int[mountData.totalFrames];
			for (int num14 = 0; num14 < array.Length; num14++)
			{
				array[num14] = 20;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 4;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 20;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 2;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.runningFrameCount;
			mountData.swimFrameDelay = 10;
			mountData.swimFrameStart = mountData.runningFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[149];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[40] = mountData;
			SetAsHorse(mountData, 275, TextureAssets.Extra[161]);
			mountData = new MountData();
			mounts[41] = mountData;
			SetAsHorse(mountData, 276, TextureAssets.Extra[162]);
			mountData = new MountData();
			mounts[42] = mountData;
			SetAsHorse(mountData, 277, TextureAssets.Extra[163]);
			mountData = new MountData();
			mounts[43] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 278;
			mountData.heightBoost = 12;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.4f;
			mountData.runSpeed = 5f;
			mountData.acceleration = 0.1f;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 8f;
			mountData.constantJump = true;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int num15 = 0; num15 < array.Length; num15++)
			{
				array[num15] = 14;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 4;
			mountData.yOffset = 10;
			mountData.playerHeadOffset = 10;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 5;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 5;
			mountData.runningFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 5;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 1;
			mountData.swimFrameDelay = 5;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[164];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[44] = mountData;
			mountData.spawnDust = 228;
			mountData.buff = 279;
			mountData.heightBoost = 24;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 3f;
			mountData.dashSpeed = 6f;
			mountData.acceleration = 0.12f;
			mountData.jumpHeight = 3;
			mountData.jumpSpeed = 1f;
			mountData.swimSpeed = mountData.runSpeed;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 10;
			array = new int[mountData.totalFrames];
			for (int num16 = 0; num16 < array.Length; num16++)
			{
				array[num16] = 9;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 3;
			mountData.yOffset = 8;
			mountData.playerHeadOffset = 16;
			mountData.runningFrameCount = 10;
			mountData.runningFrameDelay = 8;
			mountData.runningFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[166];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[45] = mountData;
			mountData.spawnDust = 6;
			mountData.buff = 280;
			mountData.heightBoost = 25;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.1f;
			mountData.runSpeed = 12f;
			mountData.dashSpeed = 16f;
			mountData.acceleration = 0.5f;
			mountData.jumpHeight = 14;
			mountData.jumpSpeed = 7f;
			mountData.emitsLight = true;
			mountData.lightColor = new Vector3(0.6f, 0.4f, 0.35f);
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num17 = 0; num17 < array.Length; num17++)
			{
				array[num17] = 30;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 0;
			mountData.xOffset = 2;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 20;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 20;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 2;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 20;
			mountData.inAirFrameStart = 1;
			mountData.swimFrameCount = mountData.runningFrameCount;
			mountData.swimFrameDelay = 20;
			mountData.swimFrameStart = mountData.runningFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[167];
				mountData.backTextureGlow = TextureAssets.GlowMask[283];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[46] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 281;
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.fallDamage = 0f;
			mountData.abilityChargeMax = 40;
			mountData.abilityCooldown = 40;
			mountData.abilityDuration = 0;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.4f;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 9.01f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 27;
			array = new int[mountData.totalFrames];
			for (int num18 = 0; num18 < array.Length; num18++)
			{
				array[num18] = 4;
				if (num18 == 1 || num18 == 2 || num18 == 7 || num18 == 8)
				{
					array[num18] += 2;
				}
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 2;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 11;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.inAirFrameCount = 11;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.swimFrameCount = mountData.runningFrameCount;
			mountData.swimFrameDelay = mountData.runningFrameDelay;
			mountData.swimFrameStart = mountData.runningFrameStart;
			santankTextureSize = new Vector2(23f, 2f);
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[168];
				mountData.frontTextureExtra = TextureAssets.Extra[168];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new MountData();
			mounts[47] = mountData;
			mountData.spawnDust = 5;
			mountData.buff = 282;
			mountData.heightBoost = 34;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 12f;
			mountData.acceleration = 0.3f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int num19 = 0; num19 < array.Length; num19++)
			{
				array[num19] = 30;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.playerHeadOffset = 34;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 15;
			mountData.runningFrameStart = 1;
			mountData.dashingFrameCount = 6;
			mountData.dashingFrameDelay = 40;
			mountData.dashingFrameStart = 9;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 15;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[169];
				mountData.backTextureGlow = TextureAssets.GlowMask[284];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[48] = mountData;
			mountData.spawnDust = 62;
			mountData.buff = 283;
			mountData.heightBoost = 14;
			mountData.flightTimeMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 5;
			mountData.jumpSpeed = 6f;
			mountData.swimSpeed = mountData.runSpeed;
			mountData.totalFrames = 6;
			array = new int[mountData.totalFrames];
			for (int num20 = 0; num20 < array.Length; num20++)
			{
				array[num20] = 9;
			}
			array[0] += 6;
			array[1] += 6;
			array[2] += 4;
			array[3] += 4;
			array[4] += 4;
			array[5] += 6;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 0;
			mountData.yOffset = 16;
			mountData.playerHeadOffset = 16;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 8;
			mountData.runningFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[170];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[49] = mountData;
			mountData.spawnDust = 35;
			mountData.buff = 305;
			mountData.heightBoost = 8;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 1f;
			mountData.acceleration = 0.4f;
			mountData.jumpHeight = 4;
			mountData.jumpSpeed = 3f;
			mountData.swimSpeed = 14f;
			mountData.blockExtraJumps = true;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 320;
			mountData.usesHover = true;
			mountData.emitsLight = true;
			mountData.lightColor = new Vector3(0.3f, 0.15f, 0.1f);
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num21 = 0; num21 < array.Length; num21++)
			{
				array[num21] = 10;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 2;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 16;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 4;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 14;
			mountData.runningFrameStart = 4;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 4;
			mountData.swimFrameCount = 4;
			mountData.swimFrameDelay = 16;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[172];
				mountData.backTextureGlow = TextureAssets.GlowMask[285];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[50] = mountData;
			mountData.spawnDust = 243;
			mountData.buff = 318;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 160;
			mountData.fallDamage = 0.5f;
			mountData.runSpeed = 5.5f;
			mountData.dashSpeed = 5.5f;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 7.25f;
			mountData.constantJump = true;
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num22 = 0; num22 < array.Length; num22++)
			{
				array[num22] = 20;
			}
			array[1] += 2;
			array[4] += 2;
			array[5] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 5;
			mountData.runningFrameDelay = 16;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 5;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[204];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new MountData();
			mounts[52] = mountData;
			mountData.delegations = new MountDelegatesData();
			mountData.delegations.MouthPosition = DelegateMethods.Mount.WolfMouthPosition;
			mountData.delegations.HandPosition = DelegateMethods.Mount.NoHandPosition;
			mountData.spawnDust = 31;
			mountData.buff = 342;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.1f;
			mountData.runSpeed = 10f;
			mountData.acceleration = 0.25f;
			mountData.jumpHeight = 18;
			mountData.jumpSpeed = 10.01f;
			mountData.totalFrames = 15;
			array = new int[mountData.totalFrames];
			for (int num23 = 0; num23 < array.Length; num23++)
			{
				array[num23] = 0;
			}
			array[1] += -14;
			array[2] += -10;
			array[3] += -8;
			array[4] += 18;
			array[5] += -2;
			ref int reference = ref array[6];
			reference = reference;
			array[7] += 2;
			array[8] += 4;
			array[9] += 4;
			array[10] += 2;
			ref int reference2 = ref array[11];
			reference2 = reference2;
			array[12] += 4;
			array[13] += 2;
			array[14] += -4;
			mountData.playerYOffsets = array;
			mountData.xOffset = 4;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 5;
			mountData.inAirFrameCount = 3;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 12;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[253];
				mountData.frontTextureExtra = TextureAssets.Extra[254];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
		}

		private static void SetAsHorse(MountData newMount, int buff, Asset<Texture2D> texture)
		{
			newMount.spawnDust = 3;
			newMount.buff = buff;
			newMount.heightBoost = 34;
			newMount.flightTimeMax = 0;
			newMount.fallDamage = 0.5f;
			newMount.runSpeed = 3f;
			newMount.dashSpeed = 9f;
			newMount.acceleration = 0.25f;
			newMount.jumpHeight = 6;
			newMount.jumpSpeed = 7.01f;
			newMount.totalFrames = 16;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 28;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			newMount.playerYOffsets = array;
			newMount.xOffset = 5;
			newMount.bodyFrame = 3;
			newMount.yOffset = 1;
			newMount.playerHeadOffset = 34;
			newMount.standingFrameCount = 1;
			newMount.standingFrameDelay = 12;
			newMount.standingFrameStart = 0;
			newMount.runningFrameCount = 7;
			newMount.runningFrameDelay = 15;
			newMount.runningFrameStart = 1;
			newMount.dashingFrameCount = 6;
			newMount.dashingFrameDelay = 40;
			newMount.dashingFrameStart = 9;
			newMount.flyingFrameCount = 6;
			newMount.flyingFrameDelay = 6;
			newMount.flyingFrameStart = 1;
			newMount.inAirFrameCount = 1;
			newMount.inAirFrameDelay = 12;
			newMount.inAirFrameStart = 15;
			newMount.idleFrameCount = 0;
			newMount.idleFrameDelay = 0;
			newMount.idleFrameStart = 0;
			newMount.idleFrameLoop = false;
			newMount.swimFrameCount = newMount.inAirFrameCount;
			newMount.swimFrameDelay = newMount.inAirFrameDelay;
			newMount.swimFrameStart = newMount.inAirFrameStart;
			if (Main.netMode != 2)
			{
				newMount.backTexture = texture;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = Asset<Texture2D>.Empty;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = newMount.backTexture.Width();
				newMount.textureHeight = newMount.backTexture.Height();
			}
		}

		private static void SetAsMinecart(MountData newMount, int buffToLeft, int buffToRight, Asset<Texture2D> texture, int verticalOffset = 0, int playerVerticalOffset = 0)
		{
			newMount.Minecart = true;
			newMount.delegations = new MountDelegatesData();
			newMount.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
			newMount.spawnDust = 213;
			newMount.buff = buffToLeft;
			newMount.extraBuff = buffToRight;
			newMount.heightBoost = 10;
			newMount.flightTimeMax = 0;
			newMount.fallDamage = 1f;
			newMount.runSpeed = 13f;
			newMount.dashSpeed = 13f;
			newMount.acceleration = 0.04f;
			newMount.jumpHeight = 15;
			newMount.jumpSpeed = 5.15f;
			newMount.blockExtraJumps = true;
			newMount.totalFrames = 3;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 8 - verticalOffset + playerVerticalOffset;
			}
			newMount.playerYOffsets = array;
			newMount.xOffset = 1;
			newMount.bodyFrame = 3;
			newMount.yOffset = 13 + verticalOffset;
			newMount.playerHeadOffset = 14;
			newMount.standingFrameCount = 1;
			newMount.standingFrameDelay = 12;
			newMount.standingFrameStart = 0;
			newMount.runningFrameCount = 3;
			newMount.runningFrameDelay = 12;
			newMount.runningFrameStart = 0;
			newMount.flyingFrameCount = 0;
			newMount.flyingFrameDelay = 0;
			newMount.flyingFrameStart = 0;
			newMount.inAirFrameCount = 0;
			newMount.inAirFrameDelay = 0;
			newMount.inAirFrameStart = 0;
			newMount.idleFrameCount = 0;
			newMount.idleFrameDelay = 0;
			newMount.idleFrameStart = 0;
			newMount.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				newMount.backTexture = Asset<Texture2D>.Empty;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = texture;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = newMount.frontTexture.Width();
				newMount.textureHeight = newMount.frontTexture.Height();
			}
		}

		public static int GetHeightBoost(int MountType)
		{
			if (MountType <= -1 || MountType >= MountID.Count)
			{
				return 0;
			}
			return mounts[MountType].heightBoost;
		}

		public int JumpHeight(float xVelocity)
		{
			int num = _data.jumpHeight;
			switch (_type)
			{
			case 0:
				num += (int)(Math.Abs(xVelocity) / 4f);
				break;
			case 1:
				num += (int)(Math.Abs(xVelocity) / 2.5f);
				break;
			case 4:
			case 49:
				if (_frameState == 4)
				{
					num += 5;
				}
				break;
			}
			if (_shouldSuperCart)
			{
				num = SuperCartJumpHeight;
			}
			return num;
		}

		public float JumpSpeed(float xVelocity)
		{
			float num = _data.jumpSpeed;
			switch (_type)
			{
			case 0:
			case 1:
				num += Math.Abs(xVelocity) / 7f;
				break;
			case 4:
			case 49:
				if (_frameState == 4)
				{
					num += 2.5f;
				}
				break;
			}
			if (_shouldSuperCart)
			{
				num = SuperCartJumpSpeed;
			}
			return num;
		}

		public bool CanFly()
		{
			if (!_active || _data.flightTimeMax == 0)
			{
				return false;
			}
			if (_type == 48)
			{
				return false;
			}
			return true;
		}

		public bool CanHover()
		{
			if (!_active || !_data.usesHover)
			{
				return false;
			}
			if (_type == 49)
			{
				return _frameState == 4;
			}
			if (!_data.usesHover)
			{
				return false;
			}
			return true;
		}

		public IEntitySource GetProjectileSpawnSource(Player mountedPlayer)
		{
			return new EntitySource_Mount(mountedPlayer, Type);
		}

		public void StartAbilityCharge(Player mountedPlayer)
		{
			if (Main.myPlayer == mountedPlayer.whoAmI)
			{
				int type = _type;
				if (type == 9)
				{
					int type2 = 441;
					float num = Main.screenPosition.X + (float)Main.mouseX;
					float num2 = Main.screenPosition.Y + (float)Main.mouseY;
					float ai = num - mountedPlayer.position.X;
					float ai2 = num2 - mountedPlayer.position.Y;
					Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), num, num2, 0f, 0f, type2, 0, 0f, mountedPlayer.whoAmI, ai, ai2);
					_abilityCharging = true;
				}
			}
			else
			{
				int type = _type;
				if (type == 9)
				{
					_abilityCharging = true;
				}
			}
		}

		public void StopAbilityCharge()
		{
			int type = _type;
			if (type == 9 || type == 46)
			{
				_abilityCharging = false;
				_abilityCooldown = _data.abilityCooldown;
				_abilityDuration = _data.abilityDuration;
			}
		}

		public bool CheckBuff(int buffID)
		{
			if (_data.buff != buffID)
			{
				return _data.extraBuff == buffID;
			}
			return true;
		}

		public void AbilityRecovery()
		{
			if (_abilityCharging)
			{
				if (_abilityCharge < _data.abilityChargeMax)
				{
					_abilityCharge++;
				}
			}
			else if (_abilityCharge > 0)
			{
				_abilityCharge--;
			}
			if (_abilityCooldown > 0)
			{
				_abilityCooldown--;
			}
			if (_abilityDuration > 0)
			{
				_abilityDuration--;
			}
		}

		public void FatigueRecovery()
		{
			if (_fatigue > 2f)
			{
				_fatigue -= 2f;
			}
			else
			{
				_fatigue = 0f;
			}
		}

		public bool Flight()
		{
			if (_flyTime <= 0)
			{
				return false;
			}
			_flyTime--;
			return true;
		}

		public void UpdateDrill(Player mountedPlayer, bool controlUp, bool controlDown)
		{
			DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
			for (int i = 0; i < drillMountData.beams.Length; i++)
			{
				DrillBeam drillBeam = drillMountData.beams[i];
				if (drillBeam.cooldown > 1)
				{
					drillBeam.cooldown--;
				}
				else if (drillBeam.cooldown == 1)
				{
					drillBeam.cooldown = 0;
					drillBeam.curTileTarget = Point16.NegativeOne;
				}
			}
			drillMountData.diodeRotation = drillMountData.diodeRotation * 0.85f + 0.15f * drillMountData.diodeRotationTarget;
			if (drillMountData.beamCooldown > 0)
			{
				drillMountData.beamCooldown--;
			}
		}

		public void UseDrill(Player mountedPlayer)
		{
			if (_type != 8 || !_abilityActive)
			{
				return;
			}
			DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
			bool flag = mountedPlayer.whoAmI == Main.myPlayer;
			if (mountedPlayer.controlUseItem)
			{
				for (int i = 0; i < amountOfBeamsAtOnce; i++)
				{
					if (drillMountData.beamCooldown != 0)
					{
						break;
					}
					for (int j = 0; j < drillMountData.beams.Length; j++)
					{
						DrillBeam drillBeam = drillMountData.beams[j];
						if (drillBeam.cooldown != 0)
						{
							continue;
						}
						Point16 point = DrillSmartCursor_Blocks(mountedPlayer, drillMountData);
						if (point == Point16.NegativeOne)
						{
							continue;
						}
						drillBeam.curTileTarget = point;
						int pickPower = drillPickPower;
						if (flag)
						{
							bool flag2 = true;
							if (WorldGen.InWorld(point.X, point.Y) && Main.tile[point.X, point.Y] != null && Main.tile[point.X, point.Y].type == 26 && !Main.hardMode)
							{
								flag2 = false;
								mountedPlayer.Hurt(PlayerDeathReason.ByOther(4), mountedPlayer.statLife / 2, -mountedPlayer.direction);
							}
							if (mountedPlayer.noBuilding)
							{
								flag2 = false;
							}
							if (flag2)
							{
								mountedPlayer.PickTile(point.X, point.Y, pickPower);
							}
						}
						Vector2 vector = new Vector2((float)(point.X << 4) + 8f, (float)(point.Y << 4) + 8f);
						float num = (vector - mountedPlayer.Center).ToRotation();
						for (int k = 0; k < 2; k++)
						{
							float num2 = num + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * (MathF.PI / 2f);
							float num3 = (float)Main.rand.NextDouble() * 2f + 2f;
							Vector2 vector2 = new Vector2((float)Math.Cos(num2) * num3, (float)Math.Sin(num2) * num3);
							int num4 = Dust.NewDust(vector, 0, 0, 230, vector2.X, vector2.Y);
							Main.dust[num4].noGravity = true;
							Main.dust[num4].customData = mountedPlayer;
						}
						if (flag)
						{
							Tile.SmoothSlope(point.X, point.Y, applyToNeighbors: true, sync: true);
						}
						drillBeam.cooldown = drillPickTime;
						drillBeam.lastPurpose = 0;
						break;
					}
				}
			}
			if (!mountedPlayer.controlUseTile)
			{
				return;
			}
			for (int l = 0; l < amountOfBeamsAtOnce; l++)
			{
				if (drillMountData.beamCooldown != 0)
				{
					break;
				}
				for (int m = 0; m < drillMountData.beams.Length; m++)
				{
					DrillBeam drillBeam2 = drillMountData.beams[m];
					if (drillBeam2.cooldown != 0)
					{
						continue;
					}
					Point16 point2 = DrillSmartCursor_Walls(mountedPlayer, drillMountData);
					if (point2 == Point16.NegativeOne)
					{
						continue;
					}
					drillBeam2.curTileTarget = point2;
					int damage = drillPickPower;
					if (flag)
					{
						bool flag3 = true;
						if (mountedPlayer.noBuilding)
						{
							flag3 = false;
						}
						if (flag3)
						{
							mountedPlayer.PickWall(point2.X, point2.Y, damage);
						}
					}
					Vector2 vector3 = new Vector2((float)(point2.X << 4) + 8f, (float)(point2.Y << 4) + 8f);
					float num5 = (vector3 - mountedPlayer.Center).ToRotation();
					for (int n = 0; n < 2; n++)
					{
						float num6 = num5 + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * (MathF.PI / 2f);
						float num7 = (float)Main.rand.NextDouble() * 2f + 2f;
						Vector2 vector4 = new Vector2((float)Math.Cos(num6) * num7, (float)Math.Sin(num6) * num7);
						int num8 = Dust.NewDust(vector3, 0, 0, 230, vector4.X, vector4.Y);
						Main.dust[num8].noGravity = true;
						Main.dust[num8].customData = mountedPlayer;
					}
					drillBeam2.cooldown = drillPickTime;
					drillBeam2.lastPurpose = 1;
					break;
				}
			}
		}

		private Point16 DrillSmartCursor_Blocks(Player mountedPlayer, DrillMountData data)
		{
			Vector2 vector = ((mountedPlayer.whoAmI != Main.myPlayer) ? data.crosshairPosition : (Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY)));
			Vector2 center = mountedPlayer.Center;
			Vector2 vector2 = vector - center;
			float num = vector2.Length();
			if (num > 224f)
			{
				num = 224f;
			}
			num += 32f;
			vector2.Normalize();
			Vector2 end = center + vector2 * num;
			Point16 tilePoint = new Point16(-1, -1);
			if (!Utils.PlotTileLine(center, end, 65.6f, delegate(int x, int y)
			{
				tilePoint = new Point16(x, y);
				for (int i = 0; i < data.beams.Length; i++)
				{
					if (data.beams[i].curTileTarget == tilePoint && data.beams[i].lastPurpose == 0)
					{
						return true;
					}
				}
				if (!WorldGen.CanKillTile(x, y))
				{
					return true;
				}
				return (Main.tile[x, y] == null || Main.tile[x, y].inActive() || !Main.tile[x, y].active()) ? true : false;
			}))
			{
				return tilePoint;
			}
			return new Point16(-1, -1);
		}

		private Point16 DrillSmartCursor_Walls(Player mountedPlayer, DrillMountData data)
		{
			Vector2 vector = ((mountedPlayer.whoAmI != Main.myPlayer) ? data.crosshairPosition : (Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY)));
			Vector2 center = mountedPlayer.Center;
			Vector2 vector2 = vector - center;
			float num = vector2.Length();
			if (num > 224f)
			{
				num = 224f;
			}
			num += 32f;
			num += 16f;
			vector2.Normalize();
			Vector2 end = center + vector2 * num;
			Point16 tilePoint = new Point16(-1, -1);
			if (!Utils.PlotTileLine(center, end, 97.6f, delegate(int x, int y)
			{
				tilePoint = new Point16(x, y);
				for (int i = 0; i < data.beams.Length; i++)
				{
					if (data.beams[i].curTileTarget == tilePoint && data.beams[i].lastPurpose == 1)
					{
						return true;
					}
				}
				Tile tile = Main.tile[x, y];
				if (tile == null)
				{
					return false;
				}
				return (tile.wall <= 0 || !Player.CanPlayerSmashWall(x, y)) ? true : false;
			}))
			{
				return tilePoint;
			}
			return new Point16(-1, -1);
		}

		public void UseAbility(Player mountedPlayer, Vector2 mousePosition, bool toggleOn)
		{
			switch (_type)
			{
			case 9:
			{
				if (Main.myPlayer != mountedPlayer.whoAmI)
				{
					break;
				}
				int type2 = 606;
				mousePosition = ClampToDeadZone(mountedPlayer, mousePosition);
				Vector2 vector5 = default(Vector2);
				vector5.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
				vector5.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				int num3 = (_frameExtra - 6) * 2;
				Vector2 vector6 = default(Vector2);
				for (int i = 0; i < 2; i++)
				{
					vector6.Y = vector5.Y + scutlixEyePositions[num3 + i].Y + (float)_data.yOffset;
					if (mountedPlayer.direction == -1)
					{
						vector6.X = vector5.X - scutlixEyePositions[num3 + i].X - (float)_data.xOffset;
					}
					else
					{
						vector6.X = vector5.X + scutlixEyePositions[num3 + i].X + (float)_data.xOffset;
					}
					Vector2 vector7 = mousePosition - vector6;
					vector7.Normalize();
					vector7 *= 14f;
					int damage3 = 150;
					vector6 += vector7;
					Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), vector6.X, vector6.Y, vector7.X, vector7.Y, type2, damage3, 0f, Main.myPlayer);
				}
				break;
			}
			case 46:
				if (Main.myPlayer == mountedPlayer.whoAmI)
				{
					if (_abilityCooldown <= 10)
					{
						int damage = 120;
						Vector2 vector = mountedPlayer.Center + new Vector2(mountedPlayer.width * -mountedPlayer.direction, 26f);
						Vector2 vector2 = new Vector2(0f, -4f).RotatedByRandom(0.10000000149011612);
						Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), vector.X, vector.Y, vector2.X, vector2.Y, 930, damage, 0f, Main.myPlayer);
						SoundEngine.PlaySound(SoundID.Item89.SoundId, (int)vector.X, (int)vector.Y, SoundID.Item89.Style, 0.2f);
					}
					int type = 14;
					int damage2 = 100;
					mousePosition = ClampToDeadZone(mountedPlayer, mousePosition);
					Vector2 vector3 = default(Vector2);
					vector3.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
					vector3.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
					Vector2 vector4 = new Vector2(vector3.X + (float)(mountedPlayer.width * mountedPlayer.direction), vector3.Y - 12f);
					Vector2 v = mousePosition - vector4;
					v = v.SafeNormalize(Vector2.Zero);
					v *= 12f;
					v = v.RotatedByRandom(0.20000000298023224);
					Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), vector4.X, vector4.Y, v.X, v.Y, type, damage2, 0f, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.Item11.SoundId, (int)vector4.X, (int)vector4.Y, SoundID.Item11.Style, 0.2f);
				}
				break;
			case 8:
				if (Main.myPlayer == mountedPlayer.whoAmI)
				{
					if (!toggleOn)
					{
						_abilityActive = false;
					}
					else if (!_abilityActive)
					{
						if (mountedPlayer.whoAmI == Main.myPlayer)
						{
							float num = Main.screenPosition.X + (float)Main.mouseX;
							float num2 = Main.screenPosition.Y + (float)Main.mouseY;
							float ai = num - mountedPlayer.position.X;
							float ai2 = num2 - mountedPlayer.position.Y;
							Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), num, num2, 0f, 0f, 453, 0, 0f, mountedPlayer.whoAmI, ai, ai2);
						}
						_abilityActive = true;
					}
				}
				else
				{
					_abilityActive = toggleOn;
				}
				break;
			}
		}

		public bool Hover(Player mountedPlayer)
		{
			bool flag = DoesHoverIgnoresFatigue();
			bool flag2 = _frameState == 2 || _frameState == 4;
			if (_type == 49)
			{
				flag2 = _frameState == 4;
			}
			if (flag2)
			{
				bool flag3 = true;
				float num = 1f;
				float num2 = mountedPlayer.gravity / Player.defaultGravity;
				if (mountedPlayer.slowFall)
				{
					num2 /= 3f;
				}
				if (num2 < 0.25f)
				{
					num2 = 0.25f;
				}
				if (!flag)
				{
					if (_flyTime > 0)
					{
						_flyTime--;
					}
					else if (_fatigue < _fatigueMax)
					{
						_fatigue += num2;
					}
					else
					{
						flag3 = false;
					}
				}
				if (_type == 12 && !mountedPlayer.MountFishronSpecial)
				{
					num = 0.5f;
				}
				float num3 = _fatigue / _fatigueMax;
				if (flag)
				{
					num3 = 0f;
				}
				bool flag4 = true;
				if (_type == 48)
				{
					flag4 = false;
				}
				float num4 = 4f * num3;
				float num5 = 4f * num3;
				bool flag5 = false;
				if (_type == 48)
				{
					num4 = 0f;
					num5 = 0f;
					if (!flag3)
					{
						flag5 = true;
					}
					if (mountedPlayer.controlDown)
					{
						num5 = 8f;
					}
				}
				if (num4 == 0f)
				{
					num4 = -0.001f;
				}
				if (num5 == 0f)
				{
					num5 = -0.001f;
				}
				float num6 = mountedPlayer.velocity.Y;
				if (flag4 && (mountedPlayer.controlUp || mountedPlayer.controlJump) && flag3)
				{
					num4 = -2f - 6f * (1f - num3);
					if (_type == 48)
					{
						num4 /= 3f;
					}
					num6 -= _data.acceleration * num;
				}
				else if (mountedPlayer.controlDown)
				{
					num6 += _data.acceleration * num;
					num5 = 8f;
				}
				else if (flag5)
				{
					float num7 = mountedPlayer.gravity * mountedPlayer.gravDir;
					num6 += num7;
					num5 = 4f;
				}
				else
				{
					_ = mountedPlayer.jump;
				}
				if (num6 < num4)
				{
					num6 = ((!(num4 - num6 < _data.acceleration)) ? (num6 + _data.acceleration * num) : num4);
				}
				else if (num6 > num5)
				{
					num6 = ((!(num6 - num5 < _data.acceleration)) ? (num6 - _data.acceleration * num) : num5);
				}
				mountedPlayer.velocity.Y = num6;
				if (num4 == -0.001f && num5 == -0.001f && num6 == -0.001f)
				{
					mountedPlayer.position.Y -= -0.001f;
				}
				mountedPlayer.fallStart = (int)(mountedPlayer.position.Y / 16f);
			}
			else if (!flag)
			{
				mountedPlayer.velocity.Y += mountedPlayer.gravity * mountedPlayer.gravDir;
			}
			else if (mountedPlayer.velocity.Y == 0f)
			{
				Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir * 1f;
				if (Collision.TileCollision(mountedPlayer.position, velocity, mountedPlayer.width, mountedPlayer.height, fallThrough: false, fall2: false, (int)mountedPlayer.gravDir).Y != 0f || mountedPlayer.controlDown)
				{
					mountedPlayer.velocity.Y = 0.001f;
				}
			}
			else if (mountedPlayer.velocity.Y == -0.001f)
			{
				mountedPlayer.velocity.Y -= -0.001f;
			}
			if (_type == 7)
			{
				float num8 = mountedPlayer.velocity.X / _data.dashSpeed;
				if ((double)num8 > 0.95)
				{
					num8 = 0.95f;
				}
				if ((double)num8 < -0.95)
				{
					num8 = -0.95f;
				}
				float fullRotation = MathF.PI / 4f * num8 / 2f;
				float num9 = Math.Abs(2f - (float)_frame / 2f) / 2f;
				Lighting.AddLight((int)(mountedPlayer.position.X + (float)(mountedPlayer.width / 2)) / 16, (int)(mountedPlayer.position.Y + (float)(mountedPlayer.height / 2)) / 16, 0.4f, 0.2f * num9, 0f);
				mountedPlayer.fullRotation = fullRotation;
			}
			else if (_type == 8)
			{
				float num10 = mountedPlayer.velocity.X / _data.dashSpeed;
				if ((double)num10 > 0.95)
				{
					num10 = 0.95f;
				}
				if ((double)num10 < -0.95)
				{
					num10 = -0.95f;
				}
				float num11 = (mountedPlayer.fullRotation = MathF.PI / 4f * num10 / 2f);
				DrillMountData obj = (DrillMountData)_mountSpecificData;
				float outerRingRotation = obj.outerRingRotation;
				outerRingRotation += mountedPlayer.velocity.X / 80f;
				if (outerRingRotation > MathF.PI)
				{
					outerRingRotation -= MathF.PI * 2f;
				}
				else if (outerRingRotation < -MathF.PI)
				{
					outerRingRotation += MathF.PI * 2f;
				}
				obj.outerRingRotation = outerRingRotation;
			}
			else if (_type == 23)
			{
				float value = (0f - mountedPlayer.velocity.Y) / _data.dashSpeed;
				value = MathHelper.Clamp(value, -1f, 1f);
				float value2 = mountedPlayer.velocity.X / _data.dashSpeed;
				value2 = MathHelper.Clamp(value2, -1f, 1f);
				float num12 = -MathF.PI / 16f * value * (float)mountedPlayer.direction;
				float num13 = MathF.PI / 16f * value2;
				float num14 = (mountedPlayer.fullRotation = num12 + num13);
				mountedPlayer.fullRotationOrigin = new Vector2(mountedPlayer.width / 2, mountedPlayer.height);
			}
			return true;
		}

		private bool DoesHoverIgnoresFatigue()
		{
			if (_type != 7 && _type != 8 && _type != 12 && _type != 23 && _type != 44)
			{
				return _type == 49;
			}
			return true;
		}

		private float GetWitchBroomTrinketRotation(Player player)
		{
			float num = Utils.Clamp(player.velocity.X / 10f, -1f, 1f);
			float num2 = 0f;
			Point point = player.Center.ToTileCoordinates();
			float num3 = 0.5f;
			if (WorldGen.InAPlaceWithWind(point.X, point.Y, 1, 1))
			{
				num3 = 1f;
			}
			num2 = (float)Math.Sin((float)player.miscCounter / 300f * (MathF.PI * 2f) * 3f) * (MathF.PI / 4f) * Math.Abs(Main.WindForVisuals) * 0.5f + MathF.PI / 4f * (0f - Main.WindForVisuals) * 0.5f;
			num2 *= num3;
			return num * (float)Math.Sin((float)player.miscCounter / 150f * (MathF.PI * 2f) * 3f) * (MathF.PI / 4f) * 0.5f + num * (MathF.PI / 4f) * 0.5f + num2;
		}

		private Vector2 GetWitchBroomTrinketOriginOffset(Player player)
		{
			return new Vector2(27 * player.direction, 5f);
		}

		public void UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			if (_frameState != state)
			{
				_frameState = state;
				if (_type != 48 || (state != 1 && state != 2))
				{
					_frameCounter = 0f;
				}
			}
			if (state != 0)
			{
				_idleTime = 0;
			}
			if (_data.emitsLight)
			{
				Point point = mountedPlayer.Center.ToTileCoordinates();
				Lighting.AddLight(point.X, point.Y, _data.lightColor.X, _data.lightColor.Y, _data.lightColor.Z);
			}
			switch (_type)
			{
			case 52:
				if (state == 4)
				{
					state = 2;
				}
				break;
			case 17:
				UpdateFrame_GolfCart(mountedPlayer, state, velocity);
				break;
			case 5:
				if (state != 2)
				{
					_frameExtra = 0;
					_frameExtraCounter = 0f;
				}
				break;
			case 7:
				state = 2;
				break;
			case 49:
			{
				if (state != 4 && mountedPlayer.wet)
				{
					_frameState = (state = 4);
				}
				velocity.Length();
				float num5 = mountedPlayer.velocity.Y * 0.05f;
				if (mountedPlayer.direction < 0)
				{
					num5 *= -1f;
				}
				mountedPlayer.fullRotation = num5;
				mountedPlayer.fullRotationOrigin = new Vector2(mountedPlayer.width / 2, mountedPlayer.height / 2);
				break;
			}
			case 43:
				if (mountedPlayer.velocity.Y == 0f)
				{
					mountedPlayer.isPerformingPogostickTricks = false;
				}
				if (mountedPlayer.isPerformingPogostickTricks)
				{
					mountedPlayer.fullRotation += (float)mountedPlayer.direction * (MathF.PI * 2f) / 30f;
				}
				else
				{
					mountedPlayer.fullRotation = (float)Math.Sign(mountedPlayer.velocity.X) * Utils.GetLerpValue(0f, RunSpeed - 0.2f, Math.Abs(mountedPlayer.velocity.X), clamped: true) * 0.4f;
				}
				mountedPlayer.fullRotationOrigin = new Vector2(mountedPlayer.width / 2, (float)mountedPlayer.height * 0.8f);
				break;
			case 9:
				if (_aiming)
				{
					break;
				}
				_frameExtraCounter += 1f;
				if (_frameExtraCounter >= 12f)
				{
					_frameExtraCounter = 0f;
					_frameExtra++;
					if (_frameExtra >= 6)
					{
						_frameExtra = 0;
					}
				}
				break;
			case 46:
				if (state != 0)
				{
					state = 1;
				}
				if (!_aiming)
				{
					if (state == 0)
					{
						_frameExtra = 12;
						_frameExtraCounter = 0f;
						break;
					}
					if (_frameExtra < 12)
					{
						_frameExtra = 12;
					}
					_frameExtraCounter += Math.Abs(velocity.X);
					if (_frameExtraCounter >= 8f)
					{
						_frameExtraCounter = 0f;
						_frameExtra++;
						if (_frameExtra >= 24)
						{
							_frameExtra = 12;
						}
					}
					break;
				}
				if (_frameExtra < 24)
				{
					_frameExtra = 24;
				}
				_frameExtraCounter += 1f;
				if (_frameExtraCounter >= 3f)
				{
					_frameExtraCounter = 0f;
					_frameExtra++;
					if (_frameExtra >= 27)
					{
						_frameExtra = 24;
					}
				}
				break;
			case 8:
			{
				if (state != 0 && state != 1)
				{
					break;
				}
				Vector2 position = default(Vector2);
				position.X = mountedPlayer.position.X;
				position.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				int num6 = (int)(position.X / 16f);
				_ = position.Y / 16f;
				float num7 = 0f;
				float num8 = mountedPlayer.width;
				while (num8 > 0f)
				{
					float num9 = (float)((num6 + 1) * 16) - position.X;
					if (num9 > num8)
					{
						num9 = num8;
					}
					num7 += Collision.GetTileRotation(position) * num9;
					num8 -= num9;
					position.X += num9;
					num6++;
				}
				float num10 = num7 / (float)mountedPlayer.width - mountedPlayer.fullRotation;
				float num11 = 0f;
				float num12 = MathF.PI / 20f;
				if (num10 < 0f)
				{
					num11 = ((!(num10 > 0f - num12)) ? (0f - num12) : num10);
				}
				else if (num10 > 0f)
				{
					num11 = ((!(num10 < num12)) ? num12 : num10);
				}
				if (num11 != 0f)
				{
					mountedPlayer.fullRotation += num11;
					if (mountedPlayer.fullRotation > MathF.PI / 4f)
					{
						mountedPlayer.fullRotation = MathF.PI / 4f;
					}
					if (mountedPlayer.fullRotation < -MathF.PI / 4f)
					{
						mountedPlayer.fullRotation = -MathF.PI / 4f;
					}
				}
				break;
			}
			case 10:
			case 40:
			case 41:
			case 42:
			case 47:
			{
				bool flag5 = Math.Abs(velocity.X) > DashSpeed - RunSpeed / 2f;
				if (state == 1)
				{
					bool flag6 = false;
					if (flag5)
					{
						state = 5;
						if (_frameExtra < 6)
						{
							flag6 = true;
						}
						_frameExtra++;
					}
					else
					{
						_frameExtra = 0;
					}
					if ((_type == 10 || _type == 47) && flag6)
					{
						int type = 6;
						if (_type == 10)
						{
							type = Utils.SelectRandom<int>(Main.rand, 176, 177, 179);
						}
						Vector2 vector8 = mountedPlayer.Center + new Vector2(mountedPlayer.width * mountedPlayer.direction, 0f);
						Vector2 vector9 = new Vector2(40f, 30f);
						float num22 = MathF.PI * 2f * Main.rand.NextFloat();
						for (float num23 = 0f; num23 < 14f; num23 += 1f)
						{
							Dust dust5 = Main.dust[Dust.NewDust(vector8, 0, 0, type)];
							Vector2 vector10 = Vector2.UnitY.RotatedBy(num23 * (MathF.PI * 2f) / 14f + num22);
							vector10 *= 0.2f * (float)_frameExtra;
							dust5.position = vector8 + vector10 * vector9;
							dust5.velocity = vector10 + new Vector2(RunSpeed - (float)(Math.Sign(velocity.X) * _frameExtra * 2), 0f);
							dust5.noGravity = true;
							if (_type == 47)
							{
								dust5.noLightEmittence = true;
							}
							dust5.scale = 1f + Main.rand.NextFloat() * 0.8f;
							dust5.fadeIn = Main.rand.NextFloat() * 2f;
							dust5.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
						}
					}
				}
				if (_type == 10 && flag5)
				{
					Dust obj3 = Main.dust[Dust.NewDust(mountedPlayer.position, mountedPlayer.width, mountedPlayer.height, Utils.SelectRandom<int>(Main.rand, 176, 177, 179))];
					obj3.velocity = Vector2.Zero;
					obj3.noGravity = true;
					obj3.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
					obj3.fadeIn = 1f + Main.rand.NextFloat() * 2f;
					obj3.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				if (_type == 47 && flag5 && velocity.Y == 0f)
				{
					int num24 = (int)mountedPlayer.Center.X / 16;
					int num25 = (int)(mountedPlayer.position.Y + (float)mountedPlayer.height - 1f) / 16;
					Tile tile = Main.tile[num24, num25 + 1];
					if (tile != null && tile.active() && tile.liquid == 0 && WorldGen.SolidTileAllowBottomSlope(num24, num25 + 1))
					{
						ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.WallOfFleshGoatMountFlames, new ParticleOrchestraSettings
						{
							PositionInWorld = new Vector2(num24 * 16 + 8, num25 * 16 + 16)
						}, mountedPlayer.whoAmI);
					}
				}
				break;
			}
			case 44:
			{
				state = 1;
				bool flag3 = Math.Abs(velocity.X) > DashSpeed - RunSpeed / 4f;
				if (_mountSpecificData == null)
				{
					_mountSpecificData = false;
				}
				bool flag4 = (bool)_mountSpecificData;
				if (flag4 && !flag3)
				{
					_mountSpecificData = false;
				}
				else if (!flag4 && flag3)
				{
					_mountSpecificData = true;
					Vector2 vector5 = mountedPlayer.Center + new Vector2(mountedPlayer.width * mountedPlayer.direction, 0f);
					Vector2 vector6 = new Vector2(40f, 30f);
					float num17 = MathF.PI * 2f * Main.rand.NextFloat();
					for (float num18 = 0f; num18 < 20f; num18 += 1f)
					{
						Dust dust4 = Main.dust[Dust.NewDust(vector5, 0, 0, 228)];
						Vector2 vector7 = Vector2.UnitY.RotatedBy(num18 * (MathF.PI * 2f) / 20f + num17);
						vector7 *= 0.8f;
						dust4.position = vector5 + vector7 * vector6;
						dust4.velocity = vector7 + new Vector2(RunSpeed - (float)Math.Sign(velocity.Length()), 0f);
						if (velocity.X > 0f)
						{
							dust4.velocity.X *= -1f;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust4.velocity *= 0.5f;
						}
						dust4.noGravity = true;
						dust4.scale = 1.5f + Main.rand.NextFloat() * 0.8f;
						dust4.fadeIn = 0f;
						dust4.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
				}
				int num19 = (int)RunSpeed - (int)Math.Abs(velocity.X);
				if (num19 <= 0)
				{
					num19 = 1;
				}
				if (Main.rand.Next(num19) == 0)
				{
					int num20 = 22;
					int num21 = mountedPlayer.width / 2 + 10;
					Vector2 bottom = mountedPlayer.Bottom;
					bottom.X -= num21;
					bottom.Y -= num20 - 6;
					Dust obj2 = Main.dust[Dust.NewDust(bottom, num21 * 2, num20, 228)];
					obj2.velocity = Vector2.Zero;
					obj2.noGravity = true;
					obj2.noLight = true;
					obj2.scale = 0.25f + Main.rand.NextFloat() * 0.8f;
					obj2.fadeIn = 0.5f + Main.rand.NextFloat() * 2f;
					obj2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				break;
			}
			case 45:
			{
				bool flag = Math.Abs(velocity.X) > DashSpeed * 0.9f;
				if (_mountSpecificData == null)
				{
					_mountSpecificData = false;
				}
				bool flag2 = (bool)_mountSpecificData;
				if (flag2 && !flag)
				{
					_mountSpecificData = false;
				}
				else if (!flag2 && flag)
				{
					_mountSpecificData = true;
					Vector2 vector2 = mountedPlayer.Center + new Vector2(mountedPlayer.width * mountedPlayer.direction, 0f);
					Vector2 vector3 = new Vector2(40f, 30f);
					float num13 = MathF.PI * 2f * Main.rand.NextFloat();
					for (float num14 = 0f; num14 < 20f; num14 += 1f)
					{
						Dust dust2 = Main.dust[Dust.NewDust(vector2, 0, 0, 6)];
						Vector2 vector4 = Vector2.UnitY.RotatedBy(num14 * (MathF.PI * 2f) / 20f + num13);
						vector4 *= 0.8f;
						dust2.position = vector2 + vector4 * vector3;
						dust2.velocity = vector4 + new Vector2(RunSpeed - (float)Math.Sign(velocity.Length()), 0f);
						if (velocity.X > 0f)
						{
							dust2.velocity.X *= -1f;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust2.velocity *= 0.5f;
						}
						dust2.noGravity = true;
						dust2.scale = 1.5f + Main.rand.NextFloat() * 0.8f;
						dust2.fadeIn = 0f;
						dust2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
				}
				if (flag)
				{
					int num15 = Utils.SelectRandom(Main.rand, new short[3] { 6, 6, 31 });
					int num16 = 6;
					Dust dust3 = Main.dust[Dust.NewDust(mountedPlayer.Center - new Vector2(num16, num16 - 12), num16 * 2, num16 * 2, num15)];
					dust3.velocity = mountedPlayer.velocity * 0.1f;
					if (Main.rand.Next(2) == 0)
					{
						dust3.noGravity = true;
					}
					dust3.scale = 0.7f + Main.rand.NextFloat() * 0.8f;
					if (Main.rand.Next(3) == 0)
					{
						dust3.fadeIn = 0.1f;
					}
					if (num15 == 31)
					{
						dust3.noGravity = true;
						dust3.scale *= 1.5f;
						dust3.fadeIn = 0.2f;
					}
					dust3.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				break;
			}
			case 48:
				state = 1;
				break;
			case 39:
				_frameExtraCounter += 1f;
				if (_frameExtraCounter > 6f)
				{
					_frameExtraCounter = 0f;
					_frameExtra++;
					if (_frameExtra > 5)
					{
						_frameExtra = 0;
					}
				}
				break;
			case 50:
				if (mountedPlayer.velocity.Y == 0f)
				{
					_frameExtraCounter = 0f;
					_frameExtra = 3;
					break;
				}
				_frameExtraCounter += 1f;
				if (Flight())
				{
					_frameExtraCounter += 1f;
				}
				if (_frameExtraCounter > 7f)
				{
					_frameExtraCounter = 0f;
					_frameExtra++;
					if (_frameExtra > 3)
					{
						_frameExtra = 0;
					}
				}
				break;
			case 14:
			{
				bool num = Math.Abs(velocity.X) > RunSpeed / 2f;
				float num2 = Math.Sign(mountedPlayer.velocity.X);
				float num3 = 12f;
				float num4 = 40f;
				if (!num)
				{
					mountedPlayer.basiliskCharge = 0f;
				}
				else
				{
					mountedPlayer.basiliskCharge = Utils.Clamp(mountedPlayer.basiliskCharge + 1f / 180f, 0f, 1f);
				}
				if ((double)mountedPlayer.position.Y > Main.worldSurface * 16.0 + 160.0)
				{
					Lighting.AddLight(mountedPlayer.Center, 0.5f, 0.1f, 0.1f);
				}
				if (num && velocity.Y == 0f)
				{
					for (int i = 0; i < 2; i++)
					{
						Dust obj = Main.dust[Dust.NewDust(mountedPlayer.BottomLeft, mountedPlayer.width, 6, 31)];
						obj.velocity = new Vector2(velocity.X * 0.15f, Main.rand.NextFloat() * -2f);
						obj.noLight = true;
						obj.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
						obj.fadeIn = 0.5f + Main.rand.NextFloat() * 1f;
						obj.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
					if (mountedPlayer.cMount == 0)
					{
						mountedPlayer.position += new Vector2(num2 * 24f, 0f);
						mountedPlayer.FloorVisuals(Falling: true);
						mountedPlayer.position -= new Vector2(num2 * 24f, 0f);
					}
				}
				if (num2 != (float)mountedPlayer.direction)
				{
					break;
				}
				for (int j = 0; j < (int)(3f * mountedPlayer.basiliskCharge); j++)
				{
					Dust dust = Main.dust[Dust.NewDust(mountedPlayer.BottomLeft, mountedPlayer.width, 6, 6)];
					Vector2 vector = mountedPlayer.Center + new Vector2(num2 * num4, num3);
					dust.position = mountedPlayer.Center + new Vector2(num2 * (num4 - 2f), num3 - 6f + Main.rand.NextFloat() * 12f);
					dust.velocity = (dust.position - vector).SafeNormalize(Vector2.Zero) * (3.5f + Main.rand.NextFloat() * 0.5f);
					if (dust.velocity.Y < 0f)
					{
						dust.velocity.Y *= 1f + 2f * Main.rand.NextFloat();
					}
					dust.velocity += mountedPlayer.velocity * 0.55f;
					dust.velocity *= mountedPlayer.velocity.Length() / RunSpeed;
					dust.velocity *= mountedPlayer.basiliskCharge;
					dust.noGravity = true;
					dust.noLight = true;
					dust.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
					dust.fadeIn = 0.5f + Main.rand.NextFloat() * 1f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				break;
			}
			}
			switch (state)
			{
			case 0:
				if (_data.idleFrameCount != 0)
				{
					if (_type == 5)
					{
						if (_fatigue != 0f)
						{
							if (_idleTime == 0)
							{
								_idleTimeNext = _idleTime + 1;
							}
						}
						else
						{
							_idleTime = 0;
							_idleTimeNext = 2;
						}
					}
					else if (_idleTime == 0)
					{
						_idleTimeNext = Main.rand.Next(900, 1500);
						if (_type == 17)
						{
							_idleTimeNext = Main.rand.Next(120, 300);
						}
					}
					_idleTime++;
				}
				_frameCounter += 1f;
				if (_data.idleFrameCount != 0 && _idleTime >= _idleTimeNext)
				{
					float num29 = _data.idleFrameDelay;
					if (_type == 5)
					{
						num29 *= 2f - 1f * _fatigue / _fatigueMax;
					}
					int num30 = (int)((float)(_idleTime - _idleTimeNext) / num29);
					int idleFrameCount = _data.idleFrameCount;
					if (num30 >= idleFrameCount)
					{
						if (_data.idleFrameLoop)
						{
							_idleTime = _idleTimeNext;
							_frame = _data.idleFrameStart;
						}
						else
						{
							_frameCounter = 0f;
							_frame = _data.standingFrameStart;
							_idleTime = 0;
						}
					}
					else
					{
						_frame = _data.idleFrameStart + num30;
						if (_data.idleFrameLoop)
						{
							if (_frame < _data.idleFrameStart || _frame >= _data.idleFrameStart + _data.idleFrameCount)
							{
								_frame = _data.idleFrameStart;
							}
						}
						else if (_frame < _data.standingFrameStart || _frame >= _data.standingFrameStart + _data.standingFrameCount)
						{
							_frame = _data.standingFrameStart;
						}
					}
					if (_type == 5)
					{
						_frameExtra = _frame;
					}
					if (_type == 17)
					{
						_frameExtra = _frame;
						_frame = 0;
					}
				}
				else
				{
					if (_frameCounter > (float)_data.standingFrameDelay)
					{
						_frameCounter -= _data.standingFrameDelay;
						_frame++;
					}
					if (_frame < _data.standingFrameStart || _frame >= _data.standingFrameStart + _data.standingFrameCount)
					{
						_frame = _data.standingFrameStart;
					}
				}
				break;
			case 1:
			{
				float num26;
				switch (_type)
				{
				case 9:
				case 46:
					num26 = ((!_flipDraw) ? Math.Abs(velocity.X) : (0f - Math.Abs(velocity.X)));
					break;
				case 44:
					num26 = Math.Max(1f, Math.Abs(velocity.X) * 0.25f);
					break;
				case 48:
					num26 = Math.Max(0.5f, velocity.Length() * 0.125f);
					break;
				case 6:
					num26 = (_flipDraw ? velocity.X : (0f - velocity.X));
					break;
				case 13:
					num26 = (_flipDraw ? velocity.X : (0f - velocity.X));
					break;
				case 50:
					num26 = Math.Abs(velocity.X) * 0.5f;
					break;
				default:
					num26 = Math.Abs(velocity.X);
					break;
				}
				_frameCounter += num26;
				if (num26 >= 0f)
				{
					if (_frameCounter > (float)_data.runningFrameDelay)
					{
						_frameCounter -= _data.runningFrameDelay;
						_frame++;
					}
					if (_frame < _data.runningFrameStart || _frame >= _data.runningFrameStart + _data.runningFrameCount)
					{
						_frame = _data.runningFrameStart;
					}
				}
				else
				{
					if (_frameCounter < 0f)
					{
						_frameCounter += _data.runningFrameDelay;
						_frame--;
					}
					if (_frame < _data.runningFrameStart || _frame >= _data.runningFrameStart + _data.runningFrameCount)
					{
						_frame = _data.runningFrameStart + _data.runningFrameCount - 1;
					}
				}
				break;
			}
			case 3:
				_frameCounter += 1f;
				if (_frameCounter > (float)_data.flyingFrameDelay)
				{
					_frameCounter -= _data.flyingFrameDelay;
					_frame++;
				}
				if (_frame < _data.flyingFrameStart || _frame >= _data.flyingFrameStart + _data.flyingFrameCount)
				{
					_frame = _data.flyingFrameStart;
				}
				break;
			case 2:
				_frameCounter += 1f;
				if (_frameCounter > (float)_data.inAirFrameDelay)
				{
					_frameCounter -= _data.inAirFrameDelay;
					_frame++;
				}
				if (_frame < _data.inAirFrameStart || _frame >= _data.inAirFrameStart + _data.inAirFrameCount)
				{
					_frame = _data.inAirFrameStart;
				}
				if (_type == 4)
				{
					if (velocity.Y < 0f)
					{
						_frame = 3;
					}
					else
					{
						_frame = 6;
					}
				}
				else if (_type == 52)
				{
					if (velocity.Y < 0f)
					{
						_frame = _data.inAirFrameStart;
					}
					if (_frame == _data.inAirFrameStart + _data.inAirFrameCount - 1)
					{
						_frameCounter = 0f;
					}
				}
				else if (_type == 5)
				{
					float num27 = _fatigue / _fatigueMax;
					_frameExtraCounter += 6f - 4f * num27;
					if (_frameExtraCounter > (float)_data.flyingFrameDelay)
					{
						_frameExtra++;
						_frameExtraCounter -= _data.flyingFrameDelay;
					}
					if (_frameExtra < _data.flyingFrameStart || _frameExtra >= _data.flyingFrameStart + _data.flyingFrameCount)
					{
						_frameExtra = _data.flyingFrameStart;
					}
				}
				else if (_type == 23)
				{
					float num28 = mountedPlayer.velocity.Length();
					if (num28 < 1f)
					{
						_frame = 0;
						_frameCounter = 0f;
					}
					else if (num28 > 5f)
					{
						_frameCounter += 1f;
					}
				}
				break;
			case 4:
			{
				int num31 = (int)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) / 2f);
				_frameCounter += num31;
				if (_frameCounter > (float)_data.swimFrameDelay)
				{
					_frameCounter -= _data.swimFrameDelay;
					_frame++;
				}
				if (_frame < _data.swimFrameStart || _frame >= _data.swimFrameStart + _data.swimFrameCount)
				{
					_frame = _data.swimFrameStart;
				}
				if (Type == 37 && velocity.X == 0f)
				{
					_frame = 4;
				}
				break;
			}
			case 5:
			{
				float num26 = _type switch
				{
					9 => (!_flipDraw) ? Math.Abs(velocity.X) : (0f - Math.Abs(velocity.X)), 
					6 => _flipDraw ? velocity.X : (0f - velocity.X), 
					13 => _flipDraw ? velocity.X : (0f - velocity.X), 
					_ => Math.Abs(velocity.X), 
				};
				_frameCounter += num26;
				if (num26 >= 0f)
				{
					if (_frameCounter > (float)_data.dashingFrameDelay)
					{
						_frameCounter -= _data.dashingFrameDelay;
						_frame++;
					}
					if (_frame < _data.dashingFrameStart || _frame >= _data.dashingFrameStart + _data.dashingFrameCount)
					{
						_frame = _data.dashingFrameStart;
					}
				}
				else
				{
					if (_frameCounter < 0f)
					{
						_frameCounter += _data.dashingFrameDelay;
						_frame--;
					}
					if (_frame < _data.dashingFrameStart || _frame >= _data.dashingFrameStart + _data.dashingFrameCount)
					{
						_frame = _data.dashingFrameStart + _data.dashingFrameCount - 1;
					}
				}
				break;
			}
			}
		}

		public void TryBeginningFlight(Player mountedPlayer, int state)
		{
			if (_frameState == state || (state != 2 && state != 3) || !CanHover() || mountedPlayer.controlUp || mountedPlayer.controlDown || mountedPlayer.controlJump)
			{
				return;
			}
			Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir;
			if (Collision.TileCollision(mountedPlayer.position + new Vector2(0f, -0.001f), velocity, mountedPlayer.width, mountedPlayer.height, fallThrough: false, fall2: false, (int)mountedPlayer.gravDir).Y != 0f)
			{
				if (DoesHoverIgnoresFatigue())
				{
					mountedPlayer.position.Y += -0.001f;
					return;
				}
				float num = mountedPlayer.gravity * mountedPlayer.gravDir;
				mountedPlayer.position.Y -= mountedPlayer.velocity.Y;
				mountedPlayer.velocity.Y -= num;
			}
		}

		public int GetIntendedGroundedFrame(Player mountedPlayer)
		{
			if (mountedPlayer.velocity.X == 0f || ((mountedPlayer.slippy || mountedPlayer.slippy2 || mountedPlayer.windPushed) && !mountedPlayer.controlLeft && !mountedPlayer.controlRight))
			{
				return 0;
			}
			return 1;
		}

		public void TryLanding(Player mountedPlayer)
		{
			if ((_frameState == 3 || _frameState == 2) && !mountedPlayer.controlUp && !mountedPlayer.controlDown && !mountedPlayer.controlJump)
			{
				Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir * 4f;
				if (Collision.TileCollision(mountedPlayer.position, velocity, mountedPlayer.width, mountedPlayer.height, fallThrough: false, fall2: false, (int)mountedPlayer.gravDir).Y == 0f)
				{
					UpdateFrame(mountedPlayer, GetIntendedGroundedFrame(mountedPlayer), mountedPlayer.velocity);
				}
			}
		}

		private void UpdateFrame_GolfCart(Player mountedPlayer, int state, Vector2 velocity)
		{
			if (state != 2)
			{
				if (_frameExtraCounter != 0f || _frameExtra != 0)
				{
					if (_frameExtraCounter == -1f)
					{
						_frameExtraCounter = 0f;
						_frameExtra = 1;
					}
					if ((_frameExtraCounter += 1f) >= 6f)
					{
						_frameExtraCounter = 0f;
						if (_frameExtra > 0)
						{
							_frameExtra--;
						}
					}
				}
				else
				{
					_frameExtra = 0;
					_frameExtraCounter = 0f;
				}
			}
			else if (velocity.Y >= 0f)
			{
				if (_frameExtra < 1)
				{
					_frameExtra = 1;
				}
				if (_frameExtra == 2)
				{
					_frameExtraCounter = -1f;
				}
				else if ((_frameExtraCounter += 1f) >= 6f)
				{
					_frameExtraCounter = 0f;
					if (_frameExtra < 2)
					{
						_frameExtra++;
					}
				}
			}
			if (state != 2 && state != 0 && state != 3 && state != 4)
			{
				EmitGolfCartWheelDust(mountedPlayer, mountedPlayer.Bottom + new Vector2(mountedPlayer.direction * -20, 0f));
				EmitGolfCartWheelDust(mountedPlayer, mountedPlayer.Bottom + new Vector2(mountedPlayer.direction * 20, 0f));
			}
			EmitGolfCartlight(mountedPlayer.Bottom + new Vector2(mountedPlayer.direction * 40, -20f), mountedPlayer.direction);
		}

		private static void EmitGolfCartSmoke(Player mountedPlayer, bool rushing)
		{
			Vector2 position = mountedPlayer.Bottom + new Vector2(-mountedPlayer.direction * 34, (0f - mountedPlayer.gravDir) * 12f);
			Dust dust = Dust.NewDustDirect(position, 0, 0, 31, -mountedPlayer.direction, (0f - mountedPlayer.gravDir) * 0.24f, 100);
			dust.position = position;
			dust.velocity *= 0.1f;
			dust.velocity += new Vector2(-mountedPlayer.direction, (0f - mountedPlayer.gravDir) * 0.25f);
			dust.scale = 0.5f;
			if (mountedPlayer.velocity.X != 0f)
			{
				dust.velocity += new Vector2((float)Math.Sign(mountedPlayer.velocity.X) * 1.3f, 0f);
			}
			if (rushing)
			{
				dust.fadeIn = 0.8f;
			}
		}

		private static void EmitGolfCartlight(Vector2 worldLocation, int playerDirection)
		{
			float num = 0f;
			if (playerDirection == -1)
			{
				num = MathF.PI;
			}
			float num2 = MathF.PI / 32f;
			int num3 = 5;
			float num4 = 200f;
			DelegateMethods.v2_1 = worldLocation.ToTileCoordinates().ToVector2();
			DelegateMethods.f_1 = num4 / 16f;
			DelegateMethods.v3_1 = new Vector3(0.7f, 0.7f, 0.7f);
			for (float num5 = 0f; num5 < (float)num3; num5 += 1f)
			{
				Vector2 vector = (num + num2 * (num5 - (float)(num3 / 2))).ToRotationVector2();
				Utils.PlotTileLine(worldLocation, worldLocation + vector * num4, 8f, DelegateMethods.CastLightOpen_StopForSolids_ScaleWithDistance);
			}
		}

		private static bool ShouldGolfCartEmitLight()
		{
			return true;
		}

		private static void EmitGolfCartWheelDust(Player mountedPlayer, Vector2 legSpot)
		{
			if (Main.rand.Next(5) != 0)
			{
				return;
			}
			Point p = (legSpot + new Vector2(0f, mountedPlayer.gravDir * 2f)).ToTileCoordinates();
			if (!WorldGen.InWorld(p.X, p.Y, 10))
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(p.X, p.Y);
			if (WorldGen.SolidTile(p))
			{
				int num = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely);
				if (num > 1)
				{
					num = 1;
				}
				Vector2 vector = new Vector2(-mountedPlayer.direction, (0f - mountedPlayer.gravDir) * 1f);
				for (int i = 0; i < num; i++)
				{
					Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(p.X, p.Y, tileSafely)];
					obj.velocity *= 0.2f;
					obj.velocity += vector;
					obj.position = legSpot;
					obj.scale *= 0.8f;
					obj.fadeIn *= 0.8f;
				}
			}
		}

		private void DoGemMinecartEffect(Player mountedPlayer, int dustType)
		{
			if (Main.rand.Next(10) == 0)
			{
				Vector2 vector = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(22f, 10f);
				Vector2 vector2 = new Vector2(0f, 10f) * mountedPlayer.Directions;
				Vector2 pos = mountedPlayer.Center + vector2 + vector;
				pos = mountedPlayer.RotatedRelativePoint(pos);
				Dust dust = Dust.NewDustPerfect(pos, dustType);
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 0.4f;
				dust.velocity *= 0.25f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
		}

		private void DoSteamMinecartEffect(Player mountedPlayer, int dustType)
		{
			float num = Math.Abs(mountedPlayer.velocity.X);
			if (!(num < 1f) && (!(num < 6f) || _frame == 0))
			{
				Vector2 vector = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 3f);
				Vector2 vector2 = new Vector2(-10f, -4f) * mountedPlayer.Directions;
				Vector2 pos = mountedPlayer.Center + vector2 + vector;
				pos = mountedPlayer.RotatedRelativePoint(pos);
				Dust dust = Dust.NewDustPerfect(pos, dustType);
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 1.8f;
				dust.velocity *= 0.25f;
				dust.velocity.Y -= 2f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
		}

		private void DoExhaustMinecartEffect(Player mountedPlayer, int dustType)
		{
			float num = mountedPlayer.velocity.Length();
			if (num < 1f && Main.rand.Next(4) != 0)
			{
				return;
			}
			int num2 = 1 + (int)num / 6;
			while (num2 > 0)
			{
				num2--;
				Vector2 vector = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 3f);
				Vector2 vector2 = new Vector2(-18f, 20f) * mountedPlayer.Directions;
				if (num > 6f)
				{
					vector2.X += 4 * mountedPlayer.direction;
				}
				if (num2 > 0)
				{
					vector2 += mountedPlayer.velocity * (num2 / 3);
				}
				Vector2 pos = mountedPlayer.Center + vector2 + vector;
				pos = mountedPlayer.RotatedRelativePoint(pos);
				Dust dust = Dust.NewDustPerfect(pos, dustType);
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 1.2f;
				dust.velocity *= 0.2f;
				if (num < 1f)
				{
					dust.velocity.X -= 0.5f * (float)mountedPlayer.direction;
				}
				dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
		}

		private void DoConfettiMinecartEffect(Player mountedPlayer)
		{
			float num = mountedPlayer.velocity.Length();
			if ((num < 1f && Main.rand.Next(6) != 0) || (num < 3f && Main.rand.Next(3) != 0))
			{
				return;
			}
			int num2 = 1 + (int)num / 6;
			while (num2 > 0)
			{
				num2--;
				float num3 = Main.rand.NextFloat() * 2f;
				Vector2 vector = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 8f);
				Vector2 vector2 = new Vector2(-18f, 4f) * mountedPlayer.Directions;
				vector2.X += num * (float)mountedPlayer.direction * 0.5f + (float)(mountedPlayer.direction * num2) * num3;
				if (num2 > 0)
				{
					vector2 += mountedPlayer.velocity * (num2 / 3);
				}
				Vector2 pos = mountedPlayer.Center + vector2 + vector;
				pos = mountedPlayer.RotatedRelativePoint(pos);
				Dust dust = Dust.NewDustPerfect(pos, 139 + Main.rand.Next(4));
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 0.5f + num3 / 2f;
				dust.velocity *= 0.2f;
				if (num < 1f)
				{
					dust.velocity.X -= 0.5f * (float)mountedPlayer.direction;
				}
				dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
		}

		public void UpdateEffects(Player mountedPlayer)
		{
			mountedPlayer.autoJump = AutoJump;
			_shouldSuperCart = MountID.Sets.Cart[_type] && mountedPlayer.UsingSuperCart;
			if (_shouldSuperCart)
			{
				CastSuperCartLaser(mountedPlayer);
				float num = 1f + Math.Abs(mountedPlayer.velocity.X) / RunSpeed * 2.5f;
				mountedPlayer.statDefense += (int)(2f * num);
			}
			switch (_type)
			{
			case 23:
			{
				Vector2 pos3 = mountedPlayer.Center + GetWitchBroomTrinketOriginOffset(mountedPlayer) + (GetWitchBroomTrinketRotation(mountedPlayer) + MathF.PI / 2f).ToRotationVector2() * 11f;
				Vector3 rgb = new Vector3(1f, 0.75f, 0.5f) * 0.85f;
				Vector2 vector11 = mountedPlayer.RotatedRelativePoint(pos3);
				Lighting.AddLight(vector11, rgb);
				if (Main.rand.Next(45) == 0)
				{
					Vector2 vector12 = Main.rand.NextVector2Circular(4f, 4f);
					Dust dust2 = Dust.NewDustPerfect(vector11 + vector12, 43, Vector2.Zero, 254, new Color(255, 255, 0, 255), 0.3f);
					if (vector12 != Vector2.Zero)
					{
						dust2.velocity = vector11.DirectionTo(dust2.position) * 0.2f;
					}
					dust2.fadeIn = 0.3f;
					dust2.noLightEmittence = true;
					dust2.customData = mountedPlayer;
					dust2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				float num13 = 0.1f;
				num13 += mountedPlayer.velocity.Length() / 30f;
				Vector2 pos4 = mountedPlayer.Center + new Vector2(18f - 20f * Main.rand.NextFloat() * (float)mountedPlayer.direction, 12f);
				Vector2 pos5 = mountedPlayer.Center + new Vector2(52 * mountedPlayer.direction, -6f);
				pos4 = mountedPlayer.RotatedRelativePoint(pos4);
				pos5 = mountedPlayer.RotatedRelativePoint(pos5);
				if (!(Main.rand.NextFloat() <= num13))
				{
					break;
				}
				float num14 = Main.rand.NextFloat();
				for (float num15 = 0f; num15 < 1f; num15 += 0.125f)
				{
					if (Main.rand.Next(15) == 0)
					{
						Vector2 spinningpoint = (MathF.PI * 2f * num15 + num14).ToRotationVector2() * new Vector2(0.5f, 1f) * 4f;
						spinningpoint = spinningpoint.RotatedBy(mountedPlayer.fullRotation);
						Dust dust3 = Dust.NewDustPerfect(pos4 + spinningpoint, 43, Vector2.Zero, 254, new Color(255, 255, 0, 255), 0.3f);
						dust3.velocity = spinningpoint * 0.025f + pos5.DirectionTo(dust3.position) * 0.5f;
						dust3.fadeIn = 0.3f;
						dust3.noLightEmittence = true;
						dust3.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
				}
				break;
			}
			case 25:
				DoGemMinecartEffect(mountedPlayer, 86);
				break;
			case 26:
				DoGemMinecartEffect(mountedPlayer, 87);
				break;
			case 27:
				DoGemMinecartEffect(mountedPlayer, 88);
				break;
			case 28:
				DoGemMinecartEffect(mountedPlayer, 89);
				break;
			case 29:
				DoGemMinecartEffect(mountedPlayer, 90);
				break;
			case 30:
				DoGemMinecartEffect(mountedPlayer, 91);
				break;
			case 31:
				DoGemMinecartEffect(mountedPlayer, 262);
				break;
			case 9:
			case 46:
			{
				if (_type == 46)
				{
					mountedPlayer.hasJumpOption_Santank = true;
				}
				Vector2 center = mountedPlayer.Center;
				Vector2 vector10 = center;
				bool flag = false;
				float num9 = 1500f;
				float num10 = 850f;
				for (int i = 0; i < 200; i++)
				{
					NPC nPC = Main.npc[i];
					if (!nPC.CanBeChasedBy(this))
					{
						continue;
					}
					Vector2 v = nPC.Center - center;
					float num11 = v.Length();
					if (num11 < num10 && ((Vector2.Distance(vector10, center) > num11 && num11 < num9) || !flag))
					{
						bool flag2 = true;
						float num12 = Math.Abs(v.ToRotation());
						if (mountedPlayer.direction == 1 && (double)num12 > 1.047197594907988)
						{
							flag2 = false;
						}
						else if (mountedPlayer.direction == -1 && (double)num12 < 2.0943951461045853)
						{
							flag2 = false;
						}
						if (Collision.CanHitLine(center, 0, 0, nPC.position, nPC.width, nPC.height) && flag2)
						{
							num9 = num11;
							vector10 = nPC.Center;
							flag = true;
						}
					}
				}
				if (flag)
				{
					bool flag3 = _abilityCooldown == 0;
					if (_type == 46)
					{
						flag3 = _abilityCooldown % 10 == 0;
					}
					if (flag3 && mountedPlayer.whoAmI == Main.myPlayer)
					{
						AimAbility(mountedPlayer, vector10);
						if (_abilityCooldown == 0)
						{
							StopAbilityCharge();
						}
						UseAbility(mountedPlayer, vector10, toggleOn: false);
					}
					else
					{
						AimAbility(mountedPlayer, vector10);
						_abilityCharging = true;
					}
				}
				else
				{
					_abilityCharging = false;
					ResetHeadPosition();
				}
				break;
			}
			case 10:
				mountedPlayer.hasJumpOption_Unicorn = true;
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
				}
				if (mountedPlayer.itemAnimation > 0 && mountedPlayer.inventory[mountedPlayer.selectedItem].type == 1260)
				{
					AchievementsHelper.HandleSpecialEvent(mountedPlayer, 5);
				}
				break;
			case 47:
				mountedPlayer.hasJumpOption_WallOfFleshGoat = true;
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
				}
				break;
			case 14:
				mountedPlayer.hasJumpOption_Basilisk = true;
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
				}
				break;
			case 40:
			case 41:
			case 42:
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
				}
				break;
			case 12:
				if (mountedPlayer.MountFishronSpecial)
				{
					Vector3 vector9 = Colors.CurrentLiquidColor.ToVector3();
					vector9 *= 0.4f;
					Point point = (mountedPlayer.Center + Vector2.UnitX * mountedPlayer.direction * 20f + mountedPlayer.velocity * 10f).ToTileCoordinates();
					if (!WorldGen.SolidTile(point.X, point.Y))
					{
						Lighting.AddLight(point.X, point.Y, vector9.X, vector9.Y, vector9.Z);
					}
					else
					{
						Lighting.AddLight(mountedPlayer.Center + Vector2.UnitX * mountedPlayer.direction * 20f, vector9.X, vector9.Y, vector9.Z);
					}
					mountedPlayer.meleeDamage += 0.15f;
					mountedPlayer.rangedDamage += 0.15f;
					mountedPlayer.magicDamage += 0.15f;
					mountedPlayer.minionDamage += 0.15f;
				}
				if (mountedPlayer.statLife <= mountedPlayer.statLifeMax2 / 2)
				{
					mountedPlayer.MountFishronSpecialCounter = 60f;
				}
				if (mountedPlayer.wet || (Main.raining && WorldGen.InAPlaceWithWind(mountedPlayer.position, mountedPlayer.width, mountedPlayer.height)))
				{
					mountedPlayer.MountFishronSpecialCounter = 420f;
				}
				break;
			case 8:
				if (mountedPlayer.ownedProjectileCounts[453] < 1)
				{
					_abilityActive = false;
				}
				break;
			case 11:
			{
				Vector3 vector4 = new Vector3(0.4f, 0.12f, 0.15f);
				float num4 = 1f + Math.Abs(mountedPlayer.velocity.X) / RunSpeed * 2.5f;
				int num5 = Math.Sign(mountedPlayer.velocity.X);
				if (num5 == 0)
				{
					num5 = mountedPlayer.direction;
				}
				if (Main.netMode == 2)
				{
					break;
				}
				vector4 *= num4;
				Lighting.AddLight(mountedPlayer.Center, vector4.X, vector4.Y, vector4.Z);
				Lighting.AddLight(mountedPlayer.Top, vector4.X, vector4.Y, vector4.Z);
				Lighting.AddLight(mountedPlayer.Bottom, vector4.X, vector4.Y, vector4.Z);
				Lighting.AddLight(mountedPlayer.Left, vector4.X, vector4.Y, vector4.Z);
				Lighting.AddLight(mountedPlayer.Right, vector4.X, vector4.Y, vector4.Z);
				float num6 = -24f;
				if (mountedPlayer.direction != num5)
				{
					num6 = -22f;
				}
				if (num5 == -1)
				{
					num6 += 1f;
				}
				Vector2 vector5 = new Vector2(num6 * (float)num5, -19f).RotatedBy(mountedPlayer.fullRotation);
				Vector2 vector6 = new Vector2(MathHelper.Lerp(0f, -8f, mountedPlayer.fullRotation / (MathF.PI / 4f)), MathHelper.Lerp(0f, 2f, Math.Abs(mountedPlayer.fullRotation / (MathF.PI / 4f)))).RotatedBy(mountedPlayer.fullRotation);
				if (num5 == Math.Sign(mountedPlayer.fullRotation))
				{
					vector6 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(mountedPlayer.fullRotation / (MathF.PI / 4f)));
				}
				Vector2 vector7 = mountedPlayer.Bottom + vector5 + vector6;
				Vector2 vector8 = mountedPlayer.oldPosition + mountedPlayer.Size * new Vector2(0.5f, 1f) + vector5 + vector6;
				if (Vector2.Distance(vector7, vector8) > 3f)
				{
					int num7 = (int)Vector2.Distance(vector7, vector8) / 3;
					if (Vector2.Distance(vector7, vector8) % 3f != 0f)
					{
						num7++;
					}
					for (float num8 = 1f; num8 <= (float)num7; num8 += 1f)
					{
						Dust obj = Main.dust[Dust.NewDust(mountedPlayer.Center, 0, 0, 182)];
						obj.position = Vector2.Lerp(vector8, vector7, num8 / (float)num7);
						obj.noGravity = true;
						obj.velocity = Vector2.Zero;
						obj.customData = mountedPlayer;
						obj.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
					}
				}
				else
				{
					Dust obj2 = Main.dust[Dust.NewDust(mountedPlayer.Center, 0, 0, 182)];
					obj2.position = vector7;
					obj2.noGravity = true;
					obj2.velocity = Vector2.Zero;
					obj2.customData = mountedPlayer;
					obj2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
				}
				break;
			}
			case 22:
			{
				mountedPlayer.lavaMax += 420;
				Vector2 vector = mountedPlayer.Center + new Vector2(20f, 10f) * mountedPlayer.Directions;
				Vector2 pos = vector + mountedPlayer.velocity;
				Vector2 pos2 = vector + new Vector2(-1f, -0.5f) * mountedPlayer.Directions;
				vector = mountedPlayer.RotatedRelativePoint(vector);
				pos = mountedPlayer.RotatedRelativePoint(pos);
				pos2 = mountedPlayer.RotatedRelativePoint(pos2);
				Vector2 value = mountedPlayer.shadowPos[2] - mountedPlayer.position + vector;
				Vector2 vector2 = pos - vector;
				vector += vector2;
				value += vector2;
				Vector2 vector3 = pos - pos2;
				float num2 = MathHelper.Clamp(mountedPlayer.velocity.Length() / 5f, 0f, 1f);
				for (float num3 = 0f; num3 <= 1f; num3 += 0.1f)
				{
					if (!(Main.rand.NextFloat() < num2))
					{
						Dust dust = Dust.NewDustPerfect(Vector2.Lerp(value, vector, num3), 65, Main.rand.NextVector2Circular(0.5f, 0.5f) * num2);
						dust.scale = 0.6f;
						dust.fadeIn = 0f;
						dust.customData = mountedPlayer;
						dust.velocity *= -1f;
						dust.noGravity = true;
						dust.velocity -= vector3;
						dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
						if (Main.rand.Next(10) == 0)
						{
							dust.fadeIn = 1.3f;
							dust.velocity = Main.rand.NextVector2Circular(3f, 3f) * num2;
						}
					}
				}
				break;
			}
			case 16:
				mountedPlayer.ignoreWater = true;
				break;
			case 24:
				DelegateMethods.v3_1 = new Vector3(0.1f, 0.3f, 1f) * 0.4f;
				Utils.PlotTileLine(mountedPlayer.MountedCenter, mountedPlayer.MountedCenter + mountedPlayer.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
				Utils.PlotTileLine(mountedPlayer.Left, mountedPlayer.Right, 40f, DelegateMethods.CastLightOpen);
				break;
			case 36:
				DoSteamMinecartEffect(mountedPlayer, 303);
				break;
			case 32:
				DoExhaustMinecartEffect(mountedPlayer, 31);
				break;
			case 34:
				DoConfettiMinecartEffect(mountedPlayer);
				break;
			case 37:
				mountedPlayer.canFloatInWater = true;
				mountedPlayer.accFlipper = true;
				break;
			case 13:
			case 15:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 33:
			case 35:
			case 38:
			case 39:
			case 43:
			case 44:
			case 45:
				break;
			}
		}

		private void CastSuperCartLaser(Player mountedPlayer)
		{
			int num = Math.Sign(mountedPlayer.velocity.X);
			if (num == 0)
			{
				num = mountedPlayer.direction;
			}
			if (mountedPlayer.whoAmI != Main.myPlayer || mountedPlayer.velocity.X == 0f)
			{
				return;
			}
			Vector2 minecartMechPoint = GetMinecartMechPoint(mountedPlayer, 20, -19);
			int damage = 60;
			int num2 = 0;
			float num3 = 0f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.immune[mountedPlayer.whoAmI] <= 0 && !nPC.dontTakeDamage && nPC.Distance(minecartMechPoint) < 300f && nPC.CanBeChasedBy(mountedPlayer) && Collision.CanHitLine(nPC.position, nPC.width, nPC.height, minecartMechPoint, 0, 0) && Math.Abs(MathHelper.WrapAngle(MathHelper.WrapAngle(nPC.AngleFrom(minecartMechPoint)) - MathHelper.WrapAngle((mountedPlayer.fullRotation + (float)num == -1f) ? MathF.PI : 0f))) < MathF.PI / 4f)
				{
					minecartMechPoint = GetMinecartMechPoint(mountedPlayer, -20, -39);
					Vector2 v = nPC.position + nPC.Size * Utils.RandomVector2(Main.rand, 0f, 1f) - minecartMechPoint;
					num3 += v.ToRotation();
					num2++;
					int num4 = Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), minecartMechPoint.X, minecartMechPoint.Y, v.X, v.Y, 591, 0, 0f, mountedPlayer.whoAmI, mountedPlayer.whoAmI);
					Main.projectile[num4].Center = nPC.Center;
					Main.projectile[num4].damage = damage;
					Main.projectile[num4].Damage();
					Main.projectile[num4].damage = 0;
					Main.projectile[num4].Center = minecartMechPoint;
				}
			}
		}

		public static Vector2 GetMinecartMechPoint(Player mountedPlayer, int offX, int offY)
		{
			int num = Math.Sign(mountedPlayer.velocity.X);
			if (num == 0)
			{
				num = mountedPlayer.direction;
			}
			float num2 = offX;
			int num3 = Math.Sign(offX);
			if (mountedPlayer.direction != num)
			{
				num2 -= (float)num3;
			}
			if (num == -1)
			{
				num2 -= (float)num3;
			}
			Vector2 vector = new Vector2(num2 * (float)num, offY).RotatedBy(mountedPlayer.fullRotation);
			Vector2 vector2 = new Vector2(MathHelper.Lerp(0f, -8f, mountedPlayer.fullRotation / (MathF.PI / 4f)), MathHelper.Lerp(0f, 2f, Math.Abs(mountedPlayer.fullRotation / (MathF.PI / 4f)))).RotatedBy(mountedPlayer.fullRotation);
			if (num == Math.Sign(mountedPlayer.fullRotation))
			{
				vector2 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(mountedPlayer.fullRotation / (MathF.PI / 4f)));
			}
			return mountedPlayer.Bottom + vector + vector2;
		}

		public void ResetFlightTime(float xVelocity)
		{
			_flyTime = (_active ? _data.flightTimeMax : 0);
			if (_type == 0)
			{
				_flyTime += (int)(Math.Abs(xVelocity) * 20f);
			}
		}

		public void CheckMountBuff(Player mountedPlayer)
		{
			if (_type == -1)
			{
				return;
			}
			for (int i = 0; i < 44; i++)
			{
				if (mountedPlayer.buffType[i] == _data.buff || (Cart && mountedPlayer.buffType[i] == _data.extraBuff))
				{
					return;
				}
			}
			Dismount(mountedPlayer);
		}

		public void ResetHeadPosition()
		{
			if (_aiming)
			{
				_aiming = false;
				if (_type != 46)
				{
					_frameExtra = 0;
				}
				_flipDraw = false;
			}
		}

		private Vector2 ClampToDeadZone(Player mountedPlayer, Vector2 position)
		{
			int num;
			int num2;
			switch (_type)
			{
			case 9:
				num = (int)scutlixTextureSize.Y;
				num2 = (int)scutlixTextureSize.X;
				break;
			case 46:
				num = (int)santankTextureSize.Y;
				num2 = (int)santankTextureSize.X;
				break;
			case 8:
				num = (int)drillTextureSize.Y;
				num2 = (int)drillTextureSize.X;
				break;
			default:
				return position;
			}
			Vector2 center = mountedPlayer.Center;
			position -= center;
			if (position.X > (float)(-num2) && position.X < (float)num2 && position.Y > (float)(-num) && position.Y < (float)num)
			{
				float num3 = (float)num2 / Math.Abs(position.X);
				float num4 = (float)num / Math.Abs(position.Y);
				if (num3 > num4)
				{
					position *= num4;
				}
				else
				{
					position *= num3;
				}
			}
			return position + center;
		}

		public bool AimAbility(Player mountedPlayer, Vector2 mousePosition)
		{
			_aiming = true;
			switch (_type)
			{
			case 9:
			{
				int frameExtra = _frameExtra;
				int direction = mountedPlayer.direction;
				float num3 = MathHelper.ToDegrees((ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center).ToRotation());
				if (num3 > 90f)
				{
					mountedPlayer.direction = -1;
					num3 = 180f - num3;
				}
				else if (num3 < -90f)
				{
					mountedPlayer.direction = -1;
					num3 = -180f - num3;
				}
				else
				{
					mountedPlayer.direction = 1;
				}
				if ((mountedPlayer.direction > 0 && mountedPlayer.velocity.X < 0f) || (mountedPlayer.direction < 0 && mountedPlayer.velocity.X > 0f))
				{
					_flipDraw = true;
				}
				else
				{
					_flipDraw = false;
				}
				if (num3 >= 0f)
				{
					if ((double)num3 < 22.5)
					{
						_frameExtra = 8;
					}
					else if ((double)num3 < 67.5)
					{
						_frameExtra = 9;
					}
					else if ((double)num3 < 112.5)
					{
						_frameExtra = 10;
					}
				}
				else if ((double)num3 > -22.5)
				{
					_frameExtra = 8;
				}
				else if ((double)num3 > -67.5)
				{
					_frameExtra = 7;
				}
				else if ((double)num3 > -112.5)
				{
					_frameExtra = 6;
				}
				float abilityCharge = AbilityCharge;
				if (abilityCharge > 0f)
				{
					Vector2 vector = default(Vector2);
					vector.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
					vector.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
					int num4 = (_frameExtra - 6) * 2;
					Vector2 vector2 = default(Vector2);
					for (int i = 0; i < 2; i++)
					{
						vector2.Y = vector.Y + scutlixEyePositions[num4 + i].Y;
						if (mountedPlayer.direction == -1)
						{
							vector2.X = vector.X - scutlixEyePositions[num4 + i].X - (float)_data.xOffset;
						}
						else
						{
							vector2.X = vector.X + scutlixEyePositions[num4 + i].X + (float)_data.xOffset;
						}
						Lighting.AddLight((int)(vector2.X / 16f), (int)(vector2.Y / 16f), 1f * abilityCharge, 0f, 0f);
					}
				}
				if (_frameExtra == frameExtra)
				{
					return mountedPlayer.direction != direction;
				}
				return true;
			}
			case 46:
			{
				int frameExtra = _frameExtra;
				int direction = mountedPlayer.direction;
				float num3 = MathHelper.ToDegrees((ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center).ToRotation());
				if (num3 > 90f)
				{
					mountedPlayer.direction = -1;
					num3 = 180f - num3;
				}
				else if (num3 < -90f)
				{
					mountedPlayer.direction = -1;
					num3 = -180f - num3;
				}
				else
				{
					mountedPlayer.direction = 1;
				}
				if ((mountedPlayer.direction > 0 && mountedPlayer.velocity.X < 0f) || (mountedPlayer.direction < 0 && mountedPlayer.velocity.X > 0f))
				{
					_flipDraw = true;
				}
				else
				{
					_flipDraw = false;
				}
				float abilityCharge = AbilityCharge;
				if (abilityCharge > 0f)
				{
					Vector2 vector3 = default(Vector2);
					vector3.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
					vector3.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
					for (int j = 0; j < 2; j++)
					{
						Vector2 vector4 = new Vector2(vector3.X + (float)(mountedPlayer.width * mountedPlayer.direction), vector3.Y - 12f);
						Lighting.AddLight((int)(vector4.X / 16f), (int)(vector4.Y / 16f), 0.7f, 0.4f, 0.4f);
					}
				}
				if (_frameExtra == frameExtra)
				{
					return mountedPlayer.direction != direction;
				}
				return true;
			}
			case 8:
			{
				Vector2 v = ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center;
				DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
				float num = v.ToRotation();
				if (num < 0f)
				{
					num += MathF.PI * 2f;
				}
				drillMountData.diodeRotationTarget = num;
				float num2 = drillMountData.diodeRotation % (MathF.PI * 2f);
				if (num2 < 0f)
				{
					num2 += MathF.PI * 2f;
				}
				if (num2 < num)
				{
					if (num - num2 > MathF.PI)
					{
						num2 += MathF.PI * 2f;
					}
				}
				else if (num2 - num > MathF.PI)
				{
					num2 -= MathF.PI * 2f;
				}
				drillMountData.diodeRotation = num2;
				drillMountData.crosshairPosition = mousePosition;
				return true;
			}
			default:
				return false;
			}
		}

		public void Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, Vector2 Position, Color drawColor, SpriteEffects playerEffect, float shadow)
		{
			if (playerDrawData == null)
			{
				return;
			}
			Texture2D texture2D2;
			Texture2D texture2D;
			switch (drawType)
			{
			case 0:
				texture2D = _data.backTexture.get_Value();
				texture2D2 = _data.backTextureGlow.get_Value();
				break;
			case 1:
				texture2D = _data.backTextureExtra.get_Value();
				texture2D2 = _data.backTextureExtraGlow.get_Value();
				break;
			case 2:
				if (_type == 0 && _idleTime >= _idleTimeNext)
				{
					return;
				}
				texture2D = _data.frontTexture.get_Value();
				texture2D2 = _data.frontTextureGlow.get_Value();
				break;
			case 3:
				texture2D = _data.frontTextureExtra.get_Value();
				texture2D2 = _data.frontTextureExtraGlow.get_Value();
				break;
			default:
				texture2D = null;
				texture2D2 = null;
				break;
			}
			int type = _type;
			if (type == 50 && texture2D != null)
			{
				PlayerQueenSlimeMountTextureContent queenSlimeMount = TextureAssets.RenderTargets.QueenSlimeMount;
				queenSlimeMount.Request();
				if (queenSlimeMount.IsReady)
				{
					texture2D = queenSlimeMount.GetTarget();
				}
			}
			if (texture2D == null)
			{
				return;
			}
			type = _type;
			if ((type == 0 || type == 9) && drawType == 3 && shadow != 0f)
			{
				return;
			}
			int num = XOffset;
			int num2 = YOffset + PlayerOffset;
			if (drawPlayer.direction <= 0 && (!Cart || !Directional))
			{
				num *= -1;
			}
			Position.X = (int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) + (float)num);
			Position.Y = (int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + (float)num2);
			int num3 = 0;
			bool flag = true;
			int num4 = _data.totalFrames;
			int num5 = _data.textureHeight;
			switch (_type)
			{
			case 23:
				num3 = _frame;
				break;
			case 9:
				num3 = drawType switch
				{
					0 => _frame, 
					2 => _frameExtra, 
					3 => _frameExtra, 
					_ => 0, 
				};
				break;
			case 46:
				num3 = drawType switch
				{
					2 => _frame, 
					3 => _frameExtra, 
					_ => 0, 
				};
				break;
			case 5:
				num3 = drawType switch
				{
					0 => _frame, 
					1 => _frameExtra, 
					_ => 0, 
				};
				break;
			case 17:
				num5 = texture2D.Height;
				switch (drawType)
				{
				case 0:
					num3 = _frame;
					num4 = 4;
					break;
				case 1:
					num3 = _frameExtra;
					num4 = 4;
					break;
				default:
					num3 = 0;
					break;
				}
				break;
			case 52:
				if (drawType == 3)
				{
					if (drawPlayer.itemAnimation > 0)
					{
						Rectangle bodyFrame = drawPlayer.bodyFrame;
						int num6 = bodyFrame.Y / bodyFrame.Height;
						int useStyle = drawPlayer.lastVisualizedSelectedItem.useStyle;
						num3 = Utils.Clamp(num6, 1, 4);
						if (num3 == 3 || num6 == 0 || useStyle == 13)
						{
							num3 = _frame;
						}
						if (useStyle == 12 && drawPlayer.itemAnimation > drawPlayer.itemAnimationMax / 2)
						{
							num3 = 3;
						}
					}
					else
					{
						_ = drawPlayer.lastVisualizedSelectedItem.holdStyle;
						num3 = _frame;
					}
				}
				else
				{
					num3 = _frame;
				}
				break;
			case 39:
				num5 = texture2D.Height;
				switch (drawType)
				{
				case 2:
					num3 = _frame;
					num4 = 3;
					break;
				case 3:
					num3 = _frameExtra;
					num4 = 6;
					break;
				default:
					num3 = 0;
					break;
				}
				break;
			default:
				num3 = _frame;
				break;
			}
			int num7 = num5 / num4;
			Rectangle value = new Rectangle(0, num7 * num3, _data.textureWidth, num7);
			if (flag)
			{
				value.Height -= 2;
			}
			switch (_type)
			{
			case 0:
				if (drawType == 3)
				{
					drawColor = Color.White;
				}
				break;
			case 9:
				if (drawType == 3)
				{
					if (_abilityCharge == 0)
					{
						return;
					}
					drawColor = Color.Multiply(Color.White, (float)_abilityCharge / (float)_data.abilityChargeMax);
					drawColor.A = 0;
				}
				break;
			case 7:
				if (drawType == 3)
				{
					drawColor = new Color(250, 250, 250, 255) * drawPlayer.stealth * (1f - shadow);
				}
				break;
			}
			Color color = new Color(drawColor.ToVector4() * 0.25f + new Vector4(0.75f));
			switch (_type)
			{
			case 45:
				if (drawType == 2)
				{
					color = new Color(150, 110, 110, 100);
				}
				break;
			case 11:
				if (drawType == 2)
				{
					color = Color.White;
					color.A = 127;
				}
				break;
			case 12:
				if (drawType == 0)
				{
					float num8 = MathHelper.Clamp(drawPlayer.MountFishronSpecialCounter / 60f, 0f, 1f);
					color = Colors.CurrentLiquidColor;
					if (color == Color.Transparent)
					{
						color = Color.White;
					}
					color.A = 127;
					color *= num8;
				}
				break;
			case 24:
				if (drawType == 2)
				{
					color = Color.SkyBlue * 0.5f;
					color.A = 20;
				}
				break;
			}
			float num9 = 0f;
			switch (_type)
			{
			case 8:
			{
				DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
				switch (drawType)
				{
				case 0:
					num9 = drillMountData.outerRingRotation - num9;
					break;
				case 3:
					num9 = drillMountData.diodeRotation - num9 - drawPlayer.fullRotation;
					break;
				}
				break;
			}
			case 7:
				num9 = drawPlayer.fullRotation;
				break;
			}
			Vector2 origin = Origin;
			type = _type;
			_ = 8;
			float scale = 1f;
			SpriteEffects spriteEffects;
			switch (_type)
			{
			case 7:
				spriteEffects = SpriteEffects.None;
				break;
			case 8:
				spriteEffects = ((drawPlayer.direction == 1 && drawType == 2) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				break;
			case 6:
			case 13:
				spriteEffects = (_flipDraw ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				break;
			default:
				spriteEffects = playerEffect;
				break;
			}
			if (MountID.Sets.FacePlayersVelocity[_type])
			{
				spriteEffects = ((Math.Sign(drawPlayer.velocity.X) == -drawPlayer.direction) ? (playerEffect ^ SpriteEffects.FlipHorizontally) : playerEffect);
			}
			bool flag2 = false;
			switch (_type)
			{
			case 50:
				if (drawType == 0)
				{
					Vector2 position = Position + new Vector2(0f, 8 - PlayerOffset + 20);
					Rectangle value6 = new Rectangle(0, num7 * _frameExtra, _data.textureWidth, num7);
					if (flag)
					{
						value6.Height -= 2;
					}
					DrawData item = new DrawData(TextureAssets.Extra[207].get_Value(), position, value6, drawColor, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
				}
				break;
			case 35:
			{
				if (drawType != 2)
				{
					break;
				}
				ExtraFrameMountData extraFrameMountData = (ExtraFrameMountData)_mountSpecificData;
				int num11 = -36;
				if (spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
				{
					num11 *= -1;
				}
				Vector2 vector2 = new Vector2(num11, -26f);
				if (shadow == 0f)
				{
					if (Math.Abs(drawPlayer.velocity.X) > 1f)
					{
						extraFrameMountData.frameCounter += Math.Min(2f, Math.Abs(drawPlayer.velocity.X * 0.4f));
						while (extraFrameMountData.frameCounter > 6f)
						{
							extraFrameMountData.frameCounter -= 6f;
							extraFrameMountData.frame++;
							if ((extraFrameMountData.frame > 2 && extraFrameMountData.frame < 5) || extraFrameMountData.frame > 7)
							{
								extraFrameMountData.frame = 0;
							}
						}
					}
					else
					{
						extraFrameMountData.frameCounter += 1f;
						while (extraFrameMountData.frameCounter > 6f)
						{
							extraFrameMountData.frameCounter -= 6f;
							extraFrameMountData.frame++;
							if (extraFrameMountData.frame > 5)
							{
								extraFrameMountData.frame = 5;
							}
						}
					}
				}
				Texture2D value4 = TextureAssets.Extra[142].get_Value();
				Rectangle value5 = value4.Frame(1, 8, 0, extraFrameMountData.frame);
				if (flag)
				{
					value5.Height -= 2;
				}
				DrawData item = new DrawData(value4, Position + vector2, value5, drawColor, num9, origin, scale, spriteEffects);
				item.shader = currentShader;
				playerDrawData.Add(item);
				break;
			}
			case 38:
				if (drawType == 0)
				{
					int num10 = 0;
					if (spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
					{
						num10 = 22;
					}
					Vector2 vector = new Vector2(num10, -10f);
					Texture2D value2 = TextureAssets.Extra[151].get_Value();
					Rectangle value3 = value2.Frame();
					DrawData item = new DrawData(value2, Position + vector, value3, drawColor, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
					flag2 = true;
				}
				break;
			}
			if (!flag2)
			{
				DrawData item = new DrawData(texture2D, Position, value, drawColor, num9, origin, scale, spriteEffects);
				item.shader = currentShader;
				playerDrawData.Add(item);
				if (texture2D2 != null)
				{
					item = new DrawData(texture2D2, Position, value, color * ((float)(int)drawColor.A / 255f), num9, origin, scale, spriteEffects);
					item.shader = currentShader;
				}
				playerDrawData.Add(item);
			}
			switch (_type)
			{
			case 50:
				if (drawType == 0)
				{
					texture2D = TextureAssets.Extra[205].get_Value();
					DrawData item = new DrawData(texture2D, Position, value, drawColor, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
					Vector2 position3 = Position + new Vector2(0f, 8 - PlayerOffset + 20);
					Rectangle value8 = new Rectangle(0, num7 * _frameExtra, _data.textureWidth, num7);
					if (flag)
					{
						value8.Height -= 2;
					}
					texture2D = TextureAssets.Extra[206].get_Value();
					item = new DrawData(texture2D, position3, value8, drawColor, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
				}
				break;
			case 45:
			{
				if (drawType != 0 || shadow != 0f)
				{
					break;
				}
				if (Math.Abs(drawPlayer.velocity.X) > DashSpeed * 0.9f)
				{
					color = new Color(255, 220, 220, 200);
					scale = 1.1f;
				}
				for (int k = 0; k < 2; k++)
				{
					Vector2 position2 = Position + new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-10, 11) * 0.1f);
					value = new Rectangle(0, num7 * 3, _data.textureWidth, num7);
					if (flag)
					{
						value.Height -= 2;
					}
					DrawData item = new DrawData(texture2D2, position2, value, color, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
				}
				break;
			}
			case 17:
				if (drawType == 1 && ShouldGolfCartEmitLight())
				{
					value = new Rectangle(0, num7 * 3, _data.textureWidth, num7);
					if (flag)
					{
						value.Height -= 2;
					}
					drawColor = Color.White * 1f;
					drawColor.A = 0;
					DrawData item = new DrawData(texture2D, Position, value, drawColor, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
				}
				break;
			case 23:
				if (drawType == 0)
				{
					texture2D = TextureAssets.Extra[114].get_Value();
					value = texture2D.Frame(2);
					int width = value.Width;
					value.Width -= 2;
					float witchBroomTrinketRotation = GetWitchBroomTrinketRotation(drawPlayer);
					Position = drawPlayer.Center + GetWitchBroomTrinketOriginOffset(drawPlayer) - Main.screenPosition;
					num9 = witchBroomTrinketRotation;
					origin = new Vector2(value.Width / 2, 0f);
					DrawData item = new DrawData(texture2D, Position.Floor(), value, drawColor, num9, origin, scale, spriteEffects);
					item.shader = currentShader;
					playerDrawData.Add(item);
					Color color3 = new Color(new Vector3(0.9f, 0.85f, 0f));
					color3.A /= 2;
					float num13 = ((float)drawPlayer.miscCounter / 75f * (MathF.PI * 2f)).ToRotationVector2().X * 1f;
					Color color4 = new Color(80, 70, 40, 0) * (num13 / 8f + 0.5f) * 0.8f;
					value.X += width;
					for (int l = 0; l < 4; l++)
					{
						item = new DrawData(texture2D, (Position + ((float)l * (MathF.PI / 2f)).ToRotationVector2() * num13).Floor(), value, color4, num9, origin, scale, spriteEffects);
						item.shader = currentShader;
						playerDrawData.Add(item);
					}
				}
				break;
			case 8:
			{
				if (drawType != 3)
				{
					break;
				}
				DrillMountData drillMountData2 = (DrillMountData)_mountSpecificData;
				Rectangle value7 = new Rectangle(0, 0, 1, 1);
				Vector2 vector3 = drillDiodePoint1.RotatedBy(drillMountData2.diodeRotation);
				Vector2 vector4 = drillDiodePoint2.RotatedBy(drillMountData2.diodeRotation);
				for (int i = 0; i < drillMountData2.beams.Length; i++)
				{
					DrillBeam drillBeam = drillMountData2.beams[i];
					if (drillBeam.curTileTarget == Point16.NegativeOne)
					{
						continue;
					}
					for (int j = 0; j < 2; j++)
					{
						Vector2 vector5 = new Vector2(drillBeam.curTileTarget.X * 16 + 8, drillBeam.curTileTarget.Y * 16 + 8) - Main.screenPosition - Position;
						Vector2 vector6;
						Color color2;
						if (j == 0)
						{
							vector6 = vector3;
							color2 = Color.CornflowerBlue;
						}
						else
						{
							vector6 = vector4;
							color2 = Color.LightGreen;
						}
						color2.A = 128;
						color2 *= 0.5f;
						Vector2 v = vector5 - vector6;
						float num12 = v.ToRotation();
						float y = v.Length();
						DrawData item = new DrawData(scale: new Vector2(2f, y), texture: TextureAssets.MagicPixel.get_Value(), position: vector6 + Position, sourceRect: value7, color: color2, rotation: num12 - MathF.PI / 2f, origin: Vector2.Zero, effect: SpriteEffects.None);
						item.ignorePlayerRotation = true;
						item.shader = currentShader;
						playerDrawData.Add(item);
					}
				}
				break;
			}
			}
		}

		public void Dismount(Player mountedPlayer)
		{
			if (_active)
			{
				bool cart = Cart;
				_active = false;
				mountedPlayer.ClearBuff(_data.buff);
				_mountSpecificData = null;
				_ = _type;
				if (cart)
				{
					mountedPlayer.ClearBuff(_data.extraBuff);
					mountedPlayer.cartFlip = false;
					mountedPlayer.lastBoost = Vector2.Zero;
				}
				mountedPlayer.fullRotation = 0f;
				mountedPlayer.fullRotationOrigin = Vector2.Zero;
				DoSpawnDust(mountedPlayer, isDismounting: true);
				Reset();
				mountedPlayer.position.Y += mountedPlayer.height;
				mountedPlayer.height = 42;
				mountedPlayer.position.Y -= mountedPlayer.height;
				if (mountedPlayer.whoAmI == Main.myPlayer)
				{
					NetMessage.SendData(13, -1, -1, null, mountedPlayer.whoAmI);
				}
			}
		}

		public void SetMount(int m, Player mountedPlayer, bool faceLeft = false)
		{
			if (_type == m || m <= -1 || m >= MountID.Count || (m == 5 && mountedPlayer.wet))
			{
				return;
			}
			if (_active)
			{
				mountedPlayer.ClearBuff(_data.buff);
				if (Cart)
				{
					mountedPlayer.ClearBuff(_data.extraBuff);
					mountedPlayer.cartFlip = false;
					mountedPlayer.lastBoost = Vector2.Zero;
				}
				mountedPlayer.fullRotation = 0f;
				mountedPlayer.fullRotationOrigin = Vector2.Zero;
				_mountSpecificData = null;
			}
			else
			{
				_active = true;
			}
			_flyTime = 0;
			_type = m;
			_data = mounts[m];
			_fatigueMax = _data.fatigueMax;
			if (Cart && !faceLeft && !Directional)
			{
				mountedPlayer.AddBuff(_data.extraBuff, 3600);
				_flipDraw = true;
			}
			else
			{
				mountedPlayer.AddBuff(_data.buff, 3600);
				_flipDraw = false;
			}
			if (_type == 44)
			{
				mountedPlayer.velocity *= 0.2f;
				mountedPlayer.dash = 0;
				mountedPlayer.dashType = 0;
				mountedPlayer.dashDelay = 0;
				mountedPlayer.dashTime = 0;
			}
			if (_type == 9 && _abilityCooldown < 20)
			{
				_abilityCooldown = 20;
			}
			if (_type == 46 && _abilityCooldown < 40)
			{
				_abilityCooldown = 40;
			}
			mountedPlayer.position.Y += mountedPlayer.height;
			for (int i = 0; i < mountedPlayer.shadowPos.Length; i++)
			{
				mountedPlayer.shadowPos[i].Y += mountedPlayer.height;
			}
			mountedPlayer.height = 42 + _data.heightBoost;
			mountedPlayer.position.Y -= mountedPlayer.height;
			for (int j = 0; j < mountedPlayer.shadowPos.Length; j++)
			{
				mountedPlayer.shadowPos[j].Y -= mountedPlayer.height;
			}
			mountedPlayer.ResetAdvancedShadows();
			if (_type == 7 || _type == 8)
			{
				mountedPlayer.fullRotationOrigin = new Vector2(mountedPlayer.width / 2, mountedPlayer.height / 2);
			}
			if (_type == 8)
			{
				_mountSpecificData = new DrillMountData();
			}
			if (_type == 35)
			{
				_mountSpecificData = new ExtraFrameMountData();
			}
			DoSpawnDust(mountedPlayer, isDismounting: false);
			if (mountedPlayer.whoAmI == Main.myPlayer)
			{
				NetMessage.SendData(13, -1, -1, null, mountedPlayer.whoAmI);
			}
		}

		private void DoSpawnDust(Player mountedPlayer, bool isDismounting)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			if (_type == 52)
			{
				for (int i = 0; i < 100; i++)
				{
					_ = _data.spawnDust;
					Dust dust = Dust.NewDustDirect(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), mountedPlayer.width + 40, mountedPlayer.height, 267, 0f, 0f, 60, new Color(130, 60, 255, 70));
					dust.scale += (float)Main.rand.Next(-10, 21) * 0.01f;
					dust.noGravity = true;
					dust.velocity += mountedPlayer.velocity * 0.8f;
					dust.velocity *= Main.rand.NextFloat();
					dust.velocity.Y += 2f * Main.rand.NextFloatDirection();
					dust.noLight = true;
					if (Main.rand.Next(3) == 0)
					{
						Dust dust2 = Dust.CloneDust(dust);
						dust2.color = Color.White;
						dust2.scale *= 0.5f;
						dust2.alpha = 0;
					}
				}
				return;
			}
			Color newColor = Color.Transparent;
			if (_type == 23)
			{
				newColor = new Color(255, 255, 0, 255);
			}
			for (int j = 0; j < 100; j++)
			{
				if (MountID.Sets.Cart[_type])
				{
					if (j % 10 == 0)
					{
						int type = Main.rand.Next(61, 64);
						int num = Gore.NewGore(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), Vector2.Zero, type);
						Main.gore[num].alpha = 100;
						Main.gore[num].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465)));
					}
					continue;
				}
				int type2 = _data.spawnDust;
				float scale = 1f;
				int alpha = 0;
				if (_type == 40 || _type == 41 || _type == 42)
				{
					type2 = ((Main.rand.Next(2) != 0) ? 16 : 31);
					scale = 0.9f;
					alpha = 50;
					if (_type == 42)
					{
						type2 = 31;
					}
					if (_type == 41)
					{
						type2 = 16;
					}
				}
				int num2 = Dust.NewDust(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), mountedPlayer.width + 40, mountedPlayer.height, type2, 0f, 0f, alpha, newColor, scale);
				Main.dust[num2].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
				if (_data.spawnDustNoGravity)
				{
					Main.dust[num2].noGravity = true;
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.dust[num2].scale *= 1.3f;
					Main.dust[num2].noGravity = true;
				}
				else
				{
					Main.dust[num2].velocity *= 0.5f;
				}
				Main.dust[num2].velocity += mountedPlayer.velocity * 0.8f;
				if (_type == 40 || _type == 41 || _type == 42)
				{
					Main.dust[num2].velocity *= Main.rand.NextFloat();
				}
			}
			if (_type == 40 || _type == 41 || _type == 42)
			{
				for (int k = 0; k < 5; k++)
				{
					int type3 = Main.rand.Next(61, 64);
					if (_type == 41 || (_type == 40 && Main.rand.Next(2) == 0))
					{
						type3 = Main.rand.Next(11, 14);
					}
					int num3 = Gore.NewGore(new Vector2(mountedPlayer.position.X + (float)(mountedPlayer.direction * 8), mountedPlayer.position.Y + 20f), Vector2.Zero, type3);
					Main.gore[num3].alpha = 100;
					Main.gore[num3].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465))) * 1.4f;
				}
			}
			if (_type == 23)
			{
				for (int l = 0; l < 4; l++)
				{
					int type4 = Main.rand.Next(61, 64);
					int num4 = Gore.NewGore(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), Vector2.Zero, type4);
					Main.gore[num4].alpha = 100;
					Main.gore[num4].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465)));
				}
			}
		}

		public bool CanMount(int m, Player mountingPlayer)
		{
			return mountingPlayer.CanFitSpace(mounts[m].heightBoost);
		}

		public bool FindTileHeight(Vector2 position, int maxTilesDown, out float tileHeight)
		{
			int num = (int)(position.X / 16f);
			int num2 = (int)(position.Y / 16f);
			for (int i = 0; i <= maxTilesDown; i++)
			{
				Tile tile = Main.tile[num, num2];
				bool flag = Main.tileSolid[tile.type];
				bool flag2 = Main.tileSolidTop[tile.type];
				if (!tile.active() || !flag || flag2)
				{
				}
				num2++;
			}
			tileHeight = 0f;
			return true;
		}
	}
}
