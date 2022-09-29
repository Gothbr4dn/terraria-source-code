using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace Terraria.Cinematics
{
	public class DD2Film : Film
	{
		private NPC _dryad;

		private NPC _ogre;

		private NPC _portal;

		private List<NPC> _army = new List<NPC>();

		private List<NPC> _critters = new List<NPC>();

		private Vector2 _startPoint;

		public DD2Film()
		{
			AppendKeyFrames(CreateDryad, CreateCritters);
			AppendSequences(120, DryadStand, DryadLookRight);
			AppendSequences(100, DryadLookRight, DryadInteract);
			AddKeyFrame(base.AppendPoint - 20, CreatePortal);
			AppendSequences(30, DryadLookLeft, DryadStand);
			AppendSequences(40, DryadConfusedEmote, DryadStand, DryadLookLeft);
			AppendKeyFrame(CreateOgre);
			AddKeyFrame(base.AppendPoint + 60, SpawnJavalinThrower);
			AddKeyFrame(base.AppendPoint + 120, SpawnGoblin);
			AddKeyFrame(base.AppendPoint + 180, SpawnGoblin);
			AddKeyFrame(base.AppendPoint + 240, SpawnWitherBeast);
			AppendSequences(30, DryadStand, DryadLookLeft);
			AppendSequences(30, DryadLookRight, DryadWalk);
			AppendSequences(300, DryadAttack, DryadLookLeft);
			AppendKeyFrame(RemoveEnemyDamage);
			AppendSequences(60, DryadLookRight, DryadStand, DryadAlertEmote);
			AddSequences(base.AppendPoint - 90, 60, OgreLookLeft, OgreStand);
			AddKeyFrame(base.AppendPoint - 12, OgreSwingSound);
			AddSequences(base.AppendPoint - 30, 50, DryadPortalKnock, DryadStand);
			AppendKeyFrame(RestoreEnemyDamage);
			AppendSequences(40, DryadPortalFade, DryadStand);
			AppendSequence(180, DryadStand);
			AddSequence(0, base.AppendPoint, PerFrameSettings);
		}

		private void PerFrameSettings(FrameEventData evt)
		{
			CombatText.clearAll();
		}

		private void CreateDryad(FrameEventData evt)
		{
			_dryad = PlaceNPCOnGround(20, _startPoint);
			_dryad.knockBackResist = 0f;
			_dryad.immortal = true;
			_dryad.dontTakeDamage = true;
			_dryad.takenDamageMultiplier = 0f;
			_dryad.immune[255] = 100000;
		}

		private void DryadInteract(FrameEventData evt)
		{
			if (_dryad != null)
			{
				_dryad.ai[0] = 9f;
				if (evt.IsFirstFrame)
				{
					_dryad.ai[1] = evt.Duration;
				}
				_dryad.localAI[0] = 0f;
			}
		}

		private void SpawnWitherBeast(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)_portal.Center.X, (int)_portal.Bottom.Y, 568);
			NPC nPC = Main.npc[num];
			nPC.knockBackResist = 0f;
			nPC.immortal = true;
			nPC.dontTakeDamage = true;
			nPC.takenDamageMultiplier = 0f;
			nPC.immune[255] = 100000;
			nPC.friendly = _ogre.friendly;
			_army.Add(nPC);
		}

		private void SpawnJavalinThrower(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)_portal.Center.X, (int)_portal.Bottom.Y, 561);
			NPC nPC = Main.npc[num];
			nPC.knockBackResist = 0f;
			nPC.immortal = true;
			nPC.dontTakeDamage = true;
			nPC.takenDamageMultiplier = 0f;
			nPC.immune[255] = 100000;
			nPC.friendly = _ogre.friendly;
			_army.Add(nPC);
		}

		private void SpawnGoblin(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)_portal.Center.X, (int)_portal.Bottom.Y, 552);
			NPC nPC = Main.npc[num];
			nPC.knockBackResist = 0f;
			nPC.immortal = true;
			nPC.dontTakeDamage = true;
			nPC.takenDamageMultiplier = 0f;
			nPC.immune[255] = 100000;
			nPC.friendly = _ogre.friendly;
			_army.Add(nPC);
		}

		private void CreateCritters(FrameEventData evt)
		{
			for (int i = 0; i < 5; i++)
			{
				float num = (float)i / 5f;
				NPC nPC = PlaceNPCOnGround(Utils.SelectRandom(Main.rand, new short[4] { 46, 46, 299, 538 }), _startPoint + new Vector2((num - 0.25f) * 400f + Main.rand.NextFloat() * 50f - 25f, 0f));
				nPC.ai[0] = 0f;
				nPC.ai[1] = 600f;
				_critters.Add(nPC);
			}
			if (_dryad != null)
			{
				for (int j = 0; j < 10; j++)
				{
					_ = (float)j / 10f;
					int num2 = NPC.NewNPC(new EntitySource_Film(), (int)_dryad.position.X + Main.rand.Next(-1000, 800), (int)_dryad.position.Y - Main.rand.Next(-50, 300), 356);
					NPC nPC2 = Main.npc[num2];
					nPC2.ai[0] = Main.rand.NextFloat() * 4f - 2f;
					nPC2.ai[1] = Main.rand.NextFloat() * 4f - 2f;
					nPC2.velocity.X = Main.rand.NextFloat() * 4f - 2f;
					_critters.Add(nPC2);
				}
			}
		}

		private void OgreSwingSound(FrameEventData evt)
		{
			SoundEngine.PlaySound(SoundID.DD2_OgreAttack, _ogre.Center);
		}

		private void DryadPortalKnock(FrameEventData evt)
		{
			if (_dryad != null)
			{
				if (evt.Frame == 20)
				{
					_dryad.velocity.Y -= 7f;
					_dryad.velocity.X -= 8f;
					SoundEngine.PlaySound(3, (int)_dryad.Center.X, (int)_dryad.Center.Y);
				}
				if (evt.Frame >= 20)
				{
					_dryad.ai[0] = 1f;
					_dryad.ai[1] = evt.Remaining;
					_dryad.rotation += 0.05f;
				}
			}
			if (_ogre != null)
			{
				if (evt.Frame > 40)
				{
					_ogre.target = Main.myPlayer;
					_ogre.direction = 1;
					return;
				}
				_ogre.direction = -1;
				_ogre.ai[1] = 0f;
				_ogre.ai[0] = Math.Min(40f, _ogre.ai[0]);
				_ogre.target = 300 + _dryad.whoAmI;
			}
		}

		private void RemoveEnemyDamage(FrameEventData evt)
		{
			_ogre.friendly = true;
			foreach (NPC item in _army)
			{
				item.friendly = true;
			}
		}

		private void RestoreEnemyDamage(FrameEventData evt)
		{
			_ogre.friendly = false;
			foreach (NPC item in _army)
			{
				item.friendly = false;
			}
		}

		private void DryadPortalFade(FrameEventData evt)
		{
			if (_dryad == null || _portal == null)
			{
				return;
			}
			if (evt.IsFirstFrame)
			{
				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, _dryad.Center);
			}
			float val = (float)(evt.Frame - 7) / (float)(evt.Duration - 7);
			val = Math.Max(0f, val);
			_dryad.color = new Color(Vector3.Lerp(Vector3.One, new Vector3(0.5f, 0f, 0.8f), val));
			_dryad.Opacity = 1f - val;
			_dryad.rotation += 0.05f * (val * 4f + 1f);
			_dryad.scale = 1f - val;
			if (_dryad.position.X < _portal.Right.X)
			{
				_dryad.velocity.X *= 0.95f;
				_dryad.velocity.Y *= 0.55f;
			}
			int num = (int)(6f * val);
			float num2 = _dryad.Size.Length() / 2f;
			num2 /= 20f;
			for (int i = 0; i < num; i++)
			{
				if (Main.rand.Next(5) == 0)
				{
					Dust dust = Dust.NewDustDirect(_dryad.position, _dryad.width, _dryad.height, 27, _dryad.velocity.X * 1f, 0f, 100);
					dust.scale = 0.55f;
					dust.fadeIn = 0.7f;
					dust.velocity *= 0.1f * num2;
					dust.velocity += _dryad.velocity;
				}
			}
		}

		private void CreatePortal(FrameEventData evt)
		{
			_portal = PlaceNPCOnGround(549, _startPoint + new Vector2(-240f, 0f));
			_portal.immortal = true;
		}

		private void DryadStand(FrameEventData evt)
		{
			if (_dryad != null)
			{
				_dryad.ai[0] = 0f;
				_dryad.ai[1] = evt.Remaining;
			}
		}

		private void DryadLookRight(FrameEventData evt)
		{
			if (_dryad != null)
			{
				_dryad.direction = 1;
				_dryad.spriteDirection = 1;
			}
		}

		private void DryadLookLeft(FrameEventData evt)
		{
			if (_dryad != null)
			{
				_dryad.direction = -1;
				_dryad.spriteDirection = -1;
			}
		}

		private void DryadWalk(FrameEventData evt)
		{
			_dryad.ai[0] = 1f;
			_dryad.ai[1] = 2f;
		}

		private void DryadConfusedEmote(FrameEventData evt)
		{
			if (_dryad != null && evt.IsFirstFrame)
			{
				EmoteBubble.NewBubble(87, new WorldUIAnchor(_dryad), evt.Duration);
			}
		}

		private void DryadAlertEmote(FrameEventData evt)
		{
			if (_dryad != null && evt.IsFirstFrame)
			{
				EmoteBubble.NewBubble(3, new WorldUIAnchor(_dryad), evt.Duration);
			}
		}

		private void CreateOgre(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)_portal.Center.X, (int)_portal.Bottom.Y, 576);
			_ogre = Main.npc[num];
			_ogre.knockBackResist = 0f;
			_ogre.immortal = true;
			_ogre.dontTakeDamage = true;
			_ogre.takenDamageMultiplier = 0f;
			_ogre.immune[255] = 100000;
		}

		private void OgreStand(FrameEventData evt)
		{
			if (_ogre != null)
			{
				_ogre.ai[0] = 0f;
				_ogre.ai[1] = 0f;
				_ogre.velocity = Vector2.Zero;
			}
		}

		private void DryadAttack(FrameEventData evt)
		{
			if (_dryad != null)
			{
				_dryad.ai[0] = 14f;
				_dryad.ai[1] = evt.Remaining;
				_dryad.dryadWard = false;
			}
		}

		private void OgreLookRight(FrameEventData evt)
		{
			if (_ogre != null)
			{
				_ogre.direction = 1;
				_ogre.spriteDirection = 1;
			}
		}

		private void OgreLookLeft(FrameEventData evt)
		{
			if (_ogre != null)
			{
				_ogre.direction = -1;
				_ogre.spriteDirection = -1;
			}
		}

		public override void OnBegin()
		{
			Main.NewText("DD2Film: Begin");
			Main.dayTime = true;
			Main.time = 27000.0;
			_startPoint = Main.screenPosition + new Vector2(Main.mouseX, (float)Main.mouseY - 32f);
			base.OnBegin();
		}

		private NPC PlaceNPCOnGround(int type, Vector2 position)
		{
			int num = (int)position.X;
			int num2 = (int)position.Y;
			int i = num / 16;
			int j;
			for (j = num2 / 16; !WorldGen.SolidTile(i, j); j++)
			{
			}
			num2 = j * 16;
			int start = 100;
			switch (type)
			{
			case 20:
				start = 1;
				break;
			case 576:
				start = 50;
				break;
			}
			int num3 = NPC.NewNPC(new EntitySource_Film(), num, num2, type, start);
			return Main.npc[num3];
		}

		public override void OnEnd()
		{
			if (_dryad != null)
			{
				_dryad.active = false;
			}
			if (_portal != null)
			{
				_portal.active = false;
			}
			if (_ogre != null)
			{
				_ogre.active = false;
			}
			foreach (NPC critter in _critters)
			{
				critter.active = false;
			}
			foreach (NPC item in _army)
			{
				item.active = false;
			}
			Main.NewText("DD2Film: End");
			base.OnEnd();
		}
	}
}
