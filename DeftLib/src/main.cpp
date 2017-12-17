//#include <iostream>
//#include "deft.h"
//#include <ctype.h>
//#include <vector>
//#include "gui/panel_types.h"
//#include <memory>
//
//using std::vector;
//
//int main(int argc, char* argv[])
//{
//	deft::init();
//
//	deft::load_sound("hit", "resources/hit.wav");
//	deft::load_sound("hit_2", "resources/hit_2.wav");	
//	deft::load_sound("hit_3", "resources/hit_3.wav");
//	deft::load_music("jumpshot", "resources/jumpshot.mp3");
//
//	deft::Rect rect = deft::Rect{ 100, 600, 50, 50 };
//
//	// Allocate panels
//	auto rect_edit = std::make_unique<deft::RectEditPanel>(deft::RectEditPanel("Rect Editor", 5, 5));
//	auto music_player = std::make_unique<deft::MusicPlayerPanel>(deft::MusicPlayerPanel("Music Player", 600, 5));
//
//	rect_edit->set_edit_rect(&rect);
//
//	// No longer own panels! Must fetch from deft to use!
//	deft::add_panel(std::move(rect_edit));
//	deft::add_panel(std::move(music_player));
//
//	while (true)
//	{
//		deft::process_events();
//
//		//
//		// Game Logic
//		//
//
//		// Sound
//		if (deft::key_typed(deft::_1))
//			deft::play_sound("hit");
//		if (deft::key_typed(deft::_2))
//			deft::play_sound("hit_2");
//		if (deft::key_typed(deft::_3))
//			deft::play_sound("hit_3");
//		if (deft::key_typed(deft::_4))
//			deft::loop_sound("hit_2", 3);
//
//		
//		//
//		// Rendering
//		//
//
//		// Clear screen
//		deft::pre_render();
//		{
//			deft::fill_rect(rect, deft::blue);
//
//			// Render deft GUI
//			deft::render_gui();
//		}
//		// Refresh screen
//		deft::post_render();
//	}
//
//	return EXIT_SUCCESS;
//}
//
 












