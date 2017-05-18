/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file newgrf.h Base for the NewGRF implementation. */

namespace Nopenttd
{

/**
 * List of different canal 'features'.
 * Each feature gets an entry in the canal spritegroup table
 */
public enum CanalFeature {
  CF_WATERSLOPE,
  CF_LOCKS,
  CF_DIKES,
  CF_ICON,
  CF_DOCKS,
  CF_RIVER_SLOPE,
  CF_RIVER_EDGE,
  CF_RIVER_GUI,
  CF_BUOY,
  CF_END,
}

/** Canal properties local to the NewGRF */
public struct CanalProperties {/// Bitmask of canal callbacks that have to be called.
  public byte callback_mask;  /// Flags controlling display.
  public byte flags;          
}

public enum GrfLoadingStage {
  GLS_FILESCAN,
  GLS_SAFETYSCAN,
  GLS_LABELSCAN,
  GLS_INIT,
  GLS_RESERVE,
  GLS_ACTIVATION,
  GLS_END,
}

//DECLARE_POSTFIX_INCREMENT(GrfLoadingStage)

public enum GrfMiscBit {
  GMB_DESERT_TREES_FIELDS    = 0, // Unsupported.
  GMB_DESERT_PAVED_ROADS     = 1,
  GMB_FIELD_BOUNDING_BOX     = 2, // Unsupported./// Use 32 pixels per train vehicle in depot gui and vehicle details. Never set in the global variable; @see GRFFile::traininfo_vehicle_width
  GMB_TRAIN_WIDTH_32_PIXELS  = 3, 
  GMB_AMBIENT_SOUND_CALLBACK = 4,
  GMB_CATENARY_ON_3RD_TRACK  = 5, // Unsupported.
  GMB_SECOND_ROCKY_TILE_SET  = 6,
}

public enum GrfSpecFeature {
  GSF_TRAINS,
  GSF_ROADVEHICLES,
  GSF_SHIPS,
  GSF_AIRCRAFT,
  GSF_STATIONS,
  GSF_CANALS,
  GSF_BRIDGES,
  GSF_HOUSES,
  GSF_GLOBALVAR,
  GSF_INDUSTRYTILES,
  GSF_INDUSTRIES,
  GSF_CARGOES,
  GSF_SOUNDFX,
  GSF_AIRPORTS,
  GSF_SIGNALS,
  GSF_OBJECTS,
  GSF_RAILTYPES,
  GSF_AIRPORTTILES,
  GSF_END,
/// Fake town GrfSpecFeature for NewGRF debugging (parent scope)
  GSF_FAKE_TOWNS = GSF_END, /// End of the fake features
  GSF_FAKE_END,             
/// An invalid spec feature
  GSF_INVALID = 0xFF,       
}



public class GRFLabel {
  public byte label;
  public uint nfo_line;
  public int pos;
  public GRFLabel next;
}

/** Dynamic data of a loaded NewGRF */
public class GRFFile {
  public string filename;
  public uint grfid;
  public const uint INVALID_GRFID = 0xFFFFFFFF;
  public byte grf_version;

  public uint sound_offset;
  public ushort num_sounds;

  public StationSpec stations;
  public HouseSpec housespec;
  public IndustrySpec industryspec;
  public IndustryTileSpec indtspec;
  public ObjectSpec objectspec;
  public AirportSpec airportspec;
  public AirportTileSpec airtspec;

  public uint[] param = new uint[0x80];/// one more than the highest set parameter
  public uint param_end;  
/// Pointer to the first label. This is a linked list, not an array.
  public GRFLabel label; 
/// Cargo translation table (local ID -> label)
  public List<CargoLabel> cargo_list;          /// Inverse cargo translation table (CargoID -> local ID)
  public uint8 cargo_map = new uint[NUM_CARGO];                     
/// Railtype translation table
  public List<RailTypeLabel> railtype_list;    
  public RailTypeByte railtype_map = new RailTypeByte[RAILTYPE_END];
/// Canal properties as set by this NewGRF
  public CanalProperties[] canal_local_properties = new CanalProperties[CF_END]; 
/// Mappings related to the languages.
  public LanguageMap language_map; 
/// Vertical offset for draing train images in depot GUI and vehicle details
  public int traininfo_vehicle_pitch;  /// Width (in pixels) of a 8/8 train vehicle in depot GUI and vehicle details
  public uint traininfo_vehicle_width; 
/// Bitset of GrfSpecFeature the grf uses
  public uint grf_features;                     /// Price base multipliers as set by the grf.
  public PriceMultipliers price_base_multipliers; 

  public GRFFile(GRFConfig config);
  
  /** Get GRF Parameter with range checking */
  public uint GetParam(uint number) 
  {
    /* Note: We implicitly test for number < lengthof(this.param) and return 0 for invalid parameters.
     *       In fact this is the more important test, as param is zeroed anyway. */
    Debug.Assert(this.param_end <= this.param.Length);
    return (number < this.param_end) ? this.param[number] : 0;
  }
}

public enum ShoreReplacement {/// No shore sprites were replaced.
  SHORE_REPLACE_NONE,       /// Shore sprites were replaced by Action5.
  SHORE_REPLACE_ACTION_5,   /// Shore sprites were replaced by ActionA (using grass tiles for the corner-shores).
  SHORE_REPLACE_ACTION_A,   /// Only corner-shores were loaded by Action5 (openttd(w/d).grf only).
  SHORE_REPLACE_ONLY_NEW,   
}

public struct GRFLoadedFeatures {/// Set if any vehicle is loaded which uses 2cc (two company colours).
  public bool has_2CC;             /// Bitmask of #LiveryScheme used by the defined engines.
  public ulong used_liveries;     /// Set if there are any newhouses loaded.
  public bool has_newhouses;       /// Set if there are any newindustries loaded.
  public bool has_newindustries;   /// It which way shore sprites were replaced.
  public ShoreReplacement shore;   
}

/**
 * Check for grf miscellaneous bits
 * @param bit The bit to check.
 * @return Whether the bit is set.
 */
public static bool HasGrfMiscBit(GrfMiscBit bit)
{
  // extern byte _misc_grf_features;
  return HasBit(_misc_grf_features, bit);
}

/* Indicates which are the newgrf features currently loaded ingame */
// extern GRFLoadedFeatures _loaded_newgrf_features;

public byte GetGRFContainerVersion() => throw new NotImplementedException();
public void LoadNewGRFFile(GRFConfig config, uint file_index, GrfLoadingStage stage, Subdirectory subdir) => throw new NotImplementedException();
public void LoadNewGRF(uint load_index, uint file_index, uint num_baseset) => throw new NotImplementedException();
public void ReloadNewGRFData() => throw new NotImplementedException(); // in saveload/afterload.cpp
public void ResetNewGRFData() => throw new NotImplementedException();
public void ResetPersistentNewGRFData() => throw new NotImplementedException();

public void grfmsg(int severity, string str)  => throw new NotImplementedException();//, ...) WARN_FORMAT(2, 3);

public bool GetGlobalVariable(byte param, uint value, GRFFile grffile);

public StringID MapGRFStringID(uint grfid, StringID str);
public void ShowNewGRFError();

