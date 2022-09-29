using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	public class CreativePowers
	{
		public abstract class APerPlayerTogglePower : ICreativePower, IOnPlayerJoining
		{
			private enum SubMessageType : byte
			{
				SyncEveryone,
				SyncOnePlayer
			}

			internal string _powerNameKey;

			internal Point _iconLocation;

			internal bool _defaultToggleState;

			private bool[] _perPlayerIsEnabled = new bool[255];

			public ushort PowerId { get; set; }

			public string ServerConfigName { get; set; }

			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			public bool IsEnabledForPlayer(int playerIndex)
			{
				if (!_perPlayerIsEnabled.IndexInRange(playerIndex))
				{
					return false;
				}
				return _perPlayerIsEnabled[playerIndex];
			}

			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				switch (reader.ReadByte())
				{
				case 0:
					Deserialize_SyncEveryone(reader, userId);
					break;
				case 1:
				{
					int playerIndex = reader.ReadByte();
					bool state = reader.ReadBoolean();
					if (Main.netMode == 2)
					{
						playerIndex = userId;
						if (!CreativePowersHelper.IsAvailableForPlayer(this, playerIndex))
						{
							break;
						}
					}
					SetEnabledState(playerIndex, state);
					break;
				}
				}
			}

			private void Deserialize_SyncEveryone(BinaryReader reader, int userId)
			{
				int num = (int)Math.Ceiling((float)_perPlayerIsEnabled.Length / 8f);
				if (Main.netMode == 2 && !CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					reader.ReadBytes(num);
					return;
				}
				for (int i = 0; i < num; i++)
				{
					BitsByte bitsByte = reader.ReadByte();
					for (int j = 0; j < 8; j++)
					{
						int num2 = i * 8 + j;
						if (num2 != Main.myPlayer)
						{
							if (num2 >= _perPlayerIsEnabled.Length)
							{
								break;
							}
							SetEnabledState(num2, bitsByte[j]);
						}
					}
				}
			}

			public void SetEnabledState(int playerIndex, bool state)
			{
				_perPlayerIsEnabled[playerIndex] = state;
				if (Main.netMode == 2)
				{
					NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 3);
					packet.Writer.Write((byte)1);
					packet.Writer.Write((byte)playerIndex);
					packet.Writer.Write(state);
					NetManager.Instance.Broadcast(packet);
				}
			}

			public void DebugCall()
			{
				RequestUse();
			}

			internal void RequestUse()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 1);
				packet.Writer.Write((byte)1);
				packet.Writer.Write((byte)Main.myPlayer);
				packet.Writer.Write(!_perPlayerIsEnabled[Main.myPlayer]);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			public void Reset()
			{
				for (int i = 0; i < _perPlayerIsEnabled.Length; i++)
				{
					_perPlayerIsEnabled[i] = _defaultToggleState;
				}
			}

			public void OnPlayerJoining(int playerIndex)
			{
				int num = (int)Math.Ceiling((float)_perPlayerIsEnabled.Length / 8f);
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, num + 1);
				packet.Writer.Write((byte)0);
				for (int i = 0; i < num; i++)
				{
					BitsByte bitsByte = (byte)0;
					for (int j = 0; j < 8; j++)
					{
						int num2 = i * 8 + j;
						if (num2 >= _perPlayerIsEnabled.Length)
						{
							break;
						}
						bitsByte[j] = _perPlayerIsEnabled[num2];
					}
					packet.Writer.Write(bitsByte);
				}
				NetManager.Instance.SendToClient(packet, playerIndex);
			}

			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				GroupOptionButton<bool> groupOptionButton = CreativePowersHelper.CreateToggleButton(info);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, Main.OurFavoriteColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(_iconLocation));
				groupOptionButton.OnClick += button_OnClick;
				groupOptionButton.OnUpdate += button_OnUpdate;
				elements.Add(groupOptionButton);
			}

			private void button_OnUpdate(UIElement affectedElement)
			{
				bool currentOption = _perPlayerIsEnabled[Main.myPlayer];
				GroupOptionButton<bool> groupOptionButton = affectedElement as GroupOptionButton<bool>;
				groupOptionButton.SetCurrentOption(currentOption);
				if (affectedElement.IsMouseHovering)
				{
					string originalText = Language.GetTextValue(groupOptionButton.IsSelected ? (_powerNameKey + "_Enabled") : (_powerNameKey + "_Disabled"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref originalText, _powerNameKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref originalText, GetIsUnlocked(), _powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}

			private void button_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				if (GetIsUnlocked() && CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					RequestUse();
				}
			}

			public abstract bool GetIsUnlocked();
		}

		public abstract class APerPlayerSliderPower : ICreativePower, IOnPlayerJoining, IProvideSliderElement, IPowerSubcategoryElement
		{
			internal Point _iconLocation;

			internal float _sliderCurrentValueCache;

			internal string _powerNameKey;

			internal float[] _cachePerPlayer = new float[256];

			internal float _sliderDefaultValue;

			private float _currentTargetValue;

			private bool _needsToCommitChange;

			private DateTime _nextTimeWeCanPush = DateTime.UtcNow;

			public ushort PowerId { get; set; }

			public string ServerConfigName { get; set; }

			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			public bool GetRemappedSliderValueFor(int playerIndex, out float value)
			{
				value = 0f;
				if (!_cachePerPlayer.IndexInRange(playerIndex))
				{
					return false;
				}
				value = RemapSliderValueToPowerValue(_cachePerPlayer[playerIndex]);
				return true;
			}

			public abstract float RemapSliderValueToPowerValue(float sliderValue);

			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				int num = reader.ReadByte();
				float num2 = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					num = userId;
					if (!CreativePowersHelper.IsAvailableForPlayer(this, num))
					{
						return;
					}
				}
				_cachePerPlayer[num] = num2;
				if (num == Main.myPlayer)
				{
					_sliderCurrentValueCache = num2;
					UpdateInfoFromSliderValueCache();
				}
			}

			internal abstract void UpdateInfoFromSliderValueCache();

			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				throw new NotImplementedException();
			}

			public void DebugCall()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 5);
				packet.Writer.Write((byte)Main.myPlayer);
				packet.Writer.Write(0f);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			public abstract UIElement ProvideSlider();

			internal float GetSliderValue()
			{
				if (Main.netMode == 1 && _needsToCommitChange)
				{
					return _currentTargetValue;
				}
				return _sliderCurrentValueCache;
			}

			internal void SetValueKeyboard(float value)
			{
				if (value != _currentTargetValue && CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					_currentTargetValue = value;
					_needsToCommitChange = true;
				}
			}

			internal void SetValueGamepad()
			{
				float sliderValue = GetSliderValue();
				float num = UILinksInitializer.HandleSliderVerticalInput(sliderValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				if (num != sliderValue)
				{
					SetValueKeyboard(num);
				}
			}

			public void PushChangeAndSetSlider(float value)
			{
				if (CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					value = MathHelper.Clamp(value, 0f, 1f);
					_sliderCurrentValueCache = value;
					_currentTargetValue = value;
					PushChange(value);
				}
			}

			public GroupOptionButton<int> GetOptionButton(CreativePowerUIElementRequestInfo info, int optionIndex, int currentOptionIndex)
			{
				GroupOptionButton<int> groupOptionButton = CreativePowersHelper.CreateCategoryButton(info, optionIndex, currentOptionIndex);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(_iconLocation));
				groupOptionButton.OnUpdate += categoryButton_OnUpdate;
				return groupOptionButton;
			}

			private void categoryButton_OnUpdate(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					GroupOptionButton<int> groupOptionButton = affectedElement as GroupOptionButton<int>;
					string originalText = Language.GetTextValue(_powerNameKey + (groupOptionButton.IsSelected ? "_Opened" : "_Closed"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref originalText, _powerNameKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref originalText, GetIsUnlocked(), _powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
				AttemptPushingChange();
			}

			private void AttemptPushingChange()
			{
				if (_needsToCommitChange && DateTime.UtcNow.CompareTo(_nextTimeWeCanPush) != -1)
				{
					PushChange(_currentTargetValue);
				}
			}

			internal void PushChange(float newSliderValue)
			{
				_needsToCommitChange = false;
				_sliderCurrentValueCache = newSliderValue;
				_nextTimeWeCanPush = DateTime.UtcNow;
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 5);
				packet.Writer.Write((byte)Main.myPlayer);
				packet.Writer.Write(newSliderValue);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			public virtual void Reset()
			{
				for (int i = 0; i < _cachePerPlayer.Length; i++)
				{
					ResetForPlayer(i);
				}
			}

			public virtual void ResetForPlayer(int playerIndex)
			{
				_cachePerPlayer[playerIndex] = _sliderDefaultValue;
				if (playerIndex == Main.myPlayer)
				{
					_sliderCurrentValueCache = _sliderDefaultValue;
					_currentTargetValue = _sliderDefaultValue;
				}
			}

			public void OnPlayerJoining(int playerIndex)
			{
				ResetForPlayer(playerIndex);
			}

			public abstract bool GetIsUnlocked();
		}

		public abstract class ASharedButtonPower : ICreativePower
		{
			internal Point _iconLocation;

			internal string _powerNameKey;

			internal string _descriptionKey;

			public ushort PowerId { get; set; }

			public string ServerConfigName { get; set; }

			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			public ASharedButtonPower()
			{
				OnCreation();
			}

			public void RequestUse()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 0);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				if (Main.netMode != 2 || CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					UsePower();
				}
			}

			internal abstract void UsePower();

			internal abstract void OnCreation();

			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				GroupOptionButton<bool> groupOptionButton = CreativePowersHelper.CreateSimpleButton(info);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(_iconLocation));
				groupOptionButton.OnClick += button_OnClick;
				groupOptionButton.OnUpdate += button_OnUpdate;
				elements.Add(groupOptionButton);
			}

			private void button_OnUpdate(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string originalText = Language.GetTextValue(_powerNameKey);
					CreativePowersHelper.AddDescriptionIfNeeded(ref originalText, _descriptionKey);
					CreativePowersHelper.AddUnlockTextIfNeeded(ref originalText, GetIsUnlocked(), _powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}

			private void button_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				if (CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					RequestUse();
				}
			}

			public abstract bool GetIsUnlocked();
		}

		public abstract class ASharedTogglePower : ICreativePower, IOnPlayerJoining
		{
			public ushort PowerId { get; set; }

			public string ServerConfigName { get; set; }

			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			public bool Enabled { get; private set; }

			public void SetPowerInfo(bool enabled)
			{
				Enabled = enabled;
			}

			public void Reset()
			{
				Enabled = false;
			}

			public void OnPlayerJoining(int playerIndex)
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 1);
				packet.Writer.Write(Enabled);
				NetManager.Instance.SendToClient(packet, playerIndex);
			}

			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				bool powerInfo = reader.ReadBoolean();
				if (Main.netMode != 2 || CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					SetPowerInfo(powerInfo);
					if (Main.netMode == 2)
					{
						NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 1);
						packet.Writer.Write(Enabled);
						NetManager.Instance.Broadcast(packet);
					}
				}
			}

			private void RequestUse()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 1);
				packet.Writer.Write(!Enabled);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				GroupOptionButton<bool> groupOptionButton = CreativePowersHelper.CreateToggleButton(info);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, Main.OurFavoriteColor);
				CustomizeButton(groupOptionButton);
				groupOptionButton.OnClick += button_OnClick;
				groupOptionButton.OnUpdate += button_OnUpdate;
				elements.Add(groupOptionButton);
			}

			private void button_OnUpdate(UIElement affectedElement)
			{
				bool enabled = Enabled;
				GroupOptionButton<bool> groupOptionButton = affectedElement as GroupOptionButton<bool>;
				groupOptionButton.SetCurrentOption(enabled);
				if (affectedElement.IsMouseHovering)
				{
					string buttonTextKey = GetButtonTextKey();
					string originalText = Language.GetTextValue(buttonTextKey + (groupOptionButton.IsSelected ? "_Enabled" : "_Disabled"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref originalText, buttonTextKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref originalText, GetIsUnlocked(), buttonTextKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}

			private void button_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				if (CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					RequestUse();
				}
			}

			internal abstract void CustomizeButton(UIElement button);

			internal abstract string GetButtonTextKey();

			public abstract bool GetIsUnlocked();
		}

		public abstract class ASharedSliderPower : ICreativePower, IOnPlayerJoining, IProvideSliderElement, IPowerSubcategoryElement
		{
			internal Point _iconLocation;

			internal float _sliderCurrentValueCache;

			internal string _powerNameKey;

			internal bool _syncToJoiningPlayers = true;

			internal float _currentTargetValue;

			private bool _needsToCommitChange;

			private DateTime _nextTimeWeCanPush = DateTime.UtcNow;

			public ushort PowerId { get; set; }

			public string ServerConfigName { get; set; }

			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				float num = reader.ReadSingle();
				if (Main.netMode != 2 || CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					_sliderCurrentValueCache = num;
					UpdateInfoFromSliderValueCache();
					if (Main.netMode == 2)
					{
						NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 4);
						packet.Writer.Write(num);
						NetManager.Instance.Broadcast(packet);
					}
				}
			}

			internal abstract void UpdateInfoFromSliderValueCache();

			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				throw new NotImplementedException();
			}

			public void DebugCall()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 4);
				packet.Writer.Write(0f);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			public abstract UIElement ProvideSlider();

			internal float GetSliderValue()
			{
				if (Main.netMode == 1 && _needsToCommitChange)
				{
					return _currentTargetValue;
				}
				return GetSliderValueInner();
			}

			internal virtual float GetSliderValueInner()
			{
				return _sliderCurrentValueCache;
			}

			internal void SetValueKeyboard(float value)
			{
				if (value != _currentTargetValue)
				{
					SetValueKeyboardForced(value);
				}
			}

			internal void SetValueKeyboardForced(float value)
			{
				if (CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					_currentTargetValue = value;
					_needsToCommitChange = true;
				}
			}

			internal void SetValueGamepad()
			{
				float sliderValue = GetSliderValue();
				float num = UILinksInitializer.HandleSliderVerticalInput(sliderValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				if (num != sliderValue)
				{
					SetValueKeyboard(num);
				}
			}

			public GroupOptionButton<int> GetOptionButton(CreativePowerUIElementRequestInfo info, int optionIndex, int currentOptionIndex)
			{
				GroupOptionButton<int> groupOptionButton = CreativePowersHelper.CreateCategoryButton(info, optionIndex, currentOptionIndex);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(_iconLocation));
				groupOptionButton.OnUpdate += categoryButton_OnUpdate;
				return groupOptionButton;
			}

			private void categoryButton_OnUpdate(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					GroupOptionButton<int> groupOptionButton = affectedElement as GroupOptionButton<int>;
					string originalText = Language.GetTextValue(_powerNameKey + (groupOptionButton.IsSelected ? "_Opened" : "_Closed"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref originalText, _powerNameKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref originalText, GetIsUnlocked(), _powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
				AttemptPushingChange();
			}

			private void AttemptPushingChange()
			{
				if (_needsToCommitChange && DateTime.UtcNow.CompareTo(_nextTimeWeCanPush) != -1)
				{
					_needsToCommitChange = false;
					_sliderCurrentValueCache = _currentTargetValue;
					_nextTimeWeCanPush = DateTime.UtcNow;
					NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 4);
					packet.Writer.Write(_currentTargetValue);
					NetManager.Instance.SendToServerOrLoopback(packet);
				}
			}

			public virtual void Reset()
			{
				_sliderCurrentValueCache = 0f;
			}

			public void OnPlayerJoining(int playerIndex)
			{
				if (_syncToJoiningPlayers)
				{
					NetPacket packet = NetCreativePowersModule.PreparePacket(PowerId, 4);
					packet.Writer.Write(_sliderCurrentValueCache);
					NetManager.Instance.SendToClient(packet, playerIndex);
				}
			}

			public abstract bool GetIsUnlocked();
		}

		public class GodmodePower : APerPlayerTogglePower, IPersistentPerPlayerContent
		{
			public GodmodePower()
			{
				_powerNameKey = "CreativePowers.Godmode";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.Godmode;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(Player player, BinaryWriter writer)
			{
				bool value = IsEnabledForPlayer(Main.myPlayer);
				writer.Write(value);
			}

			public void ResetDataForNewPlayer(Player player)
			{
				player.savedPerPlayerFieldsThatArentInThePlayerClass.godmodePowerEnabled = _defaultToggleState;
			}

			public void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool godmodePowerEnabled = reader.ReadBoolean();
				player.savedPerPlayerFieldsThatArentInThePlayerClass.godmodePowerEnabled = godmodePowerEnabled;
			}

			public void ApplyLoadedDataToOutOfPlayerFields(Player player)
			{
				if (player.savedPerPlayerFieldsThatArentInThePlayerClass.godmodePowerEnabled != IsEnabledForPlayer(player.whoAmI))
				{
					RequestUse();
				}
			}
		}

		public class FarPlacementRangePower : APerPlayerTogglePower, IPersistentPerPlayerContent
		{
			public FarPlacementRangePower()
			{
				_powerNameKey = "CreativePowers.InfinitePlacementRange";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.BlockPlacementRange;
				_defaultToggleState = true;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(Player player, BinaryWriter writer)
			{
				bool value = IsEnabledForPlayer(Main.myPlayer);
				writer.Write(value);
			}

			public void ResetDataForNewPlayer(Player player)
			{
				player.savedPerPlayerFieldsThatArentInThePlayerClass.farPlacementRangePowerEnabled = _defaultToggleState;
			}

			public void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool farPlacementRangePowerEnabled = reader.ReadBoolean();
				player.savedPerPlayerFieldsThatArentInThePlayerClass.farPlacementRangePowerEnabled = farPlacementRangePowerEnabled;
			}

			public void ApplyLoadedDataToOutOfPlayerFields(Player player)
			{
				if (player.savedPerPlayerFieldsThatArentInThePlayerClass.farPlacementRangePowerEnabled != IsEnabledForPlayer(player.whoAmI))
				{
					RequestUse();
				}
			}
		}

		public class StartDayImmediately : ASharedButtonPower
		{
			internal override void UsePower()
			{
				if (Main.netMode != 1)
				{
					Main.SkipToTime(0, setIsDayTime: true);
				}
			}

			internal override void OnCreation()
			{
				_powerNameKey = "CreativePowers.StartDayImmediately";
				_descriptionKey = _powerNameKey + "_Description";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeDawn;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		public class StartNightImmediately : ASharedButtonPower
		{
			internal override void UsePower()
			{
				if (Main.netMode != 1)
				{
					Main.SkipToTime(0, setIsDayTime: false);
				}
			}

			internal override void OnCreation()
			{
				_powerNameKey = "CreativePowers.StartNightImmediately";
				_descriptionKey = _powerNameKey + "_Description";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeDusk;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		public class StartNoonImmediately : ASharedButtonPower
		{
			internal override void UsePower()
			{
				if (Main.netMode != 1)
				{
					Main.SkipToTime(27000, setIsDayTime: true);
				}
			}

			internal override void OnCreation()
			{
				_powerNameKey = "CreativePowers.StartNoonImmediately";
				_descriptionKey = _powerNameKey + "_Description";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeNoon;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		public class StartMidnightImmediately : ASharedButtonPower
		{
			internal override void UsePower()
			{
				if (Main.netMode != 1)
				{
					Main.SkipToTime(16200, setIsDayTime: false);
				}
			}

			internal override void OnCreation()
			{
				_powerNameKey = "CreativePowers.StartMidnightImmediately";
				_descriptionKey = _powerNameKey + "_Description";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeMidnight;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		public class ModifyTimeRate : ASharedSliderPower, IPersistentPerWorldContent
		{
			public int TargetTimeRate { get; private set; }

			public ModifyTimeRate()
			{
				_powerNameKey = "CreativePowers.ModifyTimeRate";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.ModifyTime;
			}

			public override void Reset()
			{
				_sliderCurrentValueCache = 0f;
				TargetTimeRate = 1;
			}

			internal override void UpdateInfoFromSliderValueCache()
			{
				TargetTimeRate = (int)Math.Round(Utils.Remap(_sliderCurrentValueCache, 0f, 1f, 1f, 24f));
			}

			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uIVerticalSlider = CreativePowersHelper.CreateSlider(base.GetSliderValue, base.SetValueKeyboard, base.SetValueGamepad);
				uIVerticalSlider.OnUpdate += UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uIPanel = new UIPanel();
				uIPanel.Width = new StyleDimension(87f, 0f);
				uIPanel.Height = new StyleDimension(180f, 0f);
				uIPanel.HAlign = 0f;
				uIPanel.VAlign = 0.5f;
				uIPanel.Append(uIVerticalSlider);
				uIPanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uIText = new UIText("x24")
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uIText.OnMouseOver += Button_OnMouseOver;
				uIText.OnMouseOut += Button_OnMouseOut;
				uIText.OnClick += topText_OnClick;
				uIPanel.Append(uIText);
				UIText uIText2 = new UIText("x12")
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uIText2.OnMouseOver += Button_OnMouseOver;
				uIText2.OnMouseOut += Button_OnMouseOut;
				uIText2.OnClick += middleText_OnClick;
				uIPanel.Append(uIText2);
				UIText uIText3 = new UIText("x1")
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uIText3.OnMouseOver += Button_OnMouseOver;
				uIText3.OnMouseOut += Button_OnMouseOut;
				uIText3.OnClick += bottomText_OnClick;
				uIPanel.Append(uIText3);
				return uIPanel;
			}

			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12);
			}

			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0.5f);
				SoundEngine.PlaySound(12);
			}

			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12);
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(BinaryWriter writer)
			{
				writer.Write(_sliderCurrentValueCache);
			}

			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				_sliderCurrentValueCache = reader.ReadSingle();
				UpdateInfoFromSliderValueCache();
			}

			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadSingle();
			}

			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string originalText = "x" + TargetTimeRate;
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}
		}

		public class DifficultySliderPower : ASharedSliderPower, IPersistentPerWorldContent
		{
			public float StrengthMultiplierToGiveNPCs { get; private set; }

			public DifficultySliderPower()
			{
				_powerNameKey = "CreativePowers.DifficultySlider";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.EnemyStrengthSlider;
			}

			public override void Reset()
			{
				_sliderCurrentValueCache = 0f;
				UpdateInfoFromSliderValueCache();
			}

			internal override void UpdateInfoFromSliderValueCache()
			{
				if (_sliderCurrentValueCache <= 0.33f)
				{
					StrengthMultiplierToGiveNPCs = Utils.Remap(_sliderCurrentValueCache, 0f, 0.33f, 0.5f, 1f);
				}
				else
				{
					StrengthMultiplierToGiveNPCs = Utils.Remap(_sliderCurrentValueCache, 0.33f, 1f, 1f, 3f);
				}
				float num2 = (StrengthMultiplierToGiveNPCs = (float)Math.Round(StrengthMultiplierToGiveNPCs * 20f) / 20f);
			}

			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uIVerticalSlider = CreativePowersHelper.CreateSlider(base.GetSliderValue, base.SetValueKeyboard, base.SetValueGamepad);
				UIPanel uIPanel = new UIPanel();
				uIPanel.Width = new StyleDimension(82f, 0f);
				uIPanel.Height = new StyleDimension(180f, 0f);
				uIPanel.HAlign = 0f;
				uIPanel.VAlign = 0.5f;
				uIPanel.Append(uIVerticalSlider);
				uIPanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				uIVerticalSlider.OnUpdate += UpdateSliderColorAndShowMultiplierMouseOver;
				AddIndication(uIPanel, 0f, "x3", "Images/UI/WorldCreation/IconDifficultyMaster", MouseOver_Master, Click_Master);
				AddIndication(uIPanel, 1f / 3f, "x2", "Images/UI/WorldCreation/IconDifficultyExpert", MouseOver_Expert, Click_Expert);
				AddIndication(uIPanel, 2f / 3f, "x1", "Images/UI/WorldCreation/IconDifficultyNormal", MouseOver_Normal, Click_Normal);
				AddIndication(uIPanel, 1f, "x0.5", "Images/UI/WorldCreation/IconDifficultyCreative", MouseOver_Journey, Click_Journey);
				return uIPanel;
			}

			private void Click_Master(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12);
			}

			private void Click_Expert(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0.66f);
				SoundEngine.PlaySound(12);
			}

			private void Click_Normal(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0.33f);
				SoundEngine.PlaySound(12);
			}

			private void Click_Journey(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12);
			}

			private static void AddIndication(UIPanel panel, float yAnchor, string indicationText, string iconImagePath, UIElement.ElementEvent updateEvent, UIElement.MouseEvent clickEvent)
			{
				UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>(iconImagePath, (AssetRequestMode)1))
				{
					HAlign = 1f,
					VAlign = yAnchor,
					Left = new StyleDimension(4f, 0f),
					Top = new StyleDimension(2f, 0f),
					RemoveFloatingPointsFromDrawPosition = true
				};
				uIImage.OnMouseOut += Button_OnMouseOut;
				uIImage.OnMouseOver += Button_OnMouseOver;
				if (updateEvent != null)
				{
					uIImage.OnUpdate += updateEvent;
				}
				if (clickEvent != null)
				{
					uIImage.OnClick += clickEvent;
				}
				panel.Append(uIImage);
			}

			private static void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12);
			}

			private static void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12);
			}

			private void MouseOver_Journey(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Creative");
					Main.instance.MouseTextNoOverride(textValue, 0, 0);
				}
			}

			private void MouseOver_Normal(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Normal");
					Main.instance.MouseTextNoOverride(textValue, 0, 0);
				}
			}

			private void MouseOver_Expert(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Expert");
					Main.instance.MouseTextNoOverride(textValue, 0, 0);
				}
			}

			private void MouseOver_Master(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Master");
					Main.instance.MouseTextNoOverride(textValue, 0, 0);
				}
			}

			private void UpdateSliderColorAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string originalText = "x" + StrengthMultiplierToGiveNPCs.ToString("F2");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
				if (affectedElement is UIVerticalSlider uIVerticalSlider)
				{
					uIVerticalSlider.EmptyColor = Color.Black;
					Color color = (uIVerticalSlider.FilledColor = (Main.masterMode ? Main.hcColor : (Main.expertMode ? Main.mcColor : ((!(StrengthMultiplierToGiveNPCs < 1f)) ? Color.White : Main.creativeModeColor))));
				}
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(BinaryWriter writer)
			{
				writer.Write(_sliderCurrentValueCache);
			}

			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				_sliderCurrentValueCache = reader.ReadSingle();
				UpdateInfoFromSliderValueCache();
			}

			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadSingle();
			}
		}

		public class ModifyWindDirectionAndStrength : ASharedSliderPower
		{
			public ModifyWindDirectionAndStrength()
			{
				_powerNameKey = "CreativePowers.ModifyWindDirectionAndStrength";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.WindDirection;
				_syncToJoiningPlayers = false;
			}

			internal override void UpdateInfoFromSliderValueCache()
			{
				Main.windSpeedCurrent = (Main.windSpeedTarget = MathHelper.Lerp(-0.8f, 0.8f, _sliderCurrentValueCache));
			}

			internal override float GetSliderValueInner()
			{
				return Utils.GetLerpValue(-0.8f, 0.8f, Main.windSpeedTarget);
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uIVerticalSlider = CreativePowersHelper.CreateSlider(base.GetSliderValue, base.SetValueKeyboard, base.SetValueGamepad);
				uIVerticalSlider.OnUpdate += UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uIPanel = new UIPanel();
				uIPanel.Width = new StyleDimension(132f, 0f);
				uIPanel.Height = new StyleDimension(180f, 0f);
				uIPanel.HAlign = 0f;
				uIPanel.VAlign = 0.5f;
				uIPanel.Append(uIVerticalSlider);
				uIPanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uIText = new UIText(Language.GetText("CreativePowers.WindWest"))
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uIText.OnMouseOut += Button_OnMouseOut;
				uIText.OnMouseOver += Button_OnMouseOver;
				uIText.OnClick += topText_OnClick;
				uIPanel.Append(uIText);
				UIText uIText2 = new UIText(Language.GetText("CreativePowers.WindEast"))
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uIText2.OnMouseOut += Button_OnMouseOut;
				uIText2.OnMouseOver += Button_OnMouseOver;
				uIText2.OnClick += bottomText_OnClick;
				uIPanel.Append(uIText2);
				UIText uIText3 = new UIText(Language.GetText("CreativePowers.WindNone"))
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uIText3.OnMouseOut += Button_OnMouseOut;
				uIText3.OnMouseOver += Button_OnMouseOver;
				uIText3.OnClick += middleText_OnClick;
				uIPanel.Append(uIText3);
				return uIPanel;
			}

			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12);
			}

			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12);
			}

			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0.5f);
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12);
			}

			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					int num = (int)(Main.windSpeedCurrent * 50f);
					string originalText = "";
					if (num < 0)
					{
						originalText += Language.GetTextValue("GameUI.EastWind", Math.Abs(num));
					}
					else if (num > 0)
					{
						originalText += Language.GetTextValue("GameUI.WestWind", num);
					}
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}
		}

		public class ModifyRainPower : ASharedSliderPower
		{
			public ModifyRainPower()
			{
				_powerNameKey = "CreativePowers.ModifyRainPower";
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.RainStrength;
				_syncToJoiningPlayers = false;
			}

			internal override void UpdateInfoFromSliderValueCache()
			{
				if (_sliderCurrentValueCache == 0f)
				{
					Main.StopRain();
				}
				else
				{
					Main.StartRain();
				}
				Main.cloudAlpha = _sliderCurrentValueCache;
				Main.maxRaining = _sliderCurrentValueCache;
			}

			internal override float GetSliderValueInner()
			{
				return Main.cloudAlpha;
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uIVerticalSlider = CreativePowersHelper.CreateSlider(base.GetSliderValue, base.SetValueKeyboard, base.SetValueGamepad);
				uIVerticalSlider.OnUpdate += UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uIPanel = new UIPanel();
				uIPanel.Width = new StyleDimension(132f, 0f);
				uIPanel.Height = new StyleDimension(180f, 0f);
				uIPanel.HAlign = 0f;
				uIPanel.VAlign = 0.5f;
				uIPanel.Append(uIVerticalSlider);
				uIPanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uIText = new UIText(Language.GetText("CreativePowers.WeatherMonsoon"))
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uIText.OnMouseOut += Button_OnMouseOut;
				uIText.OnMouseOver += Button_OnMouseOver;
				uIText.OnClick += topText_OnClick;
				uIPanel.Append(uIText);
				UIText uIText2 = new UIText(Language.GetText("CreativePowers.WeatherClearSky"))
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uIText2.OnMouseOut += Button_OnMouseOut;
				uIText2.OnMouseOver += Button_OnMouseOver;
				uIText2.OnClick += bottomText_OnClick;
				uIPanel.Append(uIText2);
				UIText uIText3 = new UIText(Language.GetText("CreativePowers.WeatherDrizzle"))
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uIText3.OnMouseOut += Button_OnMouseOut;
				uIText3.OnMouseOver += Button_OnMouseOver;
				uIText3.OnClick += middleText_OnClick;
				uIPanel.Append(uIText3);
				return uIPanel;
			}

			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12);
			}

			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0.5f);
				SoundEngine.PlaySound(12);
			}

			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12);
			}

			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string originalText = Main.maxRaining.ToString("P0");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}
		}

		public class FreezeTime : ASharedTogglePower, IPersistentPerWorldContent
		{
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.FreezeTime));
			}

			internal override string GetButtonTextKey()
			{
				return "CreativePowers.FreezeTime";
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				SetPowerInfo(powerInfo);
			}

			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		public class FreezeWindDirectionAndStrength : ASharedTogglePower, IPersistentPerWorldContent
		{
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.WindFreeze));
			}

			internal override string GetButtonTextKey()
			{
				return "CreativePowers.FreezeWindDirectionAndStrength";
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				SetPowerInfo(powerInfo);
			}

			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		public class FreezeRainPower : ASharedTogglePower, IPersistentPerWorldContent
		{
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.RainFreeze));
			}

			internal override string GetButtonTextKey()
			{
				return "CreativePowers.FreezeRainPower";
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				SetPowerInfo(powerInfo);
			}

			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		public class StopBiomeSpreadPower : ASharedTogglePower, IPersistentPerWorldContent
		{
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.StopBiomeSpread));
			}

			internal override string GetButtonTextKey()
			{
				return "CreativePowers.StopBiomeSpread";
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				SetPowerInfo(powerInfo);
			}

			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		public class SpawnRateSliderPerPlayerPower : APerPlayerSliderPower, IPersistentPerPlayerContent
		{
			public float StrengthMultiplierToGiveNPCs { get; private set; }

			public SpawnRateSliderPerPlayerPower()
			{
				_powerNameKey = "CreativePowers.NPCSpawnRateSlider";
				_sliderDefaultValue = 0.5f;
				_iconLocation = CreativePowersHelper.CreativePowerIconLocations.EnemySpawnRate;
			}

			public bool GetShouldDisableSpawnsFor(int playerIndex)
			{
				if (!_cachePerPlayer.IndexInRange(playerIndex))
				{
					return false;
				}
				if (playerIndex == Main.myPlayer)
				{
					return _sliderCurrentValueCache == 0f;
				}
				return _cachePerPlayer[playerIndex] == 0f;
			}

			internal override void UpdateInfoFromSliderValueCache()
			{
			}

			public override float RemapSliderValueToPowerValue(float sliderValue)
			{
				if (sliderValue < 0.5f)
				{
					return Utils.Remap(sliderValue, 0f, 0.5f, 0.1f, 1f);
				}
				return Utils.Remap(sliderValue, 0.5f, 1f, 1f, 10f);
			}

			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uIVerticalSlider = CreativePowersHelper.CreateSlider(base.GetSliderValue, base.SetValueKeyboard, base.SetValueGamepad);
				uIVerticalSlider.OnUpdate += UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uIPanel = new UIPanel();
				uIPanel.Width = new StyleDimension(77f, 0f);
				uIPanel.Height = new StyleDimension(180f, 0f);
				uIPanel.HAlign = 0f;
				uIPanel.VAlign = 0.5f;
				uIPanel.Append(uIVerticalSlider);
				uIPanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uIText = new UIText("x10")
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uIText.OnMouseOut += Button_OnMouseOut;
				uIText.OnMouseOver += Button_OnMouseOver;
				uIText.OnClick += topText_OnClick;
				uIPanel.Append(uIText);
				UIText uIText2 = new UIText("x1")
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uIText2.OnMouseOut += Button_OnMouseOut;
				uIText2.OnMouseOver += Button_OnMouseOver;
				uIText2.OnClick += middleText_OnClick;
				uIPanel.Append(uIText2);
				UIText uIText3 = new UIText("x0")
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uIText3.OnMouseOut += Button_OnMouseOut;
				uIText3.OnMouseOver += Button_OnMouseOver;
				uIText3.OnClick += bottomText_OnClick;
				uIPanel.Append(uIText3);
				return uIPanel;
			}

			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12);
			}

			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				if (listeningElement is UIText uIText)
				{
					uIText.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12);
			}

			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboard(1f);
				SoundEngine.PlaySound(12);
			}

			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboard(0.5f);
				SoundEngine.PlaySound(12);
			}

			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				SetValueKeyboard(0f);
				SoundEngine.PlaySound(12);
			}

			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string originalText = "x" + RemapSliderValueToPowerValue(GetSliderValue()).ToString("F2");
					if (GetShouldDisableSpawnsFor(Main.myPlayer))
					{
						originalText = Language.GetTextValue(_powerNameKey + "EnemySpawnsDisabled");
					}
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref originalText);
					Main.instance.MouseTextNoOverride(originalText, 0, 0);
				}
			}

			public override bool GetIsUnlocked()
			{
				return true;
			}

			public void Save(Player player, BinaryWriter writer)
			{
				float sliderCurrentValueCache = _sliderCurrentValueCache;
				writer.Write(sliderCurrentValueCache);
			}

			public void ResetDataForNewPlayer(Player player)
			{
				player.savedPerPlayerFieldsThatArentInThePlayerClass.spawnRatePowerSliderValue = _sliderDefaultValue;
			}

			public void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				float spawnRatePowerSliderValue = reader.ReadSingle();
				player.savedPerPlayerFieldsThatArentInThePlayerClass.spawnRatePowerSliderValue = spawnRatePowerSliderValue;
			}

			public void ApplyLoadedDataToOutOfPlayerFields(Player player)
			{
				PushChangeAndSetSlider(player.savedPerPlayerFieldsThatArentInThePlayerClass.spawnRatePowerSliderValue);
			}
		}
	}
}
