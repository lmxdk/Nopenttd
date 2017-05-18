/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file palettes.h The colour translation of the GRF palettes. */

namespace Nopenttd.tables {
  
  public static class PaletteConstants
  {

/** Colour palette (DOS) */
public static readonly Palette _palette = {
  {
    /* transparent */
    Colour(0, 0, 0, 0),
    /* grey scale */
                      new Colour( 16,  16,  16), new Colour( 32,  32,  32), new Colour( 48,  48,  48),
    new Colour( 65,  64,  65), new Colour( 82,  80,  82), new Colour( 98, 101,  98), new Colour(115, 117, 115),
    /* regular colours */
    new Colour(131, 133, 131), new Colour(148, 149, 148), new Colour(168, 168, 168), new Colour(184, 184, 184),
    new Colour(200, 200, 200), new Colour(216, 216, 216), new Colour(232, 232, 232), new Colour(252, 252, 252),
    new Colour( 52,  60,  72), new Colour( 68,  76,  92), new Colour( 88,  96, 112), new Colour(108, 116, 132),
    new Colour(132, 140, 152), new Colour(156, 160, 172), new Colour(176, 184, 196), new Colour(204, 208, 220),
    new Colour( 48,  44,   4), new Colour( 64,  60,  12), new Colour( 80,  76,  20), new Colour( 96,  92,  28),
    new Colour(120, 120,  64), new Colour(148, 148, 100), new Colour(176, 176, 132), new Colour(204, 204, 168),
    new Colour( 72,  44,   4), new Colour( 88,  60,  20), new Colour(104,  80,  44), new Colour(124, 104,  72),
    new Colour(152, 132,  92), new Colour(184, 160, 120), new Colour(212, 188, 148), new Colour(244, 220, 176),
    new Colour( 64,   0,   4), new Colour( 88,   4,  16), new Colour(112,  16,  32), new Colour(136,  32,  52),
    new Colour(160,  56,  76), new Colour(188,  84, 108), new Colour(204, 104, 124), new Colour(220, 132, 144),
    new Colour(236, 156, 164), new Colour(252, 188, 192), new Colour(252, 212,   0), new Colour(252, 232,  60),
    new Colour(252, 248, 128), new Colour( 76,  40,   0), new Colour( 96,  60,   8), new Colour(116,  88,  28),
    new Colour(136, 116,  56), new Colour(156, 136,  80), new Colour(176, 156, 108), new Colour(196, 180, 136),
    new Colour( 68,  24,   0), new Colour( 96,  44,   4), new Colour(128,  68,   8), new Colour(156,  96,  16),
    new Colour(184, 120,  24), new Colour(212, 156,  32), new Colour(232, 184,  16), new Colour(252, 212,   0),
    new Colour(252, 248, 128), new Colour(252, 252, 192), new Colour( 32,   4,   0), new Colour( 64,  20,   8),
    new Colour( 84,  28,  16), new Colour(108,  44,  28), new Colour(128,  56,  40), new Colour(148,  72,  56),
    new Colour(168,  92,  76), new Colour(184, 108,  88), new Colour(196, 128, 108), new Colour(212, 148, 128),
    new Colour(  8,  52,   0), new Colour( 16,  64,   0), new Colour( 32,  80,   4), new Colour( 48,  96,   4),
    new Colour( 64, 112,  12), new Colour( 84, 132,  20), new Colour(104, 148,  28), new Colour(128, 168,  44),
    new Colour( 28,  52,  24), new Colour( 44,  68,  32), new Colour( 60,  88,  48), new Colour( 80, 104,  60),
    new Colour(104, 124,  76), new Colour(128, 148,  92), new Colour(152, 176, 108), new Colour(180, 204, 124),
    new Colour( 16,  52,  24), new Colour( 32,  72,  44), new Colour( 56,  96,  72), new Colour( 76, 116,  88),
    new Colour( 96, 136, 108), new Colour(120, 164, 136), new Colour(152, 192, 168), new Colour(184, 220, 200),
    new Colour( 32,  24,   0), new Colour( 56,  28,   0), new Colour( 72,  40,   4), new Colour( 88,  52,  12),
    new Colour(104,  64,  24), new Colour(124,  84,  44), new Colour(140, 108,  64), new Colour(160, 128,  88),
    new Colour( 76,  40,  16), new Colour( 96,  52,  24), new Colour(116,  68,  40), new Colour(136,  84,  56),
    new Colour(164,  96,  64), new Colour(184, 112,  80), new Colour(204, 128,  96), new Colour(212, 148, 112),
    new Colour(224, 168, 128), new Colour(236, 188, 148), new Colour( 80,  28,   4), new Colour(100,  40,  20),
    new Colour(120,  56,  40), new Colour(140,  76,  64), new Colour(160, 100,  96), new Colour(184, 136, 136),
    new Colour( 36,  40,  68), new Colour( 48,  52,  84), new Colour( 64,  64, 100), new Colour( 80,  80, 116),
    new Colour(100, 100, 136), new Colour(132, 132, 164), new Colour(172, 172, 192), new Colour(212, 212, 224),
    new Colour( 40,  20, 112), new Colour( 64,  44, 144), new Colour( 88,  64, 172), new Colour(104,  76, 196),
    new Colour(120,  88, 224), new Colour(140, 104, 252), new Colour(160, 136, 252), new Colour(188, 168, 252),
    new Colour(  0,  24, 108), new Colour(  0,  36, 132), new Colour(  0,  52, 160), new Colour(  0,  72, 184),
    new Colour(  0,  96, 212), new Colour( 24, 120, 220), new Colour( 56, 144, 232), new Colour( 88, 168, 240),
    new Colour(128, 196, 252), new Colour(188, 224, 252), new Colour( 16,  64,  96), new Colour( 24,  80, 108),
    new Colour( 40,  96, 120), new Colour( 52, 112, 132), new Colour( 80, 140, 160), new Colour(116, 172, 192),
    new Colour(156, 204, 220), new Colour(204, 240, 252), new Colour(172,  52,  52), new Colour(212,  52,  52),
    new Colour(252,  52,  52), new Colour(252, 100,  88), new Colour(252, 144, 124), new Colour(252, 184, 160),
    new Colour(252, 216, 200), new Colour(252, 244, 236), new Colour( 72,  20, 112), new Colour( 92,  44, 140),
    new Colour(112,  68, 168), new Colour(140, 100, 196), new Colour(168, 136, 224), new Colour(204, 180, 252),
    new Colour(204, 180, 252), new Colour(232, 208, 252), new Colour( 60,   0,   0), new Colour( 92,   0,   0),
    new Colour(128,   0,   0), new Colour(160,   0,   0), new Colour(196,   0,   0), new Colour(224,   0,   0),
    new Colour(252,   0,   0), new Colour(252,  80,   0), new Colour(252, 108,   0), new Colour(252, 136,   0),
    new Colour(252, 164,   0), new Colour(252, 192,   0), new Colour(252, 220,   0), new Colour(252, 252,   0),
    new Colour(204, 136,   8), new Colour(228, 144,   4), new Colour(252, 156,   0), new Colour(252, 176,  48),
    new Colour(252, 196, 100), new Colour(252, 216, 152), new Colour(  8,  24,  88), new Colour( 12,  36, 104),
    new Colour( 20,  52, 124), new Colour( 28,  68, 140), new Colour( 40,  92, 164), new Colour( 56, 120, 188),
    new Colour( 72, 152, 216), new Colour(100, 172, 224), new Colour( 92, 156,  52), new Colour(108, 176,  64),
    new Colour(124, 200,  76), new Colour(144, 224,  92), new Colour(224, 244, 252), new Colour(204, 240, 252),
    new Colour(180, 220, 236), new Colour(132, 188, 216), new Colour( 88, 152, 172),
    /* unused pink */
                                                          new Colour(212,   0, 212),
    new Colour(212,   0, 212), new Colour(212,   0, 212), new Colour(212,   0, 212), new Colour(212,   0, 212),
    new Colour(212,   0, 212), new Colour(212,   0, 212), new Colour(212,   0, 212), new Colour(212,   0, 212),
    new Colour(212,   0, 212), new Colour(212,   0, 212), new Colour(212,   0, 212),
    /* Palette animated colours (filled with data from #ExtraPaletteValues) */
                                                          new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0),
    /* pure white */
                                                          new Colour(252, 252, 252)
  },
  0,  // First dirty
  256 // Dirty count
}

/** Description of the length of the palette cycle animations *//// length of the dark blue water animation
public const uint EPV_CYCLES_DARK_WATER    =  5; /// length of the lighthouse/stadium animation
public const uint EPV_CYCLES_LIGHTHOUSE    =  4; /// length of the oil refinery's fire animation
public const uint EPV_CYCLES_OIL_REFINERY  =  7; /// length of the fizzy drinks animation
public const uint EPV_CYCLES_FIZZY_DRINK   =  5; /// length of the glittery water animation
public const uint EPV_CYCLES_GLITTER_WATER = 15; 

/** Actual palette animation tables */
public static readonly ExtraPaletteValues _extra_palette_values = new ExtraPaletteValues() {
  /* dark blue water */
  dark_water = 
  { new Colour( 32,  68, 112), new Colour( 36,  72, 116), new Colour( 40,  76, 120), new Colour( 44,  80, 124),
    new Colour( 48,  84, 128) },

  /* dark blue water Toyland */
  dark_water_toyland =
  { new Colour( 28, 108, 124), new Colour( 32, 112, 128), new Colour( 36, 116, 132), new Colour( 40, 120, 136),
    new Colour( 44, 124, 140) },

  /* lighthouse & stadium */
  lighthouse =
  { new Colour(240, 208,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0), new Colour(  0,   0,   0) },

  /* oil refinery */
  oil_refinery =
  { new Colour(252,  60,   0), new Colour(252,  84,   0), new Colour(252, 108,   0), new Colour(252, 124,   0),
    new Colour(252, 148,   0), new Colour(252, 172,   0), new Colour(252, 196,   0) },

  /* fizzy drinks */
  fizzy_drink =
  { new Colour( 76,  24,   8), new Colour(108,  44,  24), new Colour(144,  72,  52), new Colour(176, 108,  84),
    new Colour(212, 148, 128) },

  /* glittery water */
  glitter_water =
  { new Colour(216, 244, 252), new Colour(172, 208, 224), new Colour(132, 172, 196), new Colour(100, 132, 168),
    new Colour( 72, 100, 144), new Colour( 72, 100, 144), new Colour( 72, 100, 144), new Colour( 72, 100, 144),
    new Colour( 72, 100, 144), new Colour( 72, 100, 144), new Colour( 72, 100, 144), new Colour( 72, 100, 144),
    new Colour(100, 132, 168), new Colour(132, 172, 196), new Colour(172, 208, 224) },

  /* glittery water Toyland */
  glitter_water_toyland =
  { new Colour(216, 244, 252), new Colour(180, 220, 232), new Colour(148, 200, 216), new Colour(116, 180, 196),
    new Colour( 92, 164, 184), new Colour( 92, 164, 184), new Colour( 92, 164, 184), new Colour( 92, 164, 184),
    new Colour( 92, 164, 184), new Colour( 92, 164, 184), new Colour( 92, 164, 184), new Colour( 92, 164, 184),
    new Colour(116, 180, 196), new Colour(148, 200, 216), new Colour(180, 220, 232) }
};
  }

/** Description of tables for the palette animation */
public class ExtraPaletteValues {/// dark blue water
  public Colour[] dark_water; //=new [EPV_CYCLES_DARK_WATER];               /// dark blue water Toyland
  public Colour[] dark_water_toyland; //=new [EPV_CYCLES_DARK_WATER];       /// lighthouse & stadium
  public Colour[] lighthouse; //=new [EPV_CYCLES_LIGHTHOUSE];               /// oil refinery
  public Colour[] oil_refinery; //=new [EPV_CYCLES_OIL_REFINERY];           /// fizzy drinks
  public Colour[] fizzy_drink; //=new [EPV_CYCLES_FIZZY_DRINK];             /// glittery water
  public Colour[] glitter_water; //=new [EPV_CYCLES_GLITTER_WATER];         /// glittery water Toyland
  public Colour[] glitter_water_toyland; //=new [EPV_CYCLES_GLITTER_WATER]; 
}
}