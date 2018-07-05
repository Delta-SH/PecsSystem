using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// This class is an implementation for receiving others information from database
    /// </summary>
    public class Other : IOther
    {
        /// <summary>
        /// Method to get areas
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<AreaInfo> GetAreas(int lscId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                parms[0].Value = lscId;

                var areas = new List<AreaInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETAREAS, parms)) {
                    while (rdr.Read()) {
                        var area = new AreaInfo();
                        area.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        area.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        area.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        area.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        area.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        area.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        area.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        area.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        areas.Add(area);
                    }
                }
                return areas;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get areas
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<AreaInfo> GetAreas(int lscId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Area;

                var areas = new List<AreaInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETAREASBYGROUPID, parms)) {
                    while (rdr.Read()) {
                        var area = new AreaInfo();
                        area.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        area.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        area.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        area.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        area.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        area.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        area.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        area.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        areas.Add(area);
                    }
                }
                return areas;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get areas by level.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="level">level</param>
        public List<AreaInfo> GetAreas(int lscId, int groupId, int level) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@NodeLevel", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = level;
                parms[3].Value = (int)EnmNodeType.Area;

                var areas = new List<AreaInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETAREASBYNODELEVEL, parms)) {
                    while (rdr.Read()) {
                        var area = new AreaInfo();
                        area.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        area.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        area.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        area.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        area.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        area.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        area.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        area.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        areas.Add(area);
                    }
                }
                return areas;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get area
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="areaId">areaId</param>
        public AreaInfo GetArea(int lscId, int areaId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AreaID", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = areaId;

                AreaInfo area = null;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETAREA, parms)) {
                    if (rdr.Read()) {
                        area = new AreaInfo();
                        area.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        area.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        area.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        area.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        area.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        area.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        area.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        area.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    }
                }
                return area;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get stations
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<StationInfo> GetStations(int lscId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                parms[0].Value = lscId;

                var stations = new List<StationInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETSTATIONS, parms)) {
                    while (rdr.Read()) {
                        var station = new StationInfo();
                        station.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        station.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        station.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        station.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        station.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        station.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        station.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        station.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        station.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        station.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        station.StaDesc = ComUtility.DBNullStringHandler(rdr["StaDesc"]);
                        station.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        station.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        station.StaFeatureID = ComUtility.DBNullInt32Handler(rdr["StaFeatureID"]);
                        station.StaFeatureName = ComUtility.DBNullStringHandler(rdr["StaFeatureName"]);
                        station.BuildingID = ComUtility.DBNullInt32Handler(rdr["BuildingID"]);
                        station.BuildingName = ComUtility.DBNullStringHandler(rdr["BuildingName"]);
                        station.LocationWay = ComUtility.DBNullMapTypeHandler(rdr["LocationWay"]);
                        station.Longitude = ComUtility.DBNullDoubleHandler(rdr["Longitude"]);
                        station.Latitude = ComUtility.DBNullDoubleHandler(rdr["Latitude"]);
                        station.MapDesc = ComUtility.DBNullStringHandler(rdr["MapDesc"]);
                        station.STDStationID = ComUtility.DBNullStringHandler(rdr["STDStationID"]);
                        station.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        station.DevCount = ComUtility.DBNullInt32Handler(rdr["DevCount"]);
                        station.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        stations.Add(station);
                    }
                }
                return stations;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get stations
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<StationInfo> GetStations(int lscId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Sta;

                var stations = new List<StationInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETSTATIONSBYGROUPID, parms)) {
                    while (rdr.Read()) {
                        var station = new StationInfo();
                        station.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        station.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        station.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        station.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        station.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        station.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        station.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        station.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        station.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        station.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        station.StaDesc = ComUtility.DBNullStringHandler(rdr["StaDesc"]);
                        station.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        station.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        station.StaFeatureID = ComUtility.DBNullInt32Handler(rdr["StaFeatureID"]);
                        station.StaFeatureName = ComUtility.DBNullStringHandler(rdr["StaFeatureName"]);
                        station.BuildingID = ComUtility.DBNullInt32Handler(rdr["BuildingID"]);
                        station.BuildingName = ComUtility.DBNullStringHandler(rdr["BuildingName"]);
                        station.LocationWay = ComUtility.DBNullMapTypeHandler(rdr["LocationWay"]);
                        station.Longitude = ComUtility.DBNullDoubleHandler(rdr["Longitude"]);
                        station.Latitude = ComUtility.DBNullDoubleHandler(rdr["Latitude"]);
                        station.MapDesc = ComUtility.DBNullStringHandler(rdr["MapDesc"]);
                        station.STDStationID = ComUtility.DBNullStringHandler(rdr["STDStationID"]);
                        station.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        station.DevCount = ComUtility.DBNullInt32Handler(rdr["DevCount"]);
                        station.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        stations.Add(station);
                    }
                }
                return stations;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get station
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        public StationInfo GetStation(int lscId, int staId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@StaID", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = staId;

                StationInfo station = null;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETSTATION, parms)) {
                    if (rdr.Read()) {
                        station = new StationInfo();
                        station.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        station.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        station.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        station.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        station.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        station.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        station.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        station.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        station.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        station.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        station.StaDesc = ComUtility.DBNullStringHandler(rdr["StaDesc"]);
                        station.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        station.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        station.StaFeatureID = ComUtility.DBNullInt32Handler(rdr["StaFeatureID"]);
                        station.StaFeatureName = ComUtility.DBNullStringHandler(rdr["StaFeatureName"]);
                        station.BuildingID = ComUtility.DBNullInt32Handler(rdr["BuildingID"]);
                        station.BuildingName = ComUtility.DBNullStringHandler(rdr["BuildingName"]);
                        station.LocationWay = ComUtility.DBNullMapTypeHandler(rdr["LocationWay"]);
                        station.Longitude = ComUtility.DBNullDoubleHandler(rdr["Longitude"]);
                        station.Latitude = ComUtility.DBNullDoubleHandler(rdr["Latitude"]);
                        station.MapDesc = ComUtility.DBNullStringHandler(rdr["MapDesc"]);
                        station.STDStationID = ComUtility.DBNullStringHandler(rdr["STDStationID"]);
                        station.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        station.DevCount = ComUtility.DBNullInt32Handler(rdr["DevCount"]);
                        station.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    }
                }
                return station;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get devices
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<DeviceInfo> GetDevices(int lscId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                parms[0].Value = lscId;

                var devices = new List<DeviceInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETDEVICES, parms)) {
                    while (rdr.Read()) {
                        var device = new DeviceInfo();
                        device.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        device.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        device.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        device.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        device.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        device.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        device.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        device.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        device.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        device.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        device.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        device.StaMID = ComUtility.DBNullStringHandler(rdr["StaMID"]);
                        device.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        device.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        device.DevDesc = ComUtility.DBNullStringHandler(rdr["DevDesc"]);
                        device.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                        device.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                        device.AlmDevTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmDevTypeID"]);
                        device.AlmDevTypeName = ComUtility.DBNullStringHandler(rdr["AlarmDevTypeName"]);
                        device.ProdID = ComUtility.DBNullInt32Handler(rdr["ProdID"]);
                        device.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                        device.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        device.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        device.ModuleCount = ComUtility.DBNullInt32Handler(rdr["ModuleCount"]);
                        device.DevDesignCapacity = ComUtility.DBNullFloatHandler(rdr["DevDesignCapacity"]);
                        device.SingleRatedCapacity = ComUtility.DBNullFloatHandler(rdr["SingleRatedCapacity"]);
                        device.TotalRatedCapacity = ComUtility.DBNullFloatHandler(rdr["TotalRatedCapacity"]);
                        device.RedundantCapacity = ComUtility.DBNullFloatHandler(rdr["RedundantCapacity"]);
                        devices.Add(device);
                    }
                }
                return devices;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get devices
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<DeviceInfo> GetDevices(int lscId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Dev;

                var devices = new List<DeviceInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETDEVICESBYGROUPID, parms)) {
                    while (rdr.Read()) {
                        var device = new DeviceInfo();
                        device.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        device.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        device.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        device.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        device.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        device.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        device.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        device.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        device.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        device.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        device.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        device.StaMID = ComUtility.DBNullStringHandler(rdr["StaMID"]);
                        device.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        device.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        device.DevDesc = ComUtility.DBNullStringHandler(rdr["DevDesc"]);
                        device.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                        device.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                        device.AlmDevTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmDevTypeID"]);
                        device.AlmDevTypeName = ComUtility.DBNullStringHandler(rdr["AlarmDevTypeName"]);
                        device.ProdID = ComUtility.DBNullInt32Handler(rdr["ProdID"]);
                        device.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                        device.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        device.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        device.ModuleCount = ComUtility.DBNullInt32Handler(rdr["ModuleCount"]);
                        device.DevDesignCapacity = ComUtility.DBNullFloatHandler(rdr["DevDesignCapacity"]);
                        device.SingleRatedCapacity = ComUtility.DBNullFloatHandler(rdr["SingleRatedCapacity"]);
                        device.TotalRatedCapacity = ComUtility.DBNullFloatHandler(rdr["TotalRatedCapacity"]);
                        device.RedundantCapacity = ComUtility.DBNullFloatHandler(rdr["RedundantCapacity"]);
                        devices.Add(device);
                    }
                }
                return devices;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get device
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        public DeviceInfo GetDevice(int lscId, int devId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@DevID", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = devId;

                DeviceInfo device = null;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETDEVICE, parms)) {
                    if (rdr.Read()) {
                        device = new DeviceInfo();
                        device.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        device.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        device.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        device.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        device.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        device.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        device.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        device.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        device.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        device.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        device.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        device.StaMID = ComUtility.DBNullStringHandler(rdr["StaMID"]);
                        device.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        device.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        device.DevDesc = ComUtility.DBNullStringHandler(rdr["DevDesc"]);
                        device.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                        device.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                        device.AlmDevTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmDevTypeID"]);
                        device.AlmDevTypeName = ComUtility.DBNullStringHandler(rdr["AlarmDevTypeName"]);
                        device.ProdID = ComUtility.DBNullInt32Handler(rdr["ProdID"]);
                        device.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                        device.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        device.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        device.ModuleCount = ComUtility.DBNullInt32Handler(rdr["ModuleCount"]);
                        device.DevDesignCapacity = ComUtility.DBNullFloatHandler(rdr["DevDesignCapacity"]);
                        device.SingleRatedCapacity = ComUtility.DBNullFloatHandler(rdr["SingleRatedCapacity"]);
                        device.TotalRatedCapacity = ComUtility.DBNullFloatHandler(rdr["TotalRatedCapacity"]);
                        device.RedundantCapacity = ComUtility.DBNullFloatHandler(rdr["RedundantCapacity"]);
                    }
                }
                return device;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get rtus
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<RtuInfo> GetRtus(int lscId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int),
                                         new SqlParameter("@SicType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Dev;
                parms[3].Value = (int)EnmSicType.RTU;

                var rtus = new List<RtuInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETRTUS, parms)) {
                    while (rdr.Read()) {
                        var rtu = new RtuInfo();
                        rtu.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        rtu.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        rtu.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        rtu.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        rtu.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        rtu.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        rtu.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        rtu.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        rtu.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        rtu.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        rtu.StaMID = ComUtility.DBNullStringHandler(rdr["StaMID"]);
                        rtu.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        rtu.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        rtu.RtuID = ComUtility.DBNullInt32Handler(rdr["RtuID"]);
                        rtus.Add(rtu);
                    }
                }
                return rtus;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get station node count
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="alarmDeviceTypeID">alarmDeviceTypeID</param>
        public List<StationCntInfo> GetStationNodeCnt(int lscId, int groupId, int alarmDeviceTypeID) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int),
                                         new SqlParameter("@AlarmDeviceTypeID", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Dev;
                parms[3].Value = (int)EnmNodeType.Sta;
                if (alarmDeviceTypeID == ComUtility.DefaultInt32)
                    parms[4].Value = DBNull.Value;
                else
                    parms[4].Value = alarmDeviceTypeID;

                var cnts = new List<StationCntInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETSTACNT, parms)) {
                    while (rdr.Read()) {
                        var cnt = new StationCntInfo();
                        cnt.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        cnt.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        cnt.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        cnt.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        cnt.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        cnt.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        cnt.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        cnt.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        cnt.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        cnt.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        cnt.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        cnt.StaDevCount = ComUtility.DBNullInt32Handler(rdr["StaDevCount"]);
                        cnt.DevCnt = ComUtility.DBNullInt32Handler(rdr["DevCnt"]);
                        cnt.AICnt = ComUtility.DBNullInt32Handler(rdr["AICnt"]);
                        cnt.AOCnt = ComUtility.DBNullInt32Handler(rdr["AOCnt"]);
                        cnt.DICnt = ComUtility.DBNullInt32Handler(rdr["DICnt"]);
                        cnt.DOCnt = ComUtility.DBNullInt32Handler(rdr["DOCnt"]);
                        cnts.Add(cnt);
                    }
                }
                return cnts;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get station FSU count
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="alarmIds">alarmIds</param>
        /// <param name="split">split</param>
        public List<StationCntInfo> GetStationFSUCnt(int lscId, int groupId, string alarmIds, string split) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int),
                                         new SqlParameter("@AlarmIds", SqlDbType.VarChar,1024),
                                         new SqlParameter("@Split", SqlDbType.VarChar,10)};
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Dev;
                parms[3].Value = (int)EnmNodeType.Sta;
                parms[4].Value = alarmIds;
                parms[5].Value = split;

                var cnts = new List<StationCntInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETSTAFSUCNT, parms)) {
                    while (rdr.Read()) {
                        var cnt = new StationCntInfo();
                        cnt.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        cnt.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        cnt.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        cnt.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        cnt.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        cnt.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        cnt.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        cnt.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        cnt.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        cnt.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        cnt.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        cnt.StaDevCount = ComUtility.DBNullInt32Handler(rdr["StaDevCount"]);
                        cnt.DevCnt = ComUtility.DBNullInt32Handler(rdr["DevCnt"]);
                        cnt.AICnt = ComUtility.DBNullInt32Handler(rdr["AICnt"]);
                        cnt.AOCnt = ComUtility.DBNullInt32Handler(rdr["AOCnt"]);
                        cnt.DICnt = ComUtility.DBNullInt32Handler(rdr["DICnt"]);
                        cnt.DOCnt = ComUtility.DBNullInt32Handler(rdr["DOCnt"]);
                        cnts.Add(cnt);
                    }
                }
                return cnts;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get station markers
        /// </summary>
        /// <param name="staName">staName</param>
        /// <param name="minLng">minLng</param>
        /// <param name="maxLng">maxLng</param>
        /// <param name="minLat">minLat</param>
        /// <param name="maxLat">maxLat</param>
        /// <param name="rowCnt">rowCnt</param>
        public List<StationInfo> GetStationMarkers(string staName, double minLng, double maxLng, double minLat, double maxLat, int rowCnt) {
            try {
                var stations = new List<StationInfo>();
                var sqlText = String.Format(@"
                ;WITH Stations AS
                (
	                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,
	                TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name,TS.[StaID],TS.[StaName],TS.[StaDesc],TS.[StaTypeID],TST.[TypeName] AS StaTypeName,
	                TS.[NodeFeatures] AS [StaFeatureID],TSF.[TypeDesc] AS [StaFeatureName],TS.[LocationWay],TS.[Longitude],TS.[Latitude],TS.[MapDesc],TS.[STDStationID],TS.[MID],TS.[Enabled] 
	                FROM [dbo].[TM_STA] TS
	                INNER JOIN [dbo].[TM_LSC] TL ON TS.[LscID] = TL.[LscID] AND TS.[Longitude] BETWEEN {0} AND {1} AND TS.[Latitude] BETWEEN {2} AND {3}{4}
	                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
	                LEFT OUTER JOIN [dbo].[TC_StaFeatures] TSF ON TS.[NodeFeatures] = TSF.[TypeID]
	                LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
	                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
	                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                )
                SELECT TOP {5} [LscID],[LscName],[Area1ID],[Area1Name],[Area2ID],[Area2Name],[Area3ID],[Area3Name],[StaID],[StaName],[StaDesc],[StaTypeID],[StaTypeName],[StaFeatureID],[StaFeatureName],
                [LocationWay],[Longitude],[Latitude],[MapDesc],[STDStationID],[MID],[Enabled] FROM Stations;", minLng, maxLng, minLat, maxLat, staName.Equals(ComUtility.DefaultString) ? String.Empty : String.Format(" AND TS.[StaName] LIKE '%{0}%'", staName), rowCnt);

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), null)) {
                    while (rdr.Read()) {
                        var station = new StationInfo();
                        station.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        station.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        station.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        station.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        station.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        station.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        station.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        station.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        station.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        station.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        station.StaDesc = ComUtility.DBNullStringHandler(rdr["StaDesc"]);
                        station.StaTypeID = ComUtility.DBNullInt32Handler(rdr["StaTypeID"]);
                        station.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        station.StaFeatureID = ComUtility.DBNullInt32Handler(rdr["StaFeatureID"]);
                        station.StaFeatureName = ComUtility.DBNullStringHandler(rdr["StaFeatureName"]);
                        station.LocationWay = ComUtility.DBNullMapTypeHandler(rdr["LocationWay"]);
                        station.Longitude = ComUtility.DBNullDoubleHandler(rdr["Longitude"]);
                        station.Latitude = ComUtility.DBNullDoubleHandler(rdr["Latitude"]);
                        station.MapDesc = ComUtility.DBNullStringHandler(rdr["MapDesc"]);
                        station.STDStationID = ComUtility.DBNullStringHandler(rdr["STDStationID"]);
                        station.MID = ComUtility.DBNullStringHandler(rdr["MID"]);
                        station.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        stations.Add(station);
                    }
                }
                return stations;
            } catch { throw; }
        }

        /// <summary>
        /// Method to update station marker
        /// </summary>
        /// <param name="olscId">olscId</param>
        /// <param name="ostaId">ostaId</param>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        /// <param name="mapType">mapType</param>
        /// <param name="lng">lng</param>
        /// <param name="lat">lat</param>
        /// <param name="address">address</param>
        public void UpdateMarker(int olscId, int ostaId, int lscId, int staId, EnmMapType mapType, double lng, double lat, string address) {
            try {
                SqlParameter[] parms = { new SqlParameter("@OLscID", SqlDbType.Int),
                                         new SqlParameter("@OStaID", SqlDbType.Int),
                                         new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@StaID", SqlDbType.Int),
                                         new SqlParameter("@LocationWay", SqlDbType.Int),
                                         new SqlParameter("@Longitude", SqlDbType.Float),
                                         new SqlParameter("@Latitude", SqlDbType.Float),
                                         new SqlParameter("@MapDesc", SqlDbType.VarChar, 400)};
                parms[0].Value = olscId;
                parms[1].Value = ostaId;
                parms[2].Value = lscId;
                parms[3].Value = staId;
                parms[4].Value = (int)mapType;
                parms[5].Value = lng;
                parms[6].Value = lat;
                parms[7].Value = address;

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_SELECT_OTHER_UPDATEMARKER, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update local station Marker
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="ostaId">ostaId</param>
        /// <param name="staId">staId</param>
        /// <param name="mapType">mapType</param>
        /// <param name="lng">lng</param>
        /// <param name="lat">lat</param>
        /// <param name="address">address</param>
        public void UpdateLocalMarker(string connectionString, int ostaId, int staId, EnmMapType mapType, double lng, double lat, string address) {
            try {
                SqlParameter[] parms = { new SqlParameter("@OStaID", SqlDbType.Int),
                                         new SqlParameter("@StaID", SqlDbType.Int),
                                         new SqlParameter("@LocationWay", SqlDbType.Int),
                                         new SqlParameter("@Longitude", SqlDbType.Float),
                                         new SqlParameter("@Latitude", SqlDbType.Float),
                                         new SqlParameter("@MapDesc", SqlDbType.VarChar, 400) };
                parms[0].Value = ostaId;
                parms[1].Value = staId;
                parms[2].Value = (int)mapType;
                parms[3].Value = lng;
                parms[4].Value = lat;
                parms[5].Value = address;

                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_SELECT_OTHER_UPDATELOCALMARKER, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to get bat static records.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="devId">devId</param>
        /// <param name="devIndex">devIndex</param>
        /// <param name="beginFromTime">beginFromTime</param>
        /// <param name="beginToTime">beginToTime</param>
        /// <param name="endFromTime">endFromTime</param>
        /// <param name="endToTime">endToTime</param>
        /// <param name="fromInterval">fromInterval</param>
        /// <param name="toInterval">toInterval</param>
        /// <returns>bat static records</returns>
        public List<BatStaticInfo> GetBatStatic(int lscId, string lscName, int? devId, int? devIndex, DateTime? beginFromTime, DateTime? beginToTime, DateTime? endFromTime, DateTime? endToTime, double fromInterval, double toInterval) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@DevID", SqlDbType.Int),
                                     new SqlParameter("@DevIndex", SqlDbType.Int),
                                     new SqlParameter("@BeginFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@BeginToTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndToTime", SqlDbType.DateTime),
                                     new SqlParameter("@FromInterval", SqlDbType.Real),
                                     new SqlParameter("@ToInterval", SqlDbType.Real) };

            parms[0].Value = lscId;
            if (devId != null)
                parms[1].Value = devId;
            else
                parms[1].Value = DBNull.Value;
            if (devIndex != null)
                parms[2].Value = devIndex;
            else
                parms[2].Value = DBNull.Value;
            if (beginFromTime != null)
                parms[3].Value = beginFromTime;
            else
                parms[3].Value = DBNull.Value;
            if (beginToTime != null)
                parms[4].Value = beginToTime;
            else
                parms[4].Value = DBNull.Value;
            if (endFromTime != null)
                parms[5].Value = endFromTime;
            else
                parms[5].Value = DBNull.Value;
            if (endToTime != null)
                parms[6].Value = endToTime;
            else
                parms[6].Value = DBNull.Value;
            parms[7].Value = fromInterval;
            parms[8].Value = toInterval;

            var records = new List<BatStaticInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETBATSTATIC, parms)) {
                while (rdr.Read()) {
                    var rec = new BatStaticInfo();
                    rec.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    rec.LscName = lscName;
                    //rec.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                    //rec.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    //rec.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                    //rec.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    //rec.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                    //rec.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    //rec.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                    //rec.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    rec.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    //rec.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    rec.DevIndex = ComUtility.DBNullInt32Handler(rdr["DevIndex"]);
                    rec.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    rec.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    rec.LastTime = ComUtility.DBNullDoubleHandler(rdr["LastTime"]);
                    records.Add(rec);
                }
            }
            return records;
        }

        /// <summary>
        /// Method to get bat static count.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="beginFromTime">beginFromTime</param>
        /// <param name="beginToTime">beginToTime</param>
        /// <param name="endFromTime">endFromTime</param>
        /// <param name="endToTime">endToTime</param>
        /// <param name="fromInterval">fromInterval</param>
        /// <param name="toInterval">toInterval</param>
        /// <returns>bat static records count</returns>
        public List<IDValuePair<Int32,Int32>> GetBatStaticCount(int lscId, DateTime beginFromTime, DateTime beginToTime, DateTime? endFromTime, DateTime? endToTime, double fromInterval, double toInterval) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@BeginFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@BeginToTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndToTime", SqlDbType.DateTime),
                                     new SqlParameter("@FromInterval", SqlDbType.Real),
                                     new SqlParameter("@ToInterval", SqlDbType.Real) };

            parms[0].Value = lscId;
            parms[1].Value = beginFromTime;
            parms[2].Value = beginToTime;
            if (endFromTime != null)
                parms[3].Value = endFromTime;
            else
                parms[3].Value = DBNull.Value;
            if (endToTime != null)
                parms[4].Value = endToTime;
            else
                parms[4].Value = DBNull.Value;
            parms[5].Value = fromInterval;
            parms[6].Value = toInterval;

            var records = new List<IDValuePair<Int32, Int32>>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETBATSTATICCOUNT, parms)) {
                while (rdr.Read()) {
                    records.Add(new IDValuePair<Int32, Int32> {
                        ID = ComUtility.DBNullInt32Handler(rdr["LscID"]),
                        Value = ComUtility.DBNullInt32Handler(rdr["DevID"])
                    });
                }
            }
            return records;
        }

        /// <summary>
        /// Get lsc param
        /// </summary>
        /// <returns>lsc param</returns>
        public List<LscParamInfo> GetLscParam() {
            var parms = new List<LscParamInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETLSCPARAM, null)) {
                while (rdr.Read()) {
                    var parm = new LscParamInfo();
                    parm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    parm.StaTNumber = ComUtility.DBNullInt32Handler(rdr["StaTNumber"]);
                    parm.ElecDevTNumber = ComUtility.DBNullInt32Handler(rdr["ElecDevTNumber"]);
                    parms.Add(parm);
                }
            }
            return parms;
        }

        /// <summary>
        /// Get history dsc records.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="beginTime">beginTime</param>
        /// <param name="endTime">endTime</param>
        /// <returns>dsc records</returns>
        public List<DscInfo> GetHisDsc(int lscId, DateTime beginTime, DateTime endTime) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime) };
            parms[0].Value = lscId;
            parms[1].Value = beginTime;
            parms[2].Value = endTime;

            var records = new List<DscInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETHISDSC, parms)) {
                while (rdr.Read()) {
                    var rec = new DscInfo();
                    rec.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    rec.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    rec.LoadCurrent = ComUtility.DBNullFloatHandler(rdr["LoadCurrent"]);
                    rec.ModuleNum = ComUtility.DBNullInt32Handler(rdr["ModuleNum"]);
                    rec.RatedCurrent = ComUtility.DBNullFloatHandler(rdr["RatedCurrent"]);
                    rec.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    records.Add(rec);
                }
            }
            return records;
        }

        public List<SubDevInfo> GetSubDev(int lscId, string lscName, string connectionString) {
            var records = new List<SubDevInfo>();
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_OTHER_GETSUBDEV, null)) {
                while (rdr.Read()) {
                    var rec = new SubDevInfo();
                    rec.LscID = lscId;
                    rec.LscName = lscName;
                    rec.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    rec.Rate = ComUtility.DBNullFloatHandler(rdr["Rate"]);
                    rec.AlarmLevel = ComUtility.DBNullInt32Handler(rdr["AlarmLevel"]);
                    rec.EventValue = ComUtility.DBNullFloatHandler(rdr["EventValue"]);
                    rec.RateStr = ComUtility.DBNullStringHandler(rdr["RateStr"]);
                    rec.EventValue1 = ComUtility.DBNullFloatHandler(rdr["EventValue1"]);
                    rec.EventValue2 = ComUtility.DBNullFloatHandler(rdr["EventValue2"]);
                    rec.EventValue3 = ComUtility.DBNullFloatHandler(rdr["EventValue3"]);
                    rec.EventValue4 = ComUtility.DBNullFloatHandler(rdr["EventValue4"]);
                    rec.AicID = ComUtility.DBNullInt32Handler(rdr["AicID"]);
                    rec.DevCapacity = ComUtility.DBNullFloatHandler(rdr["DevCapacity"]);
                    records.Add(rec);
                }
            }
            return records;
        }

        public List<AcEventInfo> GetPubAlertEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };

            parms[0].Value = fromTime;
            parms[1].Value = toTime;

            var records = new List<AcEventInfo>();
            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_OTHER_GetPubAlertEvent, parms)) {
                while(rdr.Read()) {
                    records.Add(new AcEventInfo {
                        LscId = lscId,
                        LscName = lscName,
                        NetGroupID = ComUtility.DBNullInt32Handler(rdr["NetGroupID"]),
                        NetGroupName = ComUtility.DBNullStringHandler(rdr["NetGroupName"]),
                        PointName = ComUtility.DBNullStringHandler(rdr["PointName"]),
                        UserID = ComUtility.DBNullStringHandler(rdr["UserID"]),
                        UserName = ComUtility.DBNullStringHandler(rdr["UserName"]),
                        Department = ComUtility.DBNullStringHandler(rdr["Department"]),
                        MessageID = ComUtility.DBNullInt32Handler(rdr["MessageID"]),
                        Message = ComUtility.DBNullStringHandler(rdr["Message"]),
                        EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"])
                    });
                }
            }
            return records;
        }

        public List<AcEventInfo> GetPubGeneralEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };

            parms[0].Value = fromTime;
            parms[1].Value = toTime;

            var records = new List<AcEventInfo>();
            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_OTHER_GetPubGeneralEvent, parms)) {
                while(rdr.Read()) {
                    records.Add(new AcEventInfo {
                        LscId = lscId,
                        LscName = lscName,
                        NetGroupID = ComUtility.DBNullInt32Handler(rdr["NetGroupID"]),
                        NetGroupName = ComUtility.DBNullStringHandler(rdr["NetGroupName"]),
                        PointName = ComUtility.DBNullStringHandler(rdr["PointName"]),
                        UserID = ComUtility.DBNullStringHandler(rdr["UserID"]),
                        UserName = ComUtility.DBNullStringHandler(rdr["UserName"]),
                        Department = ComUtility.DBNullStringHandler(rdr["Department"]),
                        MessageID = ComUtility.DBNullInt32Handler(rdr["MessageID"]),
                        Message = ComUtility.DBNullStringHandler(rdr["Message"]),
                        EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"])
                    });
                }
            }
            return records;
        }

        public List<AcEventInfo> GetPubInvalidCardEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };

            parms[0].Value = fromTime;
            parms[1].Value = toTime;

            var records = new List<AcEventInfo>();
            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_OTHER_GetPubInvalidCardEvent, parms)) {
                while(rdr.Read()) {
                    records.Add(new AcEventInfo {
                        LscId = lscId,
                        LscName = lscName,
                        NetGroupID = ComUtility.DBNullInt32Handler(rdr["NetGroupID"]),
                        NetGroupName = ComUtility.DBNullStringHandler(rdr["NetGroupName"]),
                        PointName = ComUtility.DBNullStringHandler(rdr["PointName"]),
                        UserID = ComUtility.DBNullStringHandler(rdr["UserID"]),
                        UserName = ComUtility.DBNullStringHandler(rdr["UserName"]),
                        Department = ComUtility.DBNullStringHandler(rdr["Department"]),
                        MessageID = ComUtility.DBNullInt32Handler(rdr["MessageID"]),
                        Message = ComUtility.DBNullStringHandler(rdr["Message"]),
                        EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"])
                    });
                }
            }
            return records;
        }

        public List<AcEventInfo> GetPubValidCardEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };

            parms[0].Value = fromTime;
            parms[1].Value = toTime;

            var records = new List<AcEventInfo>();
            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_OTHER_GetPubValidCardEvent, parms)) {
                while(rdr.Read()) {
                    records.Add(new AcEventInfo {
                        LscId = lscId,
                        LscName = lscName,
                        NetGroupID = ComUtility.DBNullInt32Handler(rdr["NetGroupID"]),
                        NetGroupName = ComUtility.DBNullStringHandler(rdr["NetGroupName"]),
                        PointName = ComUtility.DBNullStringHandler(rdr["PointName"]),
                        UserID = ComUtility.DBNullStringHandler(rdr["UserID"]),
                        UserName = ComUtility.DBNullStringHandler(rdr["UserName"]),
                        Department = ComUtility.DBNullStringHandler(rdr["Department"]),
                        MessageID = ComUtility.DBNullInt32Handler(rdr["MessageID"]),
                        Message = ComUtility.DBNullStringHandler(rdr["Message"]),
                        EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"])
                    });
                }
            }
            return records;
        }

        public List<MaskingInfo> GetMaskings(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };

            parms[0].Value = fromTime;
            parms[1].Value = toTime;

            var records = new List<MaskingInfo>();
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_OTHER_GetMaskings, parms)) {
                while (rdr.Read()) {
                    records.Add(new MaskingInfo {
                        LscID = lscId,
                        LscName = lscName,
                        Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]),
                        Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]),
                        Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]),
                        Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]),
                        Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]),
                        Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]),
                        StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]),
                        StaName = ComUtility.DBNullStringHandler(rdr["StaName"]),
                        MaskID = ComUtility.DBNullInt32Handler(rdr["MaskID"]),
                        MaskName = ComUtility.DBNullStringHandler(rdr["MaskName"]),
                        MaskType = ComUtility.DBNullNodeTypeHandler(rdr["MaskType"]),
                        OpTime = ComUtility.DBNullDateTimeHandler(rdr["OpTime"])
                    });
                }
            }
            return records;
        }
    }
}