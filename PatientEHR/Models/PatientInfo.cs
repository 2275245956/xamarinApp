 
namespace PatientEHR.Models
{

    public class PatientInfo
    { 
        public int ObservationPatientId { get; set; }
        public int PatientNo { get; set; }

        public string Password { get; set; }

        public bool ShowVital { get; set; }
        public bool ShowInfection { get; set; }
        public string ContactTel { get; set; }
        public string Token { get; set; }
        public string LoginId { get; set; }
        public int OfficeId { get; set; }

        public string Contact01Name { get; set; }

        public string Contact01Zoku { get; set; }

        public string PatientName { get; set; }
        public string PatientKana { get; set; }
        public System.DateTime Birthday { get; set; }
        public int Sex { get; set; }

        public string Age { get; set; }
        public string  FaceImage { get; set; }

        public string  Mail { get; set; }
        public string ExpireDateTime { get; set; }
        public string DeviceId { get; set; }
    }

}
