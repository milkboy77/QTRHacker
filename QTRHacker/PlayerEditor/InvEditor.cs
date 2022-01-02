﻿using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Controls;
using QTRHacker.PlayerEditor.Controls;
using QTRHacker.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public class InvEditor : FlowItemSlotsEditor
	{
		public InvEditor(GameContext Context, Player TargetPlayer, bool Editable) :
			base(Context, TargetPlayer, TargetPlayer.Inventory, HackContext.CurrentLanguage["Inventory"], Editable, Player.ITEM_MAX_COUNT - 9)
		{
			Button SaveInv = ButtonStrip.AddButton(HackContext.CurrentLanguage["Save"]);
			SaveInv.Click += (sender, e) =>
			{
				SaveFileDialog sfd = new SaveFileDialog()
				{
					Filter = "inv files (*.inv)|*.inv",
				};
				sfd.InitialDirectory = Path.GetFullPath(HackContext.PATH_INVS);
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					SaveInventory(sfd.FileName);
					SlotsPanel.Refresh();
				}
			};

			Button LoadInv = ButtonStrip.AddButton(HackContext.CurrentLanguage["Load"]);
			LoadInv.Enabled = Editable;
			LoadInv.Click += (sender, e) =>
			{
				OpenFileDialog ofd = new()
				{
					Filter = "inv files (*.inv)|*.inv",
				};
				if (!Directory.Exists(HackContext.PATH_INVS))
					Directory.CreateDirectory(HackContext.PATH_INVS);
				ofd.InitialDirectory = Path.GetFullPath(HackContext.PATH_INVS);
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					LoadInventory(ofd.FileName);
					SlotsPanel.Refresh();
					FetchItemData(SelectedItem);
				}
			};
		}


		public void SaveInventory(string name)
		{
			if (File.Exists(name)) File.Delete(name);
			BinaryWriter bw = new BinaryWriter(new FileStream(name, FileMode.OpenOrCreate));
			var player = TargetPlayer;
			for (int i = 0; i < Player.ITEM_MAX_COUNT; i++)
			{
				var item = player.Inventory[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.ARMOR_MAX_COUNT; i++)
			{
				var item = player.Armor[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.DYE_MAX_COUNT; i++)
			{
				var item = player.Dye[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.MISC_MAX_COUNT; i++)
			{
				var item = player.MiscEquips[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.MISCDYE_MAX_COUNT; i++)
			{
				var item = player.MiscDyes[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			bw.Close();
		}
		public void LoadInventory(string name)
		{
			var form = FindForm();
			new ProgressPopupForm(form.Width / 4 * 3,
				Player.ITEM_MAX_COUNT + Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT + Player.MISCDYE_MAX_COUNT, "Loading Inventory").
				Run(form, (tick) =>
				{
					int j = 0;
					var player = TargetPlayer;
					using BinaryReader br = new BinaryReader(new FileStream(name, FileMode.Open));
					for (int i = 0; i < Player.ITEM_MAX_COUNT; i++)
					{
						j++;
						var item = player.Inventory[i];
						int type = br.ReadInt32();
						int stack = br.ReadInt32();
						byte prefix = br.ReadByte();
						if (type <= 0 && item.Type <= 0) continue;
						item.SetDefaultsAndPrefix(type, prefix);
						item.Stack = stack;
						tick(j);
					}
					for (int i = 0; i < Player.ARMOR_MAX_COUNT; i++)
					{
						j++;
						var item = player.Armor[i];
						int type = br.ReadInt32();
						int stack = br.ReadInt32();
						byte prefix = br.ReadByte();
						if (type <= 0 && item.Type <= 0) continue;
						item.SetDefaultsAndPrefix(type, prefix);
						item.Stack = stack;
						tick(j);
					}
					for (int i = 0; i < Player.DYE_MAX_COUNT; i++)
					{
						j++;
						var item = player.Dye[i];
						int type = br.ReadInt32();
						int stack = br.ReadInt32();
						byte prefix = br.ReadByte();
						if (type <= 0 && item.Type <= 0) continue;
						item.SetDefaultsAndPrefix(type, prefix);
						item.Stack = stack;
						tick(j);
					}
					for (int i = 0; i < Player.MISC_MAX_COUNT; i++)
					{
						j++;
						var item = player.MiscEquips[i];
						int type = br.ReadInt32();
						int stack = br.ReadInt32();
						byte prefix = br.ReadByte();
						if (type <= 0 && item.Type <= 0) continue;
						item.SetDefaultsAndPrefix(type, prefix);
						item.Stack = stack;
						tick(j);
					}
					for (int i = 0; i < Player.MISCDYE_MAX_COUNT; i++)
					{
						j++;
						var item = player.MiscDyes[i];
						int type = br.ReadInt32();
						int stack = br.ReadInt32();
						byte prefix = br.ReadByte();
						if (type <= 0 && item.Type <= 0) continue;
						item.SetDefaultsAndPrefix(type, prefix);
						item.Stack = stack;
						tick(j);
					}
				});
		}
	}
}
