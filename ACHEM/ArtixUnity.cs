using UnityEngine;

public static class ArtixUnity
{
	public static string KeyCodeName(KeyCode keyCode)
	{
		return keyCode switch
		{
			KeyCode.None => "None", 
			KeyCode.Backspace => "Backspace", 
			KeyCode.Tab => "Tab", 
			KeyCode.Clear => "Clear", 
			KeyCode.Return => "Return", 
			KeyCode.Pause => "Pause", 
			KeyCode.Escape => "Esc", 
			KeyCode.Space => "Space", 
			KeyCode.Exclaim => "!", 
			KeyCode.DoubleQuote => "\"", 
			KeyCode.Hash => "#", 
			KeyCode.Dollar => "$", 
			KeyCode.Ampersand => "&", 
			KeyCode.Quote => "'", 
			KeyCode.LeftParen => "(", 
			KeyCode.RightParen => ")", 
			KeyCode.Asterisk => "*", 
			KeyCode.Plus => "+", 
			KeyCode.Comma => ",", 
			KeyCode.Minus => "-", 
			KeyCode.Period => ".", 
			KeyCode.Slash => "/", 
			KeyCode.Alpha0 => "0", 
			KeyCode.Alpha1 => "1", 
			KeyCode.Alpha2 => "2", 
			KeyCode.Alpha3 => "3", 
			KeyCode.Alpha4 => "4", 
			KeyCode.Alpha5 => "5", 
			KeyCode.Alpha6 => "6", 
			KeyCode.Alpha7 => "7", 
			KeyCode.Alpha8 => "8", 
			KeyCode.Alpha9 => "9", 
			KeyCode.Colon => ":", 
			KeyCode.Semicolon => ";", 
			KeyCode.Less => "<", 
			KeyCode.Equals => "=", 
			KeyCode.Greater => ">", 
			KeyCode.Question => "?", 
			KeyCode.At => "@", 
			KeyCode.LeftBracket => "[", 
			KeyCode.Backslash => "\\", 
			KeyCode.RightBracket => "]", 
			KeyCode.Caret => "^", 
			KeyCode.Underscore => "_", 
			KeyCode.BackQuote => "`", 
			KeyCode.A => "A", 
			KeyCode.B => "B", 
			KeyCode.C => "C", 
			KeyCode.D => "D", 
			KeyCode.E => "E", 
			KeyCode.F => "F", 
			KeyCode.G => "G", 
			KeyCode.H => "H", 
			KeyCode.I => "I", 
			KeyCode.J => "J", 
			KeyCode.K => "K", 
			KeyCode.L => "L", 
			KeyCode.M => "M", 
			KeyCode.N => "N", 
			KeyCode.O => "O", 
			KeyCode.P => "P", 
			KeyCode.Q => "Q", 
			KeyCode.R => "R", 
			KeyCode.S => "S", 
			KeyCode.T => "T", 
			KeyCode.U => "U", 
			KeyCode.V => "V", 
			KeyCode.W => "W", 
			KeyCode.X => "X", 
			KeyCode.Y => "Y", 
			KeyCode.Z => "Z", 
			KeyCode.Delete => "Del", 
			KeyCode.Keypad0 => "Keypad 0", 
			KeyCode.Keypad1 => "Keypad 1", 
			KeyCode.Keypad2 => "Keypad 2", 
			KeyCode.Keypad3 => "Keypad 3", 
			KeyCode.Keypad4 => "Keypad 4", 
			KeyCode.Keypad5 => "Keypad 5", 
			KeyCode.Keypad6 => "Keypad 6", 
			KeyCode.Keypad7 => "Keypad 7", 
			KeyCode.Keypad8 => "Keypad 8", 
			KeyCode.Keypad9 => "Keypad 9", 
			KeyCode.KeypadPeriod => ".", 
			KeyCode.KeypadDivide => "/", 
			KeyCode.KeypadMultiply => "*", 
			KeyCode.KeypadMinus => "-", 
			KeyCode.KeypadPlus => "+", 
			KeyCode.KeypadEnter => "Keypad Enter", 
			KeyCode.KeypadEquals => "=", 
			KeyCode.UpArrow => "Up Arrow", 
			KeyCode.DownArrow => "Down Arrow", 
			KeyCode.RightArrow => "Right Arrow", 
			KeyCode.LeftArrow => "Left Arrow", 
			KeyCode.Insert => "Insert", 
			KeyCode.Home => "Home", 
			KeyCode.End => "End", 
			KeyCode.PageUp => "Page Up", 
			KeyCode.PageDown => "Page Down", 
			KeyCode.F1 => "F1", 
			KeyCode.F2 => "F2", 
			KeyCode.F3 => "F3", 
			KeyCode.F4 => "F4", 
			KeyCode.F5 => "F5", 
			KeyCode.F6 => "F6", 
			KeyCode.F7 => "F7", 
			KeyCode.F8 => "F8", 
			KeyCode.F9 => "F9", 
			KeyCode.F10 => "F10", 
			KeyCode.F11 => "F11", 
			KeyCode.F12 => "F12", 
			KeyCode.F13 => "F13", 
			KeyCode.F14 => "F14", 
			KeyCode.F15 => "F15", 
			KeyCode.Numlock => "NumLock", 
			KeyCode.CapsLock => "CapsLock", 
			KeyCode.ScrollLock => "ScrollLock", 
			KeyCode.RightShift => "Right Shift", 
			KeyCode.LeftShift => "Left Shift", 
			KeyCode.RightControl => "Right Ctrl", 
			KeyCode.LeftControl => "Left Ctrl", 
			KeyCode.RightAlt => "Right Alt", 
			KeyCode.LeftAlt => "Left Alt", 
			KeyCode.Mouse0 => "Left Click", 
			KeyCode.Mouse1 => "Right Click", 
			KeyCode.Mouse2 => "Middle Click", 
			KeyCode.Mouse3 => "Mouse 4", 
			KeyCode.Mouse4 => "Mouse 5", 
			KeyCode.Mouse5 => "Mouse 6", 
			KeyCode.Mouse6 => "Mouse 7", 
			KeyCode.JoystickButton0 => "(A)", 
			KeyCode.JoystickButton1 => "(B)", 
			KeyCode.JoystickButton2 => "(X)", 
			KeyCode.JoystickButton3 => "(Y)", 
			KeyCode.JoystickButton4 => "(RB)", 
			KeyCode.JoystickButton5 => "(LB)", 
			KeyCode.JoystickButton6 => "(Back)", 
			KeyCode.JoystickButton7 => "(Start)", 
			KeyCode.JoystickButton8 => "(LS)", 
			KeyCode.JoystickButton9 => "(RS)", 
			KeyCode.JoystickButton10 => "J10", 
			KeyCode.JoystickButton11 => "J11", 
			KeyCode.JoystickButton12 => "J12", 
			KeyCode.JoystickButton13 => "J13", 
			KeyCode.JoystickButton14 => "J14", 
			KeyCode.JoystickButton15 => "J15", 
			KeyCode.JoystickButton16 => "J16", 
			KeyCode.JoystickButton17 => "J17", 
			KeyCode.JoystickButton18 => "J18", 
			KeyCode.JoystickButton19 => "J19", 
			_ => keyCode.ToString(), 
		};
	}
}