﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk
{
	public class PadSchema
	{
		public enum PadInputType
		{
			Boolean,		// A single on/off button
			AnalogStick,	// An analog stick X,Y Pair
			FloatSingle,	// A single analog button (pressure sensitive button for instance)
			TargetedPair	// A X,Y pair intended to be a screen cooridnate (for zappers, mouse, stylus, etc)
		}

		// Default size of the pad
		public Size DefaultSize { get; set; }
		public bool IsConsole { get; set; }
		public IEnumerable<ButtonScema> Buttons { get; set; }

		public class ButtonScema
		{
			public string Name { get; set; }
			public string DisplayName { get; set; }
			public PadInputType Type { get; set; }
			public Point Location { get; set; }
			public Bitmap Icon { get; set; }
			public Size TargetSize { get; set; } // Specifically for TargetedPair, specifies the screen size
			public string[] SecondaryNames { get; set; } // Any other buttons necessary to operate (such as the Y axis, fire buttons, etc
			public int MaxValue { get; set; } // For non-boolean values, specifies the maximum value the button allows
		}
	}


}