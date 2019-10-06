using System;
using System.Collections.Generic;
using Repules.Model;

namespace Repules.Bll
{
    public interface IGPSRecordService
    {
        GPSRecord GetGPSRecord(Guid id);
        List<GPSRecord> GetGPSRecords();
        void UpdateGPSRecord(GPSRecord record);
        void SetColor(Flight flight);
    }
}