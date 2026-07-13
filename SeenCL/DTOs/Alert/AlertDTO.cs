namespace SeenCL.DTOs
{
    public class AlertDTO
    {
        public int AlertID { get; set; }
        public int SensorID { get; set; } // ?? ????? ??? ? ???? NOT NULL
        public string AlertType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int DeviceID { get; set; } // ?? ????? ??? ? ???? NOT NULL

        public AlertDTO(int AlertID, int SensorID, string AlertType, string Message, DateTime CreatedAt, int DeviceID)
        {
            this.AlertID = AlertID;
            this.SensorID = SensorID;
            this.AlertType = AlertType;
            this.Message = Message;
            this.CreatedAt = CreatedAt;
            this.DeviceID = DeviceID;
        }
    }
}

