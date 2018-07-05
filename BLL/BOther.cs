using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL
{
    /// <summary>
    /// A business componet to get others
    /// </summary>
    public class BOther
    {
        // Get an instance of others using the DALFactory
        private static readonly IOther otherDal = DataAccess.CreateOther();

        /// <summary>
        /// Method to get areas
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<AreaInfo> GetAreas(int lscId) {
            try {
                return otherDal.GetAreas(lscId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get areas
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<AreaInfo> GetAreas(int lscId, int groupId) {
            try {
                return otherDal.GetAreas(lscId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get areas by level.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="level">level</param>
        public List<AreaInfo> GetAreas(int lscId, int groupId, int level) {
            try {
                return otherDal.GetAreas(lscId, groupId, level);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get area
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="areaId">areaId</param>
        public AreaInfo GetArea(int lscId, int areaId) {
            try {
                return otherDal.GetArea(lscId, areaId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get stations
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<StationInfo> GetStations(int lscId) {
            try {
                return otherDal.GetStations(lscId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get stations
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<StationInfo> GetStations(int lscId, int groupId) {
            try {
                return otherDal.GetStations(lscId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get station
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        public StationInfo GetStation(int lscId, int staId) {
            try {
                return otherDal.GetStation(lscId, staId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get devices
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<DeviceInfo> GetDevices(int lscId) {
            try {
                return otherDal.GetDevices(lscId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get devices
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<DeviceInfo> GetDevices(int lscId, int groupId) {
            try {
                return otherDal.GetDevices(lscId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get device
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        public DeviceInfo GetDevice(int lscId, int devId) {
            try {
                return otherDal.GetDevice(lscId, devId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get rtus
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<RtuInfo> GetRtus(int lscId, int groupId) {
            try {
                return otherDal.GetRtus(lscId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get station node count
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        /// <param name="alarmDeviceTypeID">alarmDeviceTypeID</param>
        public List<StationCntInfo> GetStationNodeCnt(int lscId, int groupId, int alarmDeviceTypeID) {
            try {
                return otherDal.GetStationNodeCnt(lscId, groupId, alarmDeviceTypeID);
            } catch {
                throw;
            }
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
                return otherDal.GetStationFSUCnt(lscId, groupId, alarmIds, split);
            } catch {
                throw;
            }
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
                return otherDal.GetStationMarkers(staName, minLng, maxLng, minLat, maxLat, rowCnt);
            } catch {
                throw;
            }
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
                otherDal.UpdateMarker(olscId, ostaId, lscId, staId, mapType, lng, lat, address);
            } catch {
                throw;
            }
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
                otherDal.UpdateLocalMarker(connectionString, ostaId, staId, mapType, lng, lat, address);
            } catch {
                throw;
            }
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
            try {
                return otherDal.GetBatStatic(lscId, lscName, devId, devIndex, beginFromTime, beginToTime, endFromTime, endToTime, fromInterval, toInterval);
            } catch {
                throw;
            }
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
        public List<IDValuePair<Int32, Int32>> GetBatStaticCount(int lscId, DateTime beginFromTime, DateTime beginToTime, DateTime? endFromTime, DateTime? endToTime, double fromInterval, double toInterval) {
            try {
                return otherDal.GetBatStaticCount(lscId, beginFromTime, beginToTime, endFromTime, endToTime, fromInterval, toInterval);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Get lsc param
        /// </summary>
        /// <returns>lsc param</returns>
        public List<LscParamInfo> GetLscParam() {
            try {
                return otherDal.GetLscParam();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Get history dsc records.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="beginTime">beginTime</param>
        /// <param name="endTime">endTime</param>
        /// <returns>dsc records</returns>
        public List<DscInfo> GetHisDsc(int lscId, DateTime beginTime, DateTime endTime) {
            try {
                return otherDal.GetHisDsc(lscId, beginTime, endTime);
            } catch {
                throw;
            }
        }

        public List<SubDevInfo> GetSubDev(int lscId, string lscName, string connectionString) {
            try {
                return otherDal.GetSubDev(lscId, lscName, connectionString);
            } catch {
                throw;
            }
        }

        public List<AcEventInfo> GetPubAlertEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            try {
                return otherDal.GetPubAlertEvent(lscId, lscName, connectionString, fromTime, toTime);
            } catch {
                throw;
            }
        }

        public List<AcEventInfo> GetPubGeneralEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            try {
                return otherDal.GetPubGeneralEvent(lscId, lscName, connectionString, fromTime, toTime);
            } catch {
                throw;
            }
        }

        public List<AcEventInfo> GetPubInvalidCardEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            try {
                return otherDal.GetPubInvalidCardEvent(lscId, lscName, connectionString, fromTime, toTime);
            } catch {
                throw;
            }
        }

        public List<AcEventInfo> GetPubValidCardEvent(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            try {
                return otherDal.GetPubValidCardEvent(lscId, lscName, connectionString, fromTime, toTime);
            } catch {
                throw;
            }
        }

        public List<MaskingInfo> GetMaskings(int lscId, string lscName, string connectionString, DateTime fromTime, DateTime toTime) {
            try {
                return otherDal.GetMaskings(lscId, lscName, connectionString, fromTime, toTime);
            } catch {
                throw;
            }
        }
    }
}