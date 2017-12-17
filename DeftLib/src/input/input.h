#pragma once

#include <vector>
#include "../geometry/geometry.h"

namespace deft
{
	typedef enum Key
	{
		UNKNOWN = 0,
		FIRST = 0,
		BACKSPACE = 8,
		TAB = 9,
		CLEAR = 12,
		RETURN = 13,
		PAUSE = 19,
		ESC = 27,
		SPACE = 32,
		EXCL = 33,
		DBLQUOTE = 34,
		HASH = 35,
		DOLLAR = 36,
		AMPERSAND = 38,
		QUOTE = 39,
		L_PAREN = 40,
		R_PAREN = 41,
		ASTERISK = 42,
		PLUS = 43,
		COMMA = 44,
		MINUS = 45,
		PERIOD = 46,
		SLASH = 47,
		_0 = 48,
		_1 = 49,
		_2 = 50,
		_3 = 51,
		_4 = 52,
		_5 = 53,
		_6 = 54,
		_7 = 55,
		_8 = 56,
		_9 = 57,
		COLON = 58,
		SEMICOLON = 59,
		LESS_THAN = 60,
		EQUALS = 61,
		GREATER_THAN = 62,
		Q_MARK = 63,
		AT = 64,

		// Skip uppercase letters
		L_BRACKET = 91,
		BACKSLASH = 92,
		R_BRACKET = 93,
		CARET = 94,
		UNDERSCORE = 95,
		BACK_QUOTE = 96,
		A = 97,
		B = 98,
		C = 99,
		D = 100,
		E = 101,
		F = 102,
		G = 103,
		H = 104,
		I = 105,
		J = 106,
		K = 107,
		L = 108,
		M = 109,
		N = 110,
		O = 111,
		P = 112,
		Q = 113,
		R = 114,
		S = 115,
		T = 116,
		U = 117,
		V = 118,
		W = 119,
		X = 120,
		Y = 121,
		Z = 122,
		DEL = 127,
		// End ASCII mapped keys

		// TODO: World keys here...

		// Numpad keys
		KP_0 = 256,
		KP_1 = 257,
		KP_2 = 258,
		KP_3 = 259,
		KP_4 = 260,
		KP_5 = 261,
		KP_6 = 262,
		KP_7 = 263,
		KP_8 = 264,
		KP_9 = 265,
		KP_PERIOD = 266,
		KP_DIVIDE = 267,
		KP_MULTIPLY = 268,
		KP_MINUS = 269,
		KP_PLUS = 270,
		KP_ENTER = 271,
		KP_EQUALS = 272,

		// Arrows + Home / End section keys
		UP = 273,
		DOWN = 274,
		RIGHT = 275,
		LEFT = 276,
		INSERT = 277,
		HOME = 278,
		END = 279,
		PAGE_UP = 280,
		PAGE_DOWN = 281,


		// F keys
		F1 = 282,
		F2 = 283,
		F3 = 284,
		F4 = 285,
		F5 = 286,
		F6 = 287,
		F7 = 288,
		F8 = 289,
		F9 = 290,
		F10 = 291,
		F11 = 292,
		F12 = 293,
		F13 = 294,
		F14 = 295,
		F15 = 296,


		// Modifier keys
		NUM_LOCK = 300,
		CAPS_LOCK = 301,
		SCROLL_LOCKS = 302,
		R_SHIFT = 303,
		L_SHIFT = 304,
		R_CTRL = 305,
		L_CTRL = 306,
		R_ALT = 307,
		L_ALT = 308,
		R_META = 309,
		L_META = 310,
		L_SUPER = 311,
		R_SUPER = 312,
		MODE = 313,
		COMPOSE = 314,


		// TODO: Miscellaneous keys here...



	} Key;

	typedef enum 
	{
		LEFT_MOUSE = 1,
		RIGHT_MOUSE = 3
	} MouseButton;

	typedef enum InputEventType
	{
		MOUSE_RELEASED,
		MOUSE_DOWN,
		KEY_TYPED,
	} InputEventType;

	typedef struct InputEvent
	{
		InputEventType event;
		Key key;
		MouseButton btn;
		Vec2 mouse_pos;
	} InputEvent;	

	namespace input
	{
		void pump_input_events();
		bool key_down(Key key);		
		bool key_typed(Key key);
		bool key_released(Key key);

		const std::vector<InputEvent>& get_input_events();
		std::vector<std::string> get_held_keys();

		int mouse_x();
		int mouse_y();
		Vec2 mouse_pos();

		bool mouse_down(MouseButton button);
		bool mouse_pressed(MouseButton button);
		bool mouse_released(MouseButton button);

	}
}
