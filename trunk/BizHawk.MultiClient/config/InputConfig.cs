﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BizHawk.MultiClient
{
	//TODO:
	//Remove AppendMapping and TruncateMapping functions

	public partial class InputConfig : Form
	{
		int prevWidth;
		int prevHeight;
		const string ControllerStr = "Configure Controllers - ";
		public static string[] NESControlList = new string[] { "Up", "Down", "Left", "Right", "A", "B", "Select", "Start" };
		public static readonly Dictionary<string, string[]> CONTROLS = new Dictionary<string, string[]>()
		{
			{"NES", new string[8] { "Up", "Down", "Left", "Right", "A", "B", "Select", "Start" } },
			{"PC Engine / SuperGrafx", new string[8] { "Up", "Down", "Left", "Right", "I", "II", "Run", "Select" } },
			{"Sega Genesis", new string[8] { "Up", "Down", "Left", "Right", "A", "B", "C", "Start" } },
			{"SMS / GG / SG-1000", new string[8] { "Up", "Down", "Left", "Right", "B1", "B2", "Pause", "Reset" } },
			{
				// TODO: display shift / alpha names too, Also order these like on the calculator
				"TI-83", new string[50] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "ON",
				"ENTER", "Up", "Down", "Left", "Right", "+", "-", "Multiply", "Divide", "CLEAR", "^", "-", "(", ")", "TAN",
				"VARS", "COS", "PRGM", "STAT", "Matrix", "X", "STO->", "LN", "LOG", "^2", "^-1", "MATH", "ALPHA", "GRAPH",
				"TRACE", "ZOOM", "WINDOW", "Y", "2nd", "MODE", "Del", ",", "SIN" }
			}
		};

		public static readonly string[] TI83CONTROLS = new string[50] {
			"_0", "_1", "_2", "_3", "_4", "_5", "_6", "_7", "_8", "_9", "DOT", "ON", "ENTER", "UP", "DOWN", "LEFT", "RIGHT",
			"PLUS", "MINUS", "MULTIPLY", "DIVIDE", "CLEAR", "EXP", "DASH", "PARAOPEN", "PARACLOSE", "TAN", "VARS", "COS",
			"PRGM", "STAT", "MATRIX", "X", "STO", "LN", "LOG", "SQUARED", "NEG1", "MATH", "ALPHA", "GRAPH", "TRACE", "ZOOM",
			"WINDOW", "Y", "SECOND", "MODE", "DEL", "COMMA", "SIN"
		};

		public static readonly Dictionary<string, int> PADS = new Dictionary<string, int>()
		{
			{"Atari", 2}, {"Gameboy", 1}, {"NES", 4}, {"PC Engine / SuperGrafx", 5}, {"Sega Genesis", 1}, {"SMS / GG / SG-1000", 2},
			{"TI-83", 1}
		};
		public static string[] AtariControlList = new string[] { "Up", "Down", "Left", "Right", "Button" };
		private ArrayList Labels;
		private ArrayList TextBoxes;
		private string CurSelectConsole;
		private int CurSelectController;
		private bool Changed;

		public InputConfig()
		{
			InitializeComponent();
			Labels = new ArrayList();
			TextBoxes = new ArrayList();
			Changed = false;
		}

		protected override void OnShown(EventArgs e)
		{
			Input.Instance.EnableIgnoreModifiers = true;
			base.OnShown(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Input.Instance.EnableIgnoreModifiers = false;
		}

		private string AppendButtonMapping(string button, string oldmap)
		{
			//adelikat: Another relic, remove this
			//int x = oldmap.LastIndexOf(',');
			//if (x != -1)
			//	return oldmap.Substring(0, x + 2) + button;
			//else
			return button;
		}

		private void DoAtari()
		{
			Label TempLabel;
			InputWidget TempTextBox;
			this.Text = ControllerStr + "Atari";
			ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.atari_controller;
			int jpad = this.ControllComboBox.SelectedIndex;
			string[] ButtonMappings = new string[AtariControlList.Length];
			int controllers = 2;
			if (jpad < controllers)
			{
				ButtonMappings[0] = Global.Config.Atari2600Controller[jpad].Up;
				ButtonMappings[1] = Global.Config.Atari2600Controller[jpad].Down;
				ButtonMappings[2] = Global.Config.Atari2600Controller[jpad].Left;
				ButtonMappings[3] = Global.Config.Atari2600Controller[jpad].Right;
				ButtonMappings[4] = Global.Config.Atari2600Controller[jpad].Button;
				IDX_CONTROLLERENABLED.Checked = Global.Config.Atari2600Controller[jpad].Enabled;
			}
			else
			{
				ButtonMappings[0] = Global.Config.Atari2600AutoController[controllers - jpad].Up;
				ButtonMappings[1] = Global.Config.Atari2600AutoController[controllers - jpad].Down;
				ButtonMappings[2] = Global.Config.Atari2600AutoController[controllers - jpad].Left;
				ButtonMappings[3] = Global.Config.Atari2600AutoController[controllers - jpad].Right;
				ButtonMappings[4] = Global.Config.Atari2600AutoController[controllers - jpad].Button;
				IDX_CONTROLLERENABLED.Checked = Global.Config.Atari2600AutoController[controllers - jpad].Enabled;
			}

			Changed = true;
			Labels.Clear();
			TextBoxes.Clear();

			for (int i = 0; i < AtariControlList.Length; i++)
			{
				TempLabel = new Label();
				TempLabel.Text = AtariControlList[i];
				TempLabel.Location = new Point(8, 20 + (i * 24));
				Labels.Add(TempLabel);
				TempTextBox = new InputWidget();
				TempTextBox.Location = new Point(48, 20 + (i * 24));
				TextBoxes.Add(TempTextBox);
				TempTextBox.SetBindings(ButtonMappings[i]);
				ButtonsGroupBox.Controls.Add(TempTextBox);
				ButtonsGroupBox.Controls.Add(TempLabel);
			}
			
			Changed = true;
		}

		private void UpdateAtari(int prev)
		{
			ButtonsGroupBox.Controls.Clear();
			InputWidget TempBox;
			Label TempLabel;
			int controllers = 2;
			if (prev < controllers)
			{
				TempBox = TextBoxes[0] as InputWidget;
				Global.Config.Atari2600Controller[prev].Up = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600Controller[prev].Up);
				TempBox.Dispose();
				TempBox = TextBoxes[1] as InputWidget;
				Global.Config.Atari2600Controller[prev].Down = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600Controller[prev].Down);
				TempBox.Dispose();
				TempBox = TextBoxes[2] as InputWidget;
				Global.Config.Atari2600Controller[prev].Left = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600Controller[prev].Left);
				TempBox.Dispose();
				TempBox = TextBoxes[3] as InputWidget;
				Global.Config.Atari2600Controller[prev].Right = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600Controller[prev].Right);
				TempBox.Dispose();
				TempBox = TextBoxes[4] as InputWidget;
				Global.Config.Atari2600Controller[prev].Button = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600Controller[prev].Button);
				TempBox.Dispose();

				Global.Config.Atari2600Controller[prev].Enabled = IDX_CONTROLLERENABLED.Checked;
			}
			else
			{
				TempBox = TextBoxes[0] as InputWidget;
				Global.Config.Atari2600AutoController[prev - controllers].Up = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600AutoController[prev - 1].Up);
				TempBox.Dispose();
				TempBox = TextBoxes[1] as InputWidget;
				Global.Config.Atari2600AutoController[prev - controllers].Down = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600AutoController[prev - 1].Down);
				TempBox.Dispose();
				TempBox = TextBoxes[2] as InputWidget;
				Global.Config.Atari2600AutoController[prev - controllers].Left = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600AutoController[prev - 1].Left);
				TempBox.Dispose();
				TempBox = TextBoxes[3] as InputWidget;
				Global.Config.Atari2600AutoController[prev - controllers].Right = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600AutoController[prev - 1].Right);
				TempBox.Dispose();
				TempBox = TextBoxes[4] as InputWidget;
				Global.Config.Atari2600AutoController[prev - controllers].Button = AppendButtonMapping(TempBox.Text, Global.Config.Atari2600AutoController[prev - 1].Button);
				TempBox.Dispose();

				Global.Config.Atari2600AutoController[prev - controllers].Enabled = IDX_CONTROLLERENABLED.Checked;
			}
			TempBox.Dispose();
			for (int i = 0; i < AtariControlList.Length; i++)
			{
				TempLabel = Labels[i] as Label;
				TempLabel.Dispose();
			}
		}

		private void DoGameBoy()
		{
			Label TempLabel;
			InputWidget TempTextBox;
			this.Text = ControllerStr + "Gameboy";
			ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.GBController;
			string[] ButtonMappings = new string[NESControlList.Length];
			ButtonMappings[0] = Global.Config.GameBoyController.Up;
			ButtonMappings[1] = Global.Config.GameBoyController.Down;
			ButtonMappings[2] = Global.Config.GameBoyController.Left;
			ButtonMappings[3] = Global.Config.GameBoyController.Right;
			ButtonMappings[4] = Global.Config.GameBoyController.A;
			ButtonMappings[5] = Global.Config.GameBoyController.B;
			ButtonMappings[6] = Global.Config.GameBoyController.Start;
			ButtonMappings[7] = Global.Config.GameBoyController.Select;
			IDX_CONTROLLERENABLED.Enabled = false;
			Changed = true;
			Labels.Clear();
			TextBoxes.Clear();
			for (int i = 0; i < NESControlList.Length; i++)
			{
				TempLabel = new Label();
				TempLabel.Text = NESControlList[i];
				TempLabel.Location = new Point(8, 20 + (i * 24));
				Labels.Add(TempLabel);
				TempTextBox = new InputWidget();
				TempTextBox.Location = new Point(48, 20 + (i * 24));
				TextBoxes.Add(TempTextBox);
				TempTextBox.SetBindings(ButtonMappings[i]);
				ButtonsGroupBox.Controls.Add(TempTextBox);
				ButtonsGroupBox.Controls.Add(TempLabel);
			}
			Changed = true;
		}

		private void UpdateGameBoy()
		{
			ButtonsGroupBox.Controls.Clear();
			InputWidget TempBox;
			Label TempLabel;
			TempBox = TextBoxes[0] as InputWidget;
			Global.Config.GameBoyController.Up = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.Up);
			TempBox.Dispose();
			TempBox = TextBoxes[1] as InputWidget;
			Global.Config.GameBoyController.Down = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.Down);
			TempBox.Dispose();
			TempBox = TextBoxes[2] as InputWidget;
			Global.Config.GameBoyController.Left = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.Left);
			TempBox.Dispose();
			TempBox = TextBoxes[3] as InputWidget;
			Global.Config.GameBoyController.Right = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.Right);
			TempBox.Dispose();
			TempBox = TextBoxes[4] as InputWidget;
			Global.Config.GameBoyController.A = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.A);
			TempBox.Dispose();
			TempBox = TextBoxes[5] as InputWidget;
			Global.Config.GameBoyController.B = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.B);
			TempBox.Dispose();
			TempBox = TextBoxes[6] as InputWidget;
			Global.Config.GameBoyController.Start = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.Start);
			TempBox.Dispose();
			TempBox = TextBoxes[7] as InputWidget;
			Global.Config.GameBoyController.Select = AppendButtonMapping(TempBox.Text, Global.Config.GameBoyController.Select);
			TempBox.Dispose();
			for (int i = 0; i < NESControlList.Length; i++)
			{
				TempLabel = Labels[i] as Label;
				TempLabel.Dispose();
			}
			IDX_CONTROLLERENABLED.Enabled = true;
		}

		private void Do(string platform)
		{
			Label TempLabel;
			InputWidget TempTextBox;
			this.Text = ControllerStr + platform;
			object[] controller = null;
			object[] mainController = null;
			object[] autoController = null;
			switch (platform)
			{
				case "Sega Genesis":
					ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.GENController;
					controller = Global.Config.GenesisController;
					autoController = Global.Config.GenesisAutoController;
					break;
				case "NES":
					ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.NESController;
					controller = Global.Config.NESController;
					autoController = Global.Config.NESAutoController;
					break;
				case "PC Engine / SuperGrafx":
					ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.PCEngineController;
					controller = Global.Config.PCEController;
					autoController = Global.Config.PCEAutoController;
					break;
				case "SMS / GG / SG-1000":
					ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.SMSController;
					controller = Global.Config.SMSController;
					autoController = Global.Config.SMSAutoController;
					break;
				case "TI-83":
					ControllerImage.Image = BizHawk.MultiClient.Properties.Resources.TI83CalculatorCrop;
					controller = Global.Config.TI83Controller;
					break;
				default:
					return;
			}
			mainController = controller;
			int jpad = this.ControllComboBox.SelectedIndex;
			if (jpad >= PADS[platform])
			{
				jpad -= PADS[platform];
				controller = autoController;
			}
			switch (platform)
			{
				case "Sega Genesis":
					IDX_CONTROLLERENABLED.Checked = ((GenControllerTemplate)mainController[jpad]).Enabled;
					break;
				case "NES":
					IDX_CONTROLLERENABLED.Checked = ((NESControllerTemplate)mainController[jpad]).Enabled;
					break;
				case "PC Engine / SuperGrafx":
					IDX_CONTROLLERENABLED.Checked = ((PCEControllerTemplate)mainController[jpad]).Enabled;
					break;
				case "SMS / GG / SG-1000":
					IDX_CONTROLLERENABLED.Checked = ((SMSControllerTemplate)mainController[jpad]).Enabled;
					break;
				case "TI-83":
					IDX_CONTROLLERENABLED.Checked = ((TI83ControllerTemplate)mainController[jpad]).Enabled;
					break;
			}
			Labels.Clear();
			TextBoxes.Clear();
			int row = 0;
			int col = 0;
			for (int button = 0; button < CONTROLS[platform].Length; button++)
			{
				TempLabel = new Label();
				TempLabel.Text = CONTROLS[platform][button];
				int xoffset = (col * 156);
				int yoffset = (row * 24);
				TempLabel.Location = new Point(8 + xoffset, 20 + yoffset);
				Labels.Add(TempLabel);
				TempTextBox = new InputWidget();
				TempTextBox.Location = new Point(64 + xoffset, 20 + yoffset);
				TextBoxes.Add(TempTextBox);
				object field = null;
				string fieldName = CONTROLS[platform][button];
				switch (platform)
				{
					case "Sega Genesis":
					{
						GenControllerTemplate obj = (GenControllerTemplate)controller[jpad];
						field = obj.GetType().GetField(fieldName).GetValue(obj);
						break;
					}
					case "NES":
					{
						NESControllerTemplate obj = (NESControllerTemplate)controller[jpad];
						field = obj.GetType().GetField(fieldName).GetValue(obj);
						break;
					}
					case "PC Engine / SuperGrafx":
					{
						PCEControllerTemplate obj = (PCEControllerTemplate)controller[jpad];
						field = obj.GetType().GetField(fieldName).GetValue(obj);
						break;
					}
					case "SMS / GG / SG-1000":
					{
						if (button < 6)
						{
							SMSControllerTemplate obj = (SMSControllerTemplate)controller[jpad];
							field = obj.GetType().GetField(fieldName).GetValue(obj);
						}
						else if (button == 6)
							field = Global.Config.SmsPause;
						else
							field = Global.Config.SmsReset;
						break;
					}
					case "TI-83":
					{
						TI83ControllerTemplate obj = (TI83ControllerTemplate)controller[jpad];
						field = obj.GetType().GetField(TI83CONTROLS[button]).GetValue(obj);
						break;
					}
				}
				TempTextBox.SetBindings((string)field);
				ButtonsGroupBox.Controls.Add(TempTextBox);
				ButtonsGroupBox.Controls.Add(TempLabel);
				row++;
				if (row > 16)
				{
					row = 0;
					col++;
				}
			}
			Changed = true;
		}

		private void Update(int prev, string platform)
		{
			ButtonsGroupBox.Controls.Clear();
			object[] controller = null;
			object[] mainController = null;
			object[] autoController = null;
			switch (platform)
			{
				case "Sega Genesis":
					controller = Global.Config.GenesisController;
					autoController = Global.Config.GenesisAutoController;
					break;
				case "NES":
					controller = Global.Config.NESController;
					autoController = Global.Config.NESAutoController;
					break;
				case "PC Engine / SuperGrafx":
					controller = Global.Config.PCEController;
					autoController = Global.Config.PCEAutoController;
					break;
				case "SMS / GG / SG-1000":
					controller = Global.Config.SMSController;
					autoController = Global.Config.SMSAutoController;
					break;
				case "TI-83":
					controller = Global.Config.TI83Controller;
					break;
				default:
					return;
			}
			mainController = controller;
			if (prev >= PADS[platform])
			{
				prev -= PADS[platform];
				controller = autoController;
			}
			switch (platform)
			{
				case "Sega Genesis":
					((GenControllerTemplate)mainController[prev]).Enabled = IDX_CONTROLLERENABLED.Checked;
					break;
				case "NES":
					((NESControllerTemplate)mainController[prev]).Enabled = IDX_CONTROLLERENABLED.Checked;
					break;
				case "PC Engine / SuperGrafx":
					((PCEControllerTemplate)mainController[prev]).Enabled = IDX_CONTROLLERENABLED.Checked;
					break;
				case "SMS / GG / SG-1000":
					((SMSControllerTemplate)mainController[prev]).Enabled = IDX_CONTROLLERENABLED.Checked;
					break;
				case "TI-83":
					((TI83ControllerTemplate)mainController[prev]).Enabled = IDX_CONTROLLERENABLED.Checked;
					break;
			}
			for (int button = 0; button < CONTROLS[platform].Length; button++)
			{
				InputWidget TempBox = TextBoxes[button] as InputWidget;
				object field = null;
				string fieldName = CONTROLS[platform][button];
				switch (platform)
				{
					case "Sega Genesis":
					{
						GenControllerTemplate obj = (GenControllerTemplate)controller[prev];
						FieldInfo buttonField = obj.GetType().GetField(fieldName);
						field = buttonField.GetValue(obj);
						buttonField.SetValue(obj, AppendButtonMapping(TempBox.Text, (string)field));
						break;
					}
					case "NES":
					{
						NESControllerTemplate obj = (NESControllerTemplate)controller[prev];
						FieldInfo buttonField = obj.GetType().GetField(fieldName);
						field = buttonField.GetValue(obj);
						buttonField.SetValue(obj, AppendButtonMapping(TempBox.Text, (string)field));
						break;
					}
					case "PC Engine / SuperGrafx":
					{
						PCEControllerTemplate obj = (PCEControllerTemplate)controller[prev];
						FieldInfo buttonField = obj.GetType().GetField(fieldName);
						field = buttonField.GetValue(obj);
						buttonField.SetValue(obj, AppendButtonMapping(TempBox.Text, (string)field));
						break;
					}
					case "SMS / GG / SG-1000":
					{
						if (button < 6)
						{
							SMSControllerTemplate obj = (SMSControllerTemplate)controller[prev];
							FieldInfo buttonField = obj.GetType().GetField(fieldName);
							field = buttonField.GetValue(obj);
							buttonField.SetValue(obj, AppendButtonMapping(TempBox.Text, (string)field));
						}
						else if (button == 6)
							Global.Config.SmsPause = AppendButtonMapping(TempBox.Text, Global.Config.SmsPause);
						else
							Global.Config.SmsReset = AppendButtonMapping(TempBox.Text, Global.Config.SmsReset);
						break;
					}
					case "TI-83":
					{
						TI83ControllerTemplate obj = (TI83ControllerTemplate)controller[prev];
						FieldInfo buttonField = obj.GetType().GetField(TI83CONTROLS[button]);
						field = buttonField.GetValue(obj);
						buttonField.SetValue(obj, AppendButtonMapping(TempBox.Text, (string)field));
						break;
					}
				}
				TempBox.Dispose();
				Label TempLabel = Labels[button] as Label;
				TempLabel.Dispose();
			}
		}

		private void InputConfig_Load(object sender, EventArgs e)
		{
			if (Global.MainForm.INTERIM)
				SystemComboBox.Items.Add("Atari"); //When Atari is ready, add this in the designer instead

			AutoTab.Checked = Global.Config.InputConfigAutoTab;
			SetAutoTab();
			prevWidth = Size.Width;
			prevHeight = Size.Height;
			AllowLR.Checked = Global.Config.AllowUD_LR;

			if (Global.Game != null)
			{
				Dictionary<string, string> systems = new Dictionary<string, string>()
				{
					{"A26", "Atari"}, {"GB", "Gameboy"}, {"GEN", "Sega Genesis"}, {"GG", "SMS / GG / SG-1000"}, {"NES", "NES"},
					{"PCE", "PC Engine / SuperGrafx"}, {"SG", "SMS / GG / SG-1000"}, {"SGX", "PC Engine / SuperGrafx"},
					{"SMS", "SMS / GG / SG-1000"}, {"TI83", "TI-83"}
				};
				if (systems.ContainsKey(Global.Game.System))
					this.SystemComboBox.SelectedIndex = SystemComboBox.Items.IndexOf(systems[Global.Game.System]);
				else
					this.SystemComboBox.SelectedIndex = 0;
			}
		}

		private void OK_Click(object sender, EventArgs e)
		{
			if (Changed)
			{
				UpdateAll();
			}
			this.DialogResult = DialogResult.OK;
			Global.Config.AllowUD_LR = AllowLR.Checked;
			this.Close();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void SystemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Changed)
			{
				UpdateAll();
			}
			int joypads = PADS[this.SystemComboBox.SelectedItem.ToString()];
			if (this.SystemComboBox.SelectedItem.ToString() != "TI-83")
			{
				this.Width = prevWidth;
				this.Height = prevHeight;
			}
			else
			{
				if (this.Width < 700)
					this.Width = 700;
				if (this.Height < 580)
					this.Height = 580;
			}
			ControllComboBox.Items.Clear();
			for (int i = 0; i < joypads; i++)
			{
				ControllComboBox.Items.Add(string.Format("Joypad {0}", i + 1));
			}
			for (int i = 0; i < joypads; i++)
			{
				if (this.SystemComboBox.SelectedItem.ToString() != "TI-83")
					ControllComboBox.Items.Add(string.Format("Autofire Joypad {0}", i + 1));
			}
			ControllComboBox.SelectedIndex = 0;
			CurSelectConsole = this.SystemComboBox.SelectedItem.ToString();
			CurSelectController = 0;
			SetFocus();
		}
		private void ControllComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Changed)
			{
				UpdateAll();
			}
			switch (SystemComboBox.SelectedItem.ToString())
			{
				case "NES":
				case "PC Engine / SuperGrafx":
				case "Sega Genesis":
				case "SMS / GG / SG-1000":
				case "TI-83":
					Do(SystemComboBox.SelectedItem.ToString());
					break;
				case "Gameboy":
					DoGameBoy();
					break;
				case "Atari":
					DoAtari();
					break;
			}
			CurSelectController = ControllComboBox.SelectedIndex;
			SetFocus();
		}
		private void UpdateAll()
		{
			switch (CurSelectConsole)
			{
				case "NES":
				case "PC Engine / SuperGrafx":
				case "Sega Genesis":
				case "SMS / GG / SG-1000":
				case "TI-83":
					Update(CurSelectController, CurSelectConsole);
					break;
				case "Gameboy":
					UpdateGameBoy();
					break;
				case "Atari":
					UpdateAtari(CurSelectController);
					break;
			}
			Changed = false;
		}

		private void AutoTab_CheckedChanged(object sender, EventArgs e)
		{
			Global.Config.HotkeyConfigAutoTab = AutoTab.Checked;
			SetAutoTab();
		}

		private void SetFocus()
		{
			for (int x = 0; x < ButtonsGroupBox.Controls.Count; x++)
			{
				if (ButtonsGroupBox.Controls[x] is InputWidget)
				{
					ButtonsGroupBox.Controls[x].Focus();
					return;
				}
			}
		}

		private void SetAutoTab()
		{
			for (int x = 0; x < ButtonsGroupBox.Controls.Count; x++)
			{
				if (ButtonsGroupBox.Controls[x] is InputWidget)
				{
					InputWidget w = ButtonsGroupBox.Controls[x] as InputWidget;
					w.AutoTab = AutoTab.Checked;
				}
			}
		}
	}
}