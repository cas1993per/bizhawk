﻿using System.Collections.Generic;
using System.Drawing;

using BizHawk.Client.Common;
using BizHawk.Emulation.Cores.Sony.PSX;

namespace BizHawk.Client.EmuHawk
{
	[SchemaAttributes("PSX")]
	public class PSXSchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas()
		{
			yield return DualShockController(1);
			yield return DualShockController(2);
			yield return ConsoleButtons();
		}

		public static PadSchema DualShockController(int controller)
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(420, 260),
				Buttons = new[]
				{
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Up",
						DisplayName = "",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(32, 50),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Down",
						DisplayName = "",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(32, 71),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Left",
						DisplayName = "",
						Icon = Properties.Resources.Back,
						Location = new Point(11, 62),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Right",
						DisplayName = "",
						Icon = Properties.Resources.Forward,
						Location = new Point(53, 62),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " L1",
						DisplayName = "L1",
						Location = new Point(3, 32),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " R1",
						DisplayName = "R1",
						Location = new Point(191, 32),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " L2",
						DisplayName = "L2",
						Location = new Point(3, 10),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " R2",
						DisplayName = "R2",
						Location = new Point(191, 10),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " L3",
						DisplayName = "L3",
						Location = new Point(72, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " R3",
						DisplayName = "R3",
						Location = new Point(130, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Square",
						DisplayName = "",
						Icon = Properties.Resources.Square,
						Location = new Point(148, 62),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Triangle",
						DisplayName = "",
						Icon = Properties.Resources.Triangle,
						Location = new Point(169, 50),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Circle",
						DisplayName = "",
						Icon = Properties.Resources.Circle,
						Location = new Point(190, 62),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Cross",
						DisplayName = "",
						Icon = Properties.Resources.Cross,
						Location = new Point(169, 71),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Start",
						DisplayName = "S",
						Location = new Point(112, 62),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " Select",
						DisplayName = "s",
						Location = new Point(90, 62),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "P" + controller + " LStick X",
						MaxValue = 127,
						DisplayName = "",
						Location = new Point(3, 120),
						Type = PadSchema.PadInputType.AnalogStick
					},
										new PadSchema.ButtonScema
					{
						Name = "P" + controller + " RStick X",
						MaxValue = 127,
						DisplayName = "",
						Location = new Point(210, 120),
						Type = PadSchema.PadInputType.AnalogStick
					}
				}
			};
		}
		private static PadSchema ConsoleButtons()
		{
			return new PadSchema
			{
				DisplayName = "Console",
				IsConsole = true,
				DefaultSize = new Size(360, 250),
				Buttons = new[]
				{
					new PadSchema.ButtonScema
					{
						Name = "Eject",
						DisplayName = "Eject",
						Location = new Point(10, 15),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "Reset",
						DisplayName = "Reset",
						Location = new Point(60, 15),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonScema
					{
						Name = "Disc Select",
						MinValue = 1,
						MaxValue = 5,
						DisplayName = "Disc Select",
						Location = new Point(10, 40),
						TargetSize = new Size(300,100),
						Type = PadSchema.PadInputType.FloatSingle
					}
				}
			};
		}
	}
}
