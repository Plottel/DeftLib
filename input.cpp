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

			SDL_Event e;
			bool _down_keys[SDL_KEY_COUNT];
			bool _typed_keys[SDL_KEY_COUNT];
			bool _released_keys[SDL_KEY_COUNT];

			bool _down_mouse_buttons[2] = { false, false };
			bool _pressed_mouse_buttons[2] = { false, false };
			bool _released_mouse_buttons[2] = { false, false };

			int _mouse_x = 0;
			int _mouse_y = 0;
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

		void get_input()
		{

			// Events from last frame no longer relevant.
			// Typed and released is false for last frame's key events.
			for (auto& key : _new_key_events)
			{
				_typed_keys[key] = false;
				_released_keys[key] = false;
			}				
			_new_key_events.clear(); // Ready to process new events

			// Reset pressed mouse buttons.
			for (int i = 0; i < MOUSE_BUTTON_COUNT; ++i)
			{
				_pressed_mouse_buttons[i] = false;
				_released_mouse_buttons[i] = false;
			}


			while (SDL_PollEvent(&e) != 0)
			{
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
						_typed_keys[key] = true;
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
					break;

				case SDL_MOUSEBUTTONDOWN:
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