#include "audio.h"
#include <SDL_mixer.h>
#include <string>
#include <map>

using std::string;
using std::map; 

namespace deft
{
	namespace audio
	{
		namespace backend
		{
			map<string, Mix_Chunk*> sounds;
			map<string, Mix_Music*> musics;
			bool _is_playing_music = false;
		}		

		void init()
		{
			Mix_OpenAudio(
				44100,					// Frequency
				MIX_DEFAULT_FORMAT,		// Output format
				2,						// Channels - 2 = stereo, 1 = mono
				2048);					// Chunksize (sound quality)
		}

		void play_music(const string& name)
		{
			Mix_PlayMusic(backend::musics[name], -1);
			backend::_is_playing_music = true;
		}

		void play_sound(const string& name)
		{
			Mix_PlayChannel(-1, backend::sounds[name], 0);
		}

		void loop_sound(const string& name, int count)
		{
			Mix_PlayChannel(-1, backend::sounds[name], count - 1);
		}

		void load_sound(const string& name, const string& path)
		{
			backend::sounds[name] = Mix_LoadWAV(path.c_str());
		}

		void load_music(const string& name, const string& path)
		{
			backend::musics[name] = Mix_LoadMUS(path.c_str());
		}		

		void pause_music()
		{
			Mix_PauseMusic();
			backend::_is_playing_music = false;
		}

		void resume_music()
		{
			Mix_ResumeMusic();
			backend::_is_playing_music = true;
		}

		void toggle_music()
		{
			if (backend::_is_playing_music)
				pause_music();
			else
				resume_music();
		}

		void stop_music()
		{
			if (backend::_is_playing_music)
			{
				Mix_HaltMusic();
				backend::_is_playing_music = false;
			}				
		}

		void quit()
		{
			// TODO: Free chunks
			// TODO: Free musics
			Mix_Quit();
		}
	}
}