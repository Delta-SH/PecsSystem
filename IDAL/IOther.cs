using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;
using System.Data;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for others
    /// </summary>
    public interface IOther
    {
        /// <summary>
        /// Method to get areas
        /// </summary>
        /// <param name="lscId">lscId</param>
        List<AreaInfo> GetAreas(int lscId);

        /// <summary>
        /// Method to get areas
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        List<AreaInfo> GetAreas(int lscId, int groupId);

        /// <summary>
        /// Method to get areas by level.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="level">level</param>
        List<AreaInfo> GetAreas(int lscId, int groupId, int level);

        /// <summary>
        /// Method to get area
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="areaId">areaId</param>
        AreaInfo GetArea(int lscId, int areaId);

        /// <summary>
        /// Method to get stations
        /// </summary>
        /// <param name="lscId">lscId</param>
        List<StationInfo> GetStations(int lscId);

        /// <summary>
        /// Method to get stations
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        List<StationInfo> GetStations(int lscId, int groupId);

        /// <summary>
        /// Method to get station
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        StationInfo GetStation(int lscId, int staId);

        /// <summary>
        /// Method to get devices
        /// </summary>
        /// <param name="lscId">lscId</param>
        List<DeviceInfo> GetDevices(int lscId);

        /// <summary>
        /// Method to get devices
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        List<DeviceInfo> GetDevices(int lscId, int groupId);

        /// <summary>
        /// Method to get device
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        DeviceInfo GetDevice(int lscId, int devId);

        /// <summary>
        /// Method to get rtus
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        List<RtuInfo> GetRtus(int lscId, int groupId);

        /// <summary>
        /// Method to get station node count
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="alarmDeviceTypeID">alarmDeviceTypeID</param>
        List<StationCntInfo> GetStationNodeCnt(int lscId, int groupId, int alarmDeviceTypeID);

        /// <summary>
        /// Method to get station FSU count
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="alarmIds">alarmIds</param>
        /// <param name="split">split</param>
        List<StationCntInfo> GetStationFSUCnt(int lscId, int groupId, string alarmIds, string split);

        /// <summary>
        /// Method to get station markers
        /// </summary>
        /// <param name="staName">staName</param>
        /// <param name="minLng">minLng</param>
        /// <param name="maxLng">maxLng</param>
        /// <param name="minLat">minLat</param>
        /// <param name="maxLat">maxLat</param>
        /// <param name="rowCnt">rowCnt</param>
        List<StationInfo> GetStationMarkers(string staName, double minLng, double maxLng, double minLat, double maxLat, int rowCnt);

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
        void UpdateMarker(int olscId, int ostaId, int lscId, int staId, EnmMapType mapType, double lng, double lat, string address);

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
        void UpdateLocalMarker(string connectionString, int ostaId, int staId, EnmMapType mapType, double lng, double lat, string address);

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
        List<BatStaticInfo> GetBatStatic(int lscId, string lscName, int? devId, int? devIndex, DateTime? beginFromTime, DateTime? beginToTime, DateTime? endFromTime, DateTime? endToTime, double fromInterval, double toInterval);

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
        List<IDValuePair<Int32, Int32>> GetBatStaticCount(int lscId, DateTime beginFromTime, DateTime beginToTime, DateTime? endFromTime, DateTime? endToTime, double fromInterval, double toInterval);

        /// <summary>
        /// Get lsc param
        /// </summary>
        /// <returns>lsc param</returns>
        List<LscParamInfo> GetLscParam();

        /// <summary>
        /// Get history dsc records.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="beginTime">beginTime</param>
        /// <param name="endTime">endTime</param>
        /// <returns>dsc records</returns>
        List<DscInfo> GetHisDsc(int lscId, DateTime beginTime, DateTime endTime);

        List<SubDevInfo> GetSubDev(int lscId, string lscName, string connectionString);

        List<AcEventInfo> GetPubAlertEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime);

        List<AcEventInfo> GetPubGeneralEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime);

        List<AcEventInfo> GetPubInvalidCardEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime);

        List<AcEventInfo> GetPubValidCardEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime);

        List<MaskingInfo> GetMaskings(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime);
    }
}