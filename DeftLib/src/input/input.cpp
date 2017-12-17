#include "input.h"
#include <SDL.h> 
#include <vector>
#include <algorithm>
#include <iostream>

namespace deft
{
	namespace input
	{
		namespace 
		{
			const int SDL_KEY_COUNT = 322;
			const int MOUSE_BUTTON_COUNT = 2;

			bool _down_keys[SDL_KEY_COUNT];
			bool _typed_keys[SDL_KEY_COUNT];
			bool _released_keys[SDL_KEY_COUNT];

			bool _down_mouse_buttons[MOUSE_BUTTON_COUNT] = { false, false };
			bool _pressed_mouse_buttons[MOUSE_BUTTON_COUNT] = { false, false };
			bool _released_mouse_buttons[MOUSE_BUTTON_COUNT] = { false, false };

			int _mouse_x = 0;
			int _mouse_y = 0;

			SDL_Event e;
			std::vector<InputEvent> _new_input_events;
		}		

		const std::vector<InputEvent>& get_input_events()
		{
			return _new_input_events;
		}

		// Refreshed each frame with new key down and key up events.
		// Used to keep track of typed and released states.
		std::vector<Key> _new_key_events;

		std::vector<std::string> get_held_keys()
		{
			std::vector<std::string> result;
			for (auto& key : _new_key_events)
			{
				if (_typed_keys[key])
					result.push_back(std::string(SDL_GetKeyName(key)));
			}

			return result;
		}

		void pump_input_events()
		{
			// Events from last frame no longer relevant.
			// Typed and released is false for last frame's key events.
			for (auto& key : _new_key_events)
			{
				_typed_keys[key] = false;
				_released_keys[key] = false;
			}		

			_new_input_events.clear();
			_new_key_events.clear(); // Ready to process new events

			// Reset pressed mouse buttons.
			for (int i = 0; i < MOUSE_BUTTON_COUNT; ++i)
			{
				_pressed_mouse_buttons[i] = false;
				_released_mouse_buttons[i] = false;
			}


			while (SDL_PollEvent(&e) != 0)
			{
				InputEvent deft_event;
				deft_event.mouse_pos = Pt2{ (float)_mouse_x, (float)_mouse_y }; // Will be re-updated if a MouseMotion event.

				if (mouse_down(LEFT_MOUSE)) // Will be overridden by a "real" event.
				{
					deft_event.event = MOUSE_DOWN;
					deft_event.btn = LEFT_MOUSE;
				}					

				// Process SDL input for internal records
				SDL_Keycode key;
				auto b = e.button.button;

				switch (e.type)
				{
				case SDL_QUIT:
					// Do some quit thing.
					break;

				case SDL_KEYDOWN:
					key = e.key.keysym.sym;

					if (!isascii(key))
						key = e.key.keysym.scancode;

					_down_keys[key] = true;

					if (e.key.repeat == 0)
					{
						_typed_keys[key] = true;

						// Deft only cares about key typed, not down.
						deft_event.key = (Key)key;
						deft_event.event = KEY_TYPED;
					}

					_new_key_events.push_back((Key)key);
					break;

				case SDL_KEYUP:
					key = e.key.keysym.sym;

					if (!isascii(key))
						key = e.key.keysym.scancode;

					_down_keys[key] = false;
					_released_keys[key] = true;
					_new_key_events.push_back((Key)key);
					break;

				case SDL_MOUSEMOTION:
					SDL_GetMouseState(&_mouse_x, &_mouse_y);
					deft_event.mouse_pos = Vec2{ (float)_mouse_x, (float)_mouse_y };
					break;

				case SDL_MOUSEBUTTONDOWN:
					deft_event.btn = (MouseButton)b;
					deft_event.event = (InputEventType)MOUSE_DOWN;

					if (b == SDL_BUTTON_LEFT)
					{
						_pressed_mouse_buttons[0] = true;
						_down_mouse_buttons[0] = true;
					}
					else if (b == SDL_BUTTON_RIGHT)
					{
						_pressed_mouse_buttons[1] = true;
						_down_mouse_buttons[1] = true;
					}
					break;

				case SDL_MOUSEBUTTONUP:
					deft_event.btn = (MouseButton)b;
					deft_event.event = (InputEventType)MOUSE_RELEASED;

					if (b == SDL_BUTTON_LEFT)
					{
						_down_mouse_buttons[0] = false;
						_released_mouse_buttons[0] = true;
					}
					else if (b == SDL_BUTTON_RIGHT)
					{
						_down_mouse_buttons[1] = false;
						_released_mouse_buttons[1] = true;
					}
					break;

				default:
					break;
				}
			
				_new_input_events.push_back(deft_event);
			}
		}

		bool mouse_down(MouseButton button)
		{
			if (button == LEFT_MOUSE)
				return _down_mouse_buttons[0];
			else if (button == RIGHT_MOUSE)
				return _down_mouse_buttons[1];
			return false;
		}

		bool mouse_pressed(MouseButton button)
		{
			if (button == LEFT_MOUSE)
				return _pressed_mouse_buttons[0];
			else if (button == RIGHT_MOUSE)
				return _pressed_mouse_buttons[1];
			return false;
		}

		bool mouse_released(MouseButton button)
		{
			if (button == LEFT_MOUSE)
				return _released_mouse_buttons[0];
			else if (button == RIGHT_MOUSE)
				return _released_mouse_buttons[1];
			return false;
		}

		int mouse_x()
			{return _mouse_x;}

		int mouse_y()
			{return _mouse_y;}

		Vec2 mouse_pos()
			{return { (float)_mouse_x, (float)_mouse_y };}

		bool key_down(Key key)
		{
			return _down_keys[key];
		}		

		bool key_typed(Key key)
		{
			return _typed_keys[key];
		}

		bool key_released(Key key)
		{
			return _released_keys[key];
		}
	}
}