namespace ImageHunt.Model
{
    public class Geography
    {
        public Geography(double longitude, double latitude, double elevation)
        {
            Longitude = longitude;
            Latitude = latitude;
            Elevation = elevation;
        }

        /// <summary>
        /// Convert the coordinates entered in decimal degre to radian
        /// </summary>
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Elevation { get; set; }
    }
}