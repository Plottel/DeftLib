#include "graphics_backend.h"
#include "../input/input.h"
#include <iostream>
#include "../geometry/geometry.h"

namespace deft
{
	namespace graphics
	{
		namespace backend
		{
			SDL_Window* window;
			SDL_Renderer* renderer;

			Font* font_10 = nullptr;
			Font* font_12 = nullptr;
			Font* font_14 = nullptr;
			Font* font_16 = nullptr;
			Font* font_24 = nullptr;

			void init()
			{
				SDL_Init(SDL_INIT_EVERYTHING);
				TTF_Init();
				//IMG_Init();

				std::cout << SDL_GetError() << std::endl;

				window = SDL_CreateWindow
				(
					"An SDL2 window",                  // window title
					SDL_WINDOWPOS_UNDEFINED,           // initial x position
					SDL_WINDOWPOS_UNDEFINED,           // initial y position
					1200,                               // width, in pixels
					675,                               // height, in pixels
					SDL_WINDOW_OPENGL                  // flags - see below
				);

				renderer = SDL_CreateRenderer
				(
					window,
					-1,
					SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC
				);

				font_10 = TTF_OpenFont("resources/Calibri.ttf", 10);
				font_12 = TTF_OpenFont("resources/Calibri.ttf", 12);
				font_14 = TTF_OpenFont("resources/Calibri.ttf", 14);
				font_16 = TTF_OpenFont("resources/Calibri.ttf", 16);
				font_24 = TTF_OpenFont("resources/Calibri.ttf", 24);
				

				std::cout << TTF_GetError() << std::endl;
			}

			SDL_Rect r_to_sdlr(deft::Rect& rect)
			{
				return SDL_Rect{ (int)rect.x, (int)rect.y, (int)rect.w, (int)rect.h };
			}

			void outline_r(SDL_Rect* rect, Color clr, int thickness)
			{
				SDL_SetRenderDrawColor(renderer, clr.r, clr.g, clr.b, clr.a);
				SDL_RenderDrawRect(renderer, rect);				

				if (thickness <= 1)
					return;

				SDL_Rect top = { rect->x, rect->y, rect->w, thickness };
				SDL_Rect bottom = { rect->x, rect->y + rect->h - thickness, rect->w, thickness };
				SDL_Rect left = { rect->x, rect->y, thickness, rect->h };
				SDL_Rect right = { rect->x + rect->w - thickness, rect->y, thickness, rect->h };

				// Render thick border
				SDL_RenderFillRect(renderer, &top);
				SDL_RenderFillRect(renderer, &bottom);
				SDL_RenderFillRect(renderer, &left);
				SDL_RenderFillRect(renderer, &right);
			}

			void fill_r(SDL_Rect* rect, Color clr)
			{
				SDL_SetRenderDrawColor(renderer, clr.r, clr.g, clr.b, clr.a);
				SDL_RenderFillRect(renderer, rect);
			}

			void fill_circle(float x, float y, int radius, Color clr)
			{
				SDL_SetRenderDrawColor(renderer, clr.r, clr.g, clr.b, clr.a);

				for (int w = 0; w < radius * 2; ++w)
				{
					for (int h = 0; h < radius * 2; ++h)
					{
						int dx = radius - w; // horizontal offset
						int dy = radius - h; // vertical offset

						if ((dx*dx + dy*dy) <= (radius * radius))
							SDL_RenderDrawPoint(renderer, x + dx, y + dy);

					}
				}
			}

			void outline_circle(float x, float y, int radius, Color clr)
			{
				// Draw a circle using Bresenham's circle drawing method.
				SDL_SetRenderDrawColor(renderer, clr.r, clr.g, clr.b, clr.a);

				int x2 = 0;
				int y2 = radius;
				int p = 3 - (2 * radius);

				while (x2 <= y2)
				{
					SDL_RenderDrawPoint(renderer, x + x2, y + y2);
					SDL_RenderDrawPoint(renderer, x - x2, y + y2);
					SDL_RenderDrawPoint(renderer, x + x2, y - y2);
					SDL_RenderDrawPoint(renderer, x - x2, y - y2);
					SDL_RenderDrawPoint(renderer, x + y2, y + x2);
					SDL_RenderDrawPoint(renderer, x + y2, y - x2);
					SDL_RenderDrawPoint(renderer, x - y2, y + x2);
					SDL_RenderDrawPoint(renderer, x - y2, y - x2);

					x2 = x2 + 1;

					if (p<0)
						p = p + 4 * (x2)+6;
					else
					{
						p = p + 4 * (x2 - y2) + 10;
						y2 = y2 - 1;
					}
				}
			}

			void pre_render()
			{
				SDL_SetRenderDrawColor(renderer, 130, 139, 130, 255);
				SDL_RenderClear(renderer);				
			}

			void post_render()
			{
				SDL_RenderPresent(renderer);
			}

			void quit()
			{
				SDL_DestroyWindow(window);
				SDL_DestroyRenderer(renderer);


				TTF_CloseFont(font_10);
				TTF_CloseFont(font_12);
				TTF_CloseFont(font_14);
				TTF_CloseFont(font_16);
				TTF_CloseFont(font_24);

				SDL_Quit();
			}

			void render_text(const char* text, float x, float y, const Color& clr, Font* font)
			{
				SDL_Color sdl_clr = { clr.r, clr.g, clr.b, clr.a};

				SDL_Surface* text_surface = TTF_RenderText_Solid(font, text, sdl_clr);
				SDL_Texture* text_texture = SDL_CreateTextureFromSurface(renderer, text_surface);
				SDL_Rect render_target = { x, y, text_surface->w, text_surface->h };
		
				SDL_RenderCopy(renderer, text_texture, NULL, &render_target);

				SDL_FreeSurface(text_surface);
				SDL_DestroyTexture(text_texture);
			}
		}
	}
}